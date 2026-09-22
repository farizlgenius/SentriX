using System;

namespace Notifier.Contract.Constants;

public sealed class NotifierTopic
{
      // Device
      public static string IDREPORT = "DEVICE.IDREPORT";
      public static string EXCEPTION = "EXCEPTION";
      public static string MODULE_STATUS = "DEVICE.MODULE.STATUS";
      public static string EVENT_STATUS = "DEVICE.EVENT.STATUS";

      // Event
      public static string EVENT = "EVENT";
      public static string ADAPTER_EVENT = "ADAPTER.EVENT";
      public static string EXCEPTION_EVENT = "EXCEPTION.EVENT";
}
