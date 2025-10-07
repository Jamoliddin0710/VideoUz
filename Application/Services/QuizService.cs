using Application.DTOs;
using Application.Models;
using Application.ServiceContract;
using DocumentFormat.OpenXml.Office2013.Excel;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;
using QuestionType = Domain.Entities.Courses.QuestionType;

namespace Application.Services;

public class QuizService(IUnitOfWork _unitOfWork) : IQuizService
{
    public async Task<QuizDTO?> GetQuiz(long contentId, long userId)
    {
        var quiz = await _unitOfWork.QuizRepo.GetFirstOrDefaultAsync(a => a.ContentId == contentId,
            includeProperties: "Questions");

        if (quiz is null)
        {
            return null;
        }

        var result = new QuizDTO()
        {
            Id = quiz.Id,
            ContentId = quiz.ContentId,
            Title = quiz.Title,
            Description = quiz.Description,
            PassingScore = quiz.PassingScore,
            TimeLimit = quiz.TimeLimit,
            Questions = new List<QuestionDTO>(),
        };
        foreach (var question in quiz.Questions)
        {
            result.Questions.Add(new QuestionDTO()
            {
                Id = question.Id,
                QuestionType = QuestionType.SingleChoice,
                QuestionText = question.QuestionText,
                Points = question.Points,
                Order = question.Order,
                Options = question.Options.Adapt<List<QuestionOptionDTO>>(),
            });
        }

        var attempt = new QuizAttempt()
        {
            StartTime = DateTime.Now.ToUniversalTime(),
            QuizId = quiz.Id,
            UserId = userId,
        };
        quiz.QuizAttempts.Add(attempt);
        await _unitOfWork.CompleteAsync();
        result.AttemptId = attempt.Id;

        return result;
    }

    public async Task<QuizAttemptDTO> Submit(SubmitQuizRequestDTO requestDto)
    {
        var attempt = await _unitOfWork.QuizAttemptRepo.GetFirstOrDefaultAsync(a =>
            a.Id == requestDto.AttemptId && a.QuizId == requestDto.QuizId);

        if (attempt == null)
        {
            throw new ApplicationException("Quiz urinishi topilmadi");
        }

        attempt.EndTime = DateTime.Now.ToUniversalTime();

        var existingAnswers =
            await _unitOfWork.QuizAnswerRepo.GetAllAsync(a => a.AttemptId == requestDto.AttemptId);

        if (existingAnswers.Any())
        {
            _unitOfWork.QuizAnswerRepo.RemoveRange(existingAnswers);
        }

        List<QuizAnswer> answers = new List<QuizAnswer>();
        int correctAnswers = 0;
        int totalScore = 0;

        foreach (var answerDto in requestDto.Answers)
        {
            var question = await _unitOfWork.QuestionRepo.GetByIdAsync(answerDto.QuestionId);
            if (question == null) continue;


            bool isCorrect = false;

            if (answerDto.SelectedOptionId.HasValue)
            {
                var correctOption = await _unitOfWork.QuestionOptionRepo.GetFirstOrDefaultAsync(
                    o => o.QuestionId == answerDto.QuestionId && o.IsCorrect);


                isCorrect = correctOption != null && correctOption.Id == answerDto.SelectedOptionId;
            }

            var answer = new QuizAnswer
            {
                AttemptId = requestDto.AttemptId,
                QuestionId = answerDto.QuestionId,
                SelectedOptionId = answerDto.SelectedOptionId,
                IsCorrect = isCorrect
            };

            answers.Add(answer);

            if (isCorrect)
            {
                correctAnswers++;
                totalScore += question.Points;
            }
        }

        _unitOfWork.QuizAnswerRepo.AddRange(answers);
        var quiz = await _unitOfWork.QuizRepo.GetByIdAsync(requestDto.QuizId);

        int maxScore = 0;
        var questions = await _unitOfWork.QuestionRepo.GetAllAsync(q => q.QuizId == requestDto.QuizId);
        foreach (var question in questions)
        {
            maxScore += question.Points;
        }

        int percentage = maxScore > 0 ? (totalScore * 100) / maxScore : 0;
        bool isPassed = percentage >= quiz.PassingScore;

        attempt.Score = totalScore;
        attempt.MaxScore = maxScore;
        attempt.IsPassed = isPassed;
        attempt.CorrectAnswerCount = correctAnswers;

        await _unitOfWork.CompleteAsync();
        return attempt.Adapt<QuizAttemptDTO>();
    }

    public async Task<QuizResultViewModel> GetAttemptResult(long attemptId)
    {
        var result = await _unitOfWork.QuizAttemptRepo.GetFirstOrDefaultAsync(a => a.Id == attemptId,
            includeProperties: "Quiz.Questions,Quiz.Questions.Options");
      
        var model = new QuizResultViewModel()
        {
            AttemptId = attemptId,
            ContentId = result.Quiz.ContentId,
            QuizTitle = result.Quiz.Title,
            MaxScore = result.MaxScore,
            Score = result.Score,
            IsPassed = result.IsPassed,
            Percentage = (result.Score * 100) / result.MaxScore,
            CorrectAnswers = result.CorrectAnswerCount,
            TotalQuestions = result.Quiz.Questions.Count(),
            StartTime = result.StartTime,
            EndTime = result.EndTime,
        };
        foreach (var answer in result.Answers)
        {
            model.Answers.Add(new QuizAnswerDTO()
            {
                QuestionId = answer.QuestionId,
                SelectedOptionId = answer.SelectedOptionId,
                IsCorrect = answer.IsCorrect,
                QuestionText = answer.Question.QuestionText,
                QuestionOrder = answer.Question.Order,
                Explanation = answer.Question?.Options?.FirstOrDefault(a=>a.IsCorrect)?.OptionText ?? string.Empty,
                Options = answer.Question?.Options?.Adapt<List<QuestionOptionDTO>>() ?? new List<QuestionOptionDTO>()
            });
        }
        return model;
    }
}