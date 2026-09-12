using System;
using Adapter.Contract.Interfaces;
using SharedKernel.Enums;

namespace Adapter.Contract.Interfaces;

public interface IAdapterFactory
{
      IAdapter GetAdapter(Vendor vendor);
}
