using SharedKernel.Enums;

namespace Core.Contract.Interfaces;

public interface IUtility
{
     string DecodeCommandAsync(string ascii,Vendor vendor,CancellationToken ct = default);
     IReadOnlyList<object> DecodeCommandWithColorAsync(string ascii,Vendor vendor,CancellationToken ct = default);
}