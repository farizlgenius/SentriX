using Core.Contract.DTOs.Trigger;
using Core.Domain.Entities;
using SharedKernel.Domain;
using SharedKernel.Enums;

namespace Core.Application.Interfaces;

public interface ITriggerRepository : IRepository<TriggerDto,Trigger>
{
      Task<IEnumerable<OptionDto>> GetSourceAsync(CancellationToken ct = default);
      Task<(int id,short value)> GetSourceIdAndValueByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetTypeAsync(Guid source,CancellationToken ct = default);
      Task<(int id,short value)> GetTypeIdByGuidAsync(Guid guid,CancellationToken ct = default);
      Task<IEnumerable<OptionDto>> GetCodeAsync(Guid type,CancellationToken ct = default);
      Task<IEnumerable<(int id,short value)>> GetCodeIdsAndVeluesByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<IEnumerable<(int id,short value)>> GetArgIdsAndVeluesByGuidsAsync(IEnumerable<Guid> guids,CancellationToken ct = default);
      Task<IEnumerable<(int id, short externald)>> GetTrigVarIdsAndExternalIdsByGuidsAndDeviceGuidAsync(IEnumerable<Guid> guids,Guid guid,CancellationToken ct = default);
      Task<(int id, short externaId)> GetTrigVarIdAndExternalIdByGuideAsync(Guid guid, CancellationToken ct = default);
}