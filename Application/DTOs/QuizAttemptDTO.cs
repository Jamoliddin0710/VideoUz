namespace Application.DTOs;

public class QuizAttemptDTO
{
    public long QuizId { get; set; }
    public long UserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public bool IsPassed { get; set; }
}