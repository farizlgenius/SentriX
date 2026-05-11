using System;
using Adapter.Abstraction.Interfaces;

namespace Adapter.Abstraction;

public sealed class AdapterFactory : IAdapterFactory
{
      private readonly IEnumerable<IAdapter> _adapters;

      public AdapterFactory(IEnumerable<IAdapter> adapters)
      {
            _adapters = adapters;
      }

      public IAdapter GetAdapter(string vendor)
      {
            var adapter = _adapters.FirstOrDefault(a => a.Vendor == vendor);

            if (adapter == null)
                  throw new Exception($"Adapter for '{vendor}' not found");

            return adapter;
      }
}
