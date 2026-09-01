using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public sealed class Face : BaseDomain
{
      public string ImageName { get; private set; } = string.Empty;
      public Face(string ImageName)
      {
            ValidationHelper.IsNullOrEmpty(ImageName, nameof(this.ImageName));
            this.ImageName = ImageName;
      }

      public Face(
            Guid Guid,
            string ImageName
      ) : base(Guid)
      {
            ValidationHelper.IsNullOrEmpty(ImageName, nameof(ImageName));
            this.ImageName = ImageName;
      }
}