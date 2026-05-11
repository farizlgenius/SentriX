using System;
using System.Globalization;

namespace SharedKernel.Helpers;

public sealed class StringHelper
{
  public static string ToCapital(string msg)
  {
    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
    string result = ti.ToTitleCase(msg);
    return result;
  }
}