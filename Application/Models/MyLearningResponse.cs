namespace Application.Models;

public class MyLearningResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string CoverImage { get; set; }
    public int CompletionPercentage { get; set; } = 0;
    public int Rating { get; set; } = 5;
}