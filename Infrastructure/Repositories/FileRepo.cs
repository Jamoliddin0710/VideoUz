using Domain.Entities.Courses;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class FileRepo : BaseRepo<FileItem> , IFileRepo
{
    public FileRepo(AppDbContext context) : base(context)
    {
    }
}