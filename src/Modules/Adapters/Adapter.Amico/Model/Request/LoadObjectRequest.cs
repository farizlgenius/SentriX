using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record LoadObjectRequest(
    [property: JsonPropertyName("object")]
    string Object,

    [property: JsonPropertyName("fields")]
    List<string> Fields
);