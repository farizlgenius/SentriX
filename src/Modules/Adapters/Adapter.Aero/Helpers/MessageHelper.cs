using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Adapter.Aero.Enums;

namespace Adapter.Aero.Helpers;

public sealed class MessageHelper
{
  public static byte[] Serialize<T>(T obj)
        => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

  public static T Deserialize<T>(byte[] body)
      {
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Deserialized message: {message}");
            return JsonSerializer.Deserialize<T>(message)!;
      }

      public static string CommandSuccess(WriterType type,short ScpId) => $"{type.ToString()} on ScpId {ScpId}  - Successfully.";
      public static string CommandUnsuccess(WriterType type,short ScpId) => $"{type.ToString()} on ScpId {ScpId}  - Unsuccessfully.";


           // ================= JSON STYLE =================
    public static string ToJsonString(object? obj)
    {
        return ToJsonInternal(obj, 0, new HashSet<object>());
    }

    private static string ToJsonInternal(object? obj, int indent, HashSet<object> visited)
    {
        if (obj == null)
            return "null";

        var type = obj.GetType();

        if (IsSimple(type))
            return $"\"{obj}\"";

        // prevent infinite recursion
        if (!type.IsValueType)
        {
            if (visited.Contains(obj))
                return "\"<circular reference>\"";

            visited.Add(obj);
        }

        // arrays / lists
        if (obj is IEnumerable enumerable && type != typeof(string))
        {
            var items = new List<string>();
            foreach (var item in enumerable)
                items.Add(ToJsonInternal(item, indent + 1, visited));

            return "[ " + string.Join(", ", items) + " ]";
        }

        // complex object
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var sb = new StringBuilder();
        sb.Append("{ ");

        var pairs = new List<string>();
        foreach (var prop in props)
        {
            var value = prop.GetValue(obj);
            var jsonValue = ToJsonInternal(value, indent + 1, visited);
            pairs.Add($"\"{prop.Name}\" : {jsonValue}");
        }

        sb.Append(string.Join(", ", pairs));
        sb.Append(" }");

        return $"[{type.Name}] " + sb.ToString();
    }

    // ================= DEBUG STRING STYLE =================
    // public static string ToString(object? obj)
    // {
    //     return ToStringInternal(obj, 0, new HashSet<object>());
    // }

    public static string ToString(object? obj)
    {
        if (obj == null)
            return string.Empty;

        var values = new List<string>();
        BuildString(obj, values);

        return string.Join(" ", values);
    }

    private static void BuildString(object obj, List<string> values)
    {
        if (obj == null) return;

        // string → treat as primitive
        if (obj is string str)
        {
            values.Add(str);
            return;
        }

        // IEnumerable (array / list) but NOT string
        if (obj is IEnumerable enumerable && !(obj is string))
        {
            var arrayValues = new List<string>();

            foreach (var item in enumerable)
            {
                if (item == null) continue;

                if (IsSimple(item.GetType()))
                    arrayValues.Add(item.ToString());
                else
                    BuildString(item, arrayValues); // nested objects inside array
            }

            // join WITHOUT spaces → 1234
            if (arrayValues.Count > 0)
                values.Add(string.Join("", arrayValues));

            return;
        }

        // primitive / simple types
        if (IsSimple(obj.GetType()))
        {
            values.Add(obj.ToString());
            return;
        }

        // complex object → loop properties
        var props = obj.GetType()
                       .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            BuildString(value, values);
        }
    }

    private static bool IsSimple(Type type)
    {
        return type.IsPrimitive
            || type.IsEnum
            || type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(Guid);
    }    

    private static string ToStringInternal(object? obj, int indent, HashSet<object> visited)
    {
        if (obj == null)
            return "null";

        var type = obj.GetType();

        if (IsSimple(type))
            return obj.ToString() ?? "";

        if (!type.IsValueType)
        {
            if (visited.Contains(obj))
                return "<circular reference>";

            visited.Add(obj);
        }

        // arrays
        if (obj is IEnumerable enumerable && type != typeof(string))
        {
            var items = new List<string>();
            foreach (var item in enumerable)
                items.Add(ToStringInternal(item, indent + 1, visited));

            return "[ " + string.Join(", ", items) + " ]";
        }

        // complex object
        var sb = new StringBuilder();
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        sb.Append($"[{type.Name}] ");

        var pairs = new List<string>();
        foreach (var prop in props)
        {
            var value = prop.GetValue(obj);
            var text = ToStringInternal(value, indent + 1, visited);
            pairs.Add($"{prop.Name}: {text}");
        }

        sb.Append("{ " + string.Join(", ", pairs) + " }");

        return sb.ToString();
    }



}
