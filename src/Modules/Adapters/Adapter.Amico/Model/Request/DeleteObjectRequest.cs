using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record DeleteObjectRequest(
      [property: JsonPropertyName("object")]
      string Objects,
      [property: JsonPropertyName("where")]
      object Where
      );