import React, { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import {
  ControlIcon,
  DisableIcon,
  DoorIcon,
  DoorInIcon,
  DoorOutIcon,
  LockIcon,
  ModuleIcon,
  MomentIcon,
  UnlockIcon,
} from "../../icons";
import Logger from "../../utility/Logger";
import Helper from "../../utility/Helper";
import { DoorDto } from "../../model/Door/DoorDto";
import { StatusDto } from "../../model/StatusDto";
import { useToast } from "../../context/ToastContext";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { BaseTable } from "../UiElements/BaseTable";
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
import { DoorType } from "../../enum/DoorType";
import { Vendor } from "../../enum/Vendor";
import DoorInForm from "./DoorInForm";
import DoorOutForm from "./DoorOutForm";
import DoorRexOutForm from "./DoorRexOutForm";
import DoorGeneralForm from "./DoorGeneratForm";

// ACR Page
const DOOR_TABLE_HEADER: string[] = [
  "Name",
  "Door Type",
  "Status",
  "",
  "Action",
];
const DOOR_KEY: string[] = ["name", "doorType"];

// Default Value

const Door = () => {
  const { filterPermission } = useAuth();
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
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
    guid: "",
    name: "",
    metadata: "",
    readers: [],
    buzzer: null,
    rex: null,
    sensor: null,
    locationGuid: locationGuid,
    locationName: "",
    isActive: false,
    isDefault: false,
    vendor: Vendor.aero,
    type: DoorType.Single,
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
          const data: string[] = [];
          selectedObjects.map(async (a: DoorDto) => {
            data.push(a.guid);
          });
          const res = await send.post(DoorEndpoint.DELETE_RANGE, data);
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
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    search?: string,
    startDate?: string,
    endDate?: string,
    locationGuid?: string,
  ) => {
    const res = await send.get(
      DoorEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res);
    if (res.data) {
      setDoorsDto(res.data.data.items);
      setPagination(res.data.data);

      // Batch set state
      // const newStatuses = res.data.data.data.map((a: DoorDto) => ({
      //   scpId: a.scpId,
      //   driverId: a.acrId,
      //   status: 0,
      //   tamper: a.modeDesc,
      //   ac: 0,
      //   batt: 0,
      // }));

      // console.log(">>>>>>>>>." + JSON.stringify(newStatuses));

      // setStatus((prev) => [...prev, ...newStatuses]);

      // // Fetch status for each
      // res.data.data.data.forEach((a: DoorDto) => {
      //   fetchStatus(a.id);
      // });
    }
  };
  // const fetchStatus = async (id: number) => {
  //   const res = await send.get(DoorEndpoint.GET_ACR_STATUS(id));
  //   Logger.info(res);
  // };

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
  // useEffect(() => {
  //   var connection = SignalRService.getConnection();
  //   connection.on("ACR.STATUS", (status: AcrStatus) => {
  //     setStatus((prev) =>
  //       prev.map((a) =>
  //         a.guid == status.scpId && a.componentId == status.number
  //           ? {
  //               ...a,
  //               status: status.status == "" ? a.status : status.status,
  //               tamper: status.mode == "" ? a.tamper : status.mode,
  //             }
  //           : {
  //               ...a,
  //             },
  //       ),
  //     );
  //     toggleRefresh();
  //   });
  // }, []);

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

  // const content: FormContent[] = [
  //   {
  //     label: "Door",
  //     content: (
  //       <DoorForm
  //         handleClick={handleClick}
  //         dto={doorDto}
  //         setDto={setDoorDto}
  //         type={formType}
  //       />
  //     ),
  //     icon: <DoorIcon />,
  //   },
  // ];

  const content: FormContent[] = [
    {
      label: "General",
      icon: <DoorIcon />,
      content: (
        <DoorGeneralForm dto={doorDto} setDto={setDoorDto} type={formType} />
      ),
      title: "General Information",
      description: "General door information",
    },
    {
      label: "Door In",
      icon: <DoorIcon />,
      content: <DoorInForm dto={doorDto} setDto={setDoorDto} type={formType} />,
    },
    ...(doorDto.type === DoorType.Dual
      ? [
          {
            label: "Door Out",
            icon: <DoorIcon />,
            content: (
              <DoorOutForm dto={doorDto} setDto={setDoorDto} type={formType} />
            ),
          },
        ]
      : [
          {
            label: "Rex Out",
            icon: <DoorIcon />,
            content: (
              <DoorRexOutForm
                dto={doorDto}
                setDto={setDoorDto}
                type={formType}
              />
            ),
          },
        ]),

    ...(doorDto.vendor === Vendor.amico
      ? [
          /* Add your Amico-specific form object here */
        ]
      : []),
  ];

  const filterComponet = (data: any, statusDto: StatusDto[]) => {
    return [
      <>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            <Badge size="sm" color="dark">
              {statusDto.find((b) => b.guid == data.scpId)?.tamper}
            </Badge>
          </>
        </TableCell>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            {statusDto.find((b) => b.guid == data.scpId)?.status ===
            "Secure" ? (
              <Badge size="sm" color="success">
                {statusDto.find((b) => b.guid == data.scpId)?.status}
              </Badge>
            ) : statusDto.find((b) => b.guid == data.scpId)?.status ===
                "Forced Open" ||
              statusDto.find((b) => b.guid == data.scpId)?.status ===
                "Locked" ? (
              <Badge size="sm" color="error">
                {statusDto.find((b) => b.guid == data.scpId)?.status}
              </Badge>
            ) : (
              <Badge size="sm" color="warning">
                {statusDto.find((b) => b.guid == data.scpId)?.status === 0
                  ? "Error"
                  : statusDto.find((b) => b.guid == data.scpId)?.status}
              </Badge>
            )}
          </>
        </TableCell>
      </>,
    ];
  };

  {
    /* LAY OUT */
  }

  type DoorComponent =
    | "readerIn"
    | "readerOut"
    | "rex"
    | "magneticLock"
    | "buzzer"
    | "bg"
    | "sensor";

  // type DoorAccessLayout = "inOut" | "inOnly";

  // const [accessLayout, setAccessLayout] = useState<DoorAccessLayout>(
  //   doorDto.type === DoorType.Dual ? "inOut" : "inOnly",
  // );

  // const hasReaderOut = accessLayout === "inOut";

  const [selectedComponent, setSelectedComponent] =
    useState<DoorComponent>("readerIn");

  const DoorLayout = ({
    selected,
    onSelect,
  }: {
    selected: DoorComponent;
    onSelect: (component: DoorComponent) => void;
  }) => {
    const device = (
      id: DoorComponent,
      x: number,
      y: number,
      width: number,
      height: number,
    ) => {
      const active = selected === id;
      return (
        <g className="cursor-pointer" onClick={() => onSelect(id)}>
          <rect
            x={x}
            y={y}
            width={width}
            height={height}
            rx="4"
            fill={active ? "#e0f2fe" : "#72b6dc"}
            stroke={active ? "#0284c7" : "#4f87a8"}
            strokeWidth={active ? "2.5" : "1.5"}
          />
        </g>
      );
    };

    return (
      <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-100">
        <svg
          aria-label="Single ACS door accessory layout"
          className="mx-auto min-w-[760px]"
          viewBox="0 0 900 500"
          role="img"
        >
          <text x="28" y="48" fill="#3f3f46" fontSize="20" fontWeight="700">
            ACS DOOR ACCESSORY
          </text>
          <rect
            x="160"
            y="150"
            width="150"
            height="250"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="2"
          />
          <rect
            x="172"
            y="162"
            width="126"
            height="226"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="1.5"
          />
          <circle
            cx="188"
            cy="270"
            r="8"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="1.5"
          />
          {device("readerIn", 102, 252, 16, 38)}
          <text
            x="110"
            y="320"
            textAnchor="middle"
            fill="#09090b"
            fontSize="15"
            fontWeight="600"
          >
            Reader
          </text>
          <text
            x="235"
            y="433"
            textAnchor="middle"
            fill="#09090b"
            fontSize="16"
            fontWeight="600"
          >
            Outside
          </text>

          <rect
            x="500"
            y="150"
            width="150"
            height="250"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="2"
          />
          <rect
            x="512"
            y="162"
            width="126"
            height="226"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="1.5"
          />
          <circle
            cx="622"
            cy="270"
            r="8"
            fill="#fff"
            stroke="#09090b"
            strokeWidth="1.5"
          />
          {device("buzzer", 450, 144, 28, 28)}
          <text x="445" y="135" fill="#09090b" fontSize="15" fontWeight="600">
            Buzzer
          </text>
          {device("magneticLock", 515, 144, 58, 22)}
          <text
            x="544"
            y="135"
            textAnchor="middle"
            fill="#09090b"
            fontSize="15"
            fontWeight="600"
          >
            Lock
          </text>
          {device("sensor", 578, 145, 22, 10)}
          <text x="590" y="135" fill="#09090b" fontSize="15" fontWeight="600">
            Sensor
          </text>
          {device("bg", 660, 144, 28, 28)}
          <rect
            x="665"
            y="152"
            width="18"
            height="10"
            rx="1"
            fill="none"
            stroke={selected === "buzzer" ? "#0284c7" : "#4f87a8"}
            strokeWidth="1.5"
          />
          <text x="700" y="160" fill="#09090b" fontSize="15" fontWeight="600">
            Break Glass
          </text>

          {doorDto.type == DoorType.Dual
            ? device("readerOut", 665, 255, 16, 38)
            : device("rex", 665, 255, 16, 38)}
          <text x="700" y="280" fill="#09090b" fontSize="15" fontWeight="600">
            {doorDto.type == DoorType.Dual ? "Reader" : "REX"}
          </text>
          <text
            x="580"
            y="433"
            textAnchor="middle"
            fill="#09090b"
            fontSize="16"
            fontWeight="600"
          >
            Inside
          </text>
        </svg>
      </div>
    );
  };

  return (
    <>
      <PageBreadcrumb pageTitle="Doors" />
      {form ? (
        <BaseForm
          handleClick={handleClick}
          type={formType}
          tabContent={content}
          header={""}
          desc={""}
          layout={
            <DoorLayout
              selected={selectedComponent}
              onSelect={setSelectedComponent}
            />
          }
        />
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
          // status={status}
          action={action}
          permission={filterPermission(FeatureId.acr)}
          renderOptionalComponent={filterComponet}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
          specialDisplay={[
            {
              key: "doorType",
              content: (d) => (
                <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                  {d.type == DoorType.Dual ? (
                    <div className="flex items-center gap-2">
                      <DoorInIcon fontSize={20} />
                      <DoorOutIcon fontSize={20} />
                    </div>
                  ) : d.type == DoorType.Single ? (
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
