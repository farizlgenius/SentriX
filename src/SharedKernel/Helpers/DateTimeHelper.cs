using System.Globalization;
using SharedKernel.Model;

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

       public static DateTime StringToDate(string time)
      {
            return DateTimeOffset.Parse(
                time,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind
            ).UtcDateTime;
      }

      public static List<DateObject> ExtractDateFromStartEndDateTime(DateTime Start,DateTime End)
      {
            var dates = new List<DateObject>();
            for(DateTime date = Start.Date; date <= End.Date;date = date.AddDays(1))
            {
                  dates.Add(new DateObject
                  {
                        Day=(short)date.Day,
                        Month=(short)date.Month,
                        Year=(short)date.Year
                  });
            }

            return dates;
      } 

      public static DateTime IntToDateTimeUTC(int unix)
      {
            return DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;
      }
}