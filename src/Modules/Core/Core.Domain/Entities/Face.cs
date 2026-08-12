using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Face : BaseDomain
{
      public string ImageName { get; private set; } = string.Empty;
      public Guid UserGuid { get; private set; } = default!;
      public Face(string ImageName,
            Guid UserGuid)
      {
            ValidationHelper.IsNullOrEmpty(ImageName, nameof(this.ImageName));
            ValidationHelper.GuidEmpty(UserGuid, nameof(this.UserGuid));
            this.ImageName = ImageName;
            this.UserGuid = UserGuid;
      }

      public Face(
            Guid Guid,
            string ImageName,
            Guid UserGuid
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(ImageName, nameof(ImageName));
            ValidationHelper.GuidEmpty(UserGuid, nameof(UserGuid));
            this.ImageName = ImageName;
            this.UserGuid = UserGuid;
      }
}