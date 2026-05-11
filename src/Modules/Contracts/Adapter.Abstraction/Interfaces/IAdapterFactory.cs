using System;

namespace Adapter.Abstraction.Interfaces;

public interface IAdapterFactory
{
      IAdapter GetAdapter(string vendor);
}
