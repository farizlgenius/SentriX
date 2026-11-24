import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormActions, FormField, FormSection } from "../../components/form/template/FormTemplate";
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
import { ControlEndpoint } from "../../endpoint/ControlPointEndpoint";
import { MaskAction } from "../../enum/MaskAction";
import { MaskMetadata } from "../../model/Action/InputMaskMetadata";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { HttpCode } from "../../enum/Httpcode";
import { OutputCommandMetadata } from "../../model/Action/OutputCommandMetadata";
import { OutputCommand } from "../../enum/OutputCommand";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { DoorModeMetadata } from "../../model/Action/DoorModeMetadata";
import { DoorMode } from "../../enum/DoorMode";
import { DelayMetadata } from "../../model/Action/DelayMetadata";
import { ProcedureDto } from "../../model/Procedure/ProcedureDto";
import Button from "../../components/ui/button/Button";
import { AddIcon, AngleDownIcon, AngleUpIcon, TrashBinIcon, TriggerIcon } from "../../icons";
import { ActionEndpoint } from "../../endpoint/ActionEndpoint";
import { HttpStatusCode } from "axios";
import { ListOptionsTransfer } from "../../components/form/list-transfer/ListTransfer";
import Modals from "../UiElements/Modals";
import { MomentUnlockMetadata } from "../../model/Action/MomentUnlockMetadata";
import { TriggerCommand } from "../../enum/TriggerCommand";
import { ProcedureControlCommandMetadata } from "../../model/Action/ProcedureControlCommandMetadata";



const ActionForm: React.FC<PropsWithChildren<FormProp<ProcedureDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const defaulAction: ActionDto = {
            guid: "",
            name: "",
            deviceGuid: dto.deviceGuid,
            deviceName: "",
            type: ActionType.DeleteAll,
            actionType: "",
            sequence: 0,
            actionTarget: null,
            targetGuids: null,
            metadata: "",
            isActive: false,
            isDefault: false
      }
      const isReadOnly = type == FormType.INFO;
      const [deviceOption, setDeviceOption] = useState<Options[]>([]);
      const [moduleOption, setModuleOption] = useState<Options[]>([]);
      const [form, setForm] = useState<boolean>(false);
      const [option, setOption] = useState<Options[]>([]);
      const [selectedActionIndex, setSelectedActionIndex] = useState<number | null>(null);
      const { locationGuid } = useLocation();
      const [selectAction, setSelectAction] = useState<string>("");
      const [actionType, setActionType] = useState<ActionType>(ActionType.DeleteAll);
      const [actionTypeOption, setActionTypeOption] = useState<Options[]>([]);
      const [inputOption, setInputOption] = useState<Options[]>([]);
      const [outputOption, setOutputOption] = useState<Options[]>([]);
      const [doorOprion, setDoorOption] = useState<Options[]>([]);
      const [actionDto, setActionDto] = useState<ActionDto>(defaulAction);

      // List 
      const [selectedOption, setSelectedOption] = useState<Options[]>([]);
      const [availableOption, setAvailableOption] = useState<Options[]>([]);

      const fetchActionByType = async (device: string, type: ActionType) => {
            var res = await send.get(ActionEndpoint.GET_OPTION_BY_DEVICE_AND_TYPE(device, type));
            if (res.status == HttpStatusCode.Ok) {
                  switch (type) {
                        case ActionType.MonitorPointMask:
                              setInputOption(res.data.data);
                              break;
                        case ActionType.ControlPointCommand:
                              setOutputOption(res.data.data);
                              break;
                        default:
                        case ActionType.DeleteAll:
                              break;
                  }
            }
      };


      const removeSelected = () => {
            if (selectedActionIndex === null) return;
            setDto((previous) => ({
                  ...previous,
                  actions: previous.actions.filter((_, index) => index !== selectedActionIndex),
            }));
            setSelectedActionIndex(null);
            setSelectAction("");
      };

      const moveAction = (index: number, direction: -1 | 1) => {
            const targetIndex = index + direction;
            if (isReadOnly || targetIndex < 0 || targetIndex >= dto.actions.length) return;

            setDto((previous) => {
                  const actions = [...previous.actions];
                  [actions[index], actions[targetIndex]] = [actions[targetIndex], actions[index]];
                  return { ...previous, actions };
            });
            setSelectedActionIndex((selectedIndex) =>
                  selectedIndex === index
                        ? targetIndex
                        : selectedIndex === targetIndex
                              ? index
                              : selectedIndex,
            );
      };

      const handleListOptionTransfer = (data:Options[]) => {
            setActionDto(prev => ({
                  ...prev,
                  targetGuids:data.map(x => x.value.toString())
            }))
            console.log(data)
      } 

      const getOptionByType = (type: ActionType) => {
            switch (type) {
                  case ActionType.MonitorPointMask:
                        return inputOption;
                  case ActionType.ControlPointCommand:
                        return outputOption;
                  case ActionType.DoorMode:
                        return doorOprion;
                  default:
                        return [];
            }
      }

      // Helper renderer to yield dynamic form fields based on dto.type
      const renderDynamicFields = (type: ActionType) => {
            switch (type) {
                  case ActionType.MonitorPointMask:
                  
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
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
                                                defaultValue={(actionDto.metadata as MaskMetadata).setClear}
                                                onChange={(e) =>
                                                      setActionDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as MaskMetadata),
                                                                  setClear: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField className="col-span-2">
                                          <Label>Monitor Number</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>

                        );

                  case ActionType.ControlPointCommand:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">

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
                                                defaultValue={(actionDto.metadata as OutputCommandMetadata).command}
                                                onChange={(e) =>
                                                      setActionDto(prev => ({
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
                                          (actionDto.metadata as OutputCommandMetadata).command == OutputCommand.REPEAT || (actionDto.metadata as OutputCommandMetadata).command == OutputCommand.SINGLE ?
                                                <>
                                                      <FormField>
                                                            <Label>On Time</Label>
                                                            <Input
                                                                  type="number"
                                                                  value={(actionDto.metadata as OutputCommandMetadata).onTime ?? 0}
                                                                  onChange={(e) =>
                                                                        setActionDto(prev => ({
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
                                                            (actionDto.metadata as OutputCommandMetadata).command == OutputCommand.REPEAT ?
                                                                  <>
                                                                        <FormField>
                                                                              <Label>Off Time</Label>
                                                                              <Input type="number" value={(actionDto.metadata as OutputCommandMetadata).offTime ?? 0} onChange={(e) =>
                                                                                    setActionDto(prev => ({
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
                                                                              <Input type="number" value={(actionDto.metadata as OutputCommandMetadata).repeat ?? 0} onChange={(e) =>
                                                                                    setActionDto(prev => ({
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
                                    <FormField className="col-span-2">
                                          <Label>Control Number</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>
                        );
                  case ActionType.DoorMode:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">

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
                                                defaultValue={(actionDto.metadata as DoorModeMetadata).mode}
                                                onChange={(e) =>
                                                      setActionDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as DoorModeMetadata),
                                                                  mode: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField className="col-span-2">
                                          <Label>Door</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>

                        );
                  case ActionType.ForceOpenMask:
                  case ActionType.HeldOpenMask:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
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
                                                defaultValue={(actionDto.metadata as MaskMetadata).setClear}
                                                onChange={(e) =>
                                                      setActionDto(prev => ({
                                                            ...prev,
                                                            metadata: {
                                                                  ...(prev.metadata as MaskMetadata),
                                                                  setClear: Number(e.target.value)
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField className="col-span-2">
                                          <Label>Door</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>
                        )
                  case ActionType.MomentaryUnlock:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Strike Time (Second)</Label>
                                          <Input type="number" defaultValue={1} min="1" max="255" value={(actionDto.metadata as MomentUnlockMetadata).strikeTime ?? 1} onChange={e => setActionDto(prev => ({
                                                ...prev,
                                                metadata: {
                                                      ...(prev.metadata as MomentUnlockMetadata),
                                                      strikeTime: Number(e.target.value)
                                                }
                                          }))} />
                                    </FormField>
                                    <FormField className="col-span-2">
                                          <Label>Door</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>
                        )
                  case ActionType.ProcedureControlCommand:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Procedure Command</Label>
                                          <Select
                                                options={[
                                                      {
                                                            label: "Abort Delay",
                                                            value: TriggerCommand.ABORT_DELAY
                                                      }, {
                                                            label: "Execute Action Prefix 0",
                                                            value: TriggerCommand.EXE_ACTION_PREFIX_0,
                                                      },
                                                      {
                                                            label: "Execute Action Prefix 256",
                                                            value: TriggerCommand.EXE_ACTION_PREFIX_256,
                                                      },
                                                      {
                                                            label: "Execute Action Prefix 512",
                                                            value: TriggerCommand.EXE_ACTION_PREFIX_512,
                                                      },
                                                      {
                                                            label: "Execute Action Prefix 1024",
                                                            value: TriggerCommand.EXE_ACTION_PREFIX_1024,
                                                      },
                                                      {
                                                            label: "Resume Delay And Execute Action Prefix 0",
                                                            value: TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_0,
                                                      },
                                                      {
                                                            label: "Resume Delay And Execute Action Prefix 256",
                                                            value: TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_256,
                                                      },
                                                      {
                                                            label: "Resume Delay And Execute Action Prefix 512",
                                                            value: TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_512,
                                                      },
                                                      {
                                                            label: "Resume Delay And Execute Action Prefix 1024",
                                                            value: TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_1024,
                                                      },
                                                ]} 
                                                name={""}                                          
                                                defaultValue={(actionDto.metadata as ProcedureControlCommandMetadata).command ?? TriggerCommand.EXE_ACTION_PREFIX_0}
                                                onChange={e => 
                                                      setActionDto(prev => ({
                                                            ...prev,
                                                            metadata:{
                                                                 ...(prev.metadata as ProcedureControlCommandMetadata),
                                                                 command:Number(e.target.value) 
                                                            }
                                                      }))
                                                }
                                          />
                                    </FormField>
                                    <FormField className="col-span-2">
                                          <Label>Procedure</Label>
                                          {/* <Select
                                                name={""}
                                                isString={true}
                                                defaultValue={dto.srcGuid ?? ""}
                                                options={option}
                                                onChange={(e) =>
                                                      setDto(prev => ({
                                                            ...prev,
                                                            srcGuid: e.target.value
                                                      }))
                                                }
                                          /> */}
                                          <ListOptionsTransfer
                                                availableItems={availableOption}
                                                selectedItems={selectedOption}
                                                onChange={handleListOptionTransfer}
                                          />
                                    </FormField>
                              </div>
                        )
                  case ActionType.DelaySecIncrement:
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Delay (Minutes)</Label>
                                          <Input type="number" value={(actionDto.metadata as DelayMetadata).delayTime ?? 0} onChange={(e) =>
                                                setActionDto(prev => ({
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
                                          <Input type="number" value={(actionDto.metadata as DelayMetadata).delayTime ?? 0} onChange={(e) =>
                                                setActionDto(prev => ({
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

      const switchDefaultMetadata = (type:ActionType) => {
            switch(type){
                  case ActionType.ControlPointCommand:
                        return {
                              command:OutputCommand.ON,
                              onTime:1,
                              offTime:0,
                              repeat:0
                        }
                  case ActionType.DelayMillisecIncrement:
                  case ActionType.DelaySecIncrement:
                        return {
                              delayTime:0,
                        }
                  default:
                        return null;
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
            if (res.status == HttpStatusCode.Ok) {
                  setAvailableOption(res.data.data);
            }
      }

      const fetchOutput = async () => {
            const res = await send.get(ControlEndpoint.GET_OPTION_BY_DEVICE(dto.deviceGuid ?? ""));
            if (res.status == HttpCode.OK) {
                  console.log(res.data.data);
                  setAvailableOption(res.data.data);
            }
      }

      const fetchDoor = async () => {
            const res = await send.get(DoorEndpoint.GET_OPTION_BY_DEVICE(dto.deviceGuid ?? ""))
            if (res.status == HttpCode.OK) {
                  setAvailableOption(res.data.data);
            }
      }

      useEffect(() => {
            fetchDevice();
      }, [])


      return (

            <div className="grid grid-cols-2 gap-5">
                  {
                        form &&
                        <Modals isWide handleClickWithEvent={_ => {
                              setForm(false);
                              setActionDto(defaulAction)
                        }} body={<div className="grid grid-cols-2 gap-5">
                              <FormField >
                                    <Label>Name</Label>
                                    <Input type="text" disabled={isReadOnly}
                                          placeholder="Action name"
                                          onChange={(e) => {
                                                setActionDto(prev => ({
                                                      ...prev,
                                                      name: e.target.value
                                                }))
                                          }}
                                    />
                              </FormField>
                              <FormField>
                                    <Label>Type</Label>
                                    <Select
                                          disabled={isReadOnly}
                                          isString={false}
                                          defaultValue={actionDto.type}
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
                                                      label: "Delay (Millisec)",
                                                      value: ActionType.DelayMillisecIncrement
                                                },
                                                {
                                                      label: "Delay (Second)",
                                                      value: ActionType.DelaySecIncrement
                                                }
                                          ]} name={""}
                                          onChange={(e) => {
                                                setActionDto(prev => ({
                                                      ...prev,
                                                      type: Number(e.target.value),
                                                      metadata: switchDefaultMetadata(Number(e.target.value))
                                                }))
                                                fetchComponentDetail(Number(e.target.value));
                                          }}
                                    />
                              </FormField>
                              <FormField className="col-span-2">
                                    {
                                          renderDynamicFields(actionDto.type)
                                    }
                              </FormField>
                              <FormField className="col-span-2">
                                    <FormActions typeLabel={"Create"} submitName={"Add"} onCancel={_ => {
                                          setForm(false);
                                          setActionDto(defaulAction);
                                    }} 
                                    onSubmit={_ => {
                                          actionDto.metadata = JSON.stringify(actionDto.metadata);
                                          setDto(prev => ({
                                          ...prev,
                                          actions:[
                                                ...prev.actions,
                                                actionDto
                                          ]
                                    }));
                                    setActionDto(defaulAction)
                                    setForm(false)
                                    }}
                                    
                                    />
                              </FormField>

                        </div>} />
                  }

                  {/*  */}
                  <div className="mt-5 rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-4 sm:p-5 col-span-2">
                        <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
                              <div>
                                    <h4 className="font-semibold text-gray-900 dark:text-white">
                                          Action sequence
                                    </h4>
                                    <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                                          Actions run in this order. Use the arrows to change the sequence.
                                    </p>
                              </div>
                              <div className="flex gap-5">
                                    <Button
                                          disabled={isReadOnly}
                                          size="sm"
                                          variant="outline"
                                          startIcon={<AddIcon className="h-4 w-4" />}
                                          onClick={() => setForm(true)}
                                    >
                                          Add action
                                    </Button>
                                    <Button
                                          disabled={isReadOnly || selectedActionIndex === null}
                                          size="sm"
                                          variant="outline"
                                          startIcon={<TrashBinIcon className="h-4 w-4" />}
                                          onClick={removeSelected}
                                    >
                                          Remove selected
                                    </Button>
                              </div>

                        </div>
                        {dto.actions.length === 0 ? (
                              <div className="flex min-h-40 flex-col items-center justify-center rounded-xl border border-dashed border-gray-300 px-4 text-center dark:border-gray-700">
                                    <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-full bg-gray-100 text-gray-500 dark:bg-gray-800">
                                          <TriggerIcon className="h-5 w-5" />
                                    </div>
                                    <p className="font-medium text-gray-800 dark:text-white">
                                          No actions yet
                                    </p>
                                    <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                                          Add an action above to start the sequence.
                                    </p>
                              </div>
                        ) : (
                              <ol className="space-y-2">
                                    {dto.actions.map((item, index) => {
                                          const selected = selectedActionIndex === index;
                                          return (
                                                <li
                                                      key={`${item}-${index}`}
                                                      className="flex items-center gap-3"
                                                >
                                                      <button
                                                            type="button"
                                                            onClick={() => {
                                                                  setSelectedActionIndex(index);
                                                                  setSelectAction(item.guid);
                                                            }}
                                                            className={`flex min-w-0 flex-1 items-center gap-3 rounded-xl border p-3 text-left transition ${selected ? "border-brand-500 bg-brand-50 ring-2 ring-brand-500/15 dark:bg-brand-500/10" : "border-[var(--app-panel-border)] hover:border-brand-200 dark:hover:border-brand-500/50"}`}
                                                      >
                                                            <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-gray-100 text-sm font-bold text-gray-600 dark:bg-gray-800 dark:text-gray-300">
                                                                  {String(index + 1).padStart(2, "0")}
                                                            </span>
                                                            <span className="min-w-0 flex-1 truncate font-semibold text-gray-900 dark:text-white">
                                                                  {item.name}
                                                            </span>
                                                            {selected && (
                                                                  <span className="rounded-full bg-brand-500 px-2 py-0.5 text-xs font-semibold text-white">
                                                                        Selected
                                                                  </span>
                                                            )}
                                                      </button>
                                                      <div className="flex shrink-0 flex-col gap-1">
                                                            <button
                                                                  type="button"
                                                                  aria-label={`Move action ${index + 1} up`}
                                                                  title="Move up"
                                                                  disabled={isReadOnly || index === 0}
                                                                  onClick={() => moveAction(index, -1)}
                                                                  className="rounded-lg p-2 text-gray-500 transition hover:bg-gray-100 hover:text-gray-800 disabled:cursor-not-allowed disabled:opacity-40 dark:hover:bg-gray-800 dark:hover:text-white"
                                                            >
                                                                  <AngleUpIcon className="h-4 w-4" />
                                                            </button>
                                                            <button
                                                                  type="button"
                                                                  aria-label={`Move action ${index + 1} down`}
                                                                  title="Move down"
                                                                  disabled={isReadOnly || index === dto.actions.length - 1}
                                                                  onClick={() => moveAction(index, 1)}
                                                                  className="rounded-lg p-2 text-gray-500 transition hover:bg-gray-100 hover:text-gray-800 disabled:cursor-not-allowed disabled:opacity-40 dark:hover:bg-gray-800 dark:hover:text-white"
                                                            >
                                                                  <AngleDownIcon className="h-4 w-4" />
                                                            </button>
                                                      </div>
                                                </li>
                                          );
                                    })}
                              </ol>
                        )}
                  </div>

            </div>

      );
};

export default ActionForm;
