using Domain.Entities;
using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class ChannelRepo : BaseRepo<Channel>, IChannelRepo
{
    public ChannelRepo(AppDbContext context) : base(context)
    {
        
    }
}