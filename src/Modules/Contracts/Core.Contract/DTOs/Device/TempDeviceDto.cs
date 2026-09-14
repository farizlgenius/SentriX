using System.Security.Cryptography.X509Certificates;
using SharedKernel.Enums;

namespace Core.Contract.DTOs.Device;

public sealed class TempDeviceDto
{
      public int Id {get; set;}
      public Guid Guid { get; set; }
      public int SerialNumber { get; set; }
      public string Mac { get; set; } = string.Empty;
      public Vendor Vendor { get; set; } 
      public string Firmware { get; set; } = string.Empty;
      public string Ip { get; set; }= string.Empty;
      public int Port { get; set; }

      public TempDeviceDto(){}
      public TempDeviceDto(
            Guid guid,
            int id,
            int serialNumber,
            string mac,
            Vendor vendor,
            string firmware,
            string ip,
            int port
      )
      {
            Id = id;
            Guid =guid;
            SerialNumber = serialNumber;
            Mac = mac;
            Vendor =vendor;
            Firmware = firmware;
            Ip = ip;
            Port = port;
      }
}