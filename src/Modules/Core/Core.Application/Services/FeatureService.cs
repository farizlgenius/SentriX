using Core.Application.Interfaces;
using Core.Contract.DTOs.Feature;
using Core.Contract.Interfaces;

namespace Core.Application.Services;

public sealed class FeatureService(IFeatureRepository repo) : IFeature
{
      public async Task<IEnumerable<FeatureDto>> GetAsync(CancellationToken ct = default)
      {
            return await repo.GetAsync(ct);
      }
}