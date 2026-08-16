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

      public async Task<bool> CheckKeyAsync()
      {
            if (!Directory.Exists(_paths.Keys))
                  Directory.CreateDirectory(_paths.Keys);

            var pubFile = Path.Combine(_paths.Keys, "pub.key");
            var priFile = Path.Combine(_paths.Keys, "pri.key");

            if (!File.Exists(pubFile)) File.Create(pubFile).Close();
            if (!File.Exists(priFile)) File.Create(priFile).Close();

            bool pubContent = new FileInfo(pubFile).Length > 0;
            bool priContent = new FileInfo(priFile).Length > 0;

            return pubContent || priContent;
      }

      public async Task SaveKeyAsync(byte[] pubData, byte[] priData)
      {
            if (!Directory.Exists(_paths.Keys))
                  Directory.CreateDirectory(_paths.Keys);

            var pubFile = Path.Combine(_paths.Keys, "pub.key");
            var priFile = Path.Combine(_paths.Keys, "pri.key");

            if (!File.Exists(pubFile)) File.Create(pubFile).Close();
            if (!File.Exists(priFile)) File.Create(priFile).Close();

            await File.WriteAllBytesAsync(pubFile, pubData);
            await File.WriteAllBytesAsync(priFile, priData);

      }

      public async Task<string> SaveUserAsync(byte[] data, string fileName)
      {
            if (!Directory.Exists(_paths.Users))
                  Directory.CreateDirectory(_paths.Users);

            var path = Path.Combine(_paths.Users, fileName);
            await File.WriteAllBytesAsync(path, data);

            return path;
      }

      public async Task<string> SaveUserAsync(Stream stream, string fileName)
      {
            if (!Directory.Exists(_paths.Users))
                  Directory.CreateDirectory(_paths.Users);

            var safeFileName = Path.GetFileName(fileName);

            var path = Path.Combine(
                _paths.Users,
                safeFileName);

            await using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await stream.CopyToAsync(fs);

            return path;
      }


      public async Task<string> SaveMapAsync(byte[] data, string fileName)
      {
            if (!Directory.Exists(_paths.Maps))
                  Directory.CreateDirectory(_paths.Maps);

            var path = Path.Combine(_paths.Maps, fileName);
            await File.WriteAllBytesAsync(path, data);

            return path;
      }

      public async Task<string> ReadKeyAsync(string fileName)
      {
            var path = Path.Combine(_paths.Keys, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Key file not found", fileName);

            if (new FileInfo(path).Length <= 0)
                  throw new FileNotFoundException("Key empty", fileName);

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
      }

      public async Task<byte[]> ReadByteKeyAsync(string fileName)
      {
            var path = Path.Combine(_paths.Keys, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Key file not found", fileName);

            if (new FileInfo(path).Length <= 0)
                  throw new FileNotFoundException("Key empty", fileName);

            return await File.ReadAllBytesAsync(path);
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

      public async Task<string> SaveCaptureAsync(byte[] data, string fileName)
      {
            if (!Directory.Exists(_paths.Captures))
                  Directory.CreateDirectory(_paths.Captures);

            var path = Path.Combine(_paths.Captures, fileName);
            await File.WriteAllBytesAsync(path, data);

            return path;
      }

      public async Task<Stream> ReadCaptureAsync(string fileName)
      {
            var path = Path.Combine(_paths.Captures, fileName);

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

      public async Task<string> ReadCaptureBase64Async(string fileName)
      {
            var path = Path.Combine(_paths.Captures, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", fileName);

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
      }

      public void DeleteKeyAsync(string fileName)
      {
            var path = Path.Combine(_paths.Captures, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Key file not found", fileName);

            File.Delete(path);
      }

      public void DeleteCaptureAsync(string fileName)
      {
            var path = Path.Combine(_paths.Captures, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Map file not found", fileName);

            File.Delete(path);
      }

      public async Task<string> SaveCaptureAsync(Stream stream, string fileName)
      {
            Directory.CreateDirectory(_paths.Captures);

            var safeFileName = Path.GetFileName(fileName);

            var path = Path.Combine(
                _paths.Captures,
                safeFileName);

            await using var fs = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true);

            await stream.CopyToAsync(fs);

            return path;
      }

      public async Task SaveLicenseAsync(byte[] data, string fileName)
      {
            if (!Directory.Exists(_paths.Licenses))
                  Directory.CreateDirectory(_paths.Licenses);

            var licFile = Path.Combine(_paths.Licenses, "license.lic");

            if (!File.Exists(licFile)) File.Create(licFile).Close();

            await File.WriteAllBytesAsync(licFile, data);
      }

      public async Task<string> ReadLicenseAsync(string fileName)
      {
            var path = Path.Combine(_paths.Keys, fileName);

            if (!File.Exists(path))
                  throw new FileNotFoundException("Key file not found", fileName);

            if (new FileInfo(path).Length <= 0)
                  throw new FileNotFoundException("Key empty", fileName);

            var bytes = await File.ReadAllBytesAsync(path);
            return Convert.ToBase64String(bytes);
      }
}