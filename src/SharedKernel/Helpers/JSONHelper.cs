using System;
using System.Text;
using System.Text.Json;


namespace SharedKernel.Helpers;

public static class JSONHelper
{
      public static T Deserialize<T>(string body)
      {
            Console.WriteLine($"Deserialized message: {body}");
            var options = new JsonSerializerOptions
            {
                  PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(body, options)!;
      }

      public static string Serialize<T>(T body)
      {
            Console.WriteLine($"Serialized message: {body}");
            var options = new JsonSerializerOptions
            {
                  PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Serialize(body,options)!;
      }
}