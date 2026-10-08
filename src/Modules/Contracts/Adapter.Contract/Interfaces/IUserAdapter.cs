namespace Adapter.Contract.Interfaces;

public interface IUserAdapter
{
     Task AddUserAsync(
      string mac, 
      string ip,
      short deviceId,
      int cardNo,
      string pin,
      List<short> alvl,
      DateTime actTime,
      DateTime dactTime,
      string metadata,
      CancellationToken ct = default
      );

      Task DeleteUserAsync(string mac,string ip,short deviceId,int cardNumber,CancellationToken ct = default);

}