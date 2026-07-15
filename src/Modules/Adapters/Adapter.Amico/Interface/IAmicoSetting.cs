namespace Adapter.Amico.Interface;

public interface IAmicoSetting
{
      public bool Secure { get;}
      public string Login { get;  }
      public string Password { get;  }
      public string DefaultLogin {get;}
      public string DefaultPassword {get;}
}