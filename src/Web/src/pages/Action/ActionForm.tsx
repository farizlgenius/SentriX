import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormField, FormSection } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { ActionDto } from "../../model/Action/ActionDto";
import { ActionType } from "../../enum/ActionType";
import { Options } from "../../model/Options";
import { send } from "../../api/api";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { useLocation } from "../../context/LocationContext";
import { MonitorPointEndpoint } from "../../endpoint/MonitorPointEndpoint";
import { OutputEndpoint } from "../../endpoint/ControlPointEndpoint";
import { MaskAction } from "../../enum/MaskAction";
import { InputMaskMetadata } from "../../model/Action/InputMaskMetadata";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { HttpCode } from "../../enum/Httpcode";
import { OutputCommandMetadata } from "../../model/Action/OutputCommandMetadata";
import { OutputCommand } from "../../enum/OutputCommand";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { DoorModeMetadata } from "../../model/Action/DoorModeMetadata";
import { DoorMode } from "../../enum/DoorMode";
import { DelayMetadata } from "../../model/Action/DelayMetadata";



const ActionForm: React.FC<PropsWithChildren<FormProp<ActionDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const isReadOnly = type == FormType.INFO;
      const [deviceOption, setDeviceOption] = useState<Options[]>([]);
      const [moduleOption, setModuleOption] = useState<Options[]>([]);
      const [option, setOption] = useState<Options[]>([]);
      const { locationGuid } = useLocation();

      // Helper renderer to yield dynamic form fields based on dto.type
      const renderDynamicFields = () => {
            switch (dto.type) {
                  case ActionType.MonitorPointMask:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Monitor Number</Label>
                                          <Select
                                                name={""}
                                                isString={false}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid:e.target.value
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField>
                                          <Label>Mask Action</Label>
                                          <Select
                                                name={""}
                                                options={[
                                                      {
                                                            label: MaskAction[MaskAction.MASK],
                                                            value: MaskAction.MASK
                                                      },
                                                      {
                                                            label: MaskAction[MaskAction.UNMASK],
                                                            value: MaskAction.UNMASK
                                                      }
                                                ]}
                                                defaultValue={(dto.metadata as InputMaskMetadata).setClear}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as InputMaskMetadata),
                                                                  setClear: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                              </div>

                        );

                  case ActionType.ControlPointCommand:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Control Number</Label>
                                          <Select
                                                name={""}
                                                isString={false}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid:e.target.value
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField>
                                          <Label>Command</Label>
                                          <Select
                                                name={""}
                                                options={[
                                                      {
                                                            label: OutputCommand[OutputCommand.OFF],
                                                            value: OutputCommand.OFF
                                                      },
                                                      {
                                                            label: OutputCommand[OutputCommand.ON],
                                                            value: OutputCommand.ON
                                                      },
                                                      {
                                                            label: OutputCommand[OutputCommand.SINGLE],
                                                            value: OutputCommand.SINGLE
                                                      },
                                                      {
                                                            label: OutputCommand[OutputCommand.REPEAT],
                                                            value: OutputCommand.REPEAT
                                                      },
                                                ]}
                                                defaultValue={(dto.metadata as OutputCommandMetadata).command}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as OutputCommandMetadata),
                                                                  command: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    {
                                          (dto.metadata as OutputCommandMetadata).command == OutputCommand.REPEAT || (dto.metadata as OutputCommandMetadata).command == OutputCommand.SINGLE ?
                                                <>
                                                      <FormField>
                                                            <Label>On Time</Label>
                                                            <Input
                                                                  type="number"
                                                                  value={(dto.metadata as OutputCommandMetadata).onTime ?? 0}
                                                                  onChange={(e) =>
                                                                        setDto(prev => ({
                                                                              ...prev,
                                                                              metadata: {
                                                                                    ...(prev.metadata as OutputCommandMetadata),
                                                                                    onTime: Number(e.target.value)
                                                                              }
                                                                        }))
                                                                  }

                                                            />
                                                      </FormField>
                                                      {
                                                            (dto.metadata as OutputCommandMetadata).command == OutputCommand.REPEAT ?
                                                                  <>
                                                                        <FormField>
                                                                              <Label>Off Time</Label>
                                                                              <Input type="number" value={(dto.metadata as OutputCommandMetadata).offTime ?? 0} onChange={(e) =>
                                                                                    setDto(prev => ({
                                                                                          ...prev,
                                                                                          metadata: {
                                                                                                ...(prev.metadata as OutputCommandMetadata),
                                                                                                offTime: Number(e.target.value)
                                                                                          }
                                                                                    }))
                                                                              }
                                                                              />
                                                                        </FormField>
                                                                        <FormField>
                                                                              <Label>Repeat</Label>
                                                                              <Input type="number" value={(dto.metadata as OutputCommandMetadata).repeat ?? 0} onChange={(e) =>
                                                                                    setDto(prev => ({
                                                                                          ...prev,
                                                                                          metadata: {
                                                                                                ...(prev.metadata as OutputCommandMetadata),
                                                                                                repeat: Number(e.target.value)
                                                                                          }
                                                                                    }))
                                                                              } />
                                                                        </FormField>
                                                                  </>
                                                                  :
                                                                  <></>
                                                      }
                                                </>
                                                :
                                                <></>
                                    }
                              </div>
                        );
                  case ActionType.DoorMode:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Door</Label>
                                          <Select
                                                name={""}
                                                isString={false}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid:e.target.value
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField>
                                          <Label>Mode</Label>
                                          <Select
                                                name={""}
                                                options={[
                                                      {
                                                            label: DoorMode[DoorMode.Disabled],
                                                            value: DoorMode.Disabled
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.Unlocked],
                                                            value: DoorMode.Unlocked
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.Locked],
                                                            value: DoorMode.Locked
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.Facility],
                                                            value: DoorMode.Facility
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.Card],
                                                            value: DoorMode.Card
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.PIN],
                                                            value: DoorMode.PIN
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.CardPIN],
                                                            value: DoorMode.CardPIN
                                                      },
                                                      {
                                                            label: DoorMode[DoorMode.CardOrPIN],
                                                            value: DoorMode.CardOrPIN
                                                      }
                                                ]}
                                                defaultValue={(dto.metadata as DoorModeMetadata).mode}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as DoorModeMetadata),
                                                                  mode: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                              </div>

                        );
                  case ActionType.DelayMinIncrement:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Delay (Minutes)</Label>
                                          <Input type="number" value={(dto.metadata as DelayMetadata).delayTime ?? 0} onChange={(e) =>
                                                setDto(prev => ({
                                                      ...prev,
                                                      metadata: {
                                                            delayTime: Number(e.target.value)
                                                      }
                                                }))
                                          }
                                          />

                                    </FormField>

                              </div>
                        )
                  case ActionType.DelayMillisecIncrement:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Delay (Millisec)</Label>
                                          <Input type="number" value={(dto.metadata as DelayMetadata).delayTime ?? 0} onChange={(e) =>
                                                setDto(prev => ({
                                                      ...prev,
                                                      metadata: {
                                                            delayTime: Number(e.target.value)
                                                      }
                                                }))
                                          }
                                          />
                                    </FormField>

                              </div>
                        )
                  case ActionType.DeleteAll:
                  default:
                        // Return null or default fallback component when no extra fields are required
                        return null;
            }
      };

      const fetchComponentDetail = (type: ActionType) => {
            switch (type) {
                  case ActionType.MonitorPointMask:
                        fetchInput();
                        break;
                  case ActionType.ControlPointCommand:
                        fetchOutput();
                        break;
                  case ActionType.DoorMode:
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                  case ActionType.MomentaryUnlock:
                        fetchDoor();      
                  break;
                  case ActionType.DeleteAll:
                  default:
                        break;
            }
      }

      const fetchDevice = async () => {
            const res = await send.get(DeviceEndpoint.GET_LOCATION(locationGuid));
            if (res) {
                  res.data.data.map((d: DeviceDto) => {
                        setDeviceOption(prev => (
                              prev.some(x => x.value == d.guid) ?
                                    [...prev]
                                    :
                                    [...prev, {
                                          label: d.name,
                                          value: d.guid
                                    }]
                        ))
                  })
            }
      }

      const fetchModule = async (guid: string) => {
            const res = await send.get(ModuleEndpoint.GET_OPTION(guid))
            if (res.status == HttpCode.OK) {
                  setModuleOption(res.data.data);
            }
      }

      const fetchInput = async () => {
            const res = await send.get(MonitorPointEndpoint.GET_OPTION_BY_DEVICE(dto.deviceGuid ?? ""));
            if (res) {
                  setOption(res.data.data);
            }
      }

      const fetchOutput = async () => {
            const res = await send.get(OutputEndpoint.GET_OPTION_BY_DEVICE(dto.deviceGuid ?? ""));
            if (res.status == HttpCode.OK) {
                  console.log(res.data.data);
                  setOption(res.data.data);
            }
      }

      const fetchDoor = async () => {
            const res = await send.get(DoorEndpoint.GET_OPTION_BY_DEVICE(dto.deviceGuid ?? ""))
            if (res.status == HttpCode.OK) {
                  setOption(res.data.data);
            }
      }

      useEffect(() => {
            fetchDevice();
      }, [])


      return (
            <div className="grid grid-cols-2 gap-5">
                  <FormField>
                        <Label>Name</Label>
                        <Input type="text" disabled={isReadOnly}
                              placeholder="Action name"
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          name: e.target.value
                                    }))
                              }}
                        />
                  </FormField>
                  <FormField>
                        <Label>Device</Label>
                        <Select
                              isString={true}
                              name={""}
                              options={deviceOption}
                              defaultValue={dto.deviceGuid ?? ""}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          deviceGuid: e.target.value
                                    }));
                                    fetchModule(e.target.value);
                              }}
                        />
                  </FormField>
                  {/* <FormField>
                        <Label>Module</Label>
                        <Select 
                              isString={true} 
                              name={""} 
                              options={deviceOption} 
                              defaultValue={dto.deviceGuid ?? ""}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          deviceGuid: e.target.value
                                    }))
                              }}
                        />
                  </FormField> */}
                  <FormField>
                        <Label>Type</Label>
                        <Select
                              disabled={isReadOnly || dto.deviceGuid == "" || dto.deviceGuid == null}
                              isString={false}
                              defaultValue={dto.type}
                              options={[
                                    {
                                          label: "Delete All",
                                          value: ActionType.DeleteAll
                                    },
                                    {
                                          label: "Input Mask",
                                          value: ActionType.MonitorPointMask
                                    },
                                    {
                                          label: "Output Control",
                                          value: ActionType.ControlPointCommand
                                    },
                                    {
                                          label: "Door Mode",
                                          value: ActionType.DoorMode
                                    },
                                    {
                                          label: "Force Open Mask",
                                          value: ActionType.ForceOpenMask
                                    },
                                    {
                                          label: "Held Open Mask",
                                          value: ActionType.HeldOpenMask
                                    },
                                    {
                                          label: "Door Trigger Open",
                                          value: ActionType.MomentaryUnlock
                                    },
                                    {
                                          label: "Procedure",
                                          value: ActionType.ProcedureControlCommand
                                    },
                                    {
                                          label: "Trigger Variable",
                                          value: ActionType.TriggerVariableControlCommand
                                    },
                                    {
                                          label: "TimeZone",
                                          value: ActionType.TimeZoneControl
                                    },
                                    {
                                          label: "Reader LED Mode",
                                          value: ActionType.ReaderLEDModeControl
                                    },
                                    {
                                          label: "Antipassback",
                                          value: ActionType.AntipassbackControlCommand
                                    },
                                    {
                                          label: "Monitor Group",
                                          value: ActionType.MonitorPointGroupArmDisarmCommand
                                    },
                                    {
                                          label: "Action Type Prefix (Action bases)",
                                          value: ActionType.SetActionTypePrefixActionPointBased
                                    },
                                    {
                                          label: "Action Type Prefix (Count bases)",
                                          value: ActionType.SetActionTypePrefixMaskCountBased
                                    },
                                    {
                                          label: "Area",
                                          value: ActionType.ModifyAreaConfiguration
                                    },
                                    {
                                          label: "Abort Wait open",
                                          value: ActionType.AbortWaitForDoorOpenState
                                    },
                                    {
                                          label: "Temp LED Reader Mode",
                                          value: ActionType.TempReaderLEDControl
                                    },
                                    {
                                          label: "Text LCD Terminal",
                                          value: ActionType.TextLCDTerminal
                                    },
                                    {
                                          label: "Temp Door Mode",
                                          value: ActionType.TextLCDTerminal
                                    },
                                    {
                                          label: "Simulated Card",
                                          value: ActionType.HostSimulateCardRead
                                    },
                                    {
                                          label: "Use Limit",
                                          value: ActionType.SetUseLimit
                                    },
                                    {
                                          label: "Operation Mode",
                                          value: ActionType.SetOperationMode
                                    },
                                    {
                                          label: "Simulated Keypad",
                                          value: ActionType.HostSimulatedKeyPress
                                    },
                                    {
                                          label: "Delay",
                                          value: ActionType.DelayMinIncrement
                                    }
                              ]} name={""}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          type: Number(e.target.value)
                                    }))
                                    fetchComponentDetail(Number(e.target.value));
                              }}
                        />
                  </FormField>
                  <div></div>
                  <FormField className="col-span-2">
                        {
                              renderDynamicFields()
                        }
                  </FormField>
            </div>

      );
};

export default ActionForm;
