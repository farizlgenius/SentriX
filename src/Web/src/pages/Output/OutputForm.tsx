import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { OutputDto } from "../../model/ControlPoint/OutputDto";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { OutputEndpoint } from "../../endpoint/ControlPointEndpoint";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import api, { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import {
  FormField,
} from "../../components/form/template/FormTemplate";
import { Vendor } from "../../enum/Vendor";
import { AeroOutputMetadata } from "../../model/ControlPoint/AeroOutputMetadata";
import { OfflineMode } from "../../enum/OfflineMode";
import { DriveMode } from "../../enum/DriveMode";

const OutputForm: React.FC<PropsWithChildren<FormProp<OutputDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  const isReadOnly = type == FormType.INFO;
  const { locationGuid } = useLocation();
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  {
    /* Select */
  }
  const [controllerOption, setControllerOption] = useState<Options[]>([]);
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const [relayOption, setRelayOption] = useState<Options[]>([]);
  const [offlineModeOption, setOfflineModeOption] = useState<Options[]>([]);
  const [driveModeOption, setDriverModeOption] = useState<Options[]>([]);
  const [selectedDevice,setSelectedDevice] = useState<string>("");
  const [controller, setController] = useState<number>(-1);

  const handleSelect = async (
    value: string,
    e: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    switch (e.target.name) {
      case "driverId":
        fetchModuleByDevice(
          controllerOption.find((a) => a.value == Number(value))
            ?.additionalInfo,
        );
        setDto((prev) => ({
          ...prev,
          deviceComponentId: Number(value),
          mac:
            controllerOption.find((a) => a.value == Number(value))
              ?.description ?? "",
        }));
        break;
      case "moduleId":
        fetchOutput(
          moduleOption.find((a) => a.value == Number(value))?.additionalInfo,
        );
        setDto((prev) => ({
          ...prev,
          moduleComponentId: Number(value),
          model:
            moduleOption.find((a) => a.value == Number(value))?.label ?? "",
        }));
        break;
      case "offlineMode":
        console.log(value);
        setDto((prev) => ({ ...prev, offlineMode: Number(value) }));
        break;
      case "driveMode":
        console.log(value);
        setDto((prev) => ({ ...prev, driveMode: Number(value) }));
        break;
      default:
        setDto((prev) => ({ ...prev, [e.target.name]: value }));
        break;
    }
  };

  {
    /* Controller Data */
  }
  const fetchDevice = async () => {
    const res = await send.get(
      DeviceEndpoint.GET_OPTION(locationGuid),
    );
    console.log(res);
    if (res.data) {
      res.data.data.map((a: Options) => {
        setControllerOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: a.isTaken,
          },
        ]);
      });
    }
  };

  const fetchModuleByDevice = async (value: string) => {
    const res = await send.get(ModuleEndpoint.GET_OPTION(value));
    console.log(res);
    if (res.data.data) {
      res.data.data.map((a: Options) => {
        setModuleOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: a.isTaken,
          },
        ]);
      });
    }
  };

  const fetchOutput = async (value: string) => {
    var res = await send.get(ModuleEndpoint.GET_OUTPUT_SLOT(value));
    console.log(res);
    if (res) {
      res.data.data.map((a: Options) => {
        setRelayOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value
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
  }, []);

  return (
    <>
    <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input
              disabled={isReadOnly}
              name="name"
              value={dto.name}
              type="text"
              id="name"
              onChange={handleChange}
              placeholder="Control point name"
            />
          </FormField>
          <FormField>
            <Label>Controller</Label>
            <Select
              isString={true}
              name="device"
              options={controllerOption}
              placeholder="Select Option"
              onChange={e => {
                setSelectedDevice(e.target.value);
                fetchModuleByDevice(e.target.value);
                setDto(prev => ({
                  ...prev,
                  vendor: Number(controllerOption.find(x => x.value == e.target.value)?.description)
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={selectedDevice}
              disabled={isReadOnly}
            />
          </FormField>
          <FormField>
            <Label>Module</Label>
            <Select
              isString={true}
              name="deviceModuleGuid"
              options={moduleOption}
              placeholder="Select Option"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  deviceModuleGuid:e.target.value
                }))
                fetchOutput(e.target.value)
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.deviceModuleGuid}
              disabled={isReadOnly}
            />
          </FormField>
          <FormField>
            <Label>Relay</Label>
            <Select
              name="outputNo"
              options={relayOption}
              placeholder="Select Option"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  slotNo:Number(e.target.value)
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.slotNo}
              disabled={isReadOnly}
            />
          </FormField>
          {
            dto.vendor == Vendor.aero && 
            <>
            <FormField>
            <Label>Offline Mode</Label>
            <Select
              name="offlineMode"
              options={
                [
                {
                  label:"No Change",
                  value:OfflineMode.NoChange
                },
                 {
                  label:"Inactive",
                  value:OfflineMode.Inactive
                },
                 {
                  label:"Active",
                  value:OfflineMode.Active
                }
              ]}
              placeholder="Select Option"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  metadata:{
                    ...(prev.metadata as AeroOutputMetadata),
                    offlineMode:Number(e.target.value)
                  }
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={(dto.metadata as AeroOutputMetadata).offlineMode}
              disabled={isReadOnly}
            />
          </FormField>
          <FormField>
            <Label>Drive Mode</Label>
            <Select
              name="driveMode"
               options={
                [
                {
                  label:"Normal",
                  value:DriveMode.Normal
                },
                 {
                  label:"Inverted",
                  value:DriveMode.Inverted
                }
              ]}
              placeholder="Select Option"
               onChange={e => {
                setDto(prev => ({
                  ...prev,
                  metadata:{
                    ...(prev.metadata as AeroOutputMetadata),
                    driveMode:Number(e.target.value)
                  }
                }))
              }}
              className="dark:bg-dark-900"
             defaultValue={(dto.metadata as AeroOutputMetadata).driveMode}
              disabled={isReadOnly}
            />
          </FormField>
          <FormField>
            <Label htmlFor="defaultPulseTime">Pulse Time (second)</Label>
            <Input
              disabled={isReadOnly}
              defaultValue={1}
              value={(dto.metadata as AeroOutputMetadata).defaultPulse}
              min="0"
              max="500"
              name="defaultPulse"
              type="number"
              id="defaultPulse"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  metadata:{
                    ...(prev.metadata as AeroOutputMetadata),
                    defaultPulse:Number(e.target.value)
                  }
                }))
              }}
            />
          </FormField>
            </>
          }
          
        </div>
      {/* <FormSection
        title="Control Point Details"
        description="Name the location, assign its country, and add a short description."
        className="pb-10 mb-5"
      >
        
      </FormSection>
      <FormActions
        // disabled={isReadOnly}
        onSubmit={handleClick}
        onCancel={handleClick}
        cancelName="close"
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      /> */}
    </>
  );
};

export default OutputForm;
