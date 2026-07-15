using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record UpdateObjectRequest<TObject>(
      [property: JsonPropertyName("object")]
      string Object,
      [property: JsonPropertyName("values")]
      List<TObject> Values,
      [property: JsonPropertyName("where")]
      object Where);