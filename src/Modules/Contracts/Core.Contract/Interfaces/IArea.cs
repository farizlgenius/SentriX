using Core.Contract.DTOs.Area;

namespace Core.Contract.Interfaces;

public interface IArea : IService<AreaDto,CreateAreaDto,UpdateAreaDto>
{
    Task<bool> GetStatusAsync(Guid guid,CancellationToken ct = default);
}