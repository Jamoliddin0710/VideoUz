
using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
          //long way to config
        modelBuilder.ApplyConfiguration(new CommentConfig());
        modelBuilder.ApplyConfiguration(new LikeDislikeConfig());
        modelBuilder.ApplyConfiguration(new SubscribeConfig());*/
        
        //short way to config
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole>  AppRoles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Channel>  Channels { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Subscribe> Subscribes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<LikeDislike> LikeDislikes  { get; set; }
}