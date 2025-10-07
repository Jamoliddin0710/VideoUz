using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class CourseRepo : BaseRepo<Course>, ICourseRepo
{
    public CourseRepo(AppDbContext context) : base(context)
    {
    }
}