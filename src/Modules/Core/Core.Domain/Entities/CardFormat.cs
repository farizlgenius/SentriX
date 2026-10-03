using Core.Domain.Entities;
using SharedKernel.Domain;

namespace Core.Domain.Entities;

public sealed class CardFormat : BaseDomain
{
      public string Name { get; set; } = string.Empty;
      public short Fac { get; set; }
      public short Bits { get; set; }
      public short EvenParityLen { get; set; }
      public short EvenParityLoc { get; set; }
      public short OddParityLen { get; set; }
      public short OddParityLoc { get; set; }
      public short FacLen { get; set; }
      public short FacLoc { get; set; }
      public short CardNoLen { get; set; }
      public short CardNoLoc { get; set; }
      public short IssueCodeLen { get; set; }
      public short IssueCodeLoc { get; set; } 
      public string Metadata {get; set; } = string.Empty;



      public CardFormat(
            string name, 
            short fac, 
            short bits, 
            short peLn, 
            short peLoc, 
            short poLn, 
            short poLoc, 
            short fcLn, 
            short fcLoc, 
            short chLn, 
            short chLoc, 
            short icLn, 
            short icLoc,
            string metadata
            ) : base(
                  Guid.NewGuid()
            )
      {
            Name = name;
            Fac = fac;
            Bits = bits;
            EvenParityLen = peLn;
            EvenParityLoc = peLoc;
            OddParityLen = poLn;
            OddParityLoc = poLoc;
            FacLen = fcLn;
            FacLoc = fcLoc;
            CardNoLen = chLn;
            CardNoLoc = chLoc;
            IssueCodeLen = icLn;
            IssueCodeLoc = icLoc;
            Metadata = metadata;

      }
}