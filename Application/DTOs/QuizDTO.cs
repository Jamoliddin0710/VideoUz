namespace Application.DTOs;

public class QuizDTO
{
    public long Id { get; set; }
    public long ContentId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public int PassingScore { get; set; }
    public int TimeLimit { get; set; }
    public long? AttemptId { get; set; }
    public List<QuestionDTO> Questions { get; set; }
}