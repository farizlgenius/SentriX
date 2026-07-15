using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Response;

public sealed class LoadObjectResponse
{
      [JsonPropertyName("time_zones")]
      public List<TimeZone> TimeZones { get; set; } = [];

      [JsonPropertyName("holidays")]
      public List<Holiday> Holidays { get; set; } = [];

      [JsonPropertyName("users")]
      public List<User> Users { get; set; } = [];
}

public class TimeZone
{
      [JsonPropertyName("id")]
      public int Id { get; set; }
}

public class Holiday
{
      [JsonPropertyName("id")]
      public int Id { get; set; }
}


public class User
{
      [JsonPropertyName("id")]
      public int Id { get; set; }
}