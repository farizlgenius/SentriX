using System;

namespace AeroAdapter.Application.Interfaces;

public interface IObjectMapper
{
    TDestination Map<TDestination>(object source);
}