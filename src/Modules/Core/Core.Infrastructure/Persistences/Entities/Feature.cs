using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Feature : BaseEntity
{
      public string name { get; set; } = string.Empty;
      public Feature() { }
}