using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class VideoRepo :  BaseRepo<Video> , IVideoRepo
{
    public VideoRepo(AppDbContext context) : base(context)
    {
    }
}