using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record DeleteObjectAllRequest(
      [property: JsonPropertyName("object")]
      string Objects
      );