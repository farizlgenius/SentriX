using Adapter.Amico.Interface;

namespace Adapter.Amico.ValueObject;

public sealed class AmicoSetting : IAmicoSetting
{
      public bool Secure {get; set;} = false;
}