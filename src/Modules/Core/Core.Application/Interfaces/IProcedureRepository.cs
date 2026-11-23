using Core.Contract.DTOs.Procedure;
using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Application.Interfaces;

public interface IProcedureRepository : IRepository<ProcedureDto,Procedure>
{
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
      Task<short> GetFreeExternalIdByDeviceMacAsync(string mac, int max, short[] exception, CancellationToken ct = default);
}