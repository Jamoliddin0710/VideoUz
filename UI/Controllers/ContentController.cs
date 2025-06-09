using System.Text;
using Application.DTOs;
using Application.Models;
using DocumentFormat.OpenXml.Packaging;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Client;
using Infrastructure.DTOs;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using UI.Services;
using Path = System.IO.Path;
namespace UI.Controllers;

public class ContentController(
    IModuleRefitService moduleRefitService,
    IStorageRefitService storageRefitService,
    ICourseRefitService courseRefitService,
    IOptions<AppOptions> options,
    HttpClient _httpClient,
    IGPTRefitService gptRefitService,
    IContentRefitService contentRefitService)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(long moduleId)
    {
        var module = await moduleRefitService.GetById(moduleId);

        if (module is null)
            return NotFound();

        var viewModel = new ContentCreateViewModel
        {
            ModuleId = moduleId
        };

        ViewBag.ModuleTitle = module.Data?.Title ?? string.Empty;
        ViewBag.CourseTitle = module.Data?.Course?.Title ?? string.Empty;

        return View(viewModel);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CourseId = TempData["CourseId"];
            ViewBag.CourseTitle = TempData["CourseTitle"];
            ViewBag.ModuleTitle = TempData["ModuleTitle"];
            return View(model);
        }

        var dto = model.Adapt<CreateContentDTO>();

        if (model.ContentType == ContentType.Text)
        {
            dto.FileId = null;
        }
        else
        {
            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload");
                ViewBag.CourseId = TempData["CourseId"];
                ViewBag.CourseTitle = TempData["CourseTitle"];
                ViewBag.ModuleTitle = TempData["ModuleTitle"];
                return View(model);
            }

            using var stream = model.File.OpenReadStream();
            var streamPart = new StreamPart(stream, model.File.FileName, model.File.ContentType);
            var file = await storageRefitService.UploadFile(streamPart);
            dto.FileId = file?.Data?.Id;
        }

        if (dto.ContentType == ContentType.Text || dto.ContentType == ContentType.Document)
        {
            string readText = string.Empty;
            if (dto.ContentType is ContentType.Document)
            {
                dto.ContentData = await ReadDocumentTextAsync(model.File);
            }
            
            /*var questionFormat = Newtonsoft.Json.JsonConvert.SerializeObject(new List<Question>()
            {
                new Question()
                {
                    QuestionText = "O'zbekistonni poytaxti qayer ?",
                    Order = 1,
                    Points = 10,
                    Options = new List<QuestionOption>()
                    {
                        new QuestionOption()
                        {
                            Id = 1,
                            IsCorrect = false,
                            QuestionId = 1,
                            OptionText = "Qarshi",
                        },
                        new QuestionOption()
                        {
                            Id = 2,
                            IsCorrect = true,
                            QuestionId = 2,
                            OptionText = "Tashkent",
                        },
                        new QuestionOption()
                        {
                            Id = 3,
                            IsCorrect = false,
                            QuestionId = 2,
                            OptionText = "Nukus",
                        },
                        new QuestionOption()
                        {
                            Id = 4,
                            IsCorrect = false,
                            QuestionId = 2,
                            OptionText = "Urganch",
                        },
                    }
                }
            });

            var request = new OpenAiChatRequest()
            {
                Messages = new List<OpenAiMessage>()
                {
                    new OpenAiMessage()
                    {
                        Role = "system", Content =
                            $"Senga foydalanuvchi text jo'natadi sen shu text bo'yicha {questionFormat} kabi json formatda 10 ta test tuzib jo'natasan" +
                            $" agar text da 10 ta savol chiqmasa shu mavzuga aloqador o'zingdan savol qo'shasan har doim 10 ta savol bo'lishi kerak. " +
                            $"Iltimos, quyidagi C# modelga deserilize qilinadigan Faqat va Faqat toza JSON ko‘rinishida qaytaring. " +
                            $"Hech qanday izoh, matn yoki izohli so‘zlar yozmang. Faqat JSONni yuboring.\n "
                    },
                    new OpenAiMessage() { Role = "user", Content = readText }
                }
            };
            var resposne = await gptRefitService.SendMessage(request);
            var quizes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Question>>(resposne.choices
                .FirstOrDefault(a => a.message.role.Equals("assistant")).message.content);*/
        }

        await contentRefitService.Create(dto);
        var course = await courseRefitService.GetcourseByModuleId(model.ModuleId);
        return RedirectToAction("Details", "Course", new { id = course.Data });
    }

    [HttpPost]
    public async Task<ActionResult<APIResponse>> SendChatMessage([FromForm]string message)
    {
        var courses = (await courseRefitService.GetAll()).Data.Data.Select(a=> $"Kurs  - {a.Title} - {a.Description}, Ustoz - {a.User}").ToList();
        var coursedetails = string.Join("\n", courses);
        var systemPrompt = $"Sen J.Shermatov tomonidan ishlab chiqilgan course.uz platformasi uchun AI asistentsan." +
                           $"course.uz zamonaviy interaktiv onlayn kurs platformasi hisoblanadi." +
                           " Quyidagi ma’lumotlarga asoslangan holda foydalanuvchi savollariga javob bering." +
                           " Noto‘g‘ri yoki taxminiy javob bermang." +
                           "\n\nPlatforma haqida:\n- Bu platforma dasturlash va texnologiyalarni o‘rgatadi" +
                           $".\n- Kurslar: {coursedetails}- Har bir kursda videodarslar, testlar va amaliy loyihalar mavjud. Kursda tushunarli " +
                           $" description bo'lmasa o'zing qo'sh kurs nomiga qarab. Kurs descriptionini refactor qilib chiqar" +
                           "\n- Platforma foydalanuvchining shaxsiy kurslarini saqlab boradi." +
                           "\n\nYordamchi sifatida siz quyidagilarni qila olasiz" +
                           ":\n- Kurslar ro‘yxatini aytib bera olasiz.\n- Har bir kurs haqida ma’lumot bera olasiz." +
                           "\n- Foydalanuvchining o‘rgangan kurslarini eslatib bera olasiz" +
                           ".\n- Platformadan foydalanish bo‘yicha yordam bera olasiz.\n\nSiz yordamchisiz, ChatGPT emassiz." +
                           " Javoblaringiz qisqa, lo‘nda va maqsadga yo‘naltirilgan bo‘lishi kerak.\n";
        var request = new OpenAiChatRequest()
        {
            Messages = new List<OpenAiMessage>
            {
                new OpenAiMessage { Role = "system", Content = systemPrompt },
                new OpenAiMessage { Role = "user", Content = message }
            },
            Temperature = 0.5,
            MaxTokens = 1000,
        };

        var response = await gptRefitService.SendMessage(request);
        return Ok(new APIResponse(200, result:response.choices.FirstOrDefault().message));
    }

    public async Task<string> ReadDocumentTextAsync(IFormFile file)
    {
        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        using var stream = file.OpenReadStream();
        using var memStream = new MemoryStream();
        await stream.CopyToAsync(memStream);
        memStream.Position = 0;


        switch (fileExtension)
        {
            case ".docx":
                return ReadDocxText(memStream);
            default:
                return ReadPdfText(memStream);
        }
    }


    private string ReadDocxText(MemoryStream memStream)
    {
        try
        {
            using var wordDoc = WordprocessingDocument.Open(memStream, false);
            var body = wordDoc.MainDocumentPart.Document.Body;
            return body.InnerText;
        }
        catch (Exception ex)
        {
            throw new Exception("DOCX faylini o'qishda xatolik yuz berdi", ex);
        }
    }
    
    private string ReadPdfText(MemoryStream memStream)
    {
        try
        {
            var text = new StringBuilder();

            using var reader = new PdfReader(memStream.ToArray());
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                string pageContent = PdfTextExtractor.GetTextFromPage(reader, page);

                text.AppendLine($"--- Sahifa {page} ---");
                text.AppendLine(pageContent);
                text.AppendLine();
            }

            return text.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("PDF faylini o'qishda xatolik yuz berdi", ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var content = await contentRefitService.GetById(id);
        if (!content.IsSuccessful)
        {
            TempData["Error"] = "Module not found";
            return RedirectToAction("Index", "Course");
        }

        var editViewModel = new ContentEditViewModel()
        {
            Id = content.Data.Id,
            Title = content.Data.Title,
            ModuleId = content.Data.ModuleId,
            ContentType = content.Data.ContentType,
            ContentData = content.Data.ContentData,
            FileName = content.Data.FileItem.StorageName,
            Bucket = content.Data.FileItem.Bucket
        };
        return View(editViewModel);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        await contentRefitService.Delete(id);
        return Ok();
    }

    private string GetContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }

    private async Task<(Stream Stream, string FileName, string ContentType)> DownloadFileAsStreamAsync(string bucket,
        string fileName)
    {
        var apiUrl =
            $"{options.Value.BackendApi!.TrimEnd('/')}/storage/download?bucket={bucket}&fileName={fileName}";
        var response = await _httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
            throw new Exception("File read error");

        var stream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        var contentDisposition = response.Content.Headers.ContentDisposition;
        var name = contentDisposition?.FileName?.Trim('"') ?? fileName;

        return (stream, name, contentType);
    }
}