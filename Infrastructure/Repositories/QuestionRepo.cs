using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class QuestionRepo : BaseRepo<Question> , IQuestionRepo
{
    public QuestionRepo(AppDbContext context) : base(context)
    {
    }
}