using Core.Domain.Entities;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ProcedureAction
{
      public int procedure_id { get; set; }
      public Procedure procedure {get; set;} = default!;
      public int action_id {get; set;}
      public Action action {get; set; }= default!;
      public ProcedureAction(){}
      public ProcedureAction(
            int actionId
      )
      {
            action_id = actionId;
      }
}