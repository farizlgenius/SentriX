using System;

namespace Device.Contract.DTOs;

public sealed class CreateModuleDto
{
      public int Module_id { get; set; }
      public string Mac { get; set; } = string.Empty;
      public short Model { get; set; }
      public short Port { get; set; }
      public short Address { get; set; }
      public int DeviceId { get; set; }
      public int LocationId {get; set;}

}
