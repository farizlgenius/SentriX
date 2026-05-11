namespace Adapter.Aero.Enums;

public enum WriterResponse
{
      // Summary:
      //     command delivery status
      //
      //     - 0 = FAILED (could not send, SCP off-line)
      //
      //     - 1 = OK (delivered and accepted)
      //
      //     - 2 = NAK'd (command rejected by the SCP)
      //
      //     - 3 = Bad Command (command rejected by the Driver)
            

      FAILED,OK,NAK,BAD
}
