import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { ActionDto } from "../../model/Action/ActionDto";
import { ActionType } from "../../enum/ActionType";



const ActionForm: React.FC<PropsWithChildren<FormProp<ActionDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const isReadOnly = type == FormType.INFO;

      // Helper renderer to yield dynamic form fields based on dto.type
      const renderDynamicFields = () => {
            switch (dto.type) {
                  case ActionType.MonitorPointMask:
                        return (
                              <FormField>
                                    <Label>Delay (Seconds)</Label>
                                    {/* <Input
                                          type="number"
                                          disabled={isReadOnly}
                                          value={dto.delaySeconds || 0}
                                          onChange={(e) =>
                                                setDto((prev) => ({
                                                      ...prev,
                                                      delaySeconds: Number(e.target.value),
                                                }))
                                          }
                                    /> */}
                              </FormField>
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


      return (
            <div className="grid grid-cols-2 gap-5">
                  <FormField>
                        <Label>Name</Label>
                        <Input type="text" disabled={isReadOnly}
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          name: e.target.value
                                    }))
                              }}
                        />
                  </FormField>
                  <FormField>
                        <Label>Type</Label>
                        <Select
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
                  {
                        renderDynamicFields()
                  }


            </div>
      );
};

export default ActionForm;
