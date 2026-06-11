namespace Storage.Contract.Interfaces;

public interface IStorage
{
      Task<string> SaveUserAsync(byte[] data, string fileName);
      Task<string> SaveMapAsync(byte[] data, string fileName);
      Task<Stream> ReadUserAsync(string fileName);
      Task<Stream> ReadMapAsync(string fileName);
      Task<string> ReadMapBase64Async(string fileName);
      Task<string> ReadUserBase64Async(string fileName);
      Task<string> SaveUserAsync(Stream stream, string fileName);
      void DeleteUserAsync(string filename);
      void DeleteMapAsync(string filename);
}