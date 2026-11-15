using System.ComponentModel.DataAnnotations;
using Core.Domain.Entities;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ProcedureAction
{
      [Key]
      public int id {get; set;}
      public int procedure_id { get; set; }
      public Procedure procedure {get; set;} = default!;
      public int action_id {get; set;}
      public Action action {get; set; }= default!;
      public int sequence {get; set; }
      public ProcedureAction(){}
      public ProcedureAction(
            int actionId,
            int sequence
      )
      {
            action_id = actionId;
            this.sequence = sequence;
      }
}