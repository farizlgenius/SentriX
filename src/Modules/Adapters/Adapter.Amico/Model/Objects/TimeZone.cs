using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;

public sealed record TimeZone(
      [property: JsonPropertyName("id")]
      int id,
      [property: JsonPropertyName("name")]
      string name
      );