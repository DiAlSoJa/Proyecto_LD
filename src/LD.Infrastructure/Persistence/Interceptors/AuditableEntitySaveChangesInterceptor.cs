using LD.Application.Common.Interfaces.Auth;
using LD.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContextService _userContextService;

    public AuditableEntitySaveChangesInterceptor(IUserContextService userContextService)
    {
        _userContextService = userContextService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken)
    {

        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            var userId = _userContextService.UserId?.ToString();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedByUserId = userId;

                entry.Entity.LastModifiedAt = DateTime.UtcNow;
                entry.Entity.LastModifiedByUserId = userId;
            }

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedAt = DateTime.UtcNow;
                entry.Entity.LastModifiedByUserId = userId;
            }

            // SOFT DELETE
            //if (entry.State == EntityState.Deleted )
            //{
            //    entry.State = EntityState.Modified;

            //    entry.Entity.IsActive = false;
            //    entry.Entity.DeletedAt = DateTime.UtcNow;
            //    entry.Entity.DeletedByUserId = userId;

            //    entry.Entity.LastModifiedAt = DateTime.UtcNow;
            //    entry.Entity.LastModifiedByUserId = userId;
            //}
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}

