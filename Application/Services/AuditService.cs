using Application.ServiceContract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Application.Services;

public class AuditService(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
    CancellationToken cancellationToken = new CancellationToken())
  {
    AuditEntities(eventData.Context);
    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private void AuditEntities(DbContext context)
  {
    if (context == null) return;
    var userId = currentUserService.GetUserId();
    foreach (var entry in context.ChangeTracker.Entries<AuditEntity>())
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.CreatedBy = userId;
        entry.Entity.CreatedDate = DateTime.Now;
      }
      else if (entry.State == EntityState.Modified)
      {
        entry.Entity.ModifiedBy = userId;
        entry.Entity.ModifiedDate = DateTime.Now;
      }
      else if (entry.State is EntityState.Deleted)
      {
        entry.Entity.DeletedBy = userId;
        entry.Entity.DeletedDate = DateTime.Now;
      }
    }
    
  }
}