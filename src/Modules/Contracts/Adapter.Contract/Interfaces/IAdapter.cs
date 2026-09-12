using System;
using SharedKernel.Enums;


namespace Adapter.Contract.Interfaces;

public interface IAdapter
{
      Vendor Vendor {get;}
      IDeviceAdapter Device {get;}
      // IInputAdapter Monitor {get;}
      // IOutputAdapter Control {get;}
      // ITimeAdapter Time {get;}
      // IDoorAdapter Door {get;}
      // IGroupAdapter Group {get;}
      // IUserAdapter User {get;}
      // ISettingAdapter Setting {get;}
}
