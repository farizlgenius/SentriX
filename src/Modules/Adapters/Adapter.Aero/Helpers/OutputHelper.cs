namespace Adapter.Aero.Helpers;

public static class OutputHelper
{
      public static short FinalizeOutputMode(short Drive,short Offline)
      {
            switch (Drive)
            {
                  case 0:
                        return Offline switch
                        {
                              0 => 0,
                              1 => 16,
                              2 => 32,
                              _ => 0    
                        };
                  case 1:
                        return Offline switch
                        {
                              0 => 1,
                              1 => 17,
                              3 => 33,
                              _ => 0
                        };
                  default:
                        return 0;
            }
      }
}