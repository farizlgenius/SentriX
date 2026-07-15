using SharedKernel.Domain;
using SharedKernel.Helpers;

namespace Device.Domain.Entities;

public sealed class Reader 
{
      public Guid Guid {get; set;}
      public int ReaderNumber { get; private set; }
      public Guid ModuleGuid { get; private set; }
      public int LocationId {get; private set;}

      public Reader(Guid guid,short readerNumber,Guid moduleGuid,int locationId) 
      {
            ValidationHelper.ValidateNotMinus(readerNumber,nameof(ReaderNumber));
            this.Guid = guid;
            this.ReaderNumber = readerNumber;
            this.ModuleGuid= moduleGuid;
      }
}