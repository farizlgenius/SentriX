namespace Storage.Contract.Interfaces;

public interface IStorage
{
      Task<bool> CheckKeyAsync();
      Task SaveKeyAsync(byte[] pubData,byte[] priData);
      Task<string> SaveCaptureAsync(byte[] data,string fileName);
      Task<string> SaveUserAsync(byte[] data, string fileName);
      Task<string> SaveMapAsync(byte[] data, string fileName);
      Task<string> ReadKeyAsync(string fileName);
      Task<Stream> ReadCaptureAsync(string fileName);
      Task<Stream> ReadUserAsync(string fileName);
      Task<Stream> ReadMapAsync(string fileName);
      Task<string> ReadCaptureBase64Async(string fileName);
      Task<string> ReadMapBase64Async(string fileName);
      Task<string> ReadUserBase64Async(string fileName);
      Task<string> SaveUserAsync(Stream stream, string fileName);
      Task<string> SaveCaptureAsync(Stream stream, string fileName);
      void DeleteKeyAsync(string fileName);
      void DeleteCaptureAsync(string fileName);
      void DeleteUserAsync(string filename);
      void DeleteMapAsync(string filename);
}