using System.Reflection;
using System.Text;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Helpers;

public static class ObjectHelper
{
    public static string ToAsciiString(this object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var values = new List<string>();

        // Only prepend the command number for the root object
        var apiCommand = obj.GetType().GetCustomAttribute<APICommandAttribute>();
        if (apiCommand != null)
        {
            values.Add(Convert.ToInt32(apiCommand.cmdNumber).ToString());
        }

        AppendObject(values, obj);

        // Filter out empty spaces and any stray null strings
        return string.Join(" ", values.Where(v => !string.IsNullOrWhiteSpace(v) && v != "\0"));
    }

    private static void AppendObject(List<string> values, object? obj)
    {
        if (obj == null)
            return;

        var type = obj.GetType();

        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
                                  .OrderBy(f => f.MetadataToken))
        {
            AppendValue(values, field.GetValue(obj));
        }
    }

    private static void AppendValue(List<string> values, object? value)
    {
        if (value == null)
            return;

        switch (value)
        {
            case string s:
                // Clean embedded null characters from C-style strings
                var cleanStr = s.Replace("\0", string.Empty);
                if (!string.IsNullOrWhiteSpace(cleanStr))
                    values.Add(cleanStr);
                return;

            case char[] chars:
                // Filter out null characters from fixed-length buffers
                values.AddRange(chars.Where(c => c != '\0').Select(c => c.ToString()));
                return;
                
            case char c when c == '\0':
                // Ignore individual null characters
                return;

            case Array array:
                foreach (var item in array)
                    AppendValue(values, item);
                return;
        }

        var type = value.GetType();

        // Primitive/value types
        if (type.IsPrimitive || type.IsEnum || value is decimal)
        {
            values.Add(value.ToString()!);
            return;
        }

        // Nested object
        AppendObject(values, value);
    }
}