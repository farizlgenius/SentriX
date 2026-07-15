using System.Text.Json.Serialization;

namespace Adapter.Amico.Model.Objects;

public sealed record AccessLog(
    [property: JsonPropertyName("id")]
    string Id,

    [property: JsonPropertyName("time")]
    string Time,

    [property: JsonPropertyName("event")]
    string Event,

    [property: JsonPropertyName("device_id")]
    string DeviceId,

    [property: JsonPropertyName("identifier_id")]
    string IdentifierId,

    [property: JsonPropertyName("user_id")]
    string UserId,

    [property: JsonPropertyName("portal_id")]
    string PortalId,

    [property: JsonPropertyName("identification_rule_id")]
    string IdentificationRuleId,

    [property: JsonPropertyName("card_value")]
    string CardValue,

    [property: JsonPropertyName("qrcode_value")]
    string QrcodeValue,

    [property: JsonPropertyName("pin_value")]
    string PinValue,

    [property: JsonPropertyName("confidence")]
    string Confidence,

    [property: JsonPropertyName("mask")]
    string Mask
);