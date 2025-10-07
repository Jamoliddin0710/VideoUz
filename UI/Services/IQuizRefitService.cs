using Application.DTOs;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace UI.Services;

public interface IQuizRefitService
{
    [Get("/quiz/getquiz")]
    Task<ServiceResponse<QuizDTO>> GetQuiz(long contentId);

    [Post("/quiz/updateattempt")]
    Task<ServiceResponse<QuizAttemptDTO>> SubmitQuiz([Body] SubmitQuizRequestDTO requestDto);
    [Get("/quiz/getattemptresult")]
    Task<ServiceResponse<QuizResultViewModel>> GetAttemptResult(long attemptid);
}