namespace Adapter.Contract.Interfaces;

public interface IOutputAdapter
{
      Task Output(
            string mac,
            string ip,
            string metadata
      );

      Task Control(
            string mac,
            string ip,
            string metadata
      );
}