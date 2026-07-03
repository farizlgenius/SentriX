namespace Adapter.Amico.Model.Response;

public sealed record DeviceInfoResponse(
      UpTime UpTime=default!,
      int Time=0,
      Memory Memory=default!,
      License License=default!,
      Network Network=default!,
      string Serial="",
      string Version="",
      string DeviceId="",
      string SecBoxVersion="",
      string IDCloudCode="",
      bool Online=false,
      bool OnlineAvaiable=false
);

public sealed record UpTime(
      int Days=0,
      int Hours=0,
      int Minutes=0,
      int Seconds=0
);

public sealed record Memory(
      Disk Disk,
      Ram Ram
);

public sealed record Disk(
      int Free,
      int Total
);

public sealed record Ram(
      int Free,
      int Total
);

public sealed record License(
      int Users,
      int Device,
      int Type
);

public sealed record Network(
      string Mac,
      string Ip,
      string Netmask,
      string Gateway,
      int WebServerPort,
      bool SslEnabled,
      bool DhcpEnabled,
      bool TenMbps,
      string DnsPrimary,
      string DnsSecondary
);