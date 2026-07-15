using Adapter.Amico.Model.Objects;

namespace Adapter.Amico.Helper;


public static class TypeMapHelper
{
      public static readonly Dictionary<string, Type> TypeMap = new()
      {
            ["access_logs"] = typeof(AccessLog)
      };

}
