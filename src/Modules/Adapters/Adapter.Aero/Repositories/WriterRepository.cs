using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Helpers;
using Adapter.Aero.Model;
using Adapter.Aero.Persistences;
using Adapter.Aero.Persistences.Entities;
using AeroAdapter.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Messaging;

namespace Adapter.Aero.Repositories;

public sealed class WriterRepository(AeroDbContext context) : IWriterRepository
{
      public async Task AddWriterAuditAsync(int ScpId,string Mac, WriterType Command, int Tag, string Body,string? status = "PENDING")
      {
            await context.WriterAudits.AddAsync(
                        new WriterAudit
                        {
                              mac= Mac,
                              scp_id = ScpId,
                              command_code = (short)Command,
                              command = Command.ToString(),
                              tag = Tag,
                              send_at = DateTime.UtcNow,
                              received_at = DateTime.UtcNow,
                              body = Body,
                              status = status ?? "PENDING",
                              is_nak = false,
                              reason = "",
                              updated_at = DateTime.UtcNow,
                              created_at = DateTime.UtcNow
                        }
                  );

            await context.SaveChangesAsync();
      }

      public async Task<bool> IsAnyByScpIdAndTagIdAsync(int ScpId,string Mac, int Tag)
      {
            if(string.IsNullOrEmpty(Mac))
                  return false;

            return await context.WriterAudits.AsNoTracking()
            .AnyAsync(x => x.mac.Equals(Mac) && x.tag == Tag);
      }

      public async Task UpdateWriterAuditAsync(int ScpId,string Mac, int Tag,SCPReplyMessageDto.SCPReplyCmndStatusDto message)
      {
            var entities = await context.WriterAudits.Where(x => x.scp_id == ScpId && x.mac.Equals(Mac) && x.tag == Tag && x.status.Equals(WriterStatus.PENDING.ToString())).ToListAsync();
            if(entities.Count == 0)
                  return;

            foreach(var entity in entities)
            {
                  entity.Update(message.status == (short)ScpCommandStatus.OK ? WriterStatus.SUCESS.ToString() : WriterStatus.FAILED.ToString(),message.status != (short)ScpCommandStatus.NAK ? false : true,message.status == (short)ScpCommandStatus.NAK ? DescriptionHelper.GetNakReasonDescription(message.nak.reason) :string.Empty );
            }

            context.WriterAudits.UpdateRange(entities);
            await context.SaveChangesAsync();
      }
}
