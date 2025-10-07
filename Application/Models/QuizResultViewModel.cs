using Application.DTOs;

namespace Application.Models;

public class QuizResultViewModel
{
    public long AttemptId { get; set; }
    public long ContentId { get; set; }
    public int Percentage { get; set; }
    public List<QuizAnswerDTO> Answers { get; set; } = new List<QuizAnswerDTO>();
    public int PassingScore { get; set; } = 70;
    public bool IsPassed { get; set; }
    public string? QuizTitle { get; set; }
    public int MaxScore { get; set; }
    public int Score { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}