namespace Adapter.Amico.Model.Objects;

public sealed record TimeSpan(
      int TimeZoneId,
      int Start,
      int End,
      int Sun,
      int Mon,
      int Tue,
      int Wed,
      int Thu,
      int Fri,
      int Sat,
      int Hol1,
      int Hol2,
      int Hol3
);