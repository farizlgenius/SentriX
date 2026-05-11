using System;
using System.Text.RegularExpressions;

namespace SharedKernel.Helpers;

public static partial class RegexHelper
    {
        [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex EmailRegex();

        [GeneratedRegex(@"^[a-zA-Z0-9]*$")]
        public static partial Regex CharAndDigitRegex();

        [GeneratedRegex(@"^[\p{L}\p{M}\p{N} ()]+$")]
        private static partial Regex NameRegex();



        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex().IsMatch(email);
        }

        public static bool IsValidOnlyCharAndDigit(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return CharAndDigitRegex().IsMatch(name);
        }

        public static bool IsValidName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return NameRegex().IsMatch(name);
        }


    }