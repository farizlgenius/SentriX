namespace Adapter.Contract.Interfaces;

public interface IUserAdapter
{
     Task AddUserAsync(
      string mac, 
      string ip,
      short deviceId,
      int cardNo,
      short issueCode,
      string pin,
      List<short> alvl,
      short apbLoc,
      short useCount,
      DateTime actTime,
      DateTime dactTime,
      CancellationToken ct = default
      );

}