using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class QuizAttempRepo : BaseRepo<QuizAttempt> , IQuizAttemptRepo
{
    public QuizAttempRepo(AppDbContext context) : base(context)
    {
    }
}