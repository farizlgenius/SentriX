using System;
using System.Text;
using System.Text.Json;


namespace SharedKernel.Helpers;

public static class JsonHelper
{
      private static readonly JsonSerializerOptions Options = new()
      {
            PropertyNameCaseInsensitive = true
      };

      public static T? Deserialize<T>(string json)
      {
            return JsonSerializer.Deserialize<T>(json, Options);
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
}