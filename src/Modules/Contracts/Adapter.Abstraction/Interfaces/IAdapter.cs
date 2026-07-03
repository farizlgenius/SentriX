using System;

namespace Adapter.Abstraction.Interfaces;

public interface IAdapter
{
      string Vendor {get;}
      IDeviceAdapter Device {get;}
      IInputAdapter Monitor {get;}
      IOutputAdapter Control {get;}
      ITimeAdapter Time {get;}
      IDoorAdapter Door {get;}
      IGroupAdapter Group {get;}
      IUserAdapter User {get;}
      ISettingAdapter Setting {get;}
}
