using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;

public sealed record TimeZone(
      [property: JsonPropertyName("name")]
      string name
      );