namespace Core.Infrastructure.Persistences.Entities;

public sealed class OperatorLocation : BaseEntity
{
      public int operator_id {get; set;}
      public Operator @operator {get; set;} = default!;
      public int location_id {get; set;}
      public Location location {get; set;} = default!;

      public OperatorLocation(){}
      public OperatorLocation(int operator_id, int location_id)
      {
            if(operator_id == 0)
            {
                  this.location_id = location_id;
            }else if(location_id == 0)
            {
                  this.operator_id = operator_id;
            }else
            {
                  this.operator_id = operator_id;
                  this.location_id = location_id;
            }
      }
}