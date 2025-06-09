using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class QuizRepo : BaseRepo<Quiz> , IQuizRepo
{
    public QuizRepo(AppDbContext context) : base(context)
    {
    }
}