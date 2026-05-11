using System;

namespace Adapter.Aero.Entities;

public sealed class IdReport
{
      public int ScpId { get; private set; }
      public string SerialNumber { get; private set; } = string.Empty;
      public string Mac { get; private set; } = string.Empty;
      public string Ip { get; private set; } = string.Empty;
      public int Port { get; private set; }
      public string Fw { get; private set; } = string.Empty;

      public IdReport(int scpId, string serialNumber, string mac, string ip, int port, string fw)
      {
            ScpId = scpId;
            SerialNumber = serialNumber;
            Mac = mac;
            Ip = ip;
            Port = port;
            Fw = fw;
      }

      public void UpdateIp(string ip)
      {
            this.Ip = ip;
      }

      public void UpdatePort(int port)
      {
            this.Port = port;
      }
}
