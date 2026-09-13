using Aero.Application.Interfaces;

namespace Aero.Application.ValueObjects;

public sealed class CreateChannel : ICreateChannel
{
  public int nChannelId { get; set; }
  public int cType { get; set; }
  public int cPort { get; set; }
}