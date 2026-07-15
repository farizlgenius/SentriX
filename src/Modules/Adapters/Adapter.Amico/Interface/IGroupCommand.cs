using Adapter.Amico.Interfaces;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Interface;

public interface IGroupCommand : IBaseCommand
{
      Task<CreateObjectResponse> AccessRules(string ip,string session);
}