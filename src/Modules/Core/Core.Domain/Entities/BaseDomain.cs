using SharedKernel.Helpers;

namespace Core.Domain.Entities;

public class BaseDomain 
{
  public Guid Guid { get; private set; }
  public short ExternalId { get; private set; }
  public short AltrExternalId {get; private set; }

  public BaseDomain()
  {
    this.Guid = Guid.NewGuid();
  }

  public BaseDomain(
    Guid Guid,
    short ExternalId  = -1,
    short AltrExternalId = -1
  )
  {
    ValidationHelper.GuidEmpty(Guid, nameof(Guid));
    this.Guid = Guid;
    this.ExternalId = ExternalId;
    this.AltrExternalId = AltrExternalId;
  }


}