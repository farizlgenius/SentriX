using SharedKernel.Model;

namespace Adapter.Aero.Interfaces;

public interface IDoorCommand
{
      CommandResponse AccessControlReaderConfiguration(
             string Mac,
            short DeviceComponentId,
            short ComponentId,
            short AccessConfig,
            short PairComponentId,
            short ReaderModuleComponentId,
            short ReaderNumber,
            short RelayModuleComponentId,
            short RelayNumber,
            short RelayMin,
            short RelayMax,
            short RelayMode,
            short SensorModuleComponentId,
            short SensorNumber,
            short HeldOpenDelay,
            short Rex0ModuleComponentId,
            short Rex0Number,
            short Rex1ModuleComponentId,
            short Rex1Number,
            short DisableRex0Timezone,
            short DisableRex1Timezone,
            short AltrRdrModuleComponentId,
            short AltrRdrNumber,
            short AltrRdrConf,
            short AntipassbackMode,
            short AreaIn,
            short AreaOut,
            short Spare,
            short AccessControlFlag,
            short OfflineMode,
            short DefaultMode,
            short LedMode,
            short ApbDelay,
            short RelayT2,
            short HeldOpen2,
            short RelayFollowerPulse,
            short RelayFollowerDelay,
            short ExtendFeatureType,
            short InteriorPushButtonModuleComponentId,
            short InteriorPushButtonInputNumber,
            short InteriorPushButtonLongPress,
            short InteriorPushButtonOutModuleComponentId,
            short InteriorPushButtonOutRelayNumber
      );

      public CommandResponse ReaderSpecification(
            string Mac,
            short DeviceComponentId,
            short ModuleComponentId,
            short ReaderNumber,
            short DataFormat,
            short KeypadMode,
            short LedDriverMode,
            short OsdpFlag
      );
}