using Device.Application.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Messaging;

namespace Device.Application.Handler;

public sealed class ModelByModuleIdQueryHandler(IDeviceRepository repo) : IQueryHandler<ModelByModuleIdQuery, string>
{
      public async Task<string> HandleAsync(ModelByModuleIdQuery query, CancellationToken ct)
      {
            return await repo.GetModelByModuleIdAsync(query.moduleId);
      }
}