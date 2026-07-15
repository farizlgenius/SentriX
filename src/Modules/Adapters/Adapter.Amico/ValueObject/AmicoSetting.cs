using Adapter.Amico.Interface;

namespace Adapter.Amico.ValueObject;

public sealed class AmicoSetting : IAmicoSetting
{
      public bool Secure {get; set;} = false;

      public string Login {get; set;} = string.Empty;

      public string Password {get; set;} = string.Empty;

      public string DefaultLogin {get; set;} = string.Empty;

      public string DefaultPassword {get; set;} = string.Empty;
}