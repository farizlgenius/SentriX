using Core.Contract.DTOs.Door;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IDoor : IBase<DoorDto, CreateDoorDto, UpdateDoorDto>
{
      Task UploadAsync(CancellationToken ct = default);
      Task<Guid> CreateTemplateAsync(CreateTemplateDto dto,CancellationToken ct = default);
}