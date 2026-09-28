using Core.Contract.DTOs.Output;

namespace Core.Contract.Interfaces;

public interface IOutput : IBase<OutputDto, CreateOutputDto, UpdateOutputDto>
{
      Task UploadAsync(CancellationToken ct = default);
}