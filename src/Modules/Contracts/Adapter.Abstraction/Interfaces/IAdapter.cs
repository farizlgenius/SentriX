using System;

namespace Adapter.Abstraction.Interfaces;

public interface IAdapter
{
      string Vendor {get;}
      IDeviceAdapter Device {get;}
      IMonitorAdapter Monitor {get;}
      IControlAdapter Control {get;}
}
