using Adapter.Contract.Interfaces;
using SharedKernel.Enums;

namespace Aero.Infrastructure.Adapter;

public sealed class AeroAdapter : IAdapter
{
      public Vendor Vendor => Vendor.aero;

      public IDeviceAdapter Device { get; }
      public IUtilityAdapter Utility {get;}
      public ITimeAdapter Time { get; }

      //public IInputAdapter Input { get; }

      //public IOutputAdapter Output { get; }

      
      public IDoorAdapter Door { get; }
      // public IGroupAdapter Group { get; }

      // public IUserAdapter User { get; }
      // public ISettingAdapter Setting { get; }

      public AeroAdapter(
            IDeviceAdapter devices,
            IUtilityAdapter utility,
            //IOutputAdapter output,
            //IInputAdapter input,
            ITimeAdapter time,
            IDoorAdapter door
            // IAeroGroupAdapter group,
            // IAeroUserAdapter user,
            // IAeroSettingAdapter setting
      )
      {
            Device = devices;
            Utility = utility;
            //Output = output;
            //Input = input;
            Time = time;
            Door = door;
            // Group = group;
            // User = user;
            // Setting = setting;
      }
}
