namespace Adapter.Amico.Model.Objects;

public sealed record AccessRule(
      int id,
      string name,
      int type,
      int priority=0
);