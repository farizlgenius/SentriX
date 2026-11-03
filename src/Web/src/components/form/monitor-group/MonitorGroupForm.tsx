import { PropsWithChildren, useCallback, useEffect, useState } from "react";
import Input from "../input/InputField";
import Label from "../Label";
import { MonitorGroupDto, MonitorPointGroupComponent } from "../../../model/MonitorGroup/MonitorGroupDto";
import Select from "../Select";
import Button from "../../ui/button/Button";
import { useLocation } from "../../../context/LocationContext";
import { send } from "../../../api/api";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { Options } from "../../../model/Options";
import { MonitorPointEndpoint } from "../../../endpoint/MonitorPointEndpoint";
import { InputDto } from "../../../model/MonitorPoint/InputDto";
import { DoorEndpoint } from "../../../endpoint/DoorEndpoint";
import { DoorDto } from "../../../model/Door/DoorDto";
import { DoorIcon, MonitorIcon } from "../../../icons";
import { FormField } from "../template/FormTemplate";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import { MonitorType } from "../../../enum/MonitorType";

export const MonitorGroupForm: React.FC<
  PropsWithChildren<FormProp<MonitorGroupDto>>
> = ({ handleClick, dto, setDto, type }) => {
  const isReadOnly = type == FormType.INFO;
  const defaultDto: MonitorPointGroupComponent = {
    componentGuid: "",
    type: MonitorType.UNKNOWN,
  };
  const { locationGuid } = useLocation();
  const [hardwareOptions, setHardwareOptions] = useState<Options[]>([]);

  const [monitorOptions, setMonitorOptions] = useState<Options[]>([]);
  const [doorOptions,setDoorOptions] = useState<Options[]>([]);
  const [selectedComponents, setSelectedComponents] = useState<string[]>([]);
  const [selectedComponent, setSelectedComponent] = useState<MonitorPointGroupComponent>(defaultDto);
  const getComponentKey = (component: MonitorPointGroupComponent) =>
    `${component.type}:${component.componentGuid}`;

  const handleSelect = (
    value: string,
    e: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    if (e.target.name === "deviceGuid") {
      setDto((prev) => ({
        ...prev,
        deviceGuid: value,
        components: prev.deviceGuid === value ? prev.components : [],
      }));
      setMonitorOptions([]);
      setDoorOptions([]);
      setSelectedComponents([]);
      setSelectedComponent(defaultDto);
      return;
    }

    if (e.target.name === "pointType") {
      const pointType = Number(value) as MonitorType;
      setSelectedComponent({ componentGuid: "", type: pointType });
      return;
    }

    if (e.target.name === "pointNumber") {
      setSelectedComponent((prev) => ({ ...prev, componentGuid: value }));
    }
  };

  const handleClickAdd = () => {
    if (isReadOnly || !selectedComponent.componentGuid) return;
    setDto((prev) => ({
      ...prev,
      components: prev.components.some(
        (component) =>
          component.componentGuid === selectedComponent.componentGuid &&
          component.type === selectedComponent.type,
      )
        ? prev.components
        : [...prev.components, selectedComponent],
    }));
    setSelectedComponent((previous) => ({ ...previous, componentGuid: "" }));
  };

  const handleRemoveSelected = () => {
    setDto((prev) => ({
      ...prev,
      components: prev.components.filter(
        (component) =>
          !selectedComponents.includes(getComponentKey(component)),
      ),
    }));
    setSelectedComponents([]);
  };

  const toggleSelected = (component: MonitorPointGroupComponent) => {
    const key = getComponentKey(component);
    setSelectedComponents((prev) =>
      prev.includes(key) ? prev.filter((item) => item !== key) : [...prev, key],
    );
  };

  const fetchDevice = useCallback(async () => {
    const res = await send.get(DeviceEndpoint.GET_OPTION(locationGuid));
    if (res && res.data.data) {
      setHardwareOptions(res.data.data);
      
    }
  }, [locationGuid]);


  const fetchMonitor = async (guid:string) => {
    const res = await send.get(MonitorPointEndpoint.GET_OPTION_BY_DEVICE(guid));
    console.log("Monitor Options:", res.data.data);
    if (res && res.data.data) {
      setMonitorOptions(res.data.data);
    }
  };

  const fetchDoor = async (deviceGuid: string) => {
    const res = await send.get(DoorEndpoint.GET_OPTION_BY_DEVICE(deviceGuid));
    console.log("Door Options:", res.data.data);
    if (res && res.data.data) {
      setDoorOptions(res.data.data);
    }
  };

  useEffect(() => {
    fetchDevice();
  }, []);

  useEffect(() => {
    if (!dto.deviceGuid || dto.components.length === 0) return;
    const device = hardwareOptions.find(
      (option) => option.value === dto.deviceGuid,
    );
    if (!device) return;

    const loadSelectedComponentOptions = async () => {
      const options: Options[] = [];
      const selectedTypes = new Set(dto.components.map((component) => component.type));

      if (
        selectedTypes.has(MonitorType.MONITOR_POINT) &&
        device.description
      ) {
        const res = await send.get(
          MonitorPointEndpoint.MP_BY_MAC(String(device.description)),
        );
        if (res && res.data.data) {
          options.push(
            ...res.data.data.map((point: InputDto) => ({
              label: point.name,
              value: point.guid,
            })),
          );
        }
      }

      if (
        selectedTypes.has(MonitorType.FORCED_OPEN) ||
        selectedTypes.has(MonitorType.HELD_OPEN)
      ) {
        const res = await send.get(DoorEndpoint.GET_BY_LOCATION(locationGuid));
        if (res && res.data.data) {
          options.push(
            ...res.data.data
              .filter((door: DoorDto) => door.deviceGuid === dto.deviceGuid)
              .map((door: DoorDto) => ({
                label: door.name,
                value: door.guid,
              })),
          );
        }
      }

      //setMonitorOptions(options);
    };

    loadSelectedComponentOptions();
  }, [dto.components, dto.deviceGuid, hardwareOptions, locationGuid]);

  const getMonitorComponentLabel = (component: MonitorPointGroupComponent) =>
    monitorOptions.find((option) => option.value === component.componentGuid)
      ?.label ?? component.componentGuid;

    const getDoorComponentLabel = (component: MonitorPointGroupComponent) =>
    doorOptions.find((option) => option.value === component.componentGuid)
      ?.label ?? component.componentGuid;

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="name">Name</Label>
        <Input
          placeholder="Name"
          name="name"
          type="text"
          id="name"
          disabled={isReadOnly}
          onChange={(event) =>
            setDto((prev) => ({ ...prev, name: event.target.value }))
          }
          value={dto.name}
        />
      </FormField>
      <FormField>
        <Label htmlFor="deviceGuid">Device</Label>
        <Select
          isString
          options={hardwareOptions}
          defaultValue={dto.deviceGuid}
          onChange={(event) => {handleSelect(event.target.value, event);
            fetchDoor(event.target.value);
            fetchMonitor(event.target.value);
          }}
          name="deviceGuid"
          disabled={isReadOnly}
        />
      </FormField>
      <FormField className="col-span-2">
        <Label>Components</Label>
        <div className="grid grid-cols-1 gap-5 rounded-2xl border border-gray-200 p-5 dark:border-gray-800 lg:grid-cols-2 lg:p-6">
          <FormField>
            <Label htmlFor="pointType">Type</Label>
            <Select
              options={[
                {
                  label: "Monitor Point", value: MonitorType.MONITOR_POINT
                }, {
                  label: "Forced Open", value: MonitorType.FORCED_OPEN
                }, {
                  label: "Held Open", value: MonitorType.HELD_OPEN
                }
              ]}
              defaultValue={selectedComponent.type}
              onChange={(event) => handleSelect(event.target.value, event)}
              name="pointType"
              disabled={isReadOnly || !dto.deviceGuid}
            />
          </FormField>
          <FormField>
            <Label htmlFor="pointNumber">Point</Label>
            <Select
            isString
              options={selectedComponent.type === MonitorType.MONITOR_POINT ? monitorOptions : doorOptions}
              defaultValue={selectedComponent.componentGuid}
              onChange={(event) => handleSelect(event.target.value, event)}
              name="pointNumber"
              disabled={isReadOnly || (selectedComponent.type === MonitorType.MONITOR_POINT ? monitorOptions.length === 0 : doorOptions.length === 0)}
            />
          </FormField>
          <div className="flex items-end lg:col-span-2">
            <Button
              disabled={isReadOnly || !selectedComponent.componentGuid}
              onClick={handleClickAdd}
            >
              Add component
            </Button>
          </div>
          <div className="lg:col-span-2">
            <div className="mb-3 flex items-center justify-between gap-3">
              <Label>Selected components</Label>
              <Button
                variant="danger"
                size="sm"
                disabled={isReadOnly || selectedComponents.length === 0}
                onClick={handleRemoveSelected}
              >
                Remove selected
              </Button>
            </div>
            {dto.components.length === 0 ? (
              <p className="rounded-xl border border-dashed border-gray-300 p-5 text-center text-sm text-gray-500 dark:border-gray-700 dark:text-gray-400">
                No components selected.
              </p>
            ) : (
              <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 xl:grid-cols-3">
                {dto.components.map((component) => {
                  const key = getComponentKey(component);
                  const isSelected = selectedComponents.includes(key);
                  return (
                    <button
                      key={key}
                      type="button"
                      aria-pressed={isSelected}
                      disabled={isReadOnly}
                      onClick={() => toggleSelected(component)}
                      className={`flex items-center gap-3 rounded-xl border p-4 text-left transition ${
                        isSelected
                          ? "border-brand-500 bg-brand-50 dark:bg-brand-500/10"
                          : "border-gray-200 bg-white hover:bg-gray-50 dark:border-gray-800 dark:bg-gray-900 dark:hover:bg-white/[0.03]"
                      } ${isReadOnly ? "cursor-default" : "cursor-pointer"}`}
                    >
                      <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-300">
                        {
                          component.type === MonitorType.MONITOR_POINT ? 
                          <MonitorIcon />
                          
                          :
                          <DoorIcon />
                        }
                        
                      </span>
                      <span className="min-w-0">
                        <span className="block truncate text-sm font-medium text-gray-800 dark:text-white/90">
                          { component.type === MonitorType.MONITOR_POINT
                            ? getMonitorComponentLabel(component)
                            : getDoorComponentLabel(component) }
                        </span>
                        <span className="block text-xs text-gray-500 dark:text-gray-400">
                          {MonitorType[component.type]}
                        </span>
                      </span>
                    </button>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </FormField>
    </div>
  );
};
