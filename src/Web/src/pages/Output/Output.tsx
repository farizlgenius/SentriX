import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { ActiveIcon, ControlIcon, DelayIcon, DisableIcon, DisconnectIcon, ErrorIcon, InactiveIcon, LocalMaskIcon, MaskIcon, NoAttachIcon, OffIcon, OnIcon, ToggleIcon, UnknownIcon } from "../../icons";
import OutputForm from "./OutputForm";
import Logger from "../../utility/Logger";
import { OutputDto } from "../../model/ControlPoint/OutputDto";
import { StatusDto } from "../../model/StatusDto";
import Helper from "../../utility/Helper";
import { useToast } from "../../context/ToastContext";
import { ControlPointToast } from "../../model/ToastMessage";
import { OutputEndpoint } from "../../endpoint/ControlPointEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { BaseTable } from "../UiElements/BaseTable";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { TableCell } from "../../components/ui/table";
import Badge from "../../components/ui/badge/Badge";
import { ActionButton } from "../../model/ActionButton";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import SignalRService from "../../services/SignalRService";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { SignalRTopic } from "../../constants/signalr-constant";
import { Vendor } from "../../enum/Vendor";
import { Status } from "../../enum/Status";
import { OutputCommand } from "../../enum/OutputCommand";
import { OutputRepeatCommand } from "../../model/ControlPoint/OutputRepeatCommand";
import Modals from "../UiElements/Modals";
import Label from "../../components/form/Label";
import { FormField } from "../../components/form/template/FormTemplate";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";
import { AeroOutputMetadata } from "../../model/ControlPoint/AeroOutputMetadata";
import { OutputStatus } from "../../model/Status/OutputStatus";
import { StatusHelper } from "../../helper/StatusHelper";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";
import { HttpStatusCode } from "axios";

// Define Global Variable

export const OUTPUT_TABLE_HEADER: string[] = [
  "Name",
  "Pulse Time",
  "Status",
  "Enabled",
  "Action",
];
export const OUTPUT_KEY: string[] = ["name", "defaultPulse"];

const Output = () => {
  const { toggleToast } = useToast();
  const { token } = useAuth();
  const { setPagination } = usePagination();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const {
    setCreate,
    setRemove,
    setMessage,
    setUpdate,
    setInfo,
    setConfirmCreate,
    setConfirmRemove,
    setConfirmUpdate,
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const [repeatForm, setRepeatForm] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  {
    /* handle Table Action */
  }
  const handleEdit = (data: OutputDto) => {
    setControlPointDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: OutputDto) => {
    console.log(data);
    setControlPointDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const handleRemove = (data: OutputDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(OutputEndpoint.DELETE(data.guid));
      if (
        Helper.handleToastByResCode(res, ControlPointToast.DELETE, toggleToast)
      ) {
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  {
    /* Output Data */
  }
  const defaultOutputDto: OutputDto = {
    guid: "",
    name: "",
    slotNo: -1,
    metadata: {
      defaultPulse: 1,
      offlineMode: 0,
      driveMode: 0
    },
    deviceModuleGuid: "",
    vendor: Vendor.unknown,
    isActive: true,
    isDefault: false
  };
  const defaultRepeatCommand: OutputRepeatCommand = {
    onTime: 0,
    offTime: 0,
    repeat: 0
  }
  const [controlPointDto, setControlPointDto] =
    useState<OutputDto>(defaultOutputDto);
  const [repeat, setRepeat] = useState<OutputRepeatCommand>(defaultRepeatCommand);
  //const [onTime,setOnTime] = useState<number>(1);
  const [outputsDto, setOutputsDto] = useState<OutputDto[]>([]);
  const [status, setStatus] = useState<OutputStatus[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      OutputEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.status == HttpStatusCode.Ok) {
      res.data.data.items.forEach((a: OutputDto) => {
        setOutputsDto(prev => (
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
      //setOutputsDto(res.data.data.items);
      setPagination(res.data.data);

      // Batch set state
      const newStatuses = res.data.data.items.map((a: OutputDto) => {
        const status: OutputStatus = {
          guid: a.guid,
          offline: false,
          mask: false,
          localMask: false,
          entryDelayInProgress: false,
          notAttached: false,
          status: ChangeOfStateEncode.Unknown
        }
        return status;
      });

      console.log(newStatuses);

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.data.items.forEach((a: OutputDto) => {
        fetchStatus(a.guid);
      });
    }
  };

  const fetchStatus = async (outputId: string) => {
    const res = send.get(OutputEndpoint.STATUS(outputId));
    Logger.info(res);
  };

  {
    /* UseEffect */
  }
  useEffect(() => {
    const initSignalR = async () => {
      if (!token) return;

      await SignalRService.startConnection();
      const connection = SignalRService.getConnection();
      if (!connection) return;

      connection.on(SignalRTopic.OUTPUT_STATUS, (status: OutputStatus) => {
        console.log(status)
        setStatus((prev) =>
          prev.map((item) =>
            item.guid === status.guid
              ? {
                ...item,
                offline: status.offline,
                mask: status.mask,
                localMask: status.localMask,
                entryDelayInProgress: status.entryDelayInProgress,
                notAttached: status.notAttached,
                status:status.status
              }
              : item,
          ),
        );
      });

      try {
        await SignalRService.joinGroup(SignalRTopic.OUTPUT_STATUS);
      } catch (err) {
        console.error("Subscribe error:", err);
      }
    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.OUTPUT_STATUS);
    };
  }, [refresh, locationGuid, token]);

  {
    /* Button Command */
  }
  const modalHandleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    switch (e.currentTarget.name) {
      case "close":
        setRepeatForm(false);
        break;
      default:
        break;
    }
  }


  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
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
          selectedObjects.map(async (a: OutputDto) => {
            data.push(a.guid);
          });
          var res = await send.post(OutputEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              ControlPointToast.DELETE_RANGE,
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
        console.log(controlPointDto);
        setConfirmCreate(() => async () => {
          controlPointDto.metadata = JSON.stringify(controlPointDto.metadata);
          const res = await send.post(OutputEndpoint.CREATE, controlPointDto);
          if (
            Helper.handleToastByResCode(
              res,
              ControlPointToast.CREATE,
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
          const res = await send.put(OutputEndpoint.UPDATE, controlPointDto);
          if (
            Helper.handleToastByResCode(
              res,
              ControlPointToast.UPDATE,
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
        setControlPointDto(defaultOutputDto);
        setForm(false);
        break;
      case "on":
        console.log(selectedObjects);
        if (selectedObjects.length > 0) {
          selectedObjects.map(async (a: OutputDto) => {
            const res = await send.post(OutputEndpoint.TRIGGER, {
              guid: a.guid,
              command: OutputCommand.ON
            });
            Helper.handleToastByResCode(
              res,
              ControlPointToast.TOGGLE,
              toggleToast,
            );
          });
        }

        break;
      case "off":
        if (selectedObjects.length > 0) {
          selectedObjects.map(async (a: OutputDto) => {
            const res = await send.post(OutputEndpoint.TRIGGER, {
              guid: a.guid,
              command: OutputCommand.OFF
            });
            Helper.handleToastByResCode(
              res,
              ControlPointToast.TOGGLE,
              toggleToast,
            );
          });
        }
        break;
      case "toggle":
        if (selectedObjects.length > 0) {
          selectedObjects.map(async (a: OutputDto) => {
            const res = await send.post(OutputEndpoint.TRIGGER, {
              guid: a.guid,
              command: OutputCommand.SINGLE
            });
            Helper.handleToastByResCode(
              res,
              ControlPointToast.TOGGLE,
              toggleToast,
            );
          });
        }
        break;
      case "repeat":
        setRepeatForm(true)

        break;
      default:
        break;
    }
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<OutputDto[]>([]);

  const action: ActionButton[] = [
    {
      lable: "on",
      buttonName: "On",
      icon: <OnIcon />,
    },
    {
      lable: "off",
      buttonName: "Off",
      icon: <OffIcon />,
    },
    {
      lable: "toggle",
      buttonName: "Toggle",
      icon: <ToggleIcon />,
    },
    {
      lable: "repeat",
      buttonName: "Repeat",
      icon: <ToggleIcon />,
    },
  ];

  const renderOptionalComponent = (data: OutputDto) => {
    // 1. Look up the matched record once
    const matchedItem = status.find((b) => b.guid === data.guid);
    const currentChangeEncode = matchedItem?.status ?? ChangeOfStateEncode.Unknown;

    // 2. Resolve icon configuration cleanly
    const iconCos = StatusHelper.OutputChangeOfStateIconEncode(currentChangeEncode);
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

  const formContent: FormContent[] = [
    {
      icon: <ControlIcon />,
      label: "Control Point",
      content: (
        <OutputForm
          dto={controlPointDto}
          setDto={setControlPointDto}
          handleClick={handleClick}
          type={formType} handleChange={() => { }} />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Control Point" />
      {repeatForm && (
        <Modals handleClickWithEvent={modalHandleClick} body={
          <div className="grid grid-cols-2 gap-5">
            <FormField>
              <Label>On Time</Label>
              <Input type="number" value={repeat.onTime} onChange={(e) => setRepeat(prev => ({ ...prev, onTime: Number(e.target.value) }))} />
            </FormField>
            <FormField>
              <Label>Off Time</Label>
              <Input type="number" value={repeat.offTime} onChange={(e) => setRepeat(prev => ({ ...prev, offTime: Number(e.target.value) }))} />
            </FormField>
            <FormField className="col-span-2">
              <Label>Repeat Count</Label>
              <Input type="number" value={repeat.repeat} onChange={(e) => setRepeat(prev => ({ ...prev, repeat: Number(e.target.value) }))} />
            </FormField>
            <div></div>
            <FormField className="flex justify-end">
              <Button onClick={e => {
                if (selectedObjects.length > 0) {
                  selectedObjects.map(async (a: OutputDto) => {
                    const res = await send.post(OutputEndpoint.TRIGGER, {
                      guid: a.guid,
                      command: OutputCommand.REPEAT,
                      onTime: repeat.onTime,
                      offTime: repeat.offTime,
                      repeat: repeat.repeat
                    });
                    Helper.handleToastByResCode(
                      res,
                      ControlPointToast.TOGGLE,
                      toggleToast,
                    );
                  });
                }
                setRepeatForm(false);
              }
              }>OK</Button>
            </FormField>
          </div>

        } />
      )}
      {form ? (
        <>
          <BaseForm handleClick={handleClick} tabContent={formContent} header={"Control Point"} desc={""} type={FormType.CREATE} />
        </>
      ) : (
        <BaseTable<OutputDto>
          headers={OUTPUT_TABLE_HEADER}
          keys={OUTPUT_KEY}
          data={outputsDto}
          onEdit={handleEdit}
          onRemove={handleRemove}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          onClick={handleClick}
          permission={filterPermission(FeatureId.control)}
          renderOptionalComponent={renderOptionalComponent}
          action={action}
          onInfo={handleInfo}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
          specialDisplay={[
            {
              key: "defaultPulse",
              content: (data, i) => <TableCell key={i} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                {(data.metadata as AeroOutputMetadata).defaultPulse ?? 0}
              </TableCell>,
            }
          ]}
        />
      )}
    </>
  );
};

export default Output;
