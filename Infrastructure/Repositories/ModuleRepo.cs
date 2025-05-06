
using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class ModuleRepo : BaseRepo<Module>, IModuleRepo
{
    public ModuleRepo(AppDbContext context) : base(context)
    {
    }
}