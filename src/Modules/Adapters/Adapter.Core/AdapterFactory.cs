using System;
using Adapter.Abstraction.Interfaces;


namespace Adapter.Core;

public sealed class AdapterFactory : IAdapterFactory
{
      private readonly IEnumerable<IAdapter> _adapters;

      public AdapterFactory(IEnumerable<IAdapter> adapters)
      {
            _adapters = adapters;
      }

      public IAdapter GetAdapter(string vendor)
      {
            Console.WriteLine($"Requested vendor: '{vendor}'");

            foreach (var a in _adapters)
            {
                  Console.WriteLine(
                        $"Adapter: {a.GetType().Name}, Vendor: '{a.Vendor}', Match: {a.Vendor.Equals(vendor)}");
            }

            var adapter = _adapters.FirstOrDefault(a =>
                  a.Vendor.Equals(vendor, StringComparison.OrdinalIgnoreCase));

            if (adapter == null)
                  throw new Exception($"Adapter for '{vendor}' not found");

            return adapter;
      }
}
