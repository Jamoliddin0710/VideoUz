using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class QuizController(IQuizService quizService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<QuizDTO>>> GetQuiz(long contentId)
    {
        var userId = User.GetUserId();
        return Ok(await quizService.GetQuiz(contentId, userId ?? 0));
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<QuizAttemptDTO>>> UpdateAttempt(
        [FromBody] SubmitQuizRequestDTO requestDto)
    {
        return Ok(await quizService.Submit(requestDto));
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<QuizResultViewModel>>> GetAttemptResult(long attemptid)
    {
        return Ok(await quizService.GetAttemptResult(attemptid));
    }
}