using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class BaseDomain
{
  public Guid Guid { get; private set; }

  public BaseDomain()
  {
    this.Guid = Guid.NewGuid();
  }

  public BaseDomain(
    Guid Guid
  )
  {
    ValidationHelper.ValidateGuid(Guid, nameof(Guid));
    this.Guid = Guid;
  }

}