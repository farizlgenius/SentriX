using System.Text.Json;
using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;



public class WebhookRequest
{
    [JsonPropertyName("object_changes")]
    public List<ObjectChange> ObjectChanges { get; set; } = [];
}

public class ObjectChange
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("values")]
    public JsonElement Values { get; set; }
}