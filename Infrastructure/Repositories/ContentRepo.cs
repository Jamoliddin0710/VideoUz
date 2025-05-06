using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class ContentRepo : BaseRepo<Content> , IContentRepo
{
    public ContentRepo(AppDbContext context) : base(context)
    {
    }
}