using System;
using SharedKernel.Enums;

namespace Adapter.Abstraction.Interfaces;

public interface IAdapterFactory
{
      IAdapter GetAdapter(Vendor vendor);
}
