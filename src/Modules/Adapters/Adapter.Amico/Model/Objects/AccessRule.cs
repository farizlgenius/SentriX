namespace Adapter.Amico.Model.Objects;

public sealed record AccessRule(
      string name,
      int type,
      int priority=0
);