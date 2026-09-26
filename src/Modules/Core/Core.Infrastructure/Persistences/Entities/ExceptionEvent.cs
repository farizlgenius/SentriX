using System.ComponentModel.DataAnnotations;

namespace Core.Infrastructure.Persistences.Entities;

public sealed class ExceptionEvent 
{
      [Key]
      public int id { get; set; }
       public DateTime timestampe {get; set;}
       public string method {get; set;} = string.Empty;
      public string path {get; set; } = string.Empty;
      public string exception {get; set;} = string.Empty;
      public string inner_exception {get; set; }= string.Empty;
      public string stack_trace {get; set; }= string.Empty;

      public ExceptionEvent(){}
      public ExceptionEvent(
            string method,
            string path,
            string exception,
            string innerException,
            string stackTrace
      )
      {
            timestampe = DateTime.UtcNow;
            this.method = method;
            this.path = path;
            this.exception = exception;
            this.inner_exception = innerException;
            this.stack_trace =stackTrace; 
      }
}