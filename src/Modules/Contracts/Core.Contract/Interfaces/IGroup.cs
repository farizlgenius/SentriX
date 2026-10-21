using Core.Contract.DTOs.Group;

namespace Core.Contract.Interfaces;

public interface IGroup : IService<GroupDto, CreateGroupDto, UpdateGroupDto>
{
      Task UploadAsync(CancellationToken ct = default);
}