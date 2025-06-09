using Application.DTOs;
using Domain.Entities;
using Mapster;

namespace Application.Config;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Quiz, QuizDTO>.NewConfig();
        TypeAdapterConfig<Question, QuestionDTO>.NewConfig();
        TypeAdapterConfig<QuestionOption, Question>.NewConfig();
    }
}