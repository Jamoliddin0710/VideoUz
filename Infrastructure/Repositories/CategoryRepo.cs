using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class CategoryRepo : BaseRepo<Category> , ICategoryRepo
{
    public CategoryRepo(AppDbContext context) : base(context)
    {
    }
}