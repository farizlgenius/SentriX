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
import { FormContent } from "../../model/Form/FormContent";

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

const formSteps: FormContent = [
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
  const [readerInType, setReaderInType] = useState<string>(ReaderType.wiegand);
  {
    /* Out */
  }
  const [readerOutFlag, setReaderOutFlag] = useState<boolean>(false);
  const [readerOutType, setReaderOutType] = useState<string>(
    ReaderType.wiegand,
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
                
          )}
          {/* Inside */}
          {activeTab === FormTab.Inside && (
           
              )}

              {requestExitOneFlag && (
               
              )}
            </>
          )}
          {/* Relay */}
          {activeTab === FormTab.Strike && (
            
          )}
          {/* Sensor */}
          {activeTab == FormTab.Monitor && (
            
          )}
        </div>
      </FormSection>
    </div>
  );
};

export default AeroDoorForm;
