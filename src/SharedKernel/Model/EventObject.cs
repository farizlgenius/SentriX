using System.Text.Json.Serialization;

namespace SharedKernel.Model;

public sealed record EventObject(
      [property: JsonPropertyName("object_changes")]
      List<ObjectChanges> objectChanges,
     [property: JsonPropertyName("device_id")]
      string DeviceId
);
public sealed record ObjectChanges(
      [property: JsonPropertyName("object")]
      string Object,
      [property: JsonPropertyName("type")]
      string Type,
      [property: JsonPropertyName("values")]
      object Values
);