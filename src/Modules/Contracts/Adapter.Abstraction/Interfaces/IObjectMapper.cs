using System;

namespace Adapter.Abstraction.Interfaces;

public interface IObjectMapper
{
    TDestination Map<TDestination>(object source);
}