using System.Reflection;
using System.Text;

namespace SharedKernel.Helpers;

public static class ObjectHelper
{
    public static string ToAsciiString(this object obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var values = obj.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public)
            .OrderBy(f => f.MetadataToken)
            .Select(f => FormatValue(f.GetValue(obj)));

        return string.Join(" ", values.Where(v => !string.IsNullOrEmpty(v)));
    }

    private static string FormatValue(object? value)
    {
        if (value == null)
            return string.Empty;

        switch (value)
        {
            case char[] chars:
                return new string(chars);

            case int[] ints:
                return string.Concat(ints);

            case short[] shorts:
                return string.Concat(shorts);

            case byte[] bytes:
                return string.Concat(bytes);

            case Array array:
            {
                var sb = new StringBuilder();

                foreach (var item in array)
                {
                    sb.Append(item);
                }

                return sb.ToString();
            }

            default:
                return value.ToString() ?? string.Empty;
        }
    }
}