namespace Storage.Contract.Interfaces;

public interface IStorage
{
      // License 
      Task SaveLicenseAsync(byte[] data, string fileName);
      Task<string> ReadLicenseAsync(string fileName);

      // Key
      Task<bool> CheckKeyAsync();
      Task<bool> CheckEncKeyAsync();
      Task SaveKeyAsync(byte[] pubData, byte[] priData);
      Task SaveEncKeyAsync(byte[] encPubData, byte[] encPriData);
      Task<string> ReadKeyAsync(string fileName);
      Task<byte[]> ReadByteKeyAsync(string fileName);
      void DeleteKeyAsync(string fileName);

      // Capture
      Task<string> SaveCaptureAsync(byte[] data, string fileName);
      Task<Stream> ReadCaptureAsync(string fileName);
      Task<string> ReadCaptureBase64Async(string fileName);
      Task<string> SaveCaptureAsync(Stream stream, string fileName);
      void DeleteCaptureAsync(string fileName);

      // User
      Task<string> SaveUserAsync(byte[] data, string fileName);
      Task<Stream> ReadUserAsync(string fileName);
      Task<string> ReadUserBase64Async(string fileName);
      Task<string> SaveUserAsync(Stream stream, string fileName);
      void DeleteUserAsync(string filename);

      // Map
      Task<string> SaveMapAsync(byte[] data, string fileName);
      Task<Stream> ReadMapAsync(string fileName);
      Task<string> ReadMapBase64Async(string fileName);
      void DeleteMapAsync(string filename);

}