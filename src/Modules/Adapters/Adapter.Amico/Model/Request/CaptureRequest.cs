using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Request;

public sealed record CaptureRequest(
      [property:JsonPropertyName("frame_type")]
      string FrameType,
      [property:JsonPropertyName("camera")]
      string Camera
);