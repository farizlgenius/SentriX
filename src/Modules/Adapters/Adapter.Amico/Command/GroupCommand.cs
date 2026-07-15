using Adapter.Abstraction.Interfaces;
using Adapter.Amico.Interface;
using Adapter.Amico.Model.Response;

namespace Adapter.Amico.Command;

public sealed class GroupCommand(IAmicoSetting setting,IHttpClient client) : BaseCommand(client,setting),IGroupCommand
{

}