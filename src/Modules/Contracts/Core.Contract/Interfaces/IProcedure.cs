using Core.Contract.DTOs.Position;
using Core.Contract.DTOs.Procedure;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IProcedure : IService<ProcedureDto,CreateProcedureDto,UpdateProcedureDto>
{
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
}