namespace Core.Domain.Entities;

public sealed class LicensePlate : BaseDomain
{
      public string LicensePlates {get; private set;} = string.Empty;
      public Guid UserGuid {get; private set;}
      public LicensePlate(
            string Plate,
            Guid UserGuid
      )
      {
            this.LicensePlates = Plate;
            this.UserGuid = UserGuid;
      }
      public LicensePlate(
            Guid Guid,
            string Plate,
            Guid UserGuid
            ) : base(Guid)
      {
             this.LicensePlates = Plate;
            this.UserGuid = UserGuid;
      }
}