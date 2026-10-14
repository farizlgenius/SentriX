import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormField } from "../../components/form/template/FormTemplate";
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



const ActionForm: React.FC<PropsWithChildren<FormProp<ActionDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const isReadOnly = type == FormType.INFO;
      const [deviceOption,setDeviceOption] = useState<Options[]>([]);
      const [inputOption,setInputOption] = useState<Options[]>([]);
      const [outputOption,setOutputOption] = useState<Options[]>([]);
      const {locationGuid} = useLocation();

      // Helper renderer to yield dynamic form fields based on dto.type
      const renderDynamicFields = () => {
            switch (dto.type) {
                  case ActionType.MonitorPointMask:
                        fetchInput();
                        return (
                              <div className="rounded-[24px] border border-[var(--app-panel-border)] p-10 grid grid-cols-2 col-span-2 gap-5">
                                    <FormField>
                                          <Label>Monitor Number</Label>
                                          <Select 
                                                name={""} 
                                                options={inputOption}
                                                onChange={(e) => 
                                                      setDto(prev => ({
                                                            ...prev,
                                                            metadata:{
                                                                  ...(prev.metadata as InputMaskMetadata),
                                                                  inputNumber : Number(e.target.value)
                                                            }
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
                                                            label:MaskAction[MaskAction.MASK],
                                                            value:MaskAction.MASK
                                                      },
                                                      {
                                                            label:MaskAction[MaskAction.UNMASK],
                                                            value:MaskAction.UNMASK
                                                      }
                                                ]}  
                                                onChange={(e) => 
                                                      setDto(prev => ({
                                                            ...prev,
                                                            metadata:{
                                                                  ...(prev.metadata as InputMaskMetadata),
                                                                  setClear : Number(e.target.value)
                                                            }
                                                      }))
                                                }                                          
                                          />
                                    </FormField>
                              </div>

                        );

                  case ActionType.ControlPointCommand:
                        return (
                              <FormField>
                                    <Label>Output Command</Label>
                                    {/* <Input
                                          type="text"
                                          disabled={isReadOnly}
                                          value={dto.outputCommand || ""}
                                          onChange={(e) =>
                                                setDto((prev) => ({ ...prev, outputCommand: e.target.value }))
                                          }
                                    /> */}
                              </FormField>
                        );

                  case ActionType.DeleteAll:
                  default:
                        // Return null or default fallback component when no extra fields are required
                        return null;
            }
      };

      const fetchDevice = async () => {
            const res = await send.get(DeviceEndpoint.GET_LOCATION(locationGuid));
            if(res){
                  res.data.data.map((d:DeviceDto) => {
                        setDeviceOption(prev => (
                              prev.some(x => x.value == d.guid) ? 
                              [...prev]
                              :
                              [...prev,{
                                    label:d.name,
                                    value:d.guid
                              }]
                        ))
                  })
            }
      }

      const fetchInput = async () => {
            const res = await send.get(MonitorPointEndpoint.GET_BY_DEVICE(dto.deviceGuid ?? ""));
            if(res){
                  setInputOption(res.data.data);
            }
      }

      const fetchOutput = async () => {
            const res = await send.get(OutputEndpoint.GET_DEVICE(dto.deviceGuid ?? ""));
            if(res){
                  setOutputOption(res.data.data);
            }
      }

      useEffect(() => {
            fetchDevice();
      },[])


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
                        <Select isString={true} name={""} options={deviceOption} />
                  </FormField>
                  <FormField>
                        <Label>Type</Label>
                        <Select
                              // disabled={isReadOnly || dto.deviceGuid == "" || dto.deviceGuid == null}
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
                              }}
                        />
                  </FormField>
                  <div></div>
                  <FormField className="col-span-2">
                  <Label>Action Detail</Label>
                  {
                        renderDynamicFields()
                  }
                  </FormField>
                  


            </div>
      );
};

export default ActionForm;
