using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record UpdateObjectRequest<TObject>(
      [property: JsonPropertyName("object")]
      string Object,
      List<TObject> values,
      object where);