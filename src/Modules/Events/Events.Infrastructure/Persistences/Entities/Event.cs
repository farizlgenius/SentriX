using System;
using SharedKernel.Domain;

namespace Events.Infrastructure.Persistences.Entities;

public sealed class Event : BaseEntity
{
      public DateTime timestamp {get; set;}
      public string actor {get; set;} = string.Empty;
      public string module {get; set;} = string.Empty;
      public string type {get; set;} = string.Empty;
      public string image {get; set;} = string.Empty;
      public string mac {get; set;} = string.Empty;
      public string name {get; set;} = string.Empty;
      public string code {get; set;} = string.Empty;
      public string remarks {get; set;} = string.Empty;
      public string capture_image { get; set; } = string.Empty;

      public Event(){}

      public Event(
            DateTime timestamp,
            string actor,
            string module,
            string type,
            string image,
            string mac,
            string name,
            int location_id,
            string code = "",
            string remarks = "",
            string capture_image=""
            
      ){
            this.timestamp =  timestamp;
            this.actor = actor;
            this.module = module;
            this.type = type;
            this.image = image;
            this.mac = mac;
            this.name = name;
            this.code = code;
            this.remarks = remarks;
            this.location_id = location_id;
            this.capture_image = capture_image;
      }


}
