using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;

public sealed record TimeZone(
      [property: JsonPropertyName("id")]
      int Id,
      [property: JsonPropertyName("name")]
      string Name
      );