using Core.Contract.DTOs.Input;

namespace Core.Contract.Interfaces;

public interface IInput : IService<InputDto,CreateInputDto,UpdateInputDto>
{
      Task<bool> GetStatusAsync(Guid guid,CancellationToken ct = default);
      Task<bool> MaskAsync(Guid guid,CancellationToken ct = default);
      Task<bool> UnmaskAsync(Guid guid,CancellationToken ct = default);
}