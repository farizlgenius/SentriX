using SharedKernel.Helpers;

namespace User.Domain.Entities;

public sealed class Face
{
      public Guid Guid { get; private set; }
      public string ImageName { get; private set;  } = string.Empty;
      public Guid UserGuid { get; private set; }

      public Face(){}

      public Face(
            Guid guid,
            string imageName,
            Guid userGuid
            )
      {
            ValidationHelper.ValidateGuid(guid,nameof(Guid));
            ValidationHelper.ValidateGuid(userGuid,nameof(UserGuid));
            ValidationHelper.IsNullOrEmpty(imageName,nameof(ImageName));
            this.Guid = guid;
            this.ImageName = imageName;
            this.UserGuid = userGuid;
      }

}