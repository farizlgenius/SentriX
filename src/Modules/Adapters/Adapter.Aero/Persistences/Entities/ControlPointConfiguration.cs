using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class ControlPointConfiguration : BaseEntity
{
      public int aero_id {get; set;}
      public Aeros aero {get; set;} = default!;
      public short cp_number { get; set; }
      public short sio_number { get; set; }
      public short op_number { get; set; }
      public short dflt_pulse { get; set; }  
      public int output_id {get; set;}

      public ControlPointConfiguration(){}

        public ControlPointConfiguration(int aero_id, short cp_number, short sio_number, short op_number, short dflt_pulse,int output)
      {
            this.aero_id = aero_id;
            this.cp_number = cp_number;
            this.sio_number = sio_number;
            this.op_number = op_number;
            this.dflt_pulse = dflt_pulse;
            this.output_id = output;
      }    
}