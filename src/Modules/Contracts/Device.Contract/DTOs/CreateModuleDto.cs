using System;

namespace Device.Contract.DTOs;

public sealed class CreateModuleDto
{
      public int Module_id { get; set; }
      public string Mac { get; set; } = string.Empty;
      public int Model { get; set; }
      public int Port { get; set; }
      public int Address { get; set; }
      public int DeviceId { get; set; }

}
