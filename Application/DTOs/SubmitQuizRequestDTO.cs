using System.Collections;
using Domain.Entities;

namespace Application.DTOs;

public class SubmitQuizRequestDTO
{
    public long QuizId { get; set; }
    public long AttemptId { get; set; }
    public List<QuizAnswerDTO> Answers { get; set; }
}

public class QuizAnswerDTO
{
    public long QuestionId { get; set; }
    public long? SelectedOptionId { get; set; }
    public int QuestionOrder { get; set; }
    public bool IsCorrect { get; set; }
    public string? QuestionText { get; set; } 
    public List<QuestionOptionDTO>? Options { get; set; }
    public string? Explanation { get; set; }
}