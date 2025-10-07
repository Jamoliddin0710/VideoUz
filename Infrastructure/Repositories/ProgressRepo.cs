using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class ProgressRepo : BaseRepo<Progress> , IProgressRepo
{
    public ProgressRepo(AppDbContext context) : base(context)
    {
    }
}