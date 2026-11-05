using System;

namespace Notifier.Contract.Constants;

public sealed class NotifierTopic
{
      // Device
      public static string IDREPORT = "DEVICE.IDREPORT";
      public static string DEVICE_STATUS = "DEVICE.STATUS";
      public static string EXCEPTION = "EXCEPTION";
      public static string EVENT_STATUS = "DEVICE.EVENT.STATUS";
      public static string DEVICE_POWER_STATUS = "DEVICE.POWER.STATUS";

      // Module
      public static string MODULE_STATUS = "MODULE.STATUS";
      public static string MODULE_TAMPER_STATUS = "MODULE.TAMPER.STATUS";
      public static string MODULE_POWER_STATUS = "MODULE.POWER.STATUS";

      // Event
      public static string EVENT = "EVENT";
      public static string ADAPTER_EVENT = "ADAPTER.EVENT";
      public static string EXCEPTION_EVENT = "EXCEPTION.EVENT";

      // Door
      public static string DOOR_STATUS = "DOOR.STATUS";
      public static string DOOR_READER_STATUS = "DOOR.READER.STATUS";
      public static string DOOR_COS_STATUS = "DOOR.COS.STATUS";
      // Output
      public static string OUTPUT_STATUS = "OUTPUT.STATUS";
      // Input
      public static string INPUT_STATUS = "INPUT.STATUS";

      // MONITOR POINT
      public static string MONITOR_GROUP_STATUS = "MONITOR.GROUP.STATUS";

      // TimeZone
      public static string TIMEZONE_STATUS = "TIMEZONE.STATUS";
      // Area
      public static string AREA_STATUS = "AREA.STATUS";
}
