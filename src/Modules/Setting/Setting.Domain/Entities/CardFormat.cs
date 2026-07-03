using SharedKernel.Domain;

namespace Setting.Domain.Entities;

public sealed class CardFormat : BaseDomain
{
      public string Name { get; set; } = string.Empty;
      public short Fac { get; set; }
      public short Offset { get; set; }
      public short FunctionId { get; set; }
      public short Flag { get; set; }
      public short Bits { get; set; }
      public short PeLn { get; set; }
      public short PeLoc { get; set; }
      public short PoLn { get; set; }
      public short PoLoc { get; set; }
      public short FcLn { get; set; }
      public short FcLoc { get; set; }
      public short ChLn { get; set; }
      public short ChLoc { get; set; }
      public short IcLn { get; set; }
      public short IcLoc { get; set; } 
   


      public CardFormat(int Id,short ComponentId,string name, short fac, short offset, short functionId, short flag, short bits, short peLn, short peLoc, short poLn, short poLoc, short fcLn, short fcLoc, short chLn, short chLoc, short icLn, short icLoc,int locationId,bool isActive) : base(Id,ComponentId,locationId,isActive)
      {
            Name = name;
            Fac = fac;
            Offset = offset;
            FunctionId = functionId;
            Flag = flag;
            Bits = bits;
            PeLn = peLn;
            PeLoc = peLoc;
            PoLn = poLn;
            PoLoc = poLoc;
            FcLn = fcLn;
            FcLoc = fcLoc;
            ChLn = chLn;
            ChLoc = chLoc;
            IcLn = icLn;
            IcLoc = icLoc;

      }
}