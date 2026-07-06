using System.Reflection;
using System.Text;
using HID.Aero.ScpdNet.Wrapper;

namespace Adapter.Aero.Helpers;

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

        return string.Join(" ", values.Where(v => !string.IsNullOrWhiteSpace(v)));
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
                values.Add(s);
                return;

            case char[] chars:
                values.AddRange(chars.Select(c => c.ToString()));
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