using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Reader : BaseDomain
{
      public int ReaderNumber { get; private set; }
      public int ModuleId { get; private set; }

      public Reader(int id,short readerNumber,int moduleId,int locationId, bool IsActive) : base(id, 0, locationId, IsActive)
      {
            ValidationHelper.ValidateNotMinus(readerNumber,nameof(ReaderNumber));
            ValidationHelper.ValidateNotMinus(moduleId,nameof(ModuleId));
            this.ReaderNumber = readerNumber;
            this.ModuleId= moduleId;
      }
}