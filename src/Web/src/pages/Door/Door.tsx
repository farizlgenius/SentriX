import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import {
  ControlIcon,
  DisableIcon,
  DoorIcon,
  DoorInIcon,
  DoorOutIcon,
  LockIcon,
  MomentIcon,
  UnlockIcon,
} from "../../icons";
import Logger from "../../utility/Logger";
import AeroDoorForm from "./AeroDoorForm";
import Helper from "../../utility/Helper";
import {
  AeroDoorMetadata,
  AltrReader,
  Antipassback,
  DoorDto,
  ReaderIn,
  ReaderOut,
  Relay,
  Rex,
  Sensor,
} from "../../model/Door/DoorDto";
import { StatusDto } from "../../model/StatusDto";
import { useToast } from "../../context/ToastContext";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { BaseTable } from "../UiElements/BaseTable";
import SignalRService from "../../services/SignalRService";
import { ActionButton } from "../../model/ActionButton";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { TableCell } from "../../components/ui/table";
import Badge from "../../components/ui/badge/Badge";
import { DoorToast } from "../../model/ToastMessage";
import { usePagination } from "../../context/PaginationContext";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { AcrStatus as AcrStatus } from "../../model/Door/AcrStatus";
import { DoorDirection } from "../../enum/DoorDirection";
import DoorForm from "./DoorForm";
import { DoorType } from "../../enum/DoorType";

// ACR Page
export const DOOR_TABLE_HEADER: string[] = [
  "Name",
  "Door Type",
  "Status",
  "",
  "Action",
];
export const DOOR_KEY: string[] = ["name", "doorType"];

// Default Value

const Door = () => {
  const { filterPermission } = useAuth();
  const { toggleToast } = useToast();
  const { locationGuid: locationId } = useLocation();
  const { setPagination } = usePagination();
  const {
    setRemove,
    setConfirmRemove,
    setConfirmCreate,
    setCreate,
    setUpdate,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const defaultDoorDto: DoorDto = {
    id: 0,
    componentId: -1,
    name: "",
    deviceComponentId: -1,
    mac: "",
    doorType: "",
    metadata: "",
    locationId: locationId,
    type: "",
    isActive: false,
    secondComponentId: -1,
  };
  const [doorDto, setDoorDto] = useState<DoorDto>(defaultDoorDto);
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
    console.log(e.currentTarget.value);
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
          var data: number[] = [];
          selectedObjects.map(async (a: DoorDto) => {
            data.push(a.id);
          });
          var res = await send.post(DoorEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              DoorToast.DELETE_RANGE,
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
          doorDto.metadata = JSON.stringify(doorDto.metadata);
          const res = await send.post(DoorEndpoint.CREATE, doorDto);
          if (Helper.handleToastByResCode(res, DoorToast.CREATE, toggleToast)) {
            setForm(false);
            setDoorDto(defaultDoorDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          doorDto.metadata = JSON.stringify(doorDto.metadata);
          const res = await send.put(DoorEndpoint.UPDATE, doorDto);
          if (Helper.handleToastByResCode(res, DoorToast.UPDATE, toggleToast)) {
            setForm(false);
            setDoorDto(defaultDoorDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setDoorDto(defaultDoorDto);
        setForm(false);
        break;
      case "unlock":
        selectedObjects.map((a) => {
          changeDoorMode(a.id, a.scpId, a.acrId, 2);
        });
        break;
      case "lock":
        selectedObjects.map((a) => {
          changeDoorMode(a.id, a.scpId, a.acrId, 3);
        });
        break;
      case "moment":
        selectedObjects.map((a) => {
          unlockDoor(a.id);
        });
        break;
      case "secure":
        selectedObjects.map((a) => {
          console.log(a);
          changeDoorMode(a.id, a.scpId, a.acrId, a.defaultMode);
        });
        break;
      case "disable":
        selectedObjects.map((a) => {
          changeDoorMode(a.id, a.scpId, a.acrId, 1);
        });
        break;
      default:
        break;
    }
  };

  const handleRemove = (data: DoorDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(DoorEndpoint.DELETE(data.id));
      if (Helper.handleToastByResCode(res, DoorToast.DELETE, toggleToast)) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: DoorDto) => {
    setDoorDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: DoorDto) => {
    setDoorDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  {
    /* Door Data */
  }
  const [doorsDto, setDoorsDto] = useState<DoorDto[]>([]);
  const [status, setStatus] = useState<StatusDto[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      DoorEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationId,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res);
    if (res && res.data) {
      setDoorsDto(res.data.items);
      setPagination(res.data);

      // Batch set state
      const newStatuses = res.data.data.data.map((a: DoorDto) => ({
        scpId: a.scpId,
        driverId: a.acrId,
        status: 0,
        tamper: a.modeDesc,
        ac: 0,
        batt: 0,
      }));

      console.log(">>>>>>>>>." + JSON.stringify(newStatuses));

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.data.data.forEach((a: DoorDto) => {
        fetchStatus(a.id);
      });
    }
  };
  const fetchStatus = async (id: number) => {
    const res = await send.get(DoorEndpoint.GET_ACR_STATUS(id));
    Logger.info(res);
  };

  const changeDoorMode = async (
    id: number,
    scpId: number,
    acrId: number,
    mode: number,
  ) => {
    const data = {
      id,
      scpId,
      acrId,
      mode,
    };
    const res = await send.post(DoorEndpoint.POST_ACR_CHANGE_MODE, data);
    Logger.info(res);
  };
  const unlockDoor = async (id: number) => {
    const res = await send.post(DoorEndpoint.POST_ACR_UNLOCK(id));
    Logger.info(res);
  };
  {
    /* UseEffect */
  }
  useEffect(() => {
    var connection = SignalRService.getConnection();
    connection.on("ACR.STATUS", (status: AcrStatus) => {
      setStatus((prev) =>
        prev.map((a) =>
          a.deviceGuid == status.scpId && a.componentId == status.number
            ? {
                ...a,
                status: status.status == "" ? a.status : status.status,
                tamper: status.mode == "" ? a.tamper : status.mode,
              }
            : {
                ...a,
              },
        ),
      );
      toggleRefresh();
    });
  }, []);

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<DoorDto[]>([]);

  const action: ActionButton[] = [
    {
      lable: "secure",
      buttonName: "Secure (Default Mode)",
      icon: <MomentIcon />,
    },
    {
      lable: "moment",
      buttonName: "Toggle Door",
      icon: <ControlIcon />,
    },
    {
      lable: "unlock",
      buttonName: "Unlock",
      icon: <UnlockIcon />,
    },
    {
      lable: "lock",
      buttonName: "Lock",
      icon: <LockIcon />,
    },
    {
      lable: "disable",
      buttonName: "Disable",
      icon: <DisableIcon />,
    },
  ];

  const content: FormContent[] = [
    {
      label: "Door",
      content: (
        <DoorForm
          handleClick={handleClick}
          dto={doorDto}
          setDto={setDoorDto}
          type={formType}
        />
      ),
      icon: <DoorIcon />,
    },
  ];

  const filterComponet = (data: any, statusDto: StatusDto[]) => {
    return [
      <>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            <Badge size="sm" color="dark">
              {statusDto.find((b) => b.deviceGuid == data.scpId)?.tamper}
            </Badge>
          </>
        </TableCell>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            {statusDto.find((b) => b.deviceGuid == data.scpId)?.status ===
            "Secure" ? (
              <Badge size="sm" color="success">
                {statusDto.find((b) => b.deviceGuid == data.scpId)?.status}
              </Badge>
            ) : statusDto.find((b) => b.deviceGuid == data.scpId)?.status ===
                "Forced Open" ||
              statusDto.find((b) => b.deviceGuid == data.scpId)?.status ===
                "Locked" ? (
              <Badge size="sm" color="error">
                {statusDto.find((b) => b.deviceGuid == data.scpId)?.status}
              </Badge>
            ) : (
              <Badge size="sm" color="warning">
                {statusDto.find((b) => b.deviceGuid == data.scpId)?.status === 0
                  ? "Error"
                  : statusDto.find((b) => b.deviceGuid == data.scpId)?.status}
              </Badge>
            )}
          </>
        </TableCell>
      </>,
    ];
  };

  return (
    <>
      <PageBreadcrumb pageTitle="Doors" />
      {form ? (
        <BaseForm tabContent={content} header={""} desc={""} />
      ) : (
        <BaseTable<DoorDto>
          headers={DOOR_TABLE_HEADER}
          keys={DOOR_KEY}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          onInfo={handleInfo}
          onClick={handleClick}
          onEdit={handleEdit}
          onRemove={handleRemove}
          data={doorsDto}
          status={status}
          action={action}
          permission={filterPermission(FeatureId.acr)}
          renderOptionalComponent={filterComponet}
          fetchData={fetchData}
          locationGuid={locationId}
          refresh={refresh}
          specialDisplay={[
            {
              key: "doorType",
              content: (d) => (
                <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                  {d.doorType == DoorType[DoorType.Dual] ? (
                    <div className="flex items-center gap-2">
                      <DoorInIcon fontSize={20} />
                      <DoorOutIcon fontSize={20} />
                    </div>
                  ) : d.doorType == DoorType[DoorType.Single] ? (
                    <div className="flex items-center gap-5">
                      <DoorInIcon fontSize={20} />
                    </div>
                  ) : (
                    <div className="flex items-center gap-5">
                      <DoorOutIcon fontSize={20} />
                    </div>
                  )}
                </TableCell>
              ),
            },
          ]}
        />
      )}
    </>
  );
};

export default Door;
