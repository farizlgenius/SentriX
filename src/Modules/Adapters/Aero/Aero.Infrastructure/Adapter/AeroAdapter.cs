using Adapter.Contract.Interfaces;
using SharedKernel.Enums;

namespace Aero.Infrastructure.Adapter;

public sealed class AeroAdapter : IAdapter
{
      public Vendor Vendor => Vendor.aero;

      public IDeviceAdapter Device { get; }
      public IUtilityAdapter Utility {get;}

      // public IInputAdapter Monitor { get; }

      // public IOutputAdapter Control { get; }

      // public ITimeAdapter Time { get; }
      // public IDoorAdapter Door { get; }
      // public IGroupAdapter Group { get; }

      // public IUserAdapter User { get; }
      // public ISettingAdapter Setting { get; }

      public AeroAdapter(
            IDeviceAdapter devices,
            IUtilityAdapter utility
            // IAeroOutputAdapter controls,
            // IAeroInputAdapter monitor,
            // IAeroTimeAdapter time,
            // IAeroDoorAdapter door,
            // IAeroGroupAdapter group,
            // IAeroUserAdapter user,
            // IAeroSettingAdapter setting
      )
      {
            Device = devices;
            Utility = utility;
            // Control = controls;
            // Monitor = monitor;
            // Time = time;
            // Door = door;
            // Group = group;
            // User = user;
            // Setting = setting;
      }
}
