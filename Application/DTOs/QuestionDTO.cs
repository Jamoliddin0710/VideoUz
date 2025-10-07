using Domain.Entities.Courses;

namespace Application.DTOs;

public class QuestionDTO
{
    public long Id { get; set; }
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; } = QuestionType.SingleChoice;
    public int Points { get; set; }
    public int Order { get; set; }
    public List<QuestionOptionDTO> Options { get; set; }
}

public class QuestionOptionDTO
{
    public long Id { get; set; }
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
}

public class QuestionGptDTO
{
    public string QuestionText { get; set; }

    public QuestionType QuestionType { get; set; } = QuestionType.SingleChoice;
    public int Points { get; set; }

    public int Order { get; set; }
    public List<QuestionOptionGptDTO> Options { get; set; }
}

public class QuestionOptionGptDTO
{
    public string OptionText { get; set; }
    public bool IsCorrect { get; set; }
}