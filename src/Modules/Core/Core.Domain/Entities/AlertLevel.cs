using SharedKernel.Enums;

namespace Core.Domain.Entities;

public sealed class AlertLevel : BaseDomain
{
      public string Name { get; set; } = string.Empty;
      public string ColorCode {get; set; } = string.Empty;

      public AlertLevel(
            Guid guid,
            string name,
            string colorCode
      ) : base(guid)
      {
            Name = name;
            ColorCode = colorCode;
      }
      public AlertLevel(
            string name,
            string colorCode
      ) : base(Guid.NewGuid())
      {
            Name = name;
            ColorCode = colorCode;
      }
}

