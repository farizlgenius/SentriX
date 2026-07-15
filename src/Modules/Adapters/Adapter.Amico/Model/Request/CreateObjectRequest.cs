using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record CreateObjectRequest<TObject>(
      [property: JsonPropertyName("object")]
      string Object,
      [property: JsonPropertyName("values")]
      List<TObject> Values);