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
    Task<bool> CompleteAsync();
}