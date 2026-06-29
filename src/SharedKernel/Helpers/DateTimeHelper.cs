namespace SharedKernel.Helpers;

public static class DateTimeHelper
{
      public static long DateTimeToElapeSecond(string date)
      {
            if (date.Equals("") || date.Equals(null)) return 0;

            DateTimeOffset dto = DateTimeOffset.Parse(date);

            return dto.ToUnixTimeSeconds();
      }

      public static long DateTimeToElapeSecond(DateTime date)
      {
            DateTimeOffset dto = new DateTimeOffset(date);
            return dto.ToUnixTimeSeconds();
      }
}