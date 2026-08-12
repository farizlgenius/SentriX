namespace Setting.Application.Interfaces;

public interface IBaseRepository
{
  Task<bool> IsAnyByGuidAsync(Guid guid, CancellationToken ct = default);
}