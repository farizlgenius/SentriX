using Adapter.Contract.Interfaces;
using SharedKernel.Enums;

namespace Adapter.Contract;

public sealed class AdaptorFactory : IAdapterFactory
{
      private readonly IEnumerable<IAdapter> _adapters;
      public AdaptorFactory(IEnumerable<IAdapter> adapters)
      {
            _adapters = adapters;
      }
      public IAdapter GetAdapter(Vendor vendor)
      {
            Console.WriteLine($"Requested vendor: '{vendor}'");

            foreach (var a in _adapters)
            {
                  Console.WriteLine(
                        $"Adapter: {a.GetType().Name}, Vendor: '{a.Vendor}', Match: {a.Vendor.Equals(vendor)}");
            }

            var adapter = _adapters.FirstOrDefault(a => a.Vendor == vendor);

            if (adapter == null)
                  throw new Exception($"Adapter for '{vendor}' not found");

            return adapter;
      }
}