using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class EnrollmentRepo : BaseRepo<Enrollment> , IEnrollmentRepo
{
    public EnrollmentRepo(AppDbContext context) : base(context)
    {
    }
}