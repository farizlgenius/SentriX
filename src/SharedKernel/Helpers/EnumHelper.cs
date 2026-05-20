namespace SharedKernel.Helpers;

public static class EnumHelper
{
      public static T ToEnum<T>(string value) where T : struct, Enum
      {
            return Enum.TryParse<T>(value, true, out var result)
                ? result
                : throw new ArgumentException($"Invalid value '{value}' for enum {typeof(T).Name}");
      }
}