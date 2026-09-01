using System.ComponentModel.DataAnnotations;
using SharedKernel.Domain;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class Face : BaseEntity
{
      public int? user_id { get; set; }
      public User user { get; set; } = default!;

      public Face() { }

      public Face(Guid d) : base(d)
      { }

}