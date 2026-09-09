

using SharedKernel.Enums;

namespace SharedKernel.Helpers;

public class AeroModuleModelHelper
{
      public static short nInputByModel(DeviceModuleModel model)
      {
            return model switch
            {
                  DeviceModuleModel.x1100 => 7,
                  DeviceModuleModel.x100 => 7,
                  DeviceModuleModel.x200 => 19,
                  DeviceModuleModel.x300 => 5,
                  DeviceModuleModel.amico => 1,
                  _ => 0
            };
      }

       public static short nOutputByModel(DeviceModuleModel model)
      {
            return model switch
            {
                  DeviceModuleModel.x1100 => 4,
                  DeviceModuleModel.x100 => 4,
                  DeviceModuleModel.x200 => 2,
                  DeviceModuleModel.x300 => 12,
                   DeviceModuleModel.amico => 1,
                  _ => 0
            };
      }

       public static short nReaderByModel(DeviceModuleModel model)
      {
            return model switch
            {
                  DeviceModuleModel.x1100 => 4,
                  DeviceModuleModel.x100 => 4,
                  DeviceModuleModel.x200 => 0,
                  DeviceModuleModel.x300 => 0,
                   DeviceModuleModel.amico => 1,
                  _ => 0
            };
      }
}
