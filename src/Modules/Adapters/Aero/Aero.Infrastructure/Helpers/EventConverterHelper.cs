using HID.Aero.ScpdNet.Wrapper;
using SharedKernel.Enums;

namespace Aero.Infrastructure.Helpers;

public static class EventConverterHelper
{
      public static CoreEvent ToBaseEvent(short type,short code)
      {
            switch (type)
            {
                  case (short)tranType.tranTypeCardBin:
                        return CoreEvent.ACCESS_DENIED;;
                  case (short)tranType.tranTypeCardBcd:
                        return CoreEvent.ACCESS_DENIED;
                  default:
                        return CoreEvent.UNKNOWN;
            }
      }
      public static EntityType SourceTypeToEntityType(short type)
      {
            return type switch
            {
                  (short)tranSrc.tranSrcScpDiag => EntityType.DEVICE,
                  (short)tranSrc.tranSrcScpCom => EntityType.DEVICE,
                  (short)tranSrc.tranSrcScpLcl => EntityType.DEVICE,
                  (short)tranSrc.tranSrcSioDiag => EntityType.DEVICE_MODULE,
                  (short)tranSrc.tranSrcSioCom => EntityType.DEVICE_MODULE,
                  (short)tranSrc.tranSrcSioTmpr => EntityType.DEVICE_MODULE,
                  (short)tranSrc.tranSrcSioPwr => EntityType.DEVICE_MODULE,
                  (short)tranSrc.tranSrcMP => EntityType.INPUT,
                  (short)tranSrc.tranSrcCP => EntityType.OUTPUT,
                  (short)tranSrc.tranSrcACR => EntityType.DOOR,
                  (short)tranSrc.tranSrcAcrTmprAlt => EntityType.DOOR,
                  (short)tranSrc.tranSrcAcrTmpr => EntityType.DOOR,
                  (short)tranSrc.tranSrcAcrDoor => EntityType.DOOR,
                  (short)tranSrc.tranSrcAcrRex0 => EntityType.DOOR,
                  (short)tranSrc.tranSrcAcrRex1 => EntityType.DOOR,
                  (short)tranSrc.tranSrcTimeZone => EntityType.TIMEZONE,
                  (short)tranSrc.tranSrcProcedure => EntityType.PROCEDURE,
                  (short)tranSrc.tranSrcTrigger => EntityType.TRIGGER,
                  (short)tranSrc.tranSrcTrigVar => EntityType.TRIGGER,
                  (short)tranSrc.tranSrcMPG => EntityType.MONITOR_GROUP,
                  (short)tranSrc.tranSrcArea => EntityType.AREA,
                  (short)tranSrc.tranSrcLoginService => EntityType.WEB,
                  _ => EntityType.DEVICE
            };
      }
}