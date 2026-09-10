using Core.Application.Interfaces;
using Core.Contract.DTOs.Group;
using Core.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Messaging;

namespace Core.Application.Services;

public sealed class GroupService(IGroupRepository repo,
IMessageBus bus) : IGroup
{
    public Task<Guid> CreateAsync(CreateGroupDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Guid>> DeleteListAsync(IEnumerable<Guid> guids, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DisabledAsync(Guid guid, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EnabledAsync(Guid guid, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<GroupDto> GetByGuidAsync(Guid guid, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GroupDto>> GetByLocationAsync(Guid guid, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Pagination<GroupDto>> GetPaginationAsync(PaginationParams param, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> UpdateAsync(UpdateGroupDto dto, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}