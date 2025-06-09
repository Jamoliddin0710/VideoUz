using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class QuizAnswerRepo : BaseRepo<QuizAnswer>, IQuizAnswerRepo
{
    public QuizAnswerRepo(AppDbContext context) : base(context)
    {
    }
}