using System;
using System.Globalization;
using System.Net;
using System.Text;

namespace Adapter.Aero.Helpers;

public sealed class UtilitiesHelper
{
      public static long DateTimeToElapeSecond(string date)
      {
            if (date.Equals("") || date.Equals(null)) return 0;

            DateTimeOffset dto = DateTimeOffset.Parse(date);

            return dto.ToUnixTimeSeconds();
      }

      // public static string DaysInWeekToString(DaysInWeekDto days)
      // {
      //       var map = new Dictionary<string, bool>{
      //           {"Sun",days.Sunday },
      //           {
      //               "Mon",days.Monday
      //           },
      //           {
      //               "Tue",days.Tuesday
      //           },
      //           {
      //               "Wed",days.Wednesday
      //           },
      //           {
      //               "Thu",days.Thursday
      //           },
      //           {
      //               "Fri",days.Friday
      //           },
      //           {
      //               "Sat",days.Saturday
      //           }
      //       };

      //       return string.Join(",", map.Where(x => x.Value).Select(x => x.Key));
      // }


      // public static int ConvertDayToBinary(DaysInWeek days)
      // {
      //       int result = 0;
      //       result |= (days.Sunday ? 1 : 0) << 0;
      //       result |= (days.Monday ? 1 : 0) << 1;
      //       result |= (days.Tuesday ? 1 : 0) << 2;
      //       result |= (days.Wednesday ? 1 : 0) << 3;
      //       result |= (days.Thursday ? 1 : 0) << 4;
      //       result |= (days.Friday ? 1 : 0) << 5;
      //       result |= (days.Saturday ? 1 : 0) << 6;
      //       // Holiday
      //       //result |= 0 << 8;
      //       //result |= 0 << 9;
      //       //result |= 0 << 10;
      //       //result |= 0 << 11;
      //       //result |= 0 << 12;
      //       //result |= 0 << 13;
      //       //result |= 0 << 14;
      //       //result |= 0 << 15;
      //       return result;
      // }

      public static int ConvertTimeToEndMinute(string timeString)
      {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes;
      }

      public static string ByteToHexStr(byte[] data)
      {
            var reversed = data.Reverse().ToArray(); // Reverse the byte array
            return BitConverter.ToString(reversed).Replace("-","_");
      }

      public static string ByteToHex(byte[] data)
      {
            if (data == null || data.Length == 0) return string.Empty;

            byte[] trimmed = data.Reverse()
                                 .SkipWhile(b => b == 0x00)
                                 .Reverse()
                                 .ToArray();

            return BitConverter.ToString(trimmed).Replace("-", "");
      }

      public static string ByteToText(byte[] data)
      {
            return System.Text.Encoding.Default.GetString(data);
      }

      public static int HexStrToInt(string hex)
      {
            return Convert.ToInt32(hex, 16);
      }

      public static byte HexStrToByte(string hex)
      {
            return Convert.ToByte(hex, 16);
      }

      public static string ByteToTextThai(byte[] data)
      {
            Encoding thaiEncoding = Encoding.GetEncoding("windows-874"); // or "TIS-620" //windows-874
            return thaiEncoding.GetString(data);
      }

      public static byte[] TrimTrailingSpaces(byte[] data)
      {
            int i = data.Length - 1;
            while (i >= 0 && data[i] == 0x20) i--;
            return data.Take(i + 1).ToArray();
      }

      public static string CharToString(char[] chars)
      {
            return new string(chars);
      }

      public static DateTimeOffset UnixToDateTime(int timestamp)
      {
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
      }

      public static string[] UnixToDateTimeParts(int timestamp)
      {
            string[] d = new string[2];

            //var dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            var dateTime = new DateTime(1970, 1, 1).AddSeconds(timestamp).ToLocalTime();

            d[0] = dateTime.Date.ToString("yyyy-MM-dd");             // Only the date part (00:00:00 time)
            d[1] = dateTime.TimeOfDay.ToString(@"hh\:mm\:ss");        // Only the time part

            return d;

      }

      public static DateTime UnixToDateTimeUtc(int timestamp)
      {
            return DateTimeOffset
                .FromUnixTimeSeconds(timestamp)
                .UtcDateTime;
      }

      public static string ParseFirmware(short major, short minor)
      {
            string s = minor.ToString();

            return $"{major}.{s.Substring(0, s.Length - 1)}.{s[^1]}";
      }


      public static short[] GetSCPComponent(short model)
      {
            short nInput, nOutput, nReaders;
            switch (model)
            {
                  case 196:
                        nInput = 7;
                        nOutput = 4;
                        nReaders = 4;
                        break;
                  case 193:
                        nInput = 7;
                        nOutput = 4;
                        nReaders = 4;
                        break;
                  case 194:
                        nInput = 19;
                        nOutput = 2;
                        nReaders = 0;
                        break;
                  case 195:
                        nInput = 5;
                        nOutput = 12;
                        nReaders = 0;
                        break;
                  case 190:
                        nInput = 7;
                        nOutput = 4;
                        nReaders = 2;
                        break;
                  case 191:
                        nInput = 19;
                        nOutput = 2;
                        nReaders = 0;
                        break;
                  case 192:
                        nInput = 5;
                        nOutput = 12;
                        nReaders = 0;
                        break;
                  default:
                        nInput = 0;
                        nOutput = 0;
                        nReaders = 0;
                        break;
            }

            return [nInput, nOutput, nReaders];
      }


      public static string IntegerToIp(int ip)
      {
            //int ipAsInt = 3232235777; // Example: 192.168.1.1
            byte[] b = BitConverter.GetBytes(ip);
            Array.Reverse(b);
            IPAddress ipAddress = new IPAddress(b);

            // Convert to string (IPv4 format)
            string ipString = ipAddress.ToString();
            Console.WriteLine(ipString);
            return ipString;  // Strike: 192.168.1.1
      
      }

      public static int IpToInteger(string ipAdress)
      {
            var ip = IPAddress.Parse(ipAdress);
            byte[] b = ip.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
            {
                  Array.Reverse(b);
            }

            return BitConverter.ToInt32(b, 0);
      }

      public static bool IsBitSet(int value, int bit)
      {
            return (value & (1 << bit)) != 0;
      }

      public static byte[] Base64ToBytes(string base64)
      {
            if (string.IsNullOrWhiteSpace(base64))
                  throw new ArgumentException("Base64 string is empty");

            // Remove data URL prefix if present
            var commaIndex = base64.IndexOf(',');
            if (commaIndex >= 0)
                  base64 = base64[(commaIndex + 1)..];

            return Convert.FromBase64String(base64);
      }



      public static DateTime StringToDate(string time)
      {
            return DateTimeOffset.Parse(
                time,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind
            ).UtcDateTime;
      }

}
