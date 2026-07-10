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

       public static int ConvertTimeToEndMinute(string timeString)
      {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes;
      }
}