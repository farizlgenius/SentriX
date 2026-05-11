using System;
using Adapter.Aero.Enums;
using Adapter.Aero.Model;

namespace AeroAdapter.Application.Interfaces;

public interface IWriterRepository
{
      Task AddWriterAuditAsync(int ScpId,string Mac,WriterType Command,int Tag,string Body,string? status="PENDING");
      Task UpdateWriterAuditAsync(int ScpId,string Mac, int Tag,SCPReplyMessageDto.SCPReplyCmndStatusDto message);
      Task<bool> IsAnyByScpIdAndTagIdAsync(int ScpId,string Mac, int Tag);
}
