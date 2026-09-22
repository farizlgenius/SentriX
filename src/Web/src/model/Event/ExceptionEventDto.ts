
export interface ExceptionEventDto{
      timestamp:Date,
      path:string,
      exception:string,
      innerException:string;
      stackTrace:string;
}