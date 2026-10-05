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
  MonitorIcon,
  OnIcon,
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
import DoorGeneralForm from "./DoorGeneralForm";
import DoorMonitorForm from "./DoorMonitorForm";
import DoorBuzzerForm from "./DoorBuzzerForm";
import DoorRelayForm from "./DoorRelayForm";
import { Options } from "../../model/Options";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { TimezoneEndPoint } from "../../endpoint/TimezoneEndpoint";
import DoorBGForm from "./DoorBGForm";
import { ReaderDto } from "../../model/Door/ReaderDto";
import { RexDto } from "../../model/Door/RexDto";
import { Status } from "../../enum/Status";
import { DoorStatus } from "../../enum/DoorStatus";
import { DoorMode } from "../../enum/DoorMode";
import { ReaderStatus } from "../../enum/ReaderStatus";
import { InputStatus } from "../../enum/InputStatus";
import { ReaderDirection } from "../../enum/DoorDirection";
import { AeroReaderMetadata } from "../../model/Door/AeroReaderMetadata";

// ACR Page
const DOOR_TABLE_HEADER: string[] = [
  "Name",
  "Door Type",
  "Mode",
  "Status",
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
    rexes: [],
    sensor: null,
    relay: null,
    bg: null,
    locationGuid: locationGuid,
    locationName: "",
    isActive: false,
    isDefault: false,
    vendor: Vendor.aero,
    type: DoorType.Single,
    deviceGuid: "",
    deviceName: ""
  };
  const [dto, setDto] = useState<DoorDto>(defaultDoorDto);
  const [refresh, setRefresh] = useState(false);
  const [status, setStatus] = useState<StatusDto[]>([]);
  const toggleRefresh = () => setRefresh(!refresh);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [isNext, setIsNext] = useState<boolean>(false);


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
          // Reader
          if (dto.readers.length > 0) {
            dto.readers = dto.readers.map((a: ReaderDto) => ({
              ...a,
              metadata: typeof a.metadata === 'string' ? a.metadata : JSON.stringify(a.metadata)
            }))
          }
          // Relay
          if (dto.relay != null) {
            dto.relay.metadata = typeof dto.relay.metadata === 'string' ? dto.relay.metadata : JSON.stringify(dto.relay.metadata)
          }
          // Rex
          if (dto.rexes.length > 0) {
            dto.rexes = dto.rexes.map((a: RexDto) => ({
              ...a,
              metadata: typeof a.metadata === 'string' ? a.metadata : JSON.stringify(a.metadata)
            }))
          }
          // Sensor
          if (dto.sensor != null) {
            dto.sensor.metadata = typeof dto.sensor.metadata === 'string' ? dto.sensor.metadata : JSON.stringify(dto.sensor.metadata)
          }
          // Buzzer
          if (dto.buzzer != null) {
            dto.buzzer.metadata = typeof dto.buzzer.metadata === 'string' ? dto.buzzer.metadata : JSON.stringify(dto.buzzer.metadata)
          }
          // Bg
          if (dto.bg != null) {
            dto.bg.metadata = typeof dto.bg.metadata === 'string' ? dto.bg.metadata : JSON.stringify(dto.bg.metadata)
          }
          dto.metadata = typeof dto.metadata === 'string' ? dto.metadata : JSON.stringify(dto.metadata)
          dto.locationGuid = locationGuid;
          const res = await send.post(DoorEndpoint.CREATE, dto);
          if (Helper.handleToastByResCode(res, DoorToast.CREATE, toggleToast)) {
            setForm(false);
            setDto(defaultDoorDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          dto.metadata = JSON.stringify(dto.metadata);
          const res = await send.put(DoorEndpoint.UPDATE, dto);
          if (Helper.handleToastByResCode(res, DoorToast.UPDATE, toggleToast)) {
            setForm(false);
            setDto(defaultDoorDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setDto(defaultDoorDto);
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

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => {
    switch (e.target.name) {
      case "deviceGuid":
        fetchModule(e.target.value);
        break;
      case "readerIn.module":
        fetchReader(e.target.value)
        break;
      case "readerOut.module":
        fetchReader(e.target.value)
        break;
      case "rex.module":
        fetchInput(e.target.value);
        break;
      case "relay.module":
        fetchOutput(e.target.value);
        break;
      case "sensor.module":
        fetchInput(e.target.value);
        break;
      case "bg.module":
        fetchInput(e.target.value);
        break;
      case "buzzer.module":
        fetchOutput(e.target.value)
        break;
      default:
        break;
    }
  }

  const handleRemove = (data: DoorDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(DoorEndpoint.DELETE(data.guid));
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
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: DoorDto) => {
    setDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  {
    /* Door Data */
  }
  const [doorsDto, setDoorsDto] = useState<DoorDto[]>([]);
  const [deviceOptions, setDeviceOptions] = useState<Options[]>([]);
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const [readerOption, setReaderOption] = useState<Options[]>([]);
  const [inputOption, setInputOption] = useState<Options[]>([]);
  const [outputOption, setOutputOption] = useState<Options[]>([]);
  const [timeOption, setTimeOption] = useState<Options[]>([]);


  const fetchDevice = async () => {
    var res = await send.get(DeviceEndpoint.GET_LOCATION(locationGuid))
    var option = res.data.data.map((a: DeviceDto) => ({
      value: a.guid,
      label: a.name,
      description: a.mac,
      isTaken: false
    }))

    setDeviceOptions(option)

  }

  const fetchModule = async (guid: string) => {
    var res = await send.get(ModuleEndpoint.GET_BY_GUID(guid))
    var option = res.data.data.map((a: DeviceDto) => ({
      value: a.guid,
      label: a.name,
      description: a.mac,
      isTaken: false
    }))

    setModuleOption(option)
  }

  const fetchReader = async (guid: string) => {
    var res = await send.get(ModuleEndpoint.GET_READER_SLOT(guid))
    setReaderOption(res.data.data);
  }

  const fetchInput = async (guid: string) => {
    var res = await send.get(ModuleEndpoint.GET_INPUT_SLOT(guid))
    setInputOption(res.data.data);
  }

  const fetchOutput = async (guid: string) => {
    var res = await send.get(ModuleEndpoint.GET_OUTPUT_SLOT(guid))
    setOutputOption(res.data.data);
  }

  const fetchTime = async (guid: string) => {
    var res = await send.get(TimezoneEndPoint.GET_OPTION_BY_LOCATION(guid))
    setTimeOption(res.data.data);
  }
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
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

      // 1. Map over the items and parse the metadata string
      const parsedItems = res.data.data.items.map((door: DoorDto) => {
        let parsedMetadata = null; // Default value if empty or parsing fails
        let parsedReaderInMetadata = null;
        let parsedReaderOutMetadata = null;
        let parsedSensorMetadata = null;
        let parsedRelayMetadata = null;
        let parsedRex0Metadata = null;
        let parsedRex1Metadata = null;

        let readerIn = door.readers.find(x => x.readerDirection == ReaderDirection.In)?.metadata
        let readerOut = door.readers.find(x => x.readerDirection == ReaderDirection.Out)?.metadata
        let sensor = door.sensor?.metadata;
        let relay = door.relay?.metadata;
        let rex0 = door.rexes.length > 0 && door.rexes[0].metadata;
        let rex1 = door.rexes.length > 1 && door.rexes[1].metadata;


        parsedMetadata = door.metadata == undefined || door.metadata == "" ? "" : JSON.parse(door.metadata);
        parsedReaderInMetadata = readerIn == "" || readerIn == undefined ? "" : JSON.parse(readerIn as string);
        parsedReaderOutMetadata = readerOut == "" || readerOut == undefined ? "" : JSON.parse(readerOut as string);
        parsedSensorMetadata = sensor == "" || sensor == undefined ? "" : JSON.parse(sensor as string);
        parsedRelayMetadata = relay == "" || relay == undefined ? "" : JSON.parse(relay as string);
        parsedRex0Metadata = rex0 == "" || rex0 == undefined ? "" : JSON.parse(rex0 as string);
        parsedRex1Metadata = rex1 == "" || rex1 == undefined ? "" : JSON.parse(rex1 as string);

        return {
          ...door,
          metadata: parsedMetadata,
           readers:door.readers.map(x =>  
            x.readerDirection == ReaderDirection.In ? 
            {
              ...x,
              metadata:parsedReaderInMetadata
            }: x.readerDirection == ReaderDirection.Out ? {
              ...x,
              metadata:parsedReaderOutMetadata
            }:x
           ),
          sensor:{
            ...door.sensor,
            metadata:parsedSensorMetadata
          },
          relay:{
            ...door.relay,
            metadata:parsedRelayMetadata
          },
          rexes:door.rexes.map((x,i) => 
          i == 0 ? {
            ...x,
            metadata : parsedRex0Metadata
          } : i == 1 ? {
            ...x,
            metadata: parsedRex1Metadata         
          } : x
          )
         
        };
      });

      setDoorsDto(parsedItems);
      setPagination(res.data.data);

      // Batch set state
      const newStatuses = res.data.data.items.map((a: DoorDto) => ({
        guid: a.guid,
        status: DoorStatus.Unknown,
        altr1: DoorMode.Unknown,
        altr2: ReaderStatus.Unknown,
        altr3: InputStatus.Unknown, // Strile
        altr4: InputStatus.Unknown, // rex

      }));

      console.log(">>>>>>>>>." + JSON.stringify(newStatuses));

      setStatus((prev) => [...prev, ...newStatuses]);

      // Fetch status for each
      res.data.data.items.forEach((a: DoorDto) => {
        fetchStatus(a.guid);
      });
    }
  };
  const fetchStatus = async (guid: string) => {
    const res = await send.get(DoorEndpoint.STATUS(guid));
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
        <DoorGeneralForm fetchDevice={fetchDevice} handleChange={handleChange} type={formType} dto={dto} deviceOption={deviceOptions} setDto={setDto} setIsNext={setIsNext} />
      ),
      title: "General Information",
      description: "General door information",
    },

    ...(dto.vendor === Vendor.aero) ?
      [
        {
          label: "Door In",
          icon: <DoorInIcon />,
          content: <DoorInForm dto={dto} setDto={setDto} type={formType} moduleOption={moduleOption} handleChange={handleChange} readerOption={readerOption} setIsNext={setIsNext} />,
        },
        ...(dto.type === DoorType.Dual
          ? [
            {
              label: "Door Out",
              icon: <DoorOutIcon />,
              content: (
                <DoorOutForm dto={dto} setDto={setDto} type={formType} moduleOption={moduleOption} handleChange={handleChange} readerOption={readerOption} setIsNext={setIsNext} />
              ),
            },
          ]
          : [
            {
              label: "Rex Out",
              icon: <DoorOutIcon />,
              content: (
                <DoorRexOutForm
                  dto={dto}
                  setDto={setDto}
                  type={formType}
                  inputOption={inputOption}
                  handleChange={handleChange}
                  moduleOption={moduleOption}
                  fetchTime={fetchTime}
                  timeOption={timeOption}
                  setIsNext={setIsNext}
                  setInputOption={setInputOption}
                />
              ),
            },
          ]),
        {
          label: "Relay",
          icon: <DoorIcon />,
          content: <DoorRelayForm dto={dto} setDto={setDto} type={formType} handleChange={handleChange} moduleOption={moduleOption} outputOption={outputOption} setIsNext={setIsNext} />,
        },
        {
          label: "Sensor",
          icon: <MonitorIcon />,
          content: <DoorMonitorForm dto={dto} setDto={setDto} type={formType} moduleOption={moduleOption} handleChange={handleChange} inputOption={inputOption} setIsNext={setIsNext} />,
        },
        {
          label: "Buzzer",
          icon: <OnIcon />,
          content: <DoorBuzzerForm dto={dto} setDto={setDto} type={formType} moduleOption={moduleOption} outputOption={outputOption} handleChange={handleChange} setIsNext={setIsNext} />,
        },
        {
          label: "Break Glass",
          icon: <OnIcon />,
          content: <DoorBGForm dto={dto} setDto={setDto} type={formType} moduleOption={moduleOption} handleChange={handleChange} inputOption={inputOption} setIsNext={setIsNext} />,
        },

      ] : [],


    ...(dto.vendor === Vendor.amico
      ? [
        /* Add your Amico-specific form object here */
      ]
      : []),
  ];

  const renderOptional = (item: DoorDto, statusDto: StatusDto[], i: number) => {
    return [
      <React.Fragment key={i + 1}>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            <Badge size="sm" color="dark">
              {DoorMode[statusDto.find((b) => b.guid == item.guid)?.altr4 as DoorMode] ?? "Unknown"}
            </Badge>
          </>
        </TableCell>
        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
          <>
            {statusDto.find((b) => b.guid == item.guid)?.status === DoorStatus.Secure ? (
              <Badge size="sm" color="success">
                {DoorStatus[DoorStatus.Secure]}
              </Badge>
            ) : statusDto.find((b) => b.guid == item.guid)?.status === DoorStatus.Forced ||
              statusDto.find((b) => b.guid == item.guid)?.status === DoorStatus.Locked ? (
              <Badge size="sm" color="error">
                {statusDto.find((b) => b.guid == item.guid)?.status == DoorStatus.Forced ? DoorStatus[DoorStatus.Forced] : DoorStatus[DoorStatus.Locked] ?? "Unknown"}
              </Badge>
            ) : (
              <Badge size="sm" color="warning">
                {DoorStatus[statusDto.find((b) => b.guid == item.guid)?.status as DoorStatus] ?? "Unknown"}
              </Badge>
            )}
          </>
        </TableCell>
      </React.Fragment>,
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

          {dto.type == DoorType.Dual
            ? device("readerOut", 665, 255, 16, 38)
            : device("rex", 665, 255, 16, 38)}
          <text x="700" y="280" fill="#09090b" fontSize="15" fontWeight="600">
            {dto.type == DoorType.Dual ? "Reader" : "REX"}
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
          isNext={isNext}
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
          status={status}
          action={action}
          permission={filterPermission(FeatureId.acr)}
          renderOptionalComponent={renderOptional}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
          specialDisplay={[
            {
              key: "doorType",
              content: (d, i) => (
                <TableCell key={i} className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                  {d.type == DoorType.Dual ? (
                    <div className="flex items-center gap-2">
                      <DoorInIcon fontSize={20} />
                      <DoorOutIcon fontSize={20} />
                      {DoorType[d.type]}
                    </div>
                  ) : d.type == DoorType.Single ? (
                    <div className="flex items-center gap-2">
                      <DoorInIcon fontSize={20} />
                      {DoorType[d.type]}
                    </div>
                  ) : (
                    <div className="flex items-center gap-2">
                      <DoorOutIcon fontSize={20} />
                      {DoorType[d.type]}
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
