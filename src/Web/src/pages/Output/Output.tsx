import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { ActiveIcon, ControlIcon, DelayIcon, DisableIcon, DisconnectIcon, ErrorIcon, InactiveIcon, LocalMaskIcon, MaskIcon, NoAttachIcon, OffIcon, OnIcon, ToggleIcon } from "../../icons";
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
import { InputStatus } from "../../model/Status/InputStatus";
import { OutputStatus } from "../../model/Status/OutputStatus";
import { ChangeOfState } from "../../enum/ChangeOfState";
import { StatusHelper } from "../../helper/StatusHelper";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";

// Define Global Variable

export const OUTPUT_TABLE_HEADER: string[] = [
  "Name",
  "Pulse Time",
  "Status",
  "Flags",
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
    if (res && res.data) {
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
          code: ChangeOfState.Unknown,
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
                code: status.code,
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
  }, []);

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
    const currentChangeState = matchedItem?.code ?? ChangeOfState.Unknown;
    const currentChangeEncode = matchedItem?.status ?? ChangeOfStateEncode.Unknown;

    // 2. Resolve icon configuration cleanly
    const iconConfig = StatusHelper.ChangeOfStateIcon(currentChangeState);
    const iconCos = StatusHelper.ChangeOfStateIconEncode(currentChangeEncode);


    return [
      <TableCell
        key={`status-icon-${data.guid}`}
        className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
      >
        {iconConfig && (

          <span title={iconConfig.label} className="flex gap-1 items-center">
            <iconConfig.Icon className={`text-lg ${iconConfig.colorClass}`} />
            <Badge>
              {iconConfig.label}
            </Badge>
          </span>

        )}

      </TableCell>,

      <TableCell
        key={`status-badge-${data.guid}`}
        className="flex px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400 gap-2"
      >
        {
          iconCos && (
            <span title={iconCos.label} className="inline-flex items-center">
              <iconCos.Icon className={`text-lg ${iconCos.colorClass}`} />
            </span>
          )
        }
        {
          matchedItem?.entryDelayInProgress && (
            <span title="EntryDelayInProgress" className="inline-flex items-center">
              <DelayIcon className={`text-lg ${"text-brand-500"}`} />
            </span>
          )

        }
        {
          (matchedItem?.localMask || matchedItem?.mask) && (
            <span title="LocalMask" className="inline-flex items-center">
              <LocalMaskIcon className={`text-lg ${"text-yellow-500"}`} />
            </span>
          )
        }
        {
          (matchedItem?.mask) && (
            <span title="Mask" className="inline-flex items-center">
              <MaskIcon className={`text-lg ${"text-yellow-500"}`} />
            </span>
          )
        }
        {
          matchedItem?.notAttached && (
            <span title="NoAttach" className="inline-flex items-center">
              <NoAttachIcon className={`text-lg ${"text-red-500"}`} />
            </span>
          )
        }
        {
          matchedItem?.offline && (
            <span title="Offline" className="inline-flex items-center">
              <ErrorIcon className={`text-lg ${"text-red-500"}`} />
            </span>
          )
        }
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
