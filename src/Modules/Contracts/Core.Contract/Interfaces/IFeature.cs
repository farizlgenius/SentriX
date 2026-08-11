using Core.Contract.DTOs.Feature;

namespace Core.Contract.Interfaces;

public interface IFeature
{
      Task<IEnumerable<FeatureDto>> GetAsync(CancellationToken ct = default);
}