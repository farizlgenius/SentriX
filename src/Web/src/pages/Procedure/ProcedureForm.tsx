import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { ProcedureDto } from "../../model/Procedure/ProcedureDto";
import { Options } from "../../model/Options";
import { send } from "../../api/api";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { useLocation } from "../../context/LocationContext";
import { HttpStatusCode } from "axios";
import { ActionEndpoint } from "../../endpoint/ActionEndpoint";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { ActionType } from "../../enum/ActionType";
import Button from "../../components/ui/button/Button";
import { AngleDownIcon, AngleUpIcon, TrashBinIcon, TriggerIcon } from "../../icons";
import Input from "../../components/form/input/InputField";

const ato:Options[] = [
              {
                label: "Delete All",
                value: ActionType.DeleteAll,
              },
              {
                label: "Input Mask",
                value: ActionType.MonitorPointMask,
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
                value: ActionType.DelayMinIncrement,
              }
            ];

export const ProcedureForm: React.FC<
  PropsWithChildren<FormProp<ProcedureDto>>
> = ({ handleClick, dto, setDto, type }) => {
  const isReadOnly = type == FormType.INFO;
  const { locationGuid } = useLocation();
  const [actionTypeOption,setActionTypeOption] = useState<Options[]>(ato);
  const [inputOption,setInputOption] = useState<Options[]>([]);
  const [outputOption,setOutputOption] = useState<Options[]>([]);
  const [doorOprion,setDoorOption] = useState<Options[]>([]);
  const [actionOption, setActionOption] = useState<Options[]>([]);
  const [deviceOption, setDeviceOption] = useState<Options[]>([]);
  const [selectAction, setSelectAction] = useState<string>("");
  const [selectedActionIndex, setSelectedActionIndex] = useState<number | null>(null);
  const [actionType, setActionType] = useState<ActionType>(ActionType.DeleteAll);


  const fetchDevice = async () => {
    var res = await send.get(DeviceEndpoint.GET_OPTION(locationGuid));
    if (res.status == HttpStatusCode.Ok) {
      setDeviceOption(res.data.data);
    }
  };



  const fetchActionByType = async (device:string,type:ActionType) => {
    var res = await send.get(ActionEndpoint.GET_OPTION_BY_DEVICE_AND_TYPE(device, type));
    if (res.status == HttpStatusCode.Ok) {
      switch(type){
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

  const fetchAction = async (device:string) => {
    var res = await send.get(ActionEndpoint.GET_OPTION_BY_DEVICE(device));
    if (res.status == HttpStatusCode.Ok) {
      setActionOption(res.data.data);
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

  const getOptionByType = (type:ActionType) => {
    switch(type){
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



  useEffect(() => {
    fetchDevice();
  }, []);

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input type="text" disabled={isReadOnly}
          placeholder="Procedure name"
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
          isString
          name={""}
          options={deviceOption}
          onChange={e => {
            setDto(prev => ({
            ...prev,
            deviceGuid: e.target.value
          }))
          fetchAction(e.target.value);
          }}
          defaultValue={dto.deviceGuid}
        />
      </FormField>
      <FormField>
        <Label>Action Type</Label>
        <Select
          disabled={isReadOnly || dto.deviceGuid == "" || dto.deviceGuid == undefined}
          isString
          name={""}
          options={actionTypeOption}
          onChange={e => {
            setActionType(Number(e.target.value));
            setSelectAction("")
          }}
          defaultValue={actionType}
        />
      </FormField>
      <FormField>
        <Label>Action</Label>
        <Select
          disabled={isReadOnly || actionType == ActionType.DeleteAll}
          isString
          name={""}
          options={actionOption.filter(x => x.description == actionType)}
          onChange={e => {
            const action = e.target.value;
            setSelectAction(action);
            setActionType(ActionType.DeleteAll);
            setDto(prev => ({
              ...prev,
              actions: prev.actions.some(x => x == e.target.value) ? prev.actions : ([...prev.actions,e.target.value])
            }));
            setSelectedActionIndex(dto.actions.length);
          }}
          defaultValue={selectAction}
        />
      </FormField>
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
                        setSelectAction(item);
                      }}
                      className={`flex min-w-0 flex-1 items-center gap-3 rounded-xl border p-3 text-left transition ${selected ? "border-brand-500 bg-brand-50 ring-2 ring-brand-500/15 dark:bg-brand-500/10" : "border-[var(--app-panel-border)] hover:border-brand-200 dark:hover:border-brand-500/50"}`}
                    >
                      <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-gray-100 text-sm font-bold text-gray-600 dark:bg-gray-800 dark:text-gray-300">
                        {String(index + 1).padStart(2, "0")}
                      </span>
                      <span className="min-w-0 flex-1 truncate font-semibold text-gray-900 dark:text-white">
                        {actionOption.find(x => x.value == item)?.label ?? "Unknown"}
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
