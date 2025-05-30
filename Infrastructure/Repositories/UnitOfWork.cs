using Domain.RepositoryContracts;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    
    public void Dispose()
    {
       _context.Dispose();
    }

    public IChannelRepo ChannelRepo => new ChannelRepo(_context);
    public ICategoryRepo CategoryRepo => new CategoryRepo(_context);
    public IVideoRepo VideoRepo => new VideoRepo(_context);

    public async Task<bool> CompleteAsync()
    {
        var result = false;
        if (_context.ChangeTracker.HasChanges())
        {
            result = await _context.SaveChangesAsync() > 0;
        }
        return result;
    }
}