using Core.Contract.DTOs.Output;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IOutput : IService<ControlDto, CreateControlDto, UpdateOutputDto>
{
      Task UploadAsync(CancellationToken ct = default);
      Task<bool> OutputCommandAsync(ControlCommandDto command,CancellationToken ct = default);
      Task<bool> GetStatusAsync(Guid guid,CancellationToken ct  = default);
      Task<IEnumerable<OptionDto>> GetOptionByDeviceAsync(Guid guid,CancellationToken ct = default);
}