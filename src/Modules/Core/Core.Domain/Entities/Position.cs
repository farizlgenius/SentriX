namespace Core.Domain.Entities;

public sealed class Position : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public string Description {get; private set;} =string.Empty;
      public Guid DepartmentGuid {get; private set;} = default!;

      public Position(
            string Name,
            Guid DepartmentGuid
      )
      {
            this.Name = Name;
            this.DepartmentGuid = DepartmentGuid;
      }

      public Position(
            Guid Guid,
            string Name,
            Guid DepartmentGuid
      ) : base(Guid)
      {
            this.Name = Name;
            this.DepartmentGuid = DepartmentGuid;
      }
}