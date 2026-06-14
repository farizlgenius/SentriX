using System;
using SharedKernel.Domain;

namespace User.Infratructure.Persistences.Entities;

public sealed class Credential : BaseEntity
{
      public short flag { get; set; }
      public long card_number {get; set;}
      public int issue_code {get; set;}
      public string pin {get; set;} = string.Empty;
      public short use_count {get; set;}
      public DateTime act_time {get; set;}
      public DateTime deact_time {get; set;}
      public int user_id {get; set;}
      public Users user {get; set;} = default!;
      public Credential(){}
      public Credential(Domain.Entities.Credential credential) : base(
            credential.ComponentId,
            credential.LocationId,
            credential.IsActive
      )
      {
            this.user_id = user_id;
            this.flag = credential.Flag;
            this.card_number = credential.CardNumber;
            this.issue_code = credential.IssueCode;
            this.pin = credential.Pin;
            this.use_count = credential.UseCount;
            this.act_time = credential.ActiveTime;
            this.deact_time = credential.DeactiveTime;

      }
       
}
