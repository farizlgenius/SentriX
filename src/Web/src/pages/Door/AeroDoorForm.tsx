import React, {
  ChangeEvent,
  JSX,
  PropsWithChildren,
  useEffect,
  useState,
} from "react";
import Select from "../../components/form/Select";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";
import Logger from "../../utility/Logger";
import Helper from "../../utility/Helper";
import {
  AeroDoorDto,
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
import { Options } from "../../model/Options";
import Switch from "../../components/form/switch/Switch";
import { ReaderType } from "../../enum/ReaderType";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { MonitorPointEndpoint as InputEndpoint } from "../../endpoint/MonitorPointEndpoint";
import { OutputEndpoint } from "../../endpoint/ControlPointEndpoint";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorType } from "../../enum/DoorType";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import StepProgress from "../../components/form/StepProgress";
import { DoorIcon } from "../../icons";

enum FormTab {
  General,
  Inside,
  Outside,
  Strike,
  Antipassback,
  Monitor,
  Buzzer,
  Advance,
  Mode,
}

type DoorFocusComponent =
  | "readerIn"
  | "readerOut"
  | "rex"
  | "magneticLock"
  | "buzzer"
  | "sensor"
  | "exitButton"
  | "breakGlass";

const formSteps = [
  { tab: FormTab.Outside, title: "Reader In", detail: "Inside reader setup" },
  {
    tab: FormTab.Inside,
    title: "Reader Out & REX",
    detail: "Outside reader or REX setup",
  },
  { tab: FormTab.Monitor, title: "Monitor", detail: "Door sensor input setup" },
  { tab: FormTab.Buzzer, title: "Buzzer", detail: "Buzzer for alarm alert" },
  { tab: FormTab.Strike, title: "Strike", detail: "Relay and strike behavior" },
  {
    tab: FormTab.Antipassback,
    title: "Anti-passback",
    detail: "Area transition policies",
  },
  { tab: FormTab.Mode, title: "Door Mode", detail: "Offline and default mode" },
  {
    tab: FormTab.Advance,
    title: "Advance Setting",
    detail: "Flags and advanced options",
  },
];

var defaultRex: Rex = {
  rex0ModuleComponentId: -1,
  rex0Number: -1,
  rex1ModuleComponentId: -1,
  rex1Number: -1,
  disableRex0Timezone: -1,
  disableRex1Timezone: -1,
  rex0SensorMode: -1,
  rex0Debounce: 0,
  rex0HoldTime: 0,
  rex1SensorMode: -1,
  rex1Debounce: 0,
  rex1HoldTime: 0,
  rex0ModuleId: 0,
  rex1ModuleId: -1,
};
var defaultReaderIn: ReaderIn = {
  readerModuleId: -1,
  readerModuleComponentId: -1,
  readerNumber: -1,
  dataFormat: -1,
  keypadMode: -1,
  ledDriveMode: -1,
  osdpFlag: false,
  osdpBaudrate: -1,
  osdpDiscover: 0,
  osdpTracing: 0,
  osdpAddress: -1,
  osdpSecureChannel: 0,
};

var defaultReaderOut: ReaderOut = {
  readerModuleId: -1,
  readerModuleComponentId: -1,
  readerNumber: -1,
  dataFormat: -1,
  keypadMode: -1,
  ledDriveMode: -1,
  osdpFlag: false,
  osdpBaudrate: -1,
  osdpDiscover: 0,
  osdpTracing: 0,
  osdpAddress: -1,
  osdpSecureChannel: 0,
};

var defaultSensor: Sensor = {
  sensorModuleId: -1,
  sensorModuleComponentId: -1,
  sensorNumber: -1,
  heldOpenDelay: 0,
  sensorMode: -1,
  debounce: 0,
  holdTime: 0,
};

var defaultRelay: Relay = {
  relayModuleId: -1,
  relayModuleComponentId: -1,
  relayNumber: -1,
  relayMin: 1,
  relayMax: 5,
  relayDriveMode: -1,
  relayOfflineMode: -1,
};

var defaultAltReader: AltrReader = {
  altrRdrModuleId: -1,
  altrRdrModuleComponentId: -1,
  altrRdrNumber: -1,
  altrRdrConf: -1,
};

var defaultAntipassBack: Antipassback = {
  antipassbackMode: -1,
  areaIn: -1,
  areaOut: -1,
};

var defaultMetadata: AeroDoorMetadata = {
  accessConfig: -1,
  readerIn: defaultReaderIn,
  readerOut: defaultReaderOut,
  sensor: defaultSensor,
  relay: defaultRelay,
  rex: defaultRex,
  altrReader: defaultAltReader,
  antipassback: defaultAntipassBack,
  spare: 0,
  accessControlFlag: 0,
  offlineMode: -1,
  defaultMode: -1,
  ledMode: 0,
  apbDelay: 0,
  relayT2: 0,
  heldOpen2: 0,
  relayFollowerPulse: 0,
  relayFollowerDelay: 0,
  extendFeatureType: 0,
  interiorPushButtonModuleComponentId: 0,
  interiorPushButtonInputNumber: 0,
  interiorPushButtonLongPress: 0,
  interiorPushButtonOutModuleComponentId: 0,
  interiorPushButtonOutRelayNumber: 0,
};

type DoorComponent =
  | "readerIn"
  | "readerOut"
  | "rex"
  | "magneticLock"
  | "buzzer"
  | "bg"
  | "sensor";

type DoorAccessLayout = "inOut" | "inOnly";

const DoorLayout = ({
  selected,
  hasReaderOut,
  onSelect,
}: {
  selected: DoorComponent;
  hasReaderOut: boolean;
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

        {hasReaderOut
          ? device("readerOut", 665, 255, 16, 38)
          : device("rex", 665, 255, 16, 38)}
        <text x="700" y="280" fill="#09090b" fontSize="15" fontWeight="600">
          {hasReaderOut ? "Reader" : "REX"}
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

const AeroDoorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  {
    /* Layout */
  }
  const chooseAccessLayout = (nextLayout: DoorAccessLayout) => {
    const readerOutEnabled = nextLayout === "inOut";
    setAccessLayout(nextLayout);
    setSelectedComponent(readerOutEnabled ? "readerOut" : "rex");
    setDto((previous) => ({
      ...previous,
      type: readerOutEnabled ? DoorType.Dual : DoorType.Single,
    }));
  };

  const [selectedComponent, setSelectedComponent] =
    useState<DoorComponent>("readerIn");
  const [accessLayout, setAccessLayout] = useState<DoorAccessLayout>(
    dto.type === DoorType.Dual ? "inOut" : "inOnly",
  );

  const hasReaderOut = accessLayout === "inOut";
  const { locationGuid: locationId } = useLocation();
  const defaultDoorDto: AeroDoorDto = {
    id: 0,
    componentId: -1,
    name: "",
    deviceComponentId: -1,
    secondComponentId: -1,
    mac: "",
    doorType: "",
    metadata: defaultMetadata,
    locationId: locationId,
    type: "",
    isActive: false,
  };

  {
    /* In */
  }
  const [readerInFlag, setReaderInFlag] = useState<boolean>(false);
  const [readerInType, setReaderInType] = useState<string>(ReaderType.Wiegand);
  {
    /* Out */
  }
  const [readerOutFlag, setReaderOutFlag] = useState<boolean>(false);
  const [readerOutType, setReaderOutType] = useState<string>(
    ReaderType.Wiegand,
  );
  const [requestExitOneFlag, setRequestExitOneFlag] = useState<boolean>(false);
  const [requestExitTwoFlag, setRequestExitTwoFlag] = useState<boolean>(false);
  const [sensorFlag, setSensorFlag] = useState<boolean>(false);
  const [relayFlag, setRelayFlag] = useState<boolean>(false);
  const [apbFlag, setApbFlag] = useState<boolean>(false);
  const [modeFlag, setModeFlag] = useState<boolean>(false);
  const [settingFlag, setSettingFlag] = useState<boolean>(false);

  const [activeTab, setActiveTab] = useState<number>(FormTab.Outside);

  useEffect(() => {
    switch (selectedComponent) {
      case "readerIn":
        setActiveTab(FormTab.Outside);
        break;
      case "readerOut":
      case "rex":
        setActiveTab(FormTab.Inside);
        break;
      case "buzzer":
        setActiveTab(FormTab.Buzzer);
        break;
      case "magneticLock":
        setActiveTab(FormTab.Strike);
        break;
      case "sensor":
        setActiveTab(FormTab.Monitor);
        break;
      case "bg":
        setActiveTab(FormTab.Strike);
        break;
      default:
        break;
    }
  }, [selectedComponent]);

  useEffect(() => {
    if (hasReaderOut === undefined) return;
    setReaderInFlag(true);
    setReaderOutFlag(hasReaderOut);
    setRequestExitOneFlag(!hasReaderOut);
  }, [hasReaderOut]);
  const [osdpBaudRateOption, setOsdpBaudRateOption] = useState<Options[]>([]);

  {
    /* Advance */
  }
  const [spareFlag, setSpareFlag] = useState<Options[]>([]);
  const [accessFlag, setAccessFlag] = useState<Options[]>([]);
  const [osdpAddress, setOsdpAddress] = useState<Options[]>([]);
  const currentStepIndex = formSteps.findIndex(
    (step) => step.tab === activeTab,
  );
  const currentStep = formSteps[currentStepIndex];
  const isFirstStep = currentStepIndex === 0;
  const isLastStep = currentStepIndex === formSteps.length - 1;

  const goToStep = (stepIndex: number) => {
    if (stepIndex < 0 || stepIndex >= formSteps.length) return;
    setActiveTab(formSteps[stepIndex].tab);
  };

  const formatFlagDescription = (description?: string) => {
    if (!description) return "";
    return description.replace(/\s*🔹/g, "\n🔹").trim();
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  {
    /* Reader Module */
  }
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const fetchModule = async (value: number) => {
    const res = await send.get(ModuleEndpoint.GET_BY_DEVICE_ID(value));
    if (res && res.data) {
      res.data.map((a: Options) => {
        setModuleOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };
  {
    /* SCP Data */
  }
  const [controllerOption, setControllerOption] = useState<Options[]>([]);

  const fetchDevice = async () => {
    const res = await send.get(
      DeviceEndpoint.GET_OPTION_BY_TYPE(locationId, DeviceType.AERO.toString()),
    );
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setControllerOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };
  {
    /* Access Reader Config */
  }
  const [accessReaderConfigOption, setAccessReaderConfigOption] = useState<
    Options[]
  >([]);
  const fetchAccessReaderMode = async () => {
    const res = await send.get(DoorEndpoint.GET_ACCESS_READER_MODE);
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setAccessReaderConfigOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };

  {
    /* Reader In Out*/
  }
  const [readerInOption, setReaderInOption] = useState<Options[]>([]);
  const [readerOutOption, setReaderOutOption] = useState<Options[]>([]);
  const fetchReaderIn = async (module: number) => {
    if (readerInOption.length !== 0) return;
    const res = await send.get(DeviceEndpoint.GET_READER(module));
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setReaderInOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };
  const fetchReaderOut = async (module: number) => {
    if (readerOutOption.length !== 0) return;
    if (
      (dto.metadata as AeroDoorMetadata).readerIn.readerModuleComponentId ==
      module
    ) {
      setReaderOutOption(readerInOption.filter((a) => a.isTaken === false));
      return;
    }
    const res = await send.get(DeviceEndpoint.GET_READER(module));
    Logger.info(res);
    if (res && res.data) {
      res.data.map((a: Options) => {
        setReaderOutOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };
  {
    /* Input */
  }
  const [inputOption, setInputOption] = useState<Options[]>([]);
  // const [inputRex0Option, setInputRex0Option] = useState<Options[]>([])
  // const [inputRex1Option, setInputRex1Option] = useState<Options[]>([])
  // const [inputSensorOption, setInputSensorOption] = useState<Options[]>([])
  const [inputModeOption, setInputModeOption] = useState<Options[]>([]);
  const fetchInput = async (sio: number) => {
    const res = await send.get(DeviceEndpoint.GET_INPUT(sio));
    if (res && res.data) {
      res.data.map((a: Options) => {
        setInputOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchInputMode = async () => {
    if (inputModeOption.length !== 0) return;
    const res = await send.get(InputEndpoint.IP_MODE);
    if (res && res.data) {
      res.data.map((a: Options) => {
        setInputModeOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };
  {
    /* Output */
  }
  const [outputOption, setOutputOption] = useState<Options[]>([]);
  const [relayDriveOption, setRelayDriveOption] = useState<Options[]>([]);
  const [relayOfflineOption, setRelayOfflineOption] = useState<Options[]>([]);
  const fetchOutput = async (module: number) => {
    if (outputOption.length !== 0) return;
    const res = await send.get(DeviceEndpoint.GET_RELAY(module));
    if (res && res.data) {
      res.data.map((a: Options) => {
        setOutputOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };
  const fetchRelayDriveMode = async () => {
    if (relayDriveOption.length !== 0) return;
    const res = await send.get(OutputEndpoint.RELAY_DRIVE_MODE);
    if (res && res.data) {
      res.data.map((a: Options) => {
        setRelayDriveOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };
  const fetchRelayOfflineMode = async () => {
    if (relayOfflineOption.length !== 0) return;
    const res = await send.get(OutputEndpoint.RELAY_OFFLINE_MODE);
    if (res && res.data) {
      res.data.map((a: Options) => {
        setRelayOfflineOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };

  {
    /* Time Zone */
  }
  const [timeZoneOption, setTimeZoneOption] = useState<Options[]>([]);
  const fetchTimeZone = async () => {
    if (timeZoneOption.length !== 0) return;
    const res = await send.get(
      TimeZoneEndPoint.GET_OPTION_BY_LOCATION(locationId),
    );
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setTimeZoneOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            isTaken: false,
          },
        ]);
      });
    }
  };
  {
    /* Access Control Reader */
  }
  const [doorModeOption, setDoorModeOption] = useState<Options[]>([]);
  const fetchDoorMode = async () => {
    if (doorModeOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_ACR_MODE);
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setDoorModeOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };
  // const [acs,setAcs] = useState<>()
  {
    /* Anti Passback */
  }
  const [antipassbackOption, setAntipassbackMode] = useState<Options[]>([]);
  const [areaOption, setAreaOption] = useState<Options[]>([]);
  const fetchApbMode = async () => {
    if (antipassbackOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_APB_MODE);
    Logger.info(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setAntipassbackMode((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchOsdpBaudrateOption = async () => {
    if (osdpBaudRateOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_BAUD_RATE);
    if (res.data) {
      res.data.map((a: Options) => {
        setOsdpBaudRateOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchSpareMode = async () => {
    const res = await send.get(DoorEndpoint.GET_SPARE_FLAG);
    if (res.data) {
      res.data.map((a: Options) => {
        setSpareFlag((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
          },
        ]);
      });
    }
  };
  const fetchAccessControlMode = async () => {
    const res = await send.get(DoorEndpoint.GET_ACCESS_CONTROL_FLAG);
    if (res.data) {
      res.data.map((a: Options) => {
        setAccessFlag((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
          },
        ]);
      });
    }
  };

  const fetchOsdpAddress = async (module: number) => {
    const res = await send.get(DoorEndpoint.GET_OSDP_ADDRESS_BY_MODULE(module));
    if (res && res.data.data) {
      console.log(res.data.data);
      res.data.data.map((a: number) => {
        setOsdpAddress((prev) => [
          ...prev,
          {
            label: "Address " + a,
            value: a,
            isTaken: false,
          },
        ]);
      });
    }
  };

  {
    /* UseEffect */
  }
  useEffect(() => {
    fetchDevice();
    fetchAccessReaderMode();
    fetchTimeZone();
    fetchRelayDriveMode();
    fetchRelayOfflineMode();
    fetchApbMode();
    fetchDoorMode();
    fetchInputMode();
    fetchOsdpBaudrateOption();
    fetchSpareMode();
    fetchAccessControlMode();
    setDto(defaultDoorDto);
  }, []);

  return (
    <div className="grid grid-cols-5 gap-5 col-span-2">
      <StepProgress
        steps={formSteps.map((tab, i) => ({
          key: i.toString(),
          title: tab.title,
          detail: tab.detail,
          icon: <DoorIcon />,
        }))}
        activeIndex={currentStepIndex}
        onStepClick={goToStep}
        className="col-span-5"
      />
      <FormSection
        title="Door component layout"
        description="Choose the access layout, then select a component in the door elevation to configure it."
        className="col-span-3"
      >
        <div className="mb-4 flex flex-wrap gap-2">
          <button
            type="button"
            onClick={() => chooseAccessLayout("inOut")}
            className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}
          >
            Dual Reader
          </button>
          <button
            type="button"
            onClick={() => chooseAccessLayout("inOnly")}
            className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${!hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}
          >
            Single Reader
          </button>
        </div>
        <DoorLayout
          selected={selectedComponent}
          hasReaderOut={hasReaderOut}
          onSelect={setSelectedComponent}
        />
      </FormSection>
      <FormSection
        overall="Door Detail"
        title={currentStep?.title}
        description={currentStep?.detail}
        className="col-span-2"
      >
        <div className="grid grid-cols-2 gap-5">
          {/* Outside */}
          {activeTab === FormTab.Outside && (
            <>
              {readerInFlag && (
                <>
                  <FormField>
                    <Label htmlFor="ReaderType">Type</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="ReaderType"
                      options={[
                        {
                          label: "Wiegand",
                          value: ReaderType.Wiegand,
                          description: "",
                          isTaken: false,
                        },
                        {
                          label: "OSDP",
                          value: ReaderType.OSDP,
                          description: "",
                          isTaken: false,
                        },
                      ]}
                      placeholder="Select Option"
                      onChange={(value: string) => {
                        if (value == ReaderType.Wiegand) {
                          setDto((prev) => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              ledMode: 1,
                              readerIn: {
                                ...(prev.metadata as AeroDoorMetadata).readerIn,
                                osdpFlag: false,
                                osdpAddress: 0x00,
                                osdpBaudRate: 0x00,
                                osdpDiscover: 0x00,
                                osdpSecureChannel: 0x00,
                                osdpTracing: 0x00,
                              },
                            },
                          }));
                        } else {
                          setDto((prev) => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              ledMode: 7,
                              readerIn: {
                                ...(prev.metadata as AeroDoorMetadata).readerIn,
                                osdpFlag: true,
                              },
                            },
                          }));
                        }
                        setReaderInType(value);
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={readerInType}
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="readerIn.readerModuleComponentId">
                      Module
                    </Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="readerIn.readerModuleComponentId"
                      options={moduleOption}
                      placeholder="Select Option"
                      onChange={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            readerIn: {
                              ...(prev.metadata as AeroDoorMetadata).readerIn,
                              readerModuleComponentId: Number(value),
                              readerModuleId: moduleOption.find(
                                (x) => x.value == Number(value),
                              )?.additionalInfo,
                            },
                          },
                        }));
                        fetchReaderIn(
                          moduleOption.find((x) => x.value == Number(value))
                            ?.additionalInfo,
                        );
                        if (readerInType == ReaderType.OSDP)
                          fetchOsdpAddress(Number(value));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).readerIn
                          ?.readerModuleComponentId ?? ""
                      }
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="readerIn.readerNo">Slot No</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="readerIn.readerNo"
                      options={readerInOption}
                      placeholder="Select Option"
                      onChange={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            readerIn: {
                              ...(prev.metadata as AeroDoorMetadata).readerIn,
                              readerNumber: Number(value),
                            },
                          },
                        }));
                        setReaderInOption((prev) =>
                          Helper.updateOptionByValue(prev, Number(value), true),
                        );
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).readerIn
                          ?.readerNumber
                      }
                    />
                  </FormField>
                  {readerInType == ReaderType.OSDP && (
                    <>
                      <FormField>
                        <Label htmlFor="readerIn.osdpAddress">Address</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerIn.osdpAddress"
                          options={osdpAddress}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto((prev) => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerIn: {
                                  ...(prev.metadata as AeroDoorMetadata)
                                    .readerIn,
                                  osdpAddress: Number(value),
                                },
                              },
                            }));
                            Helper.updateOptionByValue(
                              osdpAddress,
                              Number(value),
                              true,
                            );
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={
                            (dto.metadata as AeroDoorMetadata).readerIn
                              ?.osdpAddress
                          }
                        />
                      </FormField>
                      <FormField>
                        <Label htmlFor="readerOut.osdpBaudrate">
                          Reader Baud Rate
                        </Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpBaudrate"
                          options={osdpBaudRateOption}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto((prev) => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerIn: {
                                  ...(prev.metadata as AeroDoorMetadata)
                                    .readerIn,
                                  osdpBaudrate: Number(value),
                                },
                              },
                            }));
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={
                            (dto.metadata as AeroDoorMetadata).readerIn
                              ?.osdpBaudrate
                          }
                        />
                      </FormField>
                      <FormField>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Auto Discover"
                            defaultChecked={true}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerIn,
                                    osdpDiscover: checked ? 0x00 : 0x08,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Tracing"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerIn,
                                    osdpTracing: checked ? 0x10 : 0x00,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Secure Channel"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerIn,
                                    osdpTracing: checked ? 0x80 : 0x00,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                      </FormField>
                    </>
                  )}
                </>
              )}
            </>
          )}
          {/* Inside */}
          {activeTab === FormTab.Inside && (
            <>
              {readerOutFlag && (
                <>
                  <FormField>
                    <Label htmlFor="ReaderType">Type</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="ReaderType"
                      options={[
                        {
                          label: "Wiegand",
                          value: ReaderType.Wiegand,
                          description: "",
                          isTaken: false,
                        },
                        {
                          label: "OSDP",
                          value: ReaderType.OSDP,
                          description: "",
                          isTaken: false,
                        },
                      ]}
                      placeholder="Select Option"
                      onChange={(value: string) => {
                        if (value == ReaderType.Wiegand) {
                          setDto((prev) => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              ledMode: 1,
                              readerOut: {
                                ...(prev.metadata as AeroDoorMetadata)
                                  .readerOut,
                                osdpFlag: false,
                                osdpAddress: 0x00,
                                osdpBaudRate: 0x00,
                                osdpDiscover: 0x00,
                                osdpSecureChannel: 0x00,
                                osdpTracing: 0x00,
                              },
                            },
                          }));
                        } else {
                          setDto((prev) => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              ledMode: 7,
                              readerOut: {
                                ...(prev.metadata as AeroDoorMetadata)
                                  .readerOut,
                                osdpFlag: true,
                              },
                            },
                          }));
                        }
                        setReaderOutType(value);
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={readerOutType}
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="readerOut.readerModuleComponentId">
                      Module
                    </Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="readerOut.readerModuleComponentId"
                      options={moduleOption}
                      placeholder="Select Option"
                      onChangeWithEvent={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            readerOut: {
                              ...(prev.metadata as AeroDoorMetadata).readerOut,
                              readerModuleComponentId: Number(value),
                              readerModuleId: moduleOption.find(
                                (x) => x.value == Number(value),
                              )?.additionalInfo,
                            },
                          },
                        }));
                        fetchReaderOut(Number(value));
                        if (readerOutType == ReaderType.OSDP)
                          fetchOsdpAddress(Number(value));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).readerOut
                          ?.readerModuleComponentId
                      }
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="readerOut.readerNumber">Slot No</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="readerOut.readerNo"
                      options={readerOutOption}
                      placeholder="Select Option"
                      onChange={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            readerOut: {
                              ...(prev.metadata as AeroDoorMetadata).readerOut,
                              readerNumber: Number(value),
                            },
                          },
                        }));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).readerOut
                          ?.readerNumber
                      }
                    />
                  </FormField>

                  {readerOutType == ReaderType.OSDP && (
                    <>
                      <FormField>
                        <Label htmlFor="readerOut.osdpAddress">Address</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpAddress"
                          options={osdpAddress}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto((prev) => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata)
                                    .readerOut,
                                  osdpAddress: Number(value),
                                },
                              },
                            }));
                            Helper.updateOptionByValue(
                              osdpAddress,
                              Number(value),
                              true,
                            );
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={
                            (dto.metadata as AeroDoorMetadata).readerOut
                              ?.osdpAddress
                          }
                        />
                      </FormField>
                      <FormField>
                        <Label htmlFor="readerOut.osdpBaudrate">Baudrate</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpBaudrate"
                          options={osdpBaudRateOption}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto((prev) => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata)
                                    .readerOut,
                                  osdpBaudrate: Number(value),
                                },
                              },
                            }));
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={
                            (dto.metadata as AeroDoorMetadata).readerOut
                              ?.osdpBaudrate
                          }
                        />
                      </FormField>
                      <FormField>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Auto Discover"
                            defaultChecked={true}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerOut,
                                    osdpDiscover: checked ? 0x00 : 0x08,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Tracing"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerOut,
                                    osdpTracing: checked ? 0x10 : 0x00,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                        <div className="mt-3">
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Secure Channel"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto((prev) => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata)
                                      .readerOut,
                                    osdpTracing: checked ? 0x80 : 0x00,
                                  },
                                },
                              }));
                            }}
                          />
                        </div>
                      </FormField>
                    </>
                  )}
                </>
              )}

              {requestExitOneFlag && (
                <>
                  <FormField>
                    <Label htmlFor="rex.rex0ModuleComponentId">
                      REX - Module
                    </Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex.rex0ModuleComponentId"
                      options={moduleOption}
                      onChange={(value: string) => {
                        if (
                          (dto.metadata as AeroDoorMetadata).rex
                            .rex0ModuleComponentId != Number(value) &&
                          inputOption.length == 0
                        ) {
                          fetchInput(
                            moduleOption.find((x) => x.value == Number(value))
                              ?.additionalInfo,
                          );
                        }
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0ModuleComponentId: Number(value),
                              rex0ModuleId: moduleOption.find(
                                (x) => x.value == Number(value),
                              )?.additionalInfo,
                            },
                          },
                        }));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).rex
                          ?.rex0ModuleComponentId ?? ""
                      }
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="rex0.inputNo">REX - Input No</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.inputNo"
                      options={inputOption.filter((x) => x.isTaken == false)}
                      onChange={(value: string) => {
                        setInputOption((prev) =>
                          Helper.updateOptionByValue(prev, Number(value), true),
                        );
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0Number: Number(value),
                            },
                          },
                        }));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).rex?.rex0Number ?? ""
                      }
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="rex0.inputMode">REX - Input Mode</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.inputMode"
                      options={inputModeOption}
                      onChange={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0SensorMode: Number(value),
                            },
                          },
                        }));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).rex
                          ?.rex0SensorMode ?? ""
                      }
                    />
                  </FormField>
                  <FormField>
                    <Label htmlFor="rex0.MaskTimeZone">
                      REX - Mask Time Zone
                    </Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.MaskTimeZone"
                      options={timeZoneOption}
                      onChange={(value: string) => {
                        setDto((prev) => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              disableRex0Timezone: Number(value),
                            },
                          },
                        }));
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={
                        (dto.metadata as AeroDoorMetadata).rex
                          ?.disableRex0Timezone ?? ""
                      }
                    />
                  </FormField>
                </>
              )}
            </>
          )}
          {/* Relay */}
          {activeTab === FormTab.Strike && (
            <>
              <FormField>
                <Label htmlFor="relay.relayModuleComponentId">
                  Relay - Module
                </Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="relay.relayModuleComponentId"
                  options={moduleOption}
                  onChange={(value: string) => {
                    fetchOutput(
                      moduleOption.find((x) => x.value == Number(value))
                        ?.additionalInfo,
                    );
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayModuleComponentId: Number(value),
                          relayModuleId: moduleOption.find(
                            (x) => x.value == Number(value),
                          )?.additionalInfo,
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).relay
                      ?.relayModuleComponentId ?? ""
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="strk.outputNo">Relay No</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strk.outputNo"
                  options={outputOption}
                  onChange={(value: string) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayNumber: Number(value),
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).relay?.relayNumber ?? ""
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="relay.relayMin">
                  Minimum Strike Active Time
                </Label>
                <Input
                  disabled={type == FormType.INFO}
                  defaultValue={1}
                  value={(dto.metadata as AeroDoorMetadata).relay?.relayMin}
                  name="relayMin"
                  type="number"
                  id="strikeMinActiveTime"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayMin: Number(e.target.value),
                        },
                      },
                    }));
                  }}
                />
              </FormField>
              <FormField>
                <Label htmlFor="strkMax">Maximum Strike Active Time</Label>
                <Input
                  disabled={type == FormType.INFO}
                  defaultValue={5}
                  value={(dto.metadata as AeroDoorMetadata).relay?.relayMax}
                  name="relayMax"
                  type="number"
                  id="strikeMaxActiveTime"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayMax: Number(e.target.value),
                        },
                      },
                    }));
                  }}
                />
              </FormField>
              <FormField>
                <Label htmlFor="relay.relayMode">Drive Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strkMode"
                  options={relayDriveOption}
                  onChange={(value: string) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayDriveMode: Number(value),
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).relay?.relayDriveMode ??
                    ""
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="relay.relayMode">Offline Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strkMode"
                  options={relayOfflineOption}
                  onChange={(value: string) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayOfflineMode: Number(value),
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).relay
                      ?.relayOfflineMode ?? ""
                  }
                />
              </FormField>
            </>
          )}
          {/* Sensor */}
          {activeTab == FormTab.Monitor && (
            <>
              <FormField>
                <Label htmlFor="sensor.sensorModuleComponentId">
                  Sensor Module
                </Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorModuleComponentId"
                  options={moduleOption}
                  onChange={(value: string) => {
                    if (
                      (dto.metadata as AeroDoorMetadata).rex
                        .rex0ModuleComponentId != Number(value)
                    ) {
                      fetchInput(
                        moduleOption.find((x) => x.value == Number(value))
                          ?.additionalInfo,
                      );
                    }
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorModuleComponentId: Number(value),
                          sensorModuleId: moduleOption.find(
                            (x) => x.value == Number(value),
                          )?.additionalInfo,
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).sensor
                      ?.sensorModuleComponentId ?? ""
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="sensor.sensorNumber">Input No</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorNumber"
                  options={inputOption.filter((x) => x.isTaken == false)}
                  onChange={(value: string) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorNumber: Number(value),
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).sensor?.sensorNumber ??
                    ""
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="sensor.sensorMode">Input Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorMode"
                  options={inputModeOption}
                  onChange={(value: string) => {
                    setDto((prev) => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorMode: Number(value),
                        },
                      },
                    }));
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={
                    (dto.metadata as AeroDoorMetadata).sensor?.sensorMode ?? ""
                  }
                />
              </FormField>
            </>
          )}
        </div>
      </FormSection>
    </div>
  );
};

export default AeroDoorForm;
