using Adapter.Contract.Interfaces;
using Core.Contract.Interfaces;
using SharedKernel.Enums;

namespace Core.Application.Services;

public sealed class UtilityService(IAdapterFactory adapter) : IUtility
{
      public string DecodeCommandAsync(string ascii,Vendor vendor,CancellationToken ct = default)
      {
            return adapter.GetAdapter(vendor).Utility.DecodeCommand(ascii);;
      }

      public IReadOnlyList<object> DecodeCommandWithColorAsync(string ascii,Vendor vendor,CancellationToken ct = default)
      {
            return adapter.GetAdapter(vendor).Utility.DecodeCommandWithColor(ascii);;
      }
}