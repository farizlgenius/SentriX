using System;
using SharedKernel.Domain;

namespace Adapter.Aero.Persistences.Entities;

public sealed class Aeros : BaseEntity
{
      public int scp_id {get; set;}
      public string mac {get; set;} = string.Empty;
      public ICollection<DriverConfiguration> driver_configurations {get; set;} = default!;
      public ICollection<SioPanelConfiguration> sio_panel_configurations {get; set;} = default!;
      public ICollection<InputPointSpecification> input_point_specifications {get; set;} = default!;
      public ICollection<OutputPointSpecification> output_point_specifications {get; set;} = default!;
      public ICollection<ControlPointConfiguration> control_point_configurations {get; set;} = default!;

      public Aeros(){}
      public Aeros(int scp_id,string mac,int location_id)
      {
            this.scp_id = scp_id;
            this.mac = mac;
            this.location_id = location_id;
            this.updated_at = DateTime.UtcNow;
            this.created_at = DateTime.UtcNow;
      }
      

}
