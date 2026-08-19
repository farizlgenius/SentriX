using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;


namespace SharedKernel.Helpers;

public static class JsonHelper
{
      private static readonly JsonSerializerOptions Options = new()
      {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      };



      public static T Deserialize<T>(string json)
      {
            return JsonSerializer.Deserialize<T>(json, Options) ?? throw new Exception($"Deserialized invalid:{nameof(T)}");
      }

      public static T? Deserialize<T>(JsonElement element)
      {
            return element.Deserialize<T>(Options);
      }

      public static string Serialize<T>(T value)
      {
            return JsonSerializer.Serialize(value, Options);
      }

      public static JsonElement ToJsonElement(string json)
      {
            using var document = JsonDocument.Parse(json);
            return document.RootElement.Clone();
      }

      public static JsonElement ToJsonElement<T>(T obj)
      {
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(obj, Options);
            using var doc = JsonDocument.Parse(bytes);
            return doc.RootElement.Clone();
      }
}