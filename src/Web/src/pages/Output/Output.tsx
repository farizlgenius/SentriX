import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { ControlIcon, OffIcon, OnIcon, ToggleIcon } from "../../icons";
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
import { InputStatus } from "../../enum/InputStatus";
import { OutputCommandDto } from "../../model/ControlPoint/OutputCommandDto";
import { OutputCommand } from "../../enum/OutputCommand";
import { OutputRepeatCommand } from "../../model/ControlPoint/OutputRepeatCommand";
import { Modal } from "../../components/ui/modal";
import Modals from "../UiElements/Modals";
import Label from "../../components/form/Label";
import { FormField } from "../../components/form/template/FormTemplate";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";

// Define Global Variable

export const OUTPUT_TABLE_HEADER: string[] = [
  "Name",
  "Pulse Time",
  "Status",
  "Action",
];
export const OUTPUT_KEY: string[] = ["name", "defaultPulse"];

const Output = () => {
  const { toggleToast } = useToast();
  const { token } = useAuth();
  const { setPagination } = usePagination();
  const { locationGuid: locationId } = useLocation();
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
    slotNo: 0,
    metadata: "",
    vendor: Vendor.aero,
    isActive: false,
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
  const [status, setStatus] = useState<StatusDto[]>([]);
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
    console.log(res);
    if (res && res.data) {
      console.log(res.data);
      setOutputsDto(res.data.data.items);
      setPagination(res.data.data);

      // Batch set state
      const newStatuses = res.data.data.items.map((a: OutputDto) => ({
        guid: a.guid,
        status: Status.Unknown,
        outputStatus: InputStatus.Unknown
      }));

      console.log(newStatuses);

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.item.forEach((a: OutputDto) => {
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

      connection.on(SignalRTopic.CP_STATUS, (status: StatusDto) => {
        // setIdReports(reports);
      });

      try {
        await SignalRService.joinGroup(SignalRTopic.CP_STATUS);
      } catch (err) {
        console.error("Subscribe error:", err);
      }
    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.CP_STATUS);
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

  const renderOptionalComponent = (data: OutputDto, statusDto: StatusDto[]) => {
    return [
      <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <Badge
          size="sm"
          color={
            statusDto.find((b) => b.guid == data.guid)?.status == Status.Active
              ? "success"
              : statusDto.find((b) => b.guid == data.guid)?.status == Status.Inactive
                ? "error"
                : "warning"
          }
        >
          {Status[statusDto.find((b) => b.guid == data.guid)?.status ?? Status.Unknown]}
        </Badge>
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
          <BaseForm tabContent={formContent} header={""} desc={""} type={FormType.CREATE} />
        </>
      ) : (
        <BaseTable<OutputDto>
          headers={OUTPUT_TABLE_HEADER}
          keys={OUTPUT_KEY}
          status={status}
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
          locationGuid={locationId}
          refresh={refresh}
        />
      )}
    </>
  );
};

export default Output;
