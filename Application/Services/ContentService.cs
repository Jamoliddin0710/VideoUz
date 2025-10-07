using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Domain.Models;
using Domain.RepositoryContracts;
using Infrastructure.Client;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ContentService(IUnitOfWork _unitOfWork, IGPTRefitService _gptRefitService, ILogger<ContentService> _logger)
    : IContentService
{
    public async Task<ContentDTO> Create(CreateContentDTO model)
    {
        var content = model.Adapt<Content>();
        if (content.ContentType is ContentType.Text or ContentType.Document)
        {
            var request = GetGptRequest(content.ContentData);
            GPTResponse response = null;
            try
            {
                response = await _gptRefitService.SendMessage(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            if (response != null)
            {
                var questions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuestionDTO>>(response.choices
                    .FirstOrDefault(a => a.message.role.Equals("assistant")).message.content);
                content.Quizzes = new List<Quiz>()
                {
                    new Quiz()
                    {
                        Title = content.Title,
                        Description = string.Empty,
                        PassingScore = 100,
                        TimeLimit = 20,
                        Questions = questions.Adapt<List<Question>>(),
                    }
                };
            }
        }

        _unitOfWork.ContentRepo.Add(content);
        await _unitOfWork.CompleteAsync();
        return content.Adapt<ContentDTO>();
    }

    private OpenAiChatRequest GetGptRequest(string document, int questionCount = 10)
    {
        var questionTemplate = new QuestionGptDTO
        {
            QuestionText = "O'zbekistonni poytaxti qayer?",
            Order = 1,
            Points = 10,
            Options = new List<QuestionOptionGptDTO>
            {
                new QuestionOptionGptDTO { OptionText = "Qarshi", IsCorrect = false },
                new QuestionOptionGptDTO { OptionText = "Toshkent", IsCorrect = true },
                new QuestionOptionGptDTO { OptionText = "Nukus", IsCorrect = false },
                new QuestionOptionGptDTO { OptionText = "Urganch", IsCorrect = false }
            }
        };

        var questionFormat = Newtonsoft.Json.JsonConvert.SerializeObject(
            new List<QuestionGptDTO> { questionTemplate },
            Newtonsoft.Json.Formatting.Indented
        );

        string systemPrompt = $@"Sen professional test tuzuvchi assistentsan. 
Quyidagi matnni tahlil qilib, undagi ma'lumotlar asosida {questionCount} ta test savoli tuzishing kerak.

Har bir savol uchun quyidagilarni ta'minlashing shart:
1. Savolning aniq va tushunarli bo'lishi
2. To'rtta variant, ularning bittasi to'g'ri javob bo'lishi
3. Variantlar aralash tartibda bo'lishi va ularda takrorlanishlar bo'lmasligi
4. Har bir savol uchun 10 ball belgilash

Savollarni JSON formatida qaytarish kerak. Qaytaradigan JSON formatga misol:
{questionFormat}

MUHIM QOIDALAR:
- Faqat toza JSON formatida javob ber
- Hech qanday qo'shimcha tushuntirish yozmalari, izohlar, kirish, preambula yoki xulosa qo'shma
- Agar matnda yetarli ma'lumot bo'lmasa, mavzuga aloqador o'zingdan savollar qo'sh
- Har doim aynan {questionCount} ta savol qaytarishing kerak
- Javoblar ichida HTML teglari, maxsus belgilar, yoki formatlash belgilari ishlatma
- Savollar tartib raqamini (Order) 1 dan boshlab ketma-ket raqamla";

        return new OpenAiChatRequest
        {
            Messages = new List<OpenAiMessage>
            {
                new OpenAiMessage { Role = "system", Content = systemPrompt },
                new OpenAiMessage { Role = "user", Content = document }
            },
            Temperature = 0.7,
            MaxTokens = 3000,
        };
    }

    public async Task<ContentDTO> GetById(long id)
    {
        var content = await _unitOfWork.ContentRepo.GetByIdAsync(id);
        return content.Adapt<ContentDTO>();
    }

    public async Task<bool> Delete(long id)
    {
        var content = await _unitOfWork.ContentRepo.GetByIdAsync(id);
        _unitOfWork.ContentRepo.Remove(content);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}