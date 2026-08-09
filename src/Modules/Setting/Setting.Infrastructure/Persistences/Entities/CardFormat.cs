using SharedKernel.Domain;

namespace Setting.Infrastructure.Persistences.Entities;

public sealed class CardFormat
{

      public string name { get; set; } = string.Empty;
      public short fac { get; set; }
      public short offset { get; set; }
      public short function_id { get; set; }
      public short flag { get; set; }
      public short bits { get; set; }
      public short pe_ln { get; set; }
      public short pe_loc { get; set; }
      public short po_ln { get; set; }
      public short po_loc { get; set; }
      public short fc_ln { get; set; }
      public short fc_loc { get; set; }
      public short ch_ln { get; set; }
      public short ch_loc { get; set; }
      public short ic_ln { get; set; }
      public short ic_loc { get; set; }

      public CardFormat()
      {
      }

      // public CardFormat(Domain.Entities.CardFormat domain) : base(domain.ComponentId, domain.LocationId, domain.IsActive, false)
      // {
      //       this.name = domain.Name;
      //       this.fac = domain.Fac;
      //       this.offset = domain.Offset;
      //       this.function_id = domain.FunctionId;
      //       this.flag = domain.Flag;
      //       this.bits = domain.Bits;
      //       this.pe_ln = domain.PeLn;
      //       this.pe_loc = domain.PeLoc;
      //       this.po_ln = domain.PoLn;
      //       this.po_loc = domain.PoLoc;
      //       this.fc_ln = domain.FcLn;
      //       this.fc_loc = domain.FcLoc;
      //       this.ch_ln = domain.ChLn;
      //       this.ch_loc = domain.ChLoc;
      //       this.ic_ln = domain.IcLn;
      //       this.ic_loc = domain.IcLoc;

      // }

      // public void Update(Domain.Entities.CardFormat domain)
      // {
      //       this.name = domain.Name;
      //       this.fac = domain.Fac;
      //       this.offset = domain.Offset;
      //       this.function_id = domain.FunctionId;
      //       this.flag = domain.Flag;
      //       this.bits = domain.Bits;
      //       this.pe_ln = domain.PeLn;
      //       this.pe_loc = domain.PeLoc;
      //       this.po_ln = domain.PoLn;
      //       this.po_loc = domain.PoLoc;
      //       this.fc_ln = domain.FcLn;
      //       this.fc_loc = domain.FcLoc;
      //       this.ch_ln = domain.ChLn;
      //       this.ch_loc = domain.ChLoc;
      //       this.ic_ln = domain.IcLn;
      //       this.ic_loc = domain.IcLoc;

      //       this.updated_at = DateTime.UtcNow;
      // }




}