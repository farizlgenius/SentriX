using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;

public sealed record Holiday(
      // [property: JsonPropertyName("id")]
      // int Id,
      [property: JsonPropertyName("name")]
      string Name,
      [property: JsonPropertyName("start")]
      int Start,
      [property: JsonPropertyName("end")]
      int End,
      [property: JsonPropertyName("hol1")]
      int Hol1,
      [property: JsonPropertyName("hol2")]
      int Hol2,
      [property: JsonPropertyName("hol3")]
      int Hol3,
      [property: JsonPropertyName("repeats")]
      int Repeats
);