namespace Aero.Application.Interfaces;

public interface IBaseRepository
{
  public bool IsBypass { get; }
  public bool Send(short command, object cfg);
}