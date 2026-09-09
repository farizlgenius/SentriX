namespace Core.Infrastructure.Persistences.Entities;

public sealed class Turnstile : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public ICollection<Lane> lanes { get; set; } = default!;
      public Turnstile() { }
}