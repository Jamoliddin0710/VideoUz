using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using UI.Services;
namespace UI.Controllers;

public class QuizController(IQuizRefitService quizRefitService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> StartQuiz(long contentId)
    {
        var quiz = await quizRefitService.GetQuiz(contentId);
        return View(quiz.Data);
    }  
    
    [HttpGet("/Quiz/ResultQuiz/{attemptId}")]

    public async Task<IActionResult> ResultQuiz(long attemptId)
    {
        var quiz = await quizRefitService.GetAttemptResult(attemptId);
        return View(quiz.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitQuizRequestDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await quizRefitService.SubmitQuiz(model);
            return Ok(new
            {
                success = true,
                attemptId = model.AttemptId,
                message = "Test muvaffaqiyatli topshirildi"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}