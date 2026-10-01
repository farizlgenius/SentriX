using System;
using SharedKernel.Enums;


namespace Adapter.Contract.Interfaces;

public interface IAdapter
{
      Vendor Vendor {get;}
      IDeviceAdapter Device {get;}
      IUtilityAdapter Utility {get;}
      IInputAdapter Input {get;}
      IOutputAdapter Output {get;}
      ITimeAdapter Time {get;}
      IDoorAdapter Door {get;}
      // IGroupAdapter Group {get;}
      // IUserAdapter User {get;}
      // ISettingAdapter Setting {get;}
}
