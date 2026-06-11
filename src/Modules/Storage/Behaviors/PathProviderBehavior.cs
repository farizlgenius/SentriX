using Storage.Interfaces;

namespace Storage.Behaviors;

public sealed class PathProviderBehavior : IFilePathProvider
{
      public string Users { get; }
      public string Maps { get; }

      public PathProviderBehavior()
      {
            // Runtime directory (where the app is running)
            var runtimeRoot = AppContext.BaseDirectory;

            Users = Path.Combine(runtimeRoot, "images", "users");
            Maps = Path.Combine(runtimeRoot, "images", "maps");

            Directory.CreateDirectory(Users);
            Directory.CreateDirectory(Maps);
      }
}