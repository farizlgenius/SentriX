namespace Adapter.Contract.Interfaces;

public interface IUtilityAdapter
{
      string DecodeCommand(string ascii);
      IReadOnlyList<object> DecodeCommandWithColor(string ascii);
}