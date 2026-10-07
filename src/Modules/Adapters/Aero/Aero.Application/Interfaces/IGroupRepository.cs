using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IGroupRepository 
{
      CommandResponse AddAccessGroup(string mac, short scpId, short groupId, short operMode, List<(short doorId, short timeZoneId)> datas);
}