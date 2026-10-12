namespace Core.Domain.Entities;

public sealed class Trigger : BaseDomain
{
      public string Name { get; private set; } = string.Empty;
      public Trigger(string name) : base(Guid.NewGuid())
      {
            Name = name;
      }

      public Trigger(
            Guid guid,
            string name
            ) : base(guid)
      {
            Name = name;
      }
}