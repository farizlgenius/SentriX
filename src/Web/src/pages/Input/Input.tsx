import React, { JSX, useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { AddIcon, MaskIcon, MonitorIcon, UnmaskIcon } from "../../icons";
import Logger from "../../utility/Logger";
import InputForm from "./InputForm";
import { InputDto } from "../../model/MonitorPoint/InputDto";
import { StatusDto } from "../../model/StatusDto";
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
import { MpStatus } from "../../model/MonitorPoint/MpStatus";
import { usePagination } from "../../context/PaginationContext";
import { Vendor } from "../../enum/Vendor";
import { InputStatus } from "../../model/Status/InputStatus";
import { ChangeOfState } from "../../enum/ChangeOfState";
import { SignalRTopic } from "../../constants/signalr-constant";

// Define Global Variable
export const MP_TABLE_HEADER: string[] = [
  "Name",
  "Main Controller",
  "Module",
  "Mode",
  "Input Mode",
  "Masked",
  "Status",
  "Action",
];
export const MP_KEY: string[] = [
  "name",
  "hardwareName",
  "moduleDescription",
  "monitorPointModeDescription",
  "inputModeDescription",
  "isMask",
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
          res = await send.post(MonitorPointEndpoint.MASK, a);
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
          res = await send.post(MonitorPointEndpoint.UNMASK, a);
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
    slotNo: 0,
    metadata: "",
    vendor: Vendor.unknown,
    deviceGuid: "",
    deviceName: "",
    deviceModuleGuid: "",
    deviceModuleName: "",
    locationGuid: "",
    locationName: "",
    isActive: false,
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
    if (res?.data.data) {
      setMonitorPointsDto(res.data.data.data);
      setPagination(res.data.data.page);

      // Batch set state
      const newStatuses = res.data.data.data.map((a: InputDto) => {
        const status: InputStatus = {
          guid: a.guid,
          status: ChangeOfState.Unknown,
          offline: false,
          mask: false,
          localMask: false,
          entryDelayInProgress: false,
          notAttached: false
        }
        return status;
      }
      );

      console.log(newStatuses);

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.data.data.forEach((a: InputDto) => {
        fetchStatus(a.guid);
      });
    }
  };
  const fetchStatus = async (guid: string) => {
    const res = await send.get(MonitorPointEndpoint.GET_MP_STATUS(guid));
    Logger.info(res);
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
        await SignalRService.joinGroup(SignalRTopic.DOOR_STATUS);
      } catch (err) {
        console.error("Subscribe error:", err);
      }

    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.DOOR_STATUS);
    };
  }, [refresh, locationGuid, token]);

  const renderOptionalComponent = (data: any) => {
    return [
      <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <Badge
          size="sm"
          color={
            status.find((b) => b.guid == data.guid)?.status ==
              "Active"
              ? "success"
              : status.find((b) => b.guid == data.guid)?.status ==
                "Inactive"
                ? "error"
                : "warning"
          }
        >
          {status.find((b) => b.guid == data.guid)?.status == ""
            ? "Error"
            : status.find((b) => b.guid == data.guid)?.status}
        </Badge>
      </TableCell>,
      <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <Badge
          size="sm"
          color={
            status.find((b) => b.guid == data.guid)?.status ==
              "Active"
              ? "success"
              : status.find((b) => b.guid == data.guid)?.status ==
                "Inactive"
                ? "error"
                : "warning"
          }
        >
          {status.find((b) => b.guid == data.guid)?.status == ""
            ? "Error"
            : status.find((b) => b.guid == data.guid)?.status}
        </Badge>
      </TableCell>
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
          status={status}
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
          locationGuid={locationId}
          refresh={refresh}
        />
      )}
    </>
  );
};

export default Input;
