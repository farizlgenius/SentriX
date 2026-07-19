import React, { ChangeEvent, PropsWithChildren, useEffect, useState } from 'react'
import Select from '../../components/form/Select';
import Label from '../../components/form/Label';
import Input from '../../components/form/input/InputField';
import Button from '../../components/ui/button/Button';
import Logger from '../../utility/Logger';
import Helper from '../../utility/Helper';
import { AeroDoorDto, AeroDoorMetadata, AltrReader, Antipassback, DoorDto, ReaderIn, ReaderOut, Relay, Rex, Sensor } from '../../model/Door/DoorDto';
import { Options } from '../../model/Options';
import Switch from '../../components/form/switch/Switch';
import { DeviceType } from '../../enum/DeviceType';
import { ReaderType } from '../../enum/ReaderType';
import { ModuleEndpoint } from '../../endpoint/ModuleEndpoint';
import { DeviceEndpoint } from '../../endpoint/DeviceEndpoint';
import { DoorEndpoint } from '../../endpoint/DoorEndpoint';
import { MonitorPointEndpoint as InputEndpoint } from '../../endpoint/MonitorPointEndpoint';
import { OutputEndpoint } from '../../endpoint/ControlPointEndpoint';
import { TimeZoneEndPoint } from '../../endpoint/TimeZoneEndpoint';
import { useLocation } from '../../context/LocationContext';
import { send } from '../../api/api';
import { FormProp, FormType } from '../../model/Form/FormProp';
import { DoorType } from '../../enum/DoorType';
import { FormField } from '../../components/form/template/FormTemplate';
import StepProgress from '../../components/form/StepProgress';





enum FormTab {
  General, Inside, Outside, Strike, Antipassback, Monitor, Advance, Mode
}

const formSteps = [
  { tab: FormTab.General, title: 'General', detail: 'Basic door configuration' },
  { tab: FormTab.Outside, title: 'In', detail: 'Inside reader setup' },
  { tab: FormTab.Inside, title: 'Out', detail: 'Outside reader or REX setup' },
  { tab: FormTab.Monitor, title: 'Monitor', detail: 'Door sensor input setup' },
  { tab: FormTab.Strike, title: 'Strike', detail: 'Relay and strike behavior' },
  { tab: FormTab.Antipassback, title: 'Anti-passback', detail: 'Area transition policies' },
  { tab: FormTab.Mode, title: 'Door Mode', detail: 'Offline and default mode' },
  { tab: FormTab.Advance, title: 'Advance Setting', detail: 'Flags and advanced options' }
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
  rex1ModuleId: -1
}
var defaultReaderIn: ReaderIn = {
  readerModuleId:-1,
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
  osdpSecureChannel: 0
}

var defaultReaderOut: ReaderOut = {
  readerModuleId:-1,
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
  osdpSecureChannel: 0
}

var defaultSensor: Sensor = {
  sensorModuleId:-1,
  sensorModuleComponentId: -1,
  sensorNumber: -1,
  heldOpenDelay: 0,
  sensorMode: -1,
  debounce: 0,
  holdTime: 0
}

var defaultRelay: Relay = {
  relayModuleId:-1,
  relayModuleComponentId: -1,
  relayNumber: -1,
  relayMin: 1,
  relayMax: 5,
  relayDriveMode: -1,
  relayOfflineMode:-1
}

var defaultAltReader: AltrReader = {
  altrRdrModuleId:-1,
  altrRdrModuleComponentId: -1,
  altrRdrNumber: -1,
  altrRdrConf: -1
}

var defaultAntipassBack: Antipassback = {
  antipassbackMode: -1,
  areaIn: -1,
  areaOut: -1
}

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
  interiorPushButtonOutRelayNumber: 0
}




const AeroDoorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({ handleClick, dto, setDto, type }) => {

  const { locationId } = useLocation();
  const defaultDoorDto: AeroDoorDto = {
    id: 0,
    componentId: -1,
    name: '',
    deviceComponentId: -1,
    secondComponentId:-1,
    mac: '',
    doorType: "",
    metadata: defaultMetadata,
    locationId: locationId,
    type: DeviceType.AERO,
    isActive: false
  }


  {/* In */ }
  const [readerInFlag, setReaderInFlag] = useState<boolean>(false);
  const [readerInType, setReaderInType] = useState<string>(ReaderType.Wiegand)
  {/* Out */ }
  const [readerOutFlag, setReaderOutFlag] = useState<boolean>(false);
  const [readerOutType, setReaderOutType] = useState<string>(ReaderType.Wiegand);
  const [requestExitOneFlag, setRequestExitOneFlag] = useState<boolean>(false);
  const [requestExitTwoFlag, setRequestExitTwoFlag] = useState<boolean>(false);
  const [sensorFlag, setSensorFlag] = useState<boolean>(false);
  const [relayFlag, setRelayFlag] = useState<boolean>(false);
  const [apbFlag, setApbFlag] = useState<boolean>(false);
  const [modeFlag, setModeFlag] = useState<boolean>(false);
  const [settingFlag, setSettingFlag] = useState<boolean>(false);

  const [activeTab, setActiveTab] = useState<number>(FormTab.General);
  const [osdpBaudRateOption, setOsdpBaudRateOption] = useState<Options[]>([])


  {/* Advance */ }
  const [spareFlag, setSpareFlag] = useState<Options[]>([]);
  const [accessFlag, setAccessFlag] = useState<Options[]>([]);
  const [osdpAddress, setOsdpAddress] = useState<Options[]>([]);
  const currentStepIndex = formSteps.findIndex((step) => step.tab === activeTab);
  const currentStep = formSteps[currentStepIndex];
  const isFirstStep = currentStepIndex === 0;
  const isLastStep = currentStepIndex === formSteps.length - 1;

  const goToStep = (stepIndex: number) => {
    if (stepIndex < 0 || stepIndex >= formSteps.length) return;
    setActiveTab(formSteps[stepIndex].tab);
  }

  const formatFlagDescription = (description?: string) => {
    if (!description) return '';
    return description.replace(/\s*🔹/g, '\n🔹').trim();
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto(prev => ({ ...prev, [e.target.name]: e.target.value }));

  };


  {/* Reader Module */ }
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const fetchModule = async (value: number) => {
    const res = await send.get(ModuleEndpoint.GET_BY_DEVICE_ID(value));
    if (res && res.data) {
      res.data.map((a: Options) => {
        setModuleOption((prev) => [...prev, {
          label: a.label,
          value: a.value,
          description:a.description,
          additionalInfo:a.additionalInfo,
          isTaken: false
        }]);
      });
    }
  }
  {/* SCP Data */ }
  const [controllerOption, setControllerOption] = useState<Options[]>([]);

  const fetchDevice = async () => {
    const res = await send.get(DeviceEndpoint.GET_OPTION_BY_TYPE(locationId, DeviceType.AERO.toString()));
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setControllerOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description,
          isTaken: false
        }])
      });
    }

  }
  {/* Access Reader Config */ }
  const [accessReaderConfigOption, setAccessReaderConfigOption] = useState<Options[]>([]);
  const fetchAccessReaderMode = async () => {
    const res = await send.get(DoorEndpoint.GET_ACCESS_READER_MODE);
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setAccessReaderConfigOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          description: a.description,
          additionalInfo:a.additionalInfo,
          isTaken: false
        }])
      });
    }
  }

  {/* Reader In Out*/ }
  const [readerInOption, setReaderInOption] = useState<Options[]>([]);
  const [readerOutOption, setReaderOutOption] = useState<Options[]>([]);
  const fetchReaderIn = async (module: number) => {
    if (readerInOption.length !== 0) return;
    const res = await send.get(DeviceEndpoint.GET_READER(module));
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setReaderInOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description,
          isTaken: false
        }])
      });

    }
  }
  const fetchReaderOut = async (module: number) => {
    if (readerOutOption.length !== 0) return;
    if ((dto.metadata as AeroDoorMetadata).readerIn.readerModuleComponentId == module) {
      setReaderOutOption(readerInOption.filter((a) => a.isTaken === false))
      return;
    }
    const res = await send.get(DeviceEndpoint.GET_READER(module));
    Logger.info(res)
    if (res && res.data) {
      res.data.map((a: Options) => {
        setReaderOutOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description,
          isTaken: false
        }])
      });

    }
  }
  {/* Input */ }
  const [inputOption,setInputOption] = useState<Options[]>([]);
  // const [inputRex0Option, setInputRex0Option] = useState<Options[]>([])
  // const [inputRex1Option, setInputRex1Option] = useState<Options[]>([])
  // const [inputSensorOption, setInputSensorOption] = useState<Options[]>([])
  const [inputModeOption, setInputModeOption] = useState<Options[]>([])
   const fetchInput = async (sio: number) => {
    const res = await send.get(DeviceEndpoint.GET_INPUT(sio));
    if (res && res.data) {
      res.data.map((a: Options) => {
        setInputOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }

  }

  const fetchInputMode = async () => {
    if (inputModeOption.length !== 0) return;
    const res = await send.get(InputEndpoint.IP_MODE)
    if (res && res.data) {
      res.data.map((a: Options) => {
        setInputModeOption(prev => [...prev, {
           label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }
  }
  {/* Output */ }
  const [outputOption, setOutputOption] = useState<Options[]>([])
  const [relayDriveOption, setRelayDriveOption] = useState<Options[]>([])
  const [relayOfflineOption, setRelayOfflineOption] = useState<Options[]>([])
  const fetchOutput = async (module: number) => {
    if (outputOption.length !== 0) return;
    const res = await send.get(DeviceEndpoint.GET_RELAY(module))
    if (res && res.data) {
      res.data.map((a: Options) => {
        setOutputOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }
  }
  const fetchRelayDriveMode = async () => {
    if (relayDriveOption.length !== 0) return;
    const res = await send.get(OutputEndpoint.RELAY_DRIVE_MODE)
    if (res && res.data) {
      res.data.map((a: Options) => {
        setRelayDriveOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }
  }
   const fetchRelayOfflineMode = async () => {
    if (relayOfflineOption.length !== 0) return;
    const res = await send.get(OutputEndpoint.RELAY_OFFLINE_MODE)
    if (res && res.data) {
      res.data.map((a: Options) => {
        setRelayOfflineOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }
  }

  {/* Time Zone */ }
  const [timeZoneOption, setTimeZoneOption] = useState<Options[]>([])
  const fetchTimeZone = async () => {
    if (timeZoneOption.length !== 0) return;
    const res = await send.get(TimeZoneEndPoint.GET_OPTION_BY_LOCATION(locationId))
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setTimeZoneOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          isTaken: false
        }])
      })
    }
  }
  {/* Access Control Reader */ }
  const [doorModeOption, setDoorModeOption] = useState<Options[]>([]);
  const fetchDoorMode = async () => {
    if (doorModeOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_ACR_MODE)
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setDoorModeOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          description: a.description,
          isTaken: false
        }])
      })
    }
  }
  // const [acs,setAcs] = useState<>()
  {/* Anti Passback */ }
  const [antipassbackOption, setAntipassbackMode] = useState<Options[]>([]);
  const [areaOption, setAreaOption] = useState<Options[]>([]);
  const fetchApbMode = async () => {
    if (antipassbackOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_APB_MODE)
    Logger.info(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setAntipassbackMode(prev => [...prev, {
          label: a.label,
          value: a.value,
          description: a.description,
          isTaken: false
        }])
      })
    }
  }

  const fetchOsdpBaudrateOption = async () => {
    if (osdpBaudRateOption.length !== 0) return;
    const res = await send.get(DoorEndpoint.GET_BAUD_RATE)
    if (res.data) {
      res.data.map((a: Options) => {
        setOsdpBaudRateOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          description:a.description,
          isTaken: false
        }])
      })
    }
  }




  const fetchSpareMode = async () => {
    const res = await send.get(DoorEndpoint.GET_SPARE_FLAG)
    if (res.data) {
      res.data.map((a: Options) => {
        setSpareFlag(prev => [...prev, {
          label: a.label,
          value: a.value,
          description: a.description
        }])
      })
    }
  }
  const fetchAccessControlMode = async () => {
    const res = await send.get(DoorEndpoint.GET_ACCESS_CONTROL_FLAG)
    if (res.data) {
      res.data.map((a: Options) => {
        setAccessFlag(prev => [...prev, {
          label: a.label,
          value: a.value,
          description: a.description
        }])
      })
    }
  }

  const fetchOsdpAddress = async (module: number) => {
    const res = await send.get(DoorEndpoint.GET_OSDP_ADDRESS_BY_MODULE(module))
    if (res && res.data.data) {
      console.log(res.data.data)
      res.data.data.map((a: number) => {
        setOsdpAddress(prev => ([...prev, {
          label: "Address " + a,
          value: a,
          isTaken: false
        }]))
      })
    }
  }
  const handleDoorModeToggle = (mode: number) => {
    switch (mode) {
      case DoorType.Single:
        setReaderInFlag(true);
        setRequestExitOneFlag(true);
        setReaderOutFlag(false);
        setSensorFlag(true);
        setRelayFlag(true);
        setApbFlag(true);
        setModeFlag(true);
        setSettingFlag(true);
        break;
      case DoorType.Dual:
        setReaderInFlag(true)
        setReaderOutFlag(true);
        setRequestExitOneFlag(false);
        setSensorFlag(true);
        setRelayFlag(true);
        setApbFlag(true);
        setModeFlag(true);
        setSettingFlag(true);
        break;
      case DoorType.Turnstile:
        setReaderInFlag(true)
        setRequestExitOneFlag(false);
        setReaderOutFlag(false);
        setSensorFlag(true);
        setRelayFlag(true);
        setApbFlag(true);
        setModeFlag(true);
        setSettingFlag(false);
        break;
      case DoorType.Elevator:
        setReaderInFlag(true)
        setRequestExitOneFlag(false);
        setReaderOutFlag(false);
        setSensorFlag(false);
        setRelayFlag(true);
        setApbFlag(false);
        setModeFlag(false);
        setSettingFlag(false);
        break;
      default:
        break;
    }
  }
  {/* UseEffect */ }
  useEffect(() => {
    fetchDevice();
    fetchAccessReaderMode();
    fetchTimeZone()
    fetchRelayDriveMode()
    fetchRelayOfflineMode();
    fetchApbMode();
    fetchDoorMode();
    fetchInputMode();
    fetchOsdpBaudrateOption();
    fetchSpareMode();
    fetchAccessControlMode();
    setDto(defaultDoorDto);
  }, [])

  return (

    <div className="flex flex-col gap-5 p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
      <StepProgress
        steps={formSteps.map((step) => ({ key: step.tab, title: step.title, detail: step.detail }))}
        activeIndex={currentStepIndex}
        onStepClick={goToStep}
      />

      <div className="rounded-xl border border-gray-200 p-4 dark:border-gray-800 lg:p-6">
        <div className="mb-4">
          <h3 className="text-lg font-semibold text-gray-900 dark:text-white">{currentStep?.title}</h3>
          <p className="text-sm text-gray-500 dark:text-gray-400">{currentStep?.detail}</p>
        </div>
        <div className="flex justify-center">
          <div className={`w-full ${activeTab === FormTab.Advance ? '' : 'lg:w-[60%]'}`}>
            {activeTab == FormTab.General &&

              <div className='flex flex-col gap-5'>
                <FormField>
                  <Label htmlFor="name">Door Name</Label>
                  <Input disabled={type == FormType.INFO} value={dto.name} name="name" type="text" id="name" onChange={
                    (e: React.ChangeEvent<HTMLInputElement>) =>
                      setDto(prev => ({ ...prev, name: e.target.value }))
                  } placeholder='Door name' />
                </FormField>
                <FormField>
                  <Label htmlFor='scpId'>Controller</Label>
                  <Select
                    disabled={type == FormType.INFO}
                    isString={false}
                    id='scpId'
                    name="scpId"
                    options={controllerOption}
                    onChange={(value: string) => {
                      setDto(prev => (
                        {
                          ...prev,
                          deviceComponentId: Number(value),
                          mac:controllerOption.find(x => x.value === Number(value))?.description ?? "not found"
                        }))
                      fetchModule(controllerOption.find(x => x.value === Number(value))?.additionalInfo);
                    }}
                    className="dark:bg-dark-900"
                    defaultValue={dto.deviceComponentId}
                  />
                </FormField>
                <FormField>
                  <Label htmlFor="accessConfig">Door Type</Label>
                  <Select
                    disabled={type == FormType.INFO}
                    id='accessConfig'
                    name="accessConfig"
                    options={accessReaderConfigOption}
                    onChange={(value: string) => {
                      setDto(prev => ({
                        ...prev,
                        doorType:accessReaderConfigOption.find(x => x.value == Number(value))?.label ?? "",
                        metadata: {
                          ...(prev.metadata as AeroDoorMetadata),
                          accessConfig: Number(value)
                        }
                      }))
                      handleDoorModeToggle(Number(value))
                    }}
                    className="dark:bg-dark-900"
                    defaultValue={(dto.metadata as AeroDoorMetadata).accessConfig}
                  />
                </FormField>
              </div>

            }

            {activeTab === FormTab.Outside &&
              <div className='flex flex-col gap-5'>

                {readerInFlag &&
                  <>
                    <FormField>
                      <Label htmlFor='ReaderType' >Reader Type</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="ReaderType"
                        options={[
                          {
                            label: "Wiegand",
                            value: ReaderType.Wiegand,
                            description: "",
                            isTaken: false,
                          }, {
                            label: "OSDP",
                            value: ReaderType.OSDP,
                            description: "",
                            isTaken: false,
                          }
                        ]}
                        placeholder="Select Option"
                        onChange={(value: string) => {
                          if (value == ReaderType.Wiegand) {
                            setDto(prev => ({
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
                                  osdpTracing: 0x00

                                }
                              }
                            }))
                          } else {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                ledMode: 7,
                                readerIn: {
                                  ...(prev.metadata as AeroDoorMetadata).readerIn,
                                  osdpFlag: true

                                }
                              }
                            }))
                          }
                          setReaderInType(value)
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={readerInType}
                      />
                    </FormField>
                    <FormField>
                      <Label htmlFor='readerIn.readerModuleComponentId' >Reader In - Module</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="readerIn.readerModuleComponentId"
                        options={moduleOption}
                        placeholder="Select Option"
                        onChange={(value: string) => {
                          setDto(prev => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              readerIn: {
                                ...(prev.metadata as AeroDoorMetadata).readerIn,
                                readerModuleComponentId: Number(value),
                                readerModuleId:moduleOption.find(x => x.value == Number(value))?.additionalInfo
                              }
                            }
                          }))
                          fetchReaderIn(moduleOption.find(x => x.value == Number(value))?.additionalInfo);
                          if (readerInType == ReaderType.OSDP) fetchOsdpAddress(Number(value))
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={(dto.metadata as AeroDoorMetadata).readerIn?.readerModuleComponentId ?? ""}
                      />
                    </FormField>
                    <FormField>
                      <Label htmlFor='readerIn.readerNo'>Reader In - No</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="readerIn.readerNo"
                        options={readerInOption}
                        placeholder="Select Option"
                        onChange={(value: string) => {
                          setDto(prev => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              readerIn: {
                                ...(prev.metadata as AeroDoorMetadata).readerIn,
                                readerNumber: Number(value)
                              }
                            }
                          }))
                          setReaderInOption(prev => Helper.updateOptionByValue(prev, Number(value), true))
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={(dto.metadata as AeroDoorMetadata).readerIn?.readerNumber}
                      />
                    </FormField>
                    {
                      readerInType == ReaderType.OSDP &&
                      <FormField>
                        <Label htmlFor='readerIn.osdpAddress'>Reader Address</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerIn.osdpAddress"
                          options={osdpAddress}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerIn: {
                                  ...(prev.metadata as AeroDoorMetadata).readerIn,
                                  osdpAddress: Number(value)
                                }
                              }
                            }))
                            Helper.updateOptionByValue(osdpAddress, Number(value), true);
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).readerIn?.osdpAddress}
                        />
                        <Label htmlFor='readerOut.osdpBaudrate'>Reader Baud Rate</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpBaudrate"
                          options={osdpBaudRateOption}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerIn: {
                                  ...(prev.metadata as AeroDoorMetadata).readerIn,
                                  osdpBaudrate: Number(value)
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).readerIn?.osdpBaudrate}
                        />
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Auto Discover"
                            defaultChecked={true}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata).readerIn,
                                    osdpDiscover: checked ? 0x00 : 0x08
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Tracing"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata).readerIn,
                                    osdpTracing: checked ? 0x10 : 0x00
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Secure Channel"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerIn: {
                                    ...(prev.metadata as AeroDoorMetadata).readerIn,
                                    osdpTracing: checked ? 0x80 : 0x00
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                      </FormField>

                    }
                  </>

                }
              </div>

            }

            {activeTab === FormTab.Inside &&

              <div className='flex flex-col gap-5'>
                {readerOutFlag &&
                  <>
                    <FormField>
                      <Label htmlFor='ReaderType' >Reader Type</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="ReaderType"
                        options={[
                          {
                            label: "Wiegand",
                            value: ReaderType.Wiegand,
                            description: "",
                            isTaken: false,
                          }, {
                            label: "OSDP",
                            value: ReaderType.OSDP,
                            description: "",
                            isTaken: false,
                          }
                        ]}
                        placeholder="Select Option"
                        onChange={(value: string) => {
                          if (value == ReaderType.Wiegand) {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                ledMode: 1,
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                                  osdpFlag: false,
                                  osdpAddress: 0x00,
                                  osdpBaudRate: 0x00,
                                  osdpDiscover: 0x00,
                                  osdpSecureChannel: 0x00,
                                  osdpTracing: 0x00

                                }
                              }
                            }))
                          } else {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                ledMode: 7,
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                                  osdpFlag: true,

                                }
                              }
                            }))
                          }
                          setReaderOutType(value)
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={readerOutType}
                      />
                    </FormField>
                    <FormField>
                      <Label htmlFor='readerOut.readerModuleComponentId'>Reader Out - Module</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="readerOut.readerModuleComponentId"
                        options={moduleOption}
                        placeholder="Select Option"
                        onChangeWithEvent={(value: string) => {
                          setDto(prev => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              readerOut: {
                                ...(prev.metadata as AeroDoorMetadata).readerOut,
                                readerModuleComponentId: Number(value),
                                readerModuleId:moduleOption.find(x => x.value == Number(value))?.additionalInfo
                              }
                            }
                          }))
                          fetchReaderOut(Number(value));
                          if (readerOutType == ReaderType.OSDP) fetchOsdpAddress(Number(value))
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={(dto.metadata as AeroDoorMetadata).readerOut?.readerModuleComponentId}
                      />
                    </FormField>
                    <FormField>
                      <Label htmlFor='readerOut.readerNumber'>Reader Out - No</Label>
                      <Select
                        disabled={type == FormType.INFO}
                        name="readerOut.readerNo"
                        options={readerOutOption}
                        placeholder="Select Option"
                        onChange={(value: string) => {
                          setDto(prev => ({
                            ...prev,
                            metadata: {
                              ...(prev.metadata as AeroDoorMetadata),
                              readerOut: {
                                ...(prev.metadata as AeroDoorMetadata).readerOut,
                                readerNumber: Number(value)
                              }
                            }
                          }))
                        }}
                        className="dark:bg-dark-900"
                        defaultValue={(dto.metadata as AeroDoorMetadata).readerOut?.readerNumber}
                      />
                    </FormField>


                    {readerOutType == ReaderType.OSDP &&
                      <FormField>
                        <Label htmlFor='readerOut.osdpAddress'>Reader Address</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpAddress"
                          options={osdpAddress}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                                  osdpAddress: Number(value)
                                }
                              }
                            }))
                            Helper.updateOptionByValue(osdpAddress, Number(value), true);
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).readerOut?.osdpAddress}
                        />
                        <Label htmlFor='readerOut.osdpBaudrate'>Reader Baud Rate</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="readerOut.osdpBaudrate"
                          options={osdpBaudRateOption}
                          placeholder="Select Option"
                          onChange={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                readerOut: {
                                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                                  osdpBaudrate: Number(value)
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).readerOut?.osdpBaudrate}
                        />
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Auto Discover"
                            defaultChecked={true}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata).readerOut,
                                    osdpDiscover: checked ? 0x00 : 0x08
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Tracing"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata).readerOut,
                                    osdpTracing: checked ? 0x10 : 0x00
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                        <div className='mt-3'>
                          <Switch
                            disabled={type == FormType.INFO}
                            label="Secure Channel"
                            defaultChecked={false}
                            onChange={(checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  readerOut: {
                                    ...(prev.metadata as AeroDoorMetadata).readerOut,
                                    osdpTracing: checked ? 0x80 : 0x00
                                  }
                                }
                              }))
                            }}
                          />
                        </div>
                      </FormField>
                    }

                  </>

                }

                {requestExitOneFlag &&
                  <div className='flex flex-col gap-1'>
                    <Label htmlFor='rex.rex0ModuleComponentId'>REX - Module</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex.rex0ModuleComponentId"
                      options={moduleOption}
                      onChange={(value: string) => {
                        if((dto.metadata as AeroDoorMetadata).rex.rex0ModuleComponentId != Number(value) && inputOption.length == 0){
                            fetchInput(moduleOption.find(x => x.value == Number(value))?.additionalInfo);
                        }
                        setDto(prev => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0ModuleComponentId: Number(value),
                              rex0ModuleId:moduleOption.find(x => x.value == Number(value))?.additionalInfo
                            }
                          }
                        }))
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={(dto.metadata as AeroDoorMetadata).rex?.rex0ModuleComponentId ?? ""}
                    />
                    <Label htmlFor='rex0.inputNo'>REX - Input No</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.inputNo"
                      options={inputOption.filter(x => x.isTaken == false)}
                      onChange={(value: string) => {
                        setInputOption(prev => Helper.updateOptionByValue(prev, Number(value), true));
                        setDto(prev => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0Number: Number(value)
                            }
                          }
                        }))
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={(dto.metadata as AeroDoorMetadata).rex?.rex0Number ?? ""}
                    />
                    <Label htmlFor="rex0.inputMode">REX - Input Mode</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.inputMode"
                      options={inputModeOption}
                      onChange={(value: string) => {
                        setDto(prev => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              rex0SensorMode: Number(value)
                            }
                          }
                        }))
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={(dto.metadata as AeroDoorMetadata).rex?.rex0SensorMode ?? ""}
                    />
                    <Label htmlFor="rex0.MaskTimeZone">REX - Mask Time Zone</Label>
                    <Select
                      disabled={type == FormType.INFO}
                      name="rex0.MaskTimeZone"
                      options={timeZoneOption}
                      onChange={(value: string) => {
                        setDto(prev => ({
                          ...prev,
                          metadata: {
                            ...(prev.metadata as AeroDoorMetadata),
                            rex: {
                              ...(prev.metadata as AeroDoorMetadata).rex,
                              disableRex0Timezone: Number(value)
                            }
                          }
                        }))
                      }}
                      className="dark:bg-dark-900"
                      defaultValue={(dto.metadata as AeroDoorMetadata).rex?.disableRex0Timezone ?? ""}
                    />

                    {requestExitTwoFlag &&
                      <>
                        <Label htmlFor="rex.rex1ModuleComponentId">Alter REX - Module</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="rex.rex1ModuleComponentId"
                          options={moduleOption}
                          onChange={(value: string) => {
                            if((dto.metadata as AeroDoorMetadata).rex.rex1ModuleComponentId != Number(value)){
                              fetchInput(moduleOption.find(x => x.value == Number(value))?.additionalInfo)
                            }
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                rex: {
                                  ...(prev.metadata as AeroDoorMetadata).rex,
                                  rex1ModuleComponentId: Number(value),
                                  rex1ModuleId:moduleOption.find(x => x.value == Number(value))?.additionalInfo
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).rex.rex1ModuleComponentId}
                        />
                        <Label htmlFor="rex1.rex1Number">Alter REX - Input No</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="rex.rex1Number"
                          options={inputOption}
                          onChange={(value: string) => {
                            setInputOption(prev => Helper.updateOptionByValue(prev, Number(value), true));
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                rex: {
                                  ...(prev.metadata as AeroDoorMetadata).rex,
                                  rex1Number: Number(value)
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).rex?.rex1Number}
                        />
                        <Label htmlFor="rex.rex1SensorMode">Alter REX - Input Mode</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="rex.rex1SensorMode"
                          options={inputModeOption}
                          onChange={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                rex: {
                                  ...(prev.metadata as AeroDoorMetadata).rex,
                                  rex1SensorMode: Number(value)
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).rex?.rex1SensorMode}
                        />
                        <Label htmlFor="rex.disableRex1Timezone">Alter REX - Time Zone</Label>
                        <Select
                          disabled={type == FormType.INFO}
                          name="rex.disableRex1Timezone"
                          options={timeZoneOption}
                          onChangeWithEvent={(value: string) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                rex: {
                                  ...(prev.metadata as AeroDoorMetadata).rex,
                                  disableRex1Timezone: Number(value)
                                }
                              }
                            }))
                          }}
                          className="dark:bg-dark-900"
                          defaultValue={(dto.metadata as AeroDoorMetadata).rex?.disableRex1Timezone}
                        />
                      </>
                    }

                  </div>
                }

              </div>


            }


            {
              activeTab === FormTab.Strike && relayFlag &&


              <FormField className='flex flex-col gap-1 max-h-[60vh] overflow-y-auto overflow-y-auto hidden-scroll'>
                <Label htmlFor="relay.relayModuleComponentId">Relay - Module</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="relay.relayModuleComponentId"
                  options={moduleOption}
                  onChange={(value: string) => {
                    fetchOutput(moduleOption.find(x => x.value == Number(value))?.additionalInfo)
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayModuleComponentId: Number(value),
                          relayModuleId: moduleOption.find(x => x.value == Number(value))?.additionalInfo
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).relay?.relayModuleComponentId ?? ""}
                />
                <Label htmlFor="strk.outputNo">Relay No</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strk.outputNo"
                  options={outputOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayNumber: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).relay?.relayNumber ?? ""}
                />
                <Label htmlFor="relay.relayMin">Minimum Strike Active Time</Label>
                <Input disabled={type == FormType.INFO} defaultValue={1} value={(dto.metadata as AeroDoorMetadata).relay?.relayMin} name="relayMin" type="number" id="strikeMinActiveTime"
                  onChange={(e: ChangeEvent<HTMLInputElement>) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayMin: Number(e.target.value)
                        }
                      }
                    }))
                  }} />
                <Label htmlFor="strkMax">Maximum Strike Active Time</Label>
                <Input disabled={type == FormType.INFO} defaultValue={5} value={(dto.metadata as AeroDoorMetadata).relay?.relayMax} name="relayMax" type="number" id="strikeMaxActiveTime" onChange={(e: ChangeEvent<HTMLInputElement>) => {
                  setDto(prev => ({
                    ...prev,
                    metadata: {
                      ...(prev.metadata as AeroDoorMetadata),
                      relay: {
                        ...(prev.metadata as AeroDoorMetadata).relay,
                        relayMax: Number(e.target.value)
                      }
                    }
                  }))
                }} />
                <Label htmlFor="relay.relayMode">Drive Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strkMode"
                  options={relayDriveOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayDriveMode: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).relay?.relayDriveMode ?? ""}
                />
                                <Label htmlFor="relay.relayMode">Offline Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="strkMode"
                  options={relayOfflineOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        relay: {
                          ...(prev.metadata as AeroDoorMetadata).relay,
                          relayOfflineMode: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).relay?.relayOfflineMode ?? ""}
                />

              </FormField>


            }

            {activeTab == FormTab.Monitor && sensorFlag &&
              <FormField className='flex flex-col gap-1'>
                <Label htmlFor="sensor.sensorModuleComponentId">Sensor Module</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorModuleComponentId"
                  options={moduleOption}
                  onChange={(value: string) => {
                    if((dto.metadata as AeroDoorMetadata).rex.rex0ModuleComponentId != Number(value)){
                              fetchInput(moduleOption.find(x => x.value == Number(value))?.additionalInfo)
                            }
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorModuleComponentId: Number(value),
                          sensorModuleId:moduleOption.find(x => x.value == Number(value))?.additionalInfo
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).sensor?.sensorModuleComponentId ?? ""}
                />
                <Label htmlFor="sensor.sensorNumber">Input No</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorNumber"
                  options={inputOption.filter(x => x.isTaken == false)}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorNumber: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).sensor?.sensorNumber ?? ""}
                />
                <Label htmlFor="sensor.sensorMode">Input Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="sensor.sensorMode"
                  options={inputModeOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        sensor: {
                          ...(prev.metadata as AeroDoorMetadata).sensor,
                          sensorMode: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).sensor?.sensorMode ?? ""}
                />
              </FormField>
            }

            {activeTab == FormTab.Antipassback && apbFlag &&

              <FormField className='flex flex-col gap-1'>
                <Label htmlFor="antiPassbackMode">Anti-Passback Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="antiPassbackMode"
                  options={antipassbackOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        antipassback: {
                          ...(prev.metadata as AeroDoorMetadata).antipassback,
                          antipassbackMode: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).antipassback?.antipassbackMode ?? ""}
                />
                <Label htmlFor="antiPassBackIn">Area From</Label>
                <Select
                  disabled={type == FormType.INFO}
                  isString={false}
                  name="antiPassBackIn"
                  options={areaOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        antipassback: {
                          ...(prev.metadata as AeroDoorMetadata).antipassback,
                          areaIn: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).antipassback?.areaIn ?? ""}
                />
                <Label htmlFor="antiPassBackOut">Area To</Label>
                <Select
                  disabled={type == FormType.INFO}
                  isString={false}
                  name="antiPassBackOut"
                  options={areaOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        antipassback: {
                          ...(prev.metadata as AeroDoorMetadata).antipassback,
                          areaOut: Number(value)
                        }
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).antipassback?.areaOut ?? ""}
                />

              </FormField>
            }
            {activeTab == FormTab.Mode && modeFlag &&
              <FormField className='flex flex-col gap-1'>

                <Label htmlFor="offlineMode">Offline Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  isString={false}
                  name="offlineMode"
                  options={doorModeOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        offlineMode: Number(value)
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).offlineMode ?? ""}
                />
                <Label htmlFor="defaultMode">Default Mode</Label>
                <Select
                  disabled={type == FormType.INFO}
                  isString={false}
                  name="defaultMode"
                  options={doorModeOption}
                  onChange={(value: string) => {
                    setDto(prev => ({
                      ...prev,
                      metadata: {
                        ...(prev.metadata as AeroDoorMetadata),
                        defaultMode: Number(value)
                      }
                    }))
                  }}
                  className="dark:bg-dark-900"
                  defaultValue={(dto.metadata as AeroDoorMetadata).defaultMode ?? ""}
                />
              </FormField>

            }

            {activeTab == FormTab.Advance && settingFlag &&
              <div className="grid grid-cols-2 gap-4">
                <div className="rounded-2xl border border-gray-200 bg-gray-50/80 p-5 dark:border-gray-800 dark:bg-white/[0.02]">
                  <h4 className="text-base font-semibold text-gray-900 dark:text-white">Access Control Flags</h4>
                  <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">Enable only the behaviors this door should enforce during normal operation.</p>
                  <div className="mt-4 grid grid-cols-1 gap-3">
                    {accessFlag.map((d, i) => (
                      <div key={i} className="rounded-xl border border-gray-200 bg-white px-4 py-3 dark:border-gray-700 dark:bg-gray-900">
                        <Switch
                          disabled={type == FormType.INFO}
                          label={d.label}
                          defaultChecked={false}
                          onChange={(checked: boolean) => {
                            setDto(prev => ({
                              ...prev,
                              metadata: {
                                ...(prev.metadata as AeroDoorMetadata),
                                accessControlFlag: checked ? (prev.metadata as AeroDoorMetadata).accessControlFlag | Number(d.value) : (prev.metadata as AeroDoorMetadata).accessControlFlag & (~Number(d.value))
                              }
                            }))
                          }}
                        />
                        {d.description && (
                          <p className="mt-1 whitespace-pre-line text-xs text-gray-500 dark:text-gray-400">
                            {formatFlagDescription(d.description)}
                          </p>
                        )}
                      </div>
                    ))}
                  </div>
                </div>

                <div className="rounded-2xl border border-gray-200 bg-gray-50/80 p-5 dark:border-gray-800 dark:bg-white/[0.02]">
                  <h4 className="text-base font-semibold text-gray-900 dark:text-white">Advanced Flags</h4>
                  <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">Additional low-level options. Keep these disabled unless your scenario requires them.</p>
                  <div className="mt-4 grid grid-cols-1 gap-3">
                    {spareFlag.map((d, i) => (
                      <div key={i} className="rounded-xl border border-gray-200 bg-white px-4 py-3 dark:border-gray-700 dark:bg-gray-900">
                        <Switch
                          disabled={type == FormType.INFO}
                          label={d.label}
                          defaultChecked={false}
                          onChange={
                            (checked: boolean) => {
                              setDto(prev => ({
                                ...prev,
                                metadata: {
                                  ...(prev.metadata as AeroDoorMetadata),
                                  spare: checked ? (prev.metadata as AeroDoorMetadata).spare | Number(d.value) : (prev.metadata as AeroDoorMetadata).spare & (~Number(d.value))
                                }
                              }))
                            }
                          }
                        />
                        {d.description && (
                          <p className="mt-1 whitespace-pre-line text-xs text-gray-500 dark:text-gray-400">
                            {formatFlagDescription(d.description)}
                          </p>
                        )}
                      </div>
                    ))}
                  </div>
                </div>
              </div>

            }
          </div>
        </div>

        <div className="mt-6 flex w-full items-center justify-between gap-3">
          <div>
            {!isFirstStep && (
              <Button
                variant="outline"
                onClick={() => goToStep(currentStepIndex - 1)}
                className="min-w-[120px]"
                size="sm"
              >
                Back
              </Button>
            )}
          </div>
          <div className="flex gap-3">
            <Button variant='danger' onClickWithEvent={handleClick} name='close' className="min-w-[120px]" size="sm">Cancel</Button>
            {isLastStep ? (
              <Button
                disabled={type == FormType.INFO}
                onClickWithEvent={(event) => {
                  // Do something first
                  setDto(dto)

                  // Then call handleClick
                  handleClick?.(event);
                }}
                name={type == FormType.UPDATE ? "update" : "create"}
                className="min-w-[120px]"
                size="sm"
              >
                {type == FormType.UPDATE ? "Update" : "Create"}
              </Button>
            ) : (
              <Button onClick={() => goToStep(currentStepIndex + 1)} className="min-w-[120px]" size="sm">
                Next
              </Button>
            )}
          </div>
        </div>
      </div>
    </div>


  )
}

export default AeroDoorForm
