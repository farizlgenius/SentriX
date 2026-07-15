using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Response;

public sealed record DeviceInfoResponse(
      UpTime UpTime=default!,
      int Time=0,
      [property: JsonPropertyName("daylight_savings_time_active")]
      bool DaylightSavingsTimeActive=false,
      Memory Memory=default!,
      License License=default!,
      Network Network=default!,
      string Serial="",
      string Version="",
      [property: JsonPropertyName("device_id")]
      string DeviceId="",
      [property: JsonPropertyName("secbox_version")]
      string SecBoxVersion="",
      [property: JsonPropertyName("iDCloud_code")]
      string IDCloudCode="",
      bool Online=false,
      bool OnlineAvaiable=false
);

public sealed record Biometrcs(
       [property: JsonPropertyName("max_num_records")]
      int MaxNumRecords,
       [property: JsonPropertyName("max_possible_num_records")]
      int MaxPossibleNumRecords
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
     
      [property: JsonPropertyName("web_server_port")]
      int WebServerPort,
      bool SslEnabled,
      bool DhcpEnabled,
      [property: JsonPropertyName("ten_mbps")]
      bool TenMbps,
      [property: JsonPropertyName("primary_dns")]
      string DnsPrimary,
      [property: JsonPropertyName("secondary_dns")]
      string DnsSecondary
);