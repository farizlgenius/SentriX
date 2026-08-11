using Core.Contract.DTOs.Feature;

namespace Core.Application.Interfaces;

public interface IFeatureRepository
{
      Task<IEnumerable<FeatureDto>> GetAsync(CancellationToken ct = default);
}