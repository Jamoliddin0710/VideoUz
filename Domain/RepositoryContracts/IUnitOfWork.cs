namespace Domain.RepositoryContracts;

public interface IUnitOfWork : IDisposable
{
    IChannelRepo ChannelRepo { get; }
    ICategoryRepo CategoryRepo { get; }
    IVideoRepo VideoRepo { get; }
    ICourseRepo CourseRepo { get; }
    IFileRepo FileRepo { get; }
    IModuleRepo ModuleRepo { get; }
    IContentRepo ContentRepo { get; }
    IProgressRepo ProgressRepo { get; }
    IEnrollmentRepo EnrollmentRepo { get; }
    IQuizRepo QuizRepo { get; }
    IQuizAttemptRepo QuizAttemptRepo { get; }
    IQuizAnswerRepo QuizAnswerRepo { get; }
    IQuestionRepo QuestionRepo { get; }
    IQuestionOptionRepo QuestionOptionRepo { get; }
    Task<bool> CompleteAsync();
}