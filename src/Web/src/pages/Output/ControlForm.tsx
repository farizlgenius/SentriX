import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { ControlDto } from "../../model/ControlPoint/ControlDto";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import  { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import {
  FormField,
} from "../../components/form/template/FormTemplate";
import { Vendor } from "../../enum/Vendor";
import { AeroOutputMetadata } from "../../model/ControlPoint/AeroOutputMetadata";
import { OfflineMode } from "../../enum/OfflineMode";
import { DriveMode } from "../../enum/DriveMode";
import { HttpStatusCode } from "axios";

const ControlForm: React.FC<PropsWithChildren<FormProp<ControlDto>>> = ({
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


  {
    /* Controller Data */
  }
  const fetchDevice = async () => {
    const res = await send.get(
      DeviceEndpoint.GET_OPTION(locationGuid),
    );
    if (res.status == HttpStatusCode.Ok) {
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
    if (res.status == HttpStatusCode.Ok) {
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
    if (res.status == HttpStatusCode.Ok) {
      res.data.data.map((a: Options) => {
        setRelayOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken:a.isTaken
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
    if(type == FormType.INFO || type == FormType.UPDATE){
      fetchModuleByDevice(dto.device.guid ?? "");
      fetchOutput(dto.deviceModule.guid ?? "");
    }    
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
                fetchModuleByDevice(e.target.value);
                setDto(prev => ({
                  ...prev,
                  device:{
                    ...prev.device,
                    guid:e.target.value,
                    vendor:Number(controllerOption.find(x => x.value == e.target.value)?.description) 
                  }
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.device.guid as string}
              disabled={isReadOnly || dto.name == ""}
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
                  deviceModule:{
                    ...prev.deviceModule,
                    guid:e.target.value,
                    vendor:Number(moduleOption.find(x => x.value == e.target.value)?.description) 
                  }
                }))
                fetchOutput(e.target.value)
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.deviceModule.guid as string}
              disabled={isReadOnly || dto.device.guid == ""}
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
              disabled={isReadOnly || dto.deviceModule.guid == ""}
            />
          </FormField>
          {
            dto.deviceModule.vendor == Vendor.aero && 
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
    </>
  );
};

export default ControlForm;
