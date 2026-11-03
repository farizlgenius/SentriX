import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { DelayIcon, ErrorIcon, LocalMaskIcon, MaskIcon, MonitorIcon, NoAttachIcon, UnmaskIcon, UnknownIcon } from "../../icons";
import Logger from "../../utility/Logger";
import InputForm from "./InputForm";
import { InputDto } from "../../model/MonitorPoint/InputDto";
import { MonitorPointToast } from "../../model/ToastMessage";
import { useToast } from "../../context/ToastContext";
import Helper from "../../utility/Helper";
import { MonitorPointEndpoint } from "../../endpoint/MonitorPointEndpoint";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { BaseTable } from "../UiElements/BaseTable";
import SignalRService from "../../services/SignalRService";
import { ActionButton } from "../../model/ActionButton";
import { TableCell } from "../../components/ui/table";
import Badge from "../../components/ui/badge/Badge";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { Vendor } from "../../enum/Vendor";
import { InputStatus } from "../../model/Status/InputStatus";
import { SignalRTopic } from "../../constants/signalr-constant";
import { StatusHelper } from "../../helper/StatusHelper";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";
import { InputLatchingMode } from "../../enum/InputLatchingMode";
import { InputMode } from "../../enum/InputMode";
import { InputLogFunction } from "../../enum/InputLogFunction";
import { HttpStatusCode } from "axios";

// Define Global Variable
export const MP_TABLE_HEADER: string[] = [
  "Name",
  "Status",
  "Enable",
  "Action",
];
export const MP_KEY: string[] = [
  "name",
];

const Input = () => {
  const { filterPermission, token } = useAuth();
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { setPagination } = usePagination();
  const {
    setCreate,
    setRemove,
    setUpdate,
    setConfirmCreate,
    setConfirmRemove,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  {
    /* handle Table Action */
  }
  const handleEdit = (data: InputDto) => {
    setMonitorPointDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: InputDto) => {
    setMonitorPointDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const handleRemove = (data: InputDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(MonitorPointEndpoint.DELETE(data.guid));
      if (
        Helper.handleToastByResCode(res, MonitorPointToast.DELETE, toggleToast)
      ) {
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    let res;
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObjects.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          var data: string[] = [];
          selectedObjects.map(async (a: InputDto) => {
            data.push(a.guid);
          });
          var res = await send.post(MonitorPointEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorPointToast.DELETE_RANGE,
              toggleToast,
            )
          ) {
            setRemove(false);
            toggleRefresh();
          }
        });
        setRemove(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          monitorPointDto.locationGuid = locationGuid;
          monitorPointDto.metadata = JSON.stringify(monitorPointDto.metadata);
          const res = await send.post(
            MonitorPointEndpoint.CREATE,
            monitorPointDto,
          );
          if (
            Helper.handleToastByResCode(
              res,
              MonitorPointToast.CREATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(
            MonitorPointEndpoint.UPDATE,
            monitorPointDto,
          );
          if (
            Helper.handleToastByResCode(
              res,
              MonitorPointToast.UPDATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "cancel":
      case "close":
        setMonitorPointDto(defaultDto);
        setForm(false);
        break;
      case "mask":
        selectedObjects.forEach(async (a: InputDto) => {
          res = await send.post(MonitorPointEndpoint.MASK(a.guid));
          if (
            Helper.handleToastByResCode(
              res,
              MonitorPointToast.MASK,
              toggleToast,
            )
          ) {
            toggleRefresh();
          }
        });

        break;
      case "unmask":
        selectedObjects.forEach(async (a: InputDto) => {
          res = await send.post(MonitorPointEndpoint.UNMASK(a.guid));
          if (
            Helper.handleToastByResCode(
              res,
              MonitorPointToast.UNMASK,
              toggleToast,
            )
          ) {
            toggleRefresh();
          }
        });
        break;
      default:
        break;
    }
  };

  {
    /* input Data */
  }
  const defaultDto: InputDto = {
    guid: "",
    name: "",
    slotNo: -1,
    metadata: {
      debounce:4,
      latchingMode:InputLatchingMode.NORMAL,
      mode:InputMode.NC,
      delayEntry:0,
      delayExit:0,
      holdTime:2,
      lfCode:InputLogFunction.NO_MASKED
    },
    vendor: Vendor.unknown,
    deviceGuid: "",
    deviceName: "",
    deviceModuleGuid: "",
    deviceModuleName: "",
    locationGuid: locationGuid,
    locationName: "",
    isActive: true,
    isDefault: false
  };
  const [monitorPointsDto, setMonitorPointsDto] = useState<InputDto[]>([]);
  const [monitorPointDto, setMonitorPointDto] = useState<InputDto>(defaultDto);
  const [status, setStatus] = useState<InputStatus[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      MonitorPointEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res);
    if (res.status == HttpStatusCode.Ok) {
      res.data.data.items.forEach((a: InputDto) => {
              setMonitorPointsDto(prev => (
                prev.some(x => x.guid == a.guid) ?
                  [...prev]
                  :
                  [
      
                    ...prev,
                    {
                      ...a,
                      metadata: JSON.parse(a.metadata as string)
                    }
                  ]))
            })
      setPagination(res.data.data);

      // Batch set state
      const newStatuses = res.data.data.items.map((a: InputDto) => {
        const status: InputStatus = {
          guid: a.guid,
          code: ChangeOfStateEncode.Unknown,
          offline: false,
          mask: false,
          localMask: false,
          entryDelayInProgress: false,
          notAttached: false,
          status: ChangeOfStateEncode.Unknown
        }
        return status;
      }
      );

      console.log(newStatuses);

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.data.items.forEach((a: InputDto) => {
        fetchStatus(a.guid);
      });
    }
  };
  const fetchStatus = async (guid: string) => {
    await send.get(MonitorPointEndpoint.GET_MP_STATUS(guid));
    
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<InputDto[]>([]);

  {
    /* UseEffect */
  }
  useEffect(() => {
    const initSignalR = async () => {
      if (!token) return;

      await SignalRService.startConnection();
      const connection = SignalRService.getConnection();
      if (!connection) return;

      connection.on(SignalRTopic.INPUT_STATUS, (status: InputStatus) => {
        console.log(status);
        setStatus((prev) =>
          prev.map((item) =>
            item.guid === status.guid
              ? {
                ...item,
                status: status.status,
                offline: status.offline,
                mask: status.mask,
                localMask: status.localMask,
                entryDelayInProgress: status.entryDelayInProgress,
                notAttached: status.notAttached
              }
              : item,
          ),
        );
      });



      try {
        await SignalRService.joinGroup(SignalRTopic.INPUT_STATUS);
      } catch (err) {
        console.error("Subscribe error:", err);
      }

    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.INPUT_STATUS);
    };
  }, [refresh, locationGuid, token]);

  const renderOptionalComponent = (data: InputDto) => {
    // 1. Look up the matched record once
    const matchedItem = status.find((b) => b.guid === data.guid);
    const currentChangeEncode = matchedItem?.status ?? ChangeOfStateEncode.Unknown;

    // 2. Resolve icon configuration cleanly
    const iconCos = StatusHelper.ChangeOfStateIconEncode(currentChangeEncode);
    const statusLabel =
      currentChangeEncode === ChangeOfStateEncode.GroudFault
        ? "Ground fault"
        : currentChangeEncode === ChangeOfStateEncode.OpenCircuit
          ? "Open circuit"
          : currentChangeEncode === ChangeOfStateEncode.ForeignVoltage
            ? "Foreign voltage"
            : currentChangeEncode === ChangeOfStateEncode.NonSettlingError
              ? "Non-settling error"
              : currentChangeEncode === ChangeOfStateEncode.SupervisoryFaultCodes
                ? "Supervisory fault"
                : iconCos.label || "Unknown";
    const statusColor: "success" | "error" | "warning" | "light" =
      currentChangeEncode === ChangeOfStateEncode.Active
        ? "success"
        : currentChangeEncode === ChangeOfStateEncode.Inactive ||
            currentChangeEncode === ChangeOfStateEncode.GroudFault ||
            currentChangeEncode === ChangeOfStateEncode.Short ||
            currentChangeEncode === ChangeOfStateEncode.NonSettlingError
          ? "error"
          : currentChangeEncode === ChangeOfStateEncode.OpenCircuit ||
              currentChangeEncode === ChangeOfStateEncode.ForeignVoltage ||
              currentChangeEncode === ChangeOfStateEncode.SupervisoryFaultCodes
            ? "warning"
            : "light";

    return [
      <TableCell
        key={`status-badge-${data.guid}`}
        className="px-4 py-3 text-start text-theme-sm"
      >
        <div className="flex flex-wrap items-center gap-1.5">
          <Badge
            color={statusColor}
            size="sm"
            startIcon={
              currentChangeEncode === ChangeOfStateEncode.Unknown ? (
                <UnknownIcon className="h-3.5 w-3.5" />
              ) : (
                <iconCos.Icon className="h-3.5 w-3.5" />
              )
            }
          >
            {statusLabel}
          </Badge>
          {matchedItem?.entryDelayInProgress && (
            <Badge
              color="info"
              size="sm"
              startIcon={<DelayIcon className="h-3.5 w-3.5" />}
            >
              Entry delay
            </Badge>
          )}
          {matchedItem?.localMask && (
            <Badge
              color="warning"
              size="sm"
              startIcon={<LocalMaskIcon className="h-3.5 w-3.5" />}
            >
              Locally masked
            </Badge>
          )}
          {matchedItem?.mask && (
            <Badge
              color="warning"
              size="sm"
              startIcon={<MaskIcon className="h-3.5 w-3.5" />}
            >
              Masked
            </Badge>
          )}
          {matchedItem?.notAttached && (
            <Badge
              color="error"
              size="sm"
              startIcon={<NoAttachIcon className="h-3.5 w-3.5" />}
            >
              Not attached
            </Badge>
          )}
          {matchedItem?.offline && (
            <Badge
              color="error"
              size="sm"
              startIcon={<ErrorIcon className="h-3.5 w-3.5" />}
            >
              Offline
            </Badge>
          )}
        </div>
      </TableCell>,
    ];
  };

  const action: ActionButton[] = [
    {
      lable: "mask",
      buttonName: "Mask",
      icon: <MaskIcon />,
    },
    {
      lable: "unmask",
      buttonName: "Unmask",
      icon: <UnmaskIcon />,
    },
  ];

  const tabContent: FormContent[] = [
    {
      label: "Monitor Point",
      icon: <MonitorIcon />,
      content: (
        <InputForm
          handleClick={handleClick}
          dto={monitorPointDto}
          setDto={setMonitorPointDto}
          type={formType} handleChange={() => { }} />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Monitor Point" />
      {form ? (
        <BaseForm tabContent={tabContent} handleClick={handleClick} type={formType} />
      ) : (
        <BaseTable<InputDto>
          headers={MP_TABLE_HEADER}
          keys={MP_KEY}
          data={monitorPointsDto}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onInfo={handleInfo}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          onClick={handleClick}
          action={action}
          renderOptionalComponent={renderOptionalComponent}
          permission={filterPermission(FeatureId.monitor)}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
        />
      )}
    </>
  );
};

export default Input;
