using System.Security.Claims;
using System.Threading.Channels;
using Core.Contract.DTOs.Events.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Core.Infrastructure.Interceptors;

public sealed class AuditSaveChangesInterceptor(Channel<AuditTrailInsert> channel, IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{



      public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
          DbContextEventData eventData,
          InterceptionResult<int> result,
          CancellationToken cancellationToken = default)
      {
            var dbContext = eventData.Context;
            if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            // 1. Identify modified/added/deleted entities (Exclude AuditTrail itself to avoid loops)
            var entries = dbContext.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
                .Where(e => e.Entity is IAuditableEntity)
                .ToList();

            if (!entries.Any()) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var context = httpContextAccessor.HttpContext;

            // 2. Extract metadata from HttpContext
            string username = context?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? context?.User.FindFirstValue("sub")
                           ?? "System";

            string method = context?.Request.Method ?? "System";
            string path = context?.Request.Path ?? "N/A";
            string? tenantStr = context?.Items["tenants"] as string;

            Guid? locationGuid = Guid.TryParse(tenantStr, out var parsedGuid) ? parsedGuid : null;
            string clientIp = context?.Connection.RemoteIpAddress?.ToString() ?? "";

            foreach (var entry in entries)
            {
                  // 3. Compute strongly-typed JSON diff using helper
                  string diffJson = AuditHelper.GetEntityChangesJson(entry);

                  Guid entityGuid = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == "guid")?.CurrentValue as Guid? 
                        ?? Guid.Empty;


                  string entityName = entry.Properties
                        .FirstOrDefault(p => p.Metadata.Name == "name")?.CurrentValue?.ToString() ?? "";


                  var auditMessage = new AuditTrailInsert(
                        entry.Entity.GetType().Name,
                        MapEntityState(entry.State),
                        username,
                        clientIp,
                        entityGuid, 
                        entityName,
                        diffJson
                  );

                  // 4. Non-blocking write to in-memory channel (< 1ms execution time)
                  channel.Writer.TryWrite(auditMessage);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
      }

      private static AuditAction MapEntityState(EntityState state) => state switch
      {
            EntityState.Added => AuditAction.Create,
            EntityState.Modified => AuditAction.Update,
            EntityState.Deleted => AuditAction.Delete,
            _ => AuditAction.Read
      };
}