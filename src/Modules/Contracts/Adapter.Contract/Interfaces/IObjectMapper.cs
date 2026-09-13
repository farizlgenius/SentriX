namespace Adapter.Contract.Interfaces;

public interface IObjectMapper
{
  TDestination Map<TDestination>(object source);
}