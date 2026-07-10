namespace Adapter.Amico.Model.Objects;

public sealed record Holiday(
      string Name,
      int Start,
      int End,
      int Hol1,
      int Hol2,
      int Hol3,
      int Repeats
);