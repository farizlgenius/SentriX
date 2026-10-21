using Core.Contract.DTOs.Output;

namespace Core.Contract.Interfaces;

public interface IOutput : IService<OutputDto, CreateOutputDto, UpdateOutputDto>
{
      Task UploadAsync(CancellationToken ct = default);
}