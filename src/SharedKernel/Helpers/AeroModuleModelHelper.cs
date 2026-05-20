

using SharedKernel.Enums;

namespace SharedKernel.Helpers;

public class AeroModuleModelHelper
{
      public static short nInputByModel(SioModel model)
      {
            return model switch
            {
                  SioModel.x1100 => 7,
                  SioModel.x100 => 7,
                  SioModel.x200 => 19,
                  SioModel.x300 => 5,
                  SioModel.v100 => 7,
                  SioModel.v200 => 19,
                  SioModel.v300 => 5,
                  SioModel.APERIO => 193,
                  _ => 0
            };
      }

       public static short nOutputByModel(SioModel model)
      {
            return model switch
            {
                  SioModel.x1100 => 4,
                  SioModel.x100 => 4,
                  SioModel.x200 => 2,
                  SioModel.x300 => 12,
                  SioModel.v100 => 4,
                  SioModel.v200 => 2,
                  SioModel.v300 => 12,
                  SioModel.APERIO => 64,
                  _ => 0
            };
      }

       public static short nReaderByModel(SioModel model)
      {
            return model switch
            {
                  SioModel.x1100 => 4,
                  SioModel.x100 => 4,
                  SioModel.x200 => 0,
                  SioModel.x300 => 0,
                  SioModel.v100 => 2,
                  SioModel.v200 => 0,
                  SioModel.v300 => 0,
                  SioModel.APERIO => 64,
                  _ => 0
            };
      }
}
