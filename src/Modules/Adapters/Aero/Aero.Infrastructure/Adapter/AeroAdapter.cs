using Adapter.Contract.Interfaces;
using Core.Contract.Interfaces;
using SharedKernel.Enums;

namespace Aero.Infrastructure.Adapter;

public sealed class AeroAdapter : IAdapter
{
      public Vendor Vendor => Vendor.aero;

      public IDeviceAdapter Device { get; }
      public IModuleAdapter Module {get;}
      public IUtilityAdapter Utility {get;}
      public ITimeAdapter Time { get; }

      public IInputAdapter Input { get; }

      public IOutputAdapter Output { get; }

      
      public IDoorAdapter Door { get; }

      public IGroupAdapter Group { get; }


      public IUserAdapter User { get; }
      public IAreaAdapter Area {get; }
      public IMonitorGroupAdapter MonitorGroup {get; }
      public ITriggerAdapter Trigger {get;}
      public IProcedureAdapter Procedure {get; }

      public AeroAdapter(
            IDeviceAdapter devices,
            IModuleAdapter module,
            IUtilityAdapter utility,
            IOutputAdapter output,
            IInputAdapter input,
            ITimeAdapter time,
            IDoorAdapter door,
            IGroupAdapter group,
            IUserAdapter user,
            IAreaAdapter area,
            IMonitorGroupAdapter monitorGroup,
            ITriggerAdapter trigger,
            IProcedureAdapter procedure
            // IAeroSettingAdapter setting
      )
      {
            Device = devices;
            Module = module;
            Utility = utility;
            Output = output;
            Input = input;
            Time = time;
            Door = door;
            Group = group;
            User = user;
            Area = area;
            MonitorGroup = monitorGroup;
            Trigger = trigger;
            Procedure = procedure;
            // Setting = setting;
      }
}
