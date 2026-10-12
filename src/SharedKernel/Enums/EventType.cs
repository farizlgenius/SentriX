namespace SharedKernel.Enums;

public enum EventType
{
      SYSTEM=0x01,
      MODULE_COMM=0x02,
      CARD_BINARY=0x03,
      CARD_DATA=0x04,
      CARD_FULL=0x05,
      CARD_ID=0x06,
      CHANGE_OF_STATE=0x07,
      REX=0x08,
      DOOR_CHANGE_OF_STATE=0x09,
      PROCEDURE=0x0A,
      USER_CMND=0x0B,
      ACTIVATE=0x0C,
      DOOR_MODE=0x0D,
      MONITOR_GROUP=0x0E,
      AREA=0x0F,
      USER_CMND_EXTEND=0x12,
      USE_LIMIT = 0x13,
      WEB_ACTIVITY=0x14,
      OPER_MODE=0x18,
      ELEVATOR_STATE=0x1A,
      FILE_DOWNLOAD=0x1B,
      ELEVATOR_ACCESS=0x1D,

}