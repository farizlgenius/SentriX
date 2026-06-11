using Storage.Contract.Interfaces;
using Storage.Interfaces;

namespace Storage.Behaviors;

public sealed class StorageBehavior : IStorage
{
      private readonly IFilePathProvider _paths;

      public StorageBehavior(IFilePathProvider paths)
      {
            _paths = paths;
      }

      public async Task<string> SaveUserAsync(byte[] data, string fileName)
      {
            var path = Path.Combine(_paths.Users, fileName);
            await File.WriteAllBytesAsync(path, data);

            return path;
      }

      public async Task<string> SaveUserAsync(Stream stream, string fileName)
      {
            Directory.CreateDirectory(_paths.Users);

            var path = Path.Combine(_paths.Users, fileName);

            await using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            await stream.CopyToAsync(fs);
            return path;
      }


      public async Task<string> SaveMapAsync(byte[] data, string fileName)
      {
            var path = Path.Combine(_paths.Maps, fileName);
            await File.WriteAllBytesAsync(path, data);

            return path;
      }

      public async Task<Stream> ReadUserAsync(string fileName)
      {
            var path = Path.Combine(_paths.Users, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("User file not found", fileName);

            Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            return await Task.FromResult(stream);
      }

      public async Task<string> ReadUserBase64Async(string fileName)
      {
            var path = Path.Combine(_paths.Users, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("User file not found", fileName);

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
      }

      public async Task<Stream> ReadMapAsync(string fileName)
      {
            var path = Path.Combine(_paths.Maps, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", fileName);

            Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            return await Task.FromResult(stream);
      }

      public async Task<string> ReadMapBase64Async(string fileName)
      {
            var path = Path.Combine(_paths.Maps, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", fileName);

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
      }

      public void DeleteUserAsync(string filename)
      {
            var path = Path.Combine(_paths.Users, filename);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", filename);

            File.Delete(path);
      }

      public void DeleteMapAsync(string filename)
      {
            var path = Path.Combine(_paths.Maps, filename);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", filename);

            File.Delete(path);
      }
}