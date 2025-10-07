using Application.DTOs;
using Application.Models;

namespace Application.ServiceContract;

public interface IQuizService
{
    Task<QuizDTO> GetQuiz(long contentId, long userId);
    Task<QuizAttemptDTO> Submit(SubmitQuizRequestDTO requestDto);
    Task<QuizResultViewModel> GetAttemptResult(long attemptId);
}