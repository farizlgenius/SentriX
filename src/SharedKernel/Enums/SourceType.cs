namespace SharedKernel.Enums;

public enum SourceType
{
      DEVICE_DIAG,      
      DEVICE_COMM,
      DEVICE_LOCAL_MONITOR,
      MODULE_DIAG,
      MODULE_COMM,
      MODULE_TAMPER,
      MODULE_POWER,
      MONITOR_POINT,
      CONTROL_POINT,
      DOOR,
      READER_TAMPER=0x0A,
      DOOR_SENSOR=0x0B,
      REX_01=0x0D,
      REX_02=0x0E,
      TIMEZONE=0x0F,
      PROCEDURE=0x10,
      TRIGGER=0x11,
      TRIGGER_VARIABLE=0x12,
      MONITOR_GROUP=0x13,
      AREA=0x14,
      ALTR_READER=0x15,
      LOGIN_SERVICE=0x18

}