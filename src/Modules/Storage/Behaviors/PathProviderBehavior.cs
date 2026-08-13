using System.Text.RegularExpressions;
using Storage.Interfaces;

namespace Storage.Behaviors;

public sealed class PathProviderBehavior : IFilePathProvider
{
      public string Users { get; }
      public string Maps { get; }
      public string Operators {get;}
      public string Captures {get;}
      public string Keys {get;}

      public PathProviderBehavior()
      {
            // Runtime directory (where the app is running)
            var runtimeRoot = AppContext.BaseDirectory;

            Users = Path.Combine(runtimeRoot, "images", "users");
            Maps = Path.Combine(runtimeRoot, "images", "maps");
            Operators = Path.Combine(runtimeRoot,"images","operators");
            Captures = Path.Combine(runtimeRoot,"images","captures");
            Keys = Path.Combine(runtimeRoot,"key");

            Directory.CreateDirectory(Users);
            Directory.CreateDirectory(Maps);
            Directory.CreateDirectory(Operators);
            Directory.CreateDirectory(Captures);
            Directory.CreateDirectory(Keys);
      }
}