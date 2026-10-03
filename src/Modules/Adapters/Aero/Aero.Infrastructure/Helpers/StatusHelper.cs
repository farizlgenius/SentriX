using SharedKernel.Enums;

namespace Aero.Infrastructure.Helpers;

public static class StatusHelper{
      public static CommStatus EncodeCommStatus(short d)
      {
            return (CommStatus)d;
      }
}