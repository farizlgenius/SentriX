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
  ReaderIcon,
  RelayOffIcon,
  RelayOnIcon,
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
import { TableCell, TableRow } from "../../components/ui/table";
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
import { DoorMode } from "../../enum/DoorMode";
import { ReaderStatus } from "../../enum/ReaderStatus";
import { InputStatus } from "../../enum/InputStatus";
import { ReaderDirection } from "../../enum/DoorDirection";
import { ReaderMode } from "../../enum/ReaderMode";
import { AeroReaderMetadata } from "../../model/Door/AeroReaderMetadata";
import SignalRService from "../../services/SignalRService";
import { SignalRTopic } from "../../constants/signalr-constant";
import DoorAdvanceForm from "./DoorAdvanceForm";
import { AeroDoorMetadata } from "../../model/Door/AeroDoorMetadata";
import { DoorCondition } from "../../enum/DoorCondition";
import { DoorStatus } from "../../model/Status/DoorStatus";
import { StatusHelper } from "../../helper/StatusHelper";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";
import { DoorExtendFeatureType } from "../../enum/DoorExtendFeatureType";

// ACR Page
const DOOR_TABLE_HEADER: string[] = [
  "Name",
  "Type",
];
const DOOR_KEY: string[] = ["name", "doorType"];

// Default Value

const Door = () => {
  const { filterPermission,token } = useAuth();
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
    metadata: {
      offlineMode: DoorMode.Unlocked,
      defaultMode: DoorMode.Card,
      defaultLedMode: 0,
      forceCardPin: false,
      doubleCard: true,
      outputSelectionTracking: false,
      lockedOverrid: true,
      decreaseUseLimit: false,
      requireUseLimit: false,
      deniedDuress: false,
      quietRex: false,
      filterStatus: false,
      doubleCardAccess: false,
      hostPermission: false,
      hostOfflineGrant: false
    },
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
    deviceName: "",
    followerRelay: null,
    extendFeatureType: DoorExtendFeatureType.NONE,
    interiorPushButton: null,
    interiorPushOutputButton : null
  };
  const [dto, setDto] = useState<DoorDto>(defaultDoorDto);
  const [refresh, setRefresh] = useState(false);
  const [status, setStatus] = useState<DoorStatus[]>([]);
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
        setActiveTab("General");
        setSelectedComponent("readerIn");
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
          changeDoorMode(a.guid, DoorMode.Unlocked);
        });
        break;
      case "lock":
        selectedObjects.map((a) => {
          changeDoorMode(a.guid, DoorMode.Locked);
        });
        break;
      case "moment":
        selectedObjects.map((a) => {
          unlockDoor(a.guid);
        });
        break;
      case "secure":
        selectedObjects.map((a) => {
          console.log(a);
          changeDoorMode(a.guid, (a.metadata as AeroDoorMetadata).defaultMode ?? DoorMode.Card);
        });
        break;
      case "disable":
        selectedObjects.map((a) => {
          changeDoorMode(a.guid, DoorMode.Disabled);
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
    setActiveTab("General");
    setSelectedComponent("readerIn");
    setForm(true);
  };

  const handleInfo = (data: DoorDto) => {
    setDto(data);
    setFormType(FormType.INFO);
    setActiveTab("General");
    setSelectedComponent("readerIn");
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


        parsedMetadata = door.metadata == undefined || door.metadata == "" ? "" : JSON.parse(door.metadata as string);
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
          sensor: door.sensor
            ? {
                ...door.sensor,
                metadata: parsedSensorMetadata,
              }
            : null,
          relay: door.relay
            ? {
                ...door.relay,
                metadata: parsedRelayMetadata,
              }
            : null,
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
      const newStatuses = res.data.data.items.map((a: DoorDto) => {
        const status:DoorStatus = {
          guid: a.guid,
          mode: DoorMode.Unknown,
          readerStatus: ReaderStatus.Unknown,
          strikeStatus: ChangeOfStateEncode.Unknown,
          sensorStatus: ChangeOfStateEncode.Unknown,
          doorStatus: ChangeOfStateEncode.Unknown,
          rex0Status: ChangeOfStateEncode.Unknown,
          rex1Status: ChangeOfStateEncode.Unknown,
          ledModeControl: 0,
          accessControlFlag: 0,
          alternateReaderStatus: ReaderStatus.Unknown,
          extendedAccessControlFlag: 0,
          extendedFeatureType: 0,
          deviceType: 0,
          extendedFeatureStatus: 0
        }
        return status;
      });

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
    guid: string,
    mode: number,
  ) => {
    const data = {
      guid,
      mode,
    };
    const res = await send.post(DoorEndpoint.POST_ACR_CHANGE_MODE, data);
    Logger.info(res);
  };
  const unlockDoor = async (guid: string) => {
    const res = await send.post(DoorEndpoint.POST_ACR_UNLOCK(guid));
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
        {
          label:"Advance",
          icon:<DoorIcon/>,
          content:<DoorAdvanceForm dto={dto} setDto={setDto} type={formType} handleChange={handleChange} setIsNext={setIsNext} />
        }

      ] : [],


    ...(dto.vendor === Vendor.amico
      ? [
        /* Add your Amico-specific form object here */
      ]
      : []),
  ];

  const renderDoorStatusSubTable = (data: DoorDto) => {
    const doorStatus = status.find((item) => item.guid === data.guid);
    const readerIn = StatusHelper.ReaderStatusParser(
      doorStatus?.readerStatus ?? ReaderStatus.Unknown,
    );
    const readerOut = StatusHelper.ReaderStatusParser(
      doorStatus?.alternateReaderStatus ?? ReaderStatus.Unknown,
    );
    const strike = StatusHelper.OutputChangeOfStateIconEncode(
      doorStatus?.strikeStatus ?? ChangeOfStateEncode.Unknown,
    );
    const doorContact = StatusHelper.ChangeOfStateIconEncode(
      doorStatus?.doorStatus ?? ChangeOfStateEncode.Unknown,
    );
    const sensor = StatusHelper.ChangeOfStateIconEncode(
      doorStatus?.sensorStatus ?? ChangeOfStateEncode.Unknown,
    );
    const rex0 = StatusHelper.ChangeOfStateIconEncode(
      doorStatus?.rex0Status ?? ChangeOfStateEncode.Unknown,
    );
    const rex1 = StatusHelper.ChangeOfStateIconEncode(
      doorStatus?.rex1Status ?? ChangeOfStateEncode.Unknown,
    );
    const mode = doorStatus?.mode ?? DoorMode.Unknown;
    const hasReaderIn = data.readers.some(
      (reader) => reader.readerDirection === ReaderDirection.In,
    );
    const hasReaderOut = data.readers.some(
      (reader) => reader.readerDirection === ReaderDirection.Out,
    );
    const formatFlag = (value: number | undefined) =>
      value === undefined
        ? "Unknown"
        : `${value} (0x${(value & 0xffff).toString(16).toUpperCase().padStart(4, "0")})`;
    const renderStatusCard = (
      label: string,
      display: ReturnType<typeof StatusHelper.ReaderStatusParser> | null,
    ) => (
      <div
        key={label}
        className="flex min-w-0 items-center justify-between gap-3 rounded-lg border border-gray-100 bg-white px-3 py-2.5 dark:border-white/[0.08] dark:bg-gray-900"
      >
        <span className="text-sm text-gray-600 dark:text-gray-300">{label}</span>
        {display ? (
          <Badge
            color={display.colorClass}
            size="sm"
            startIcon={<display.Icon className="h-3.5 w-3.5" />}
          >
            {display.label === "light" ? "Unknown" : display.label}
          </Badge>
        ) : (
          <Badge color="light" size="sm">Not configured</Badge>
        )}
      </div>
    );
    const renderValueCard = (label: string, value: string) => (
      <div
        key={label}
        className="flex min-w-0 items-center justify-between gap-3 rounded-lg border border-gray-100 bg-white px-3 py-2.5 dark:border-white/[0.08] dark:bg-gray-900"
      >
        <span className="text-sm text-gray-600 dark:text-gray-300">{label}</span>
        <span className="break-all text-right font-mono text-xs font-medium text-gray-800 dark:text-gray-200">
          {value}
        </span>
      </div>
    );
    const renderSection = (title: string, children: React.ReactNode) => (
      <section className="rounded-xl border border-gray-200 bg-gray-50/70 p-4 dark:border-white/[0.08] dark:bg-white/[0.02]">
        <h3 className="mb-3 text-sm font-semibold text-gray-800 dark:text-white/90">
          {title}
        </h3>
        <div className="grid gap-2 sm:grid-cols-2">{children}</div>
      </section>
    );

    return (
      <TableRow
        key={`${data.guid}-status-details`}
        className="bg-gray-50/70 dark:bg-white/[0.02]"
      >
        <TableCell colspan={DOOR_TABLE_HEADER.length + 3} className="px-5 py-4">
          <div className="overflow-hidden rounded-xl border border-gray-200 bg-white shadow-sm dark:border-white/[0.08] dark:bg-gray-900">
            <div className="flex flex-wrap items-center justify-between gap-3 border-b border-gray-100 px-4 py-3 dark:border-white/[0.05]">
              <div>
                <h2 className="text-sm font-semibold text-gray-800 dark:text-white/90">
                  Live door details
                </h2>
                <p className="mt-0.5 text-xs text-gray-500 dark:text-gray-400">
                  Component states and controller values for {data.name}
                </p>
              </div>
              <Badge color="info" size="sm">
                Mode: {DoorMode[mode] ?? "Unknown"}
              </Badge>
            </div>

            <div className="grid gap-4 p-4 xl:grid-cols-3">
              {renderSection(
                "Access",
                <>
                  {renderStatusCard("Reader in", hasReaderIn ? readerIn : null)}
                  {renderStatusCard("Reader out", hasReaderOut ? readerOut : null)}
                </>,
              )}
              {renderSection(
                "Door hardware",
                <>
                  {renderStatusCard("Strike / lock", data.relay ? strike : null)}
                  {renderStatusCard("Door contact", doorContact)}
                  {renderStatusCard("Sensor", data.sensor ? sensor : null)}
                  {renderStatusCard("REX 0", data.rexes[0] ? rex0 : null)}
                  {renderStatusCard("REX 1", data.rexes[1] ? rex1 : null)}
                </>,
              )}
              {renderSection(
                "Controller details",
                <>
                  {renderValueCard(
                    "LED mode control",
                    String(doorStatus?.ledModeControl ?? "Unknown"),
                  )}
                  {renderValueCard(
                    "Access control flag",
                    formatFlag(doorStatus?.accessControlFlag),
                  )}
                  {renderValueCard(
                    "Extended access control flag",
                    formatFlag(doorStatus?.extendedAccessControlFlag),
                  )}
                  {renderValueCard(
                    "Extended feature type",
                    String(doorStatus?.extendedFeatureType ?? "Unknown"),
                  )}
                  {renderValueCard(
                    "Device type",
                    String(doorStatus?.deviceType ?? "Unknown"),
                  )}
                  {renderValueCard(
                    "Extended feature status",
                    String(doorStatus?.extendedFeatureStatus ?? "Unknown"),
                  )}
                </>,
              )}
            </div>
          </div>
        </TableCell>
      </TableRow>
    );
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
  const [activeTab, setActiveTab] = useState("General");

  const hasAssignedSlot = (
    component: { deviceModuleGuid: string; slotNo: number } | null | undefined,
  ) =>
    component != null &&
    Boolean(component.deviceModuleGuid.trim()) &&
    Number.isFinite(component.slotNo) &&
    component.slotNo >= 0;
  const readerIn = dto.readers.find(
    (reader) => reader.readerDirection === ReaderDirection.In,
  );
  const readerInMetadata = readerIn?.metadata as AeroReaderMetadata;
  const readerInConfigured =
    hasAssignedSlot(readerIn) &&
    (readerIn?.mode !== ReaderMode.osdp ||
      (readerInMetadata?.address !== undefined &&
        readerInMetadata.address !== -1 &&
        readerInMetadata?.baudrate !== undefined &&
        readerInMetadata.baudrate !== -1));
  const configured: Record<DoorComponent, boolean> = {
    readerIn: readerInConfigured,
    readerOut: hasAssignedSlot(
      dto.readers.find(
        (reader) => reader.readerDirection === ReaderDirection.Out,
      ),
    ),
    rex: hasAssignedSlot(dto.rexes[0]),
    magneticLock: hasAssignedSlot(dto.relay),
    buzzer: hasAssignedSlot(dto.buzzer),
    bg: hasAssignedSlot(dto.bg),
    sensor: hasAssignedSlot(dto.sensor),
  };
  const requiredSetup = [
    {
      label: "General information",
      configured: Boolean(dto.name.trim() && dto.deviceGuid.trim()),
    },
    { label: "Reader in", configured: configured.readerIn },
    {
      label: dto.type === DoorType.Dual ? "Reader out" : "REX",
      configured: dto.type === DoorType.Dual ? configured.readerOut : configured.rex,
    },
    { label: "Relay", configured: configured.magneticLock },
    { label: "Sensor", configured: configured.sensor },
  ];
  const missingRequirements = requiredSetup
    .filter((requirement) => !requirement.configured)
    .map((requirement) => requirement.label);
  const canSubmitDoor = missingRequirements.length === 0;

  const componentTabs: Record<DoorComponent, string> = {
    readerIn: "Door In",
    readerOut: "Door Out",
    rex: "Rex Out",
    magneticLock: "Relay",
    buzzer: "Buzzer",
    bg: "Break Glass",
    sensor: "Sensor",
  };
  const tabComponents: Partial<Record<string, DoorComponent>> = {
    "Door In": "readerIn",
    "Door Out": "readerOut",
    "Rex Out": "rex",
    Relay: "magneticLock",
    Buzzer: "buzzer",
    "Break Glass": "bg",
    Sensor: "sensor",
  };

  const handleLayoutSelect = (component: DoorComponent) => {
    setSelectedComponent(component);
    const targetTab = componentTabs[component];
    if (content.some((tab) => tab.label === targetTab)) {
      setActiveTab(targetTab);
    }
  };

  const handleFormTabChange = (tab: string) => {
    setActiveTab(tab);
    const component = tabComponents[tab];
    if (component) {
      setSelectedComponent(component);
    }
  };

  const DoorLayout = ({
    selected,
    onSelect,
  }: {
    selected: DoorComponent;
    onSelect: (component: DoorComponent) => void;
  }) => {
    const labels: Record<DoorComponent, string> = {
      readerIn: "Reader in",
      readerOut: "Reader out",
      rex: "Request to exit",
      magneticLock: "Lock / relay",
      buzzer: "Buzzer",
      bg: "Break glass",
      sensor: "Sensor",
    };
    const device = (
      id: DoorComponent,
      x: number,
      y: number,
      width: number,
      height: number,
    ) => {
      const active = selected === id;
      const isConfigured = configured[id];
      const color = active
        ? { fill: "#dbeafe", stroke: "#2563eb" }
        : isConfigured
          ? { fill: "#dcfce7", stroke: "#16a34a" }
          : { fill: "#f3f4f6", stroke: "#9ca3af" };

      return (
        <g
          className="cursor-pointer"
          role="button"
          tabIndex={0}
          aria-pressed={active}
          aria-label={`${labels[id]}: ${isConfigured ? "configured" : "not configured"}. Select to open settings.`}
          onClick={() => onSelect(id)}
          onKeyDown={(event) => {
            if (event.key === "Enter" || event.key === " ") {
              event.preventDefault();
              onSelect(id);
            }
          }}
        >
          <title>{`${labels[id]} - ${isConfigured ? "Configured" : "Not configured"}`}</title>
          <rect
            x={x}
            y={y}
            width={width}
            height={height}
            rx="4"
            fill={color.fill}
            stroke={color.stroke}
            strokeWidth={active ? "2.5" : "1.5"}
          />
          {isConfigured && (
            <circle
              cx={x + width - 1}
              cy={y + 1}
              r="6"
              fill="#16a34a"
              stroke="#fff"
              strokeWidth="1.5"
              pointerEvents="none"
            />
          )}
        </g>
      );
    };

    return (
      <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-900">
        <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
          <p className="text-sm text-gray-500 dark:text-gray-400">
            Choose a component to jump to its settings.
          </p>
          <div className="flex flex-wrap gap-3 text-xs text-gray-600 dark:text-gray-300">
            <span className="inline-flex items-center gap-1.5">
              <span className="h-2.5 w-2.5 rounded-full bg-green-500" />
              Configured
            </span>
            <span className="inline-flex items-center gap-1.5">
              <span className="h-2.5 w-2.5 rounded-full border border-gray-400 bg-gray-100" />
              Not configured
            </span>
            <span className="inline-flex items-center gap-1.5">
              <span className="h-2.5 w-2.5 rounded-full bg-blue-500" />
              Selected
            </span>
          </div>
        </div>
        <svg
          aria-label="ACS door component layout"
          className="mx-auto min-w-[760px]"
          viewBox="0 0 900 500"
          role="group"
        >
          <text
            x="28"
            y="48"
            className="fill-gray-700 dark:fill-gray-100"
            fontSize="20"
            fontWeight="700"
          >
            ACS DOOR ACCESSORY
          </text>
          <rect
            x="160"
            y="150"
            width="150"
            height="250"
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="2"
          />
          <rect
            x="172"
            y="162"
            width="126"
            height="226"
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="1.5"
          />
          <circle
            cx="188"
            cy="270"
            r="8"
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="1.5"
          />
          {device("readerIn", 102, 252, 16, 38)}
          <text
            x="110"
            y="320"
            textAnchor="middle"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
            Reader
          </text>
          <text
            x="235"
            y="433"
            textAnchor="middle"
            className="fill-gray-900 dark:fill-gray-100"
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
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="2"
          />
          <rect
            x="512"
            y="162"
            width="126"
            height="226"
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="1.5"
          />
          <circle
            cx="622"
            cy="270"
            r="8"
            className="fill-white stroke-gray-900 dark:fill-gray-800 dark:stroke-gray-300"
            strokeWidth="1.5"
          />
          {device("buzzer", 450, 144, 28, 28)}
          <text
            x="445"
            y="135"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
            Buzzer
          </text>
          {device("magneticLock", 515, 144, 58, 22)}
          <text
            x="544"
            y="135"
            textAnchor="middle"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
            Lock
          </text>
          {device("sensor", 578, 145, 22, 10)}
          <text
            x="590"
            y="135"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
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
            stroke={selected === "bg" ? "#2563eb" : configured.bg ? "#16a34a" : "#9ca3af"}
            strokeWidth="1.5"
            pointerEvents="none"
          />
          <text
            x="700"
            y="160"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
            Break Glass
          </text>

          {dto.type == DoorType.Dual
            ? device("readerOut", 665, 255, 16, 38)
            : device("rex", 665, 255, 16, 38)}
          <text
            x="700"
            y="280"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="15"
            fontWeight="600"
          >
            {dto.type == DoorType.Dual ? "Reader" : "REX"}
          </text>
          <text
            x="580"
            y="433"
            textAnchor="middle"
            className="fill-gray-900 dark:fill-gray-100"
            fontSize="16"
            fontWeight="600"
          >
            Inside
          </text>
        </svg>
        <p className="mt-2 text-sm text-gray-600 dark:text-gray-300" aria-live="polite">
          Selected: <span className="font-semibold">{labels[selected]}</span>
          <span className="ml-2 text-gray-500 dark:text-gray-400">
            {configured[selected] ? "Configured" : "Not configured"}
          </span>
        </p>
        <div className="mt-4 grid gap-2 sm:grid-cols-2 xl:grid-cols-3">
          {requiredSetup.map((requirement) => (
            <div
              key={requirement.label}
              className="flex items-center justify-between gap-2 rounded-lg border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/50 px-3 py-2 text-sm"
            >
              <span className="text-gray-700 dark:text-gray-200">
                {requirement.label}
              </span>
              <span
                className={
                  requirement.configured
                    ? "font-medium text-green-700 dark:text-green-400"
                    : "font-medium text-amber-700 dark:text-amber-400"
                }
              >
                {requirement.configured ? "Ready" : "Required"}
              </span>
            </div>
          ))}
        </div>
      </div>
    );
  };

  useEffect(() => {
      const initSignalR = async () => {
        if (!token) return;
  
        await SignalRService.startConnection();
        const connection = SignalRService.getConnection();
        if (!connection) return;
  
        connection.on(SignalRTopic.DOOR_STATUS, (status: DoorStatus) => {
          console.log(status);
          setStatus((prev) =>
            prev.map((item) =>
              item.guid === status.guid
                ? {
                  ...item,
                  mode: status.mode,
                  readerStatus: status.readerStatus,
                  strikeStatus:status.strikeStatus,
                  sensorStatus: status.sensorStatus,
                  doorStatus: status.doorStatus,
                  rex0Status: status.rex0Status,
                  rex1Status: status.rex1Status,
                  ledModeControl: 0,
                  accessControlFlag: 0,
                  alternateReaderStatus: status.alternateReaderStatus,
                  extendedAccessControlFlag: 0,
                  extendedFeatureType: 0,
                  deviceType: 0,
                  extendedFeatureStatus: 0
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
          validateSteps={false}
          submitOnEveryStep
          canSubmit={canSubmitDoor}
          submitHint={
            canSubmitDoor
              ? "Required setup is complete. Buzzer, break glass, and other components are optional."
              : `Complete required setup: ${missingRequirements.join(", ")}. Optional components can be skipped.`
          }
          activeTab={activeTab}
          onActiveTabChange={handleFormTabChange}
          layout={
            <DoorLayout
              selected={selectedComponent}
              onSelect={handleLayoutSelect}
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
          action={action}
          permission={filterPermission(FeatureId.acr)}
          subTable={renderDoorStatusSubTable}
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
                      {/* {DoorType[d.type]} */}
                    </div>
                  ) : d.type == DoorType.Single ? (
                    <div className="flex items-center gap-2">
                      <DoorInIcon fontSize={20} />
                      {/* {DoorType[d.type]} */}
                    </div>
                  ) : (
                    <div className="flex items-center gap-2">
                      <DoorOutIcon fontSize={20} />
                      {/* {DoorType[d.type]} */}
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
