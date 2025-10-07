using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class QuestionOptionRepo : BaseRepo<QuestionOption>, IQuestionOptionRepo
{
    public QuestionOptionRepo(AppDbContext context) : base(context)
    {
    }
}