import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import Button from "../../components/ui/button/Button";
import { Options } from "../../model/Options";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { InputDto } from "../../model/MonitorPoint/InputDto";
import { ModeDto } from "../../model/ModeDto";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { MonitorPointEndpoint } from "../../endpoint/MonitorPointEndpoint";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import { HttpStatusCode } from "axios";
import { AeroInputMetadata } from "../../model/MonitorPoint/AeroInputMetadata";
import { InputMode } from "../../enum/InputMode";
import { InputLatchingMode } from "../../enum/InputLatchingMode";

const InputForm: React.FC<PropsWithChildren<FormProp<InputDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const isReadOnly = type == FormType.INFO;
  const { locationGuid } = useLocation();

  {
    /* Select */
  }
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const [controllerOption, setControllerOption] = useState<Options[]>([]);
  const [inputOption, setInputOption] = useState<Options[]>([]);
  const [selectedController,setSelectedController] = useState<string>("");


  {
    /* Controller Data */
  }
  const fetchController = async () => {
    let res = await send.get(
      DeviceEndpoint.GET_LOCATION(locationGuid),
    );
    if (res.data) {
      res.data.data.map((a: DeviceDto) => {
        setControllerOption((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.guid,
            description: a.mac,
            additionalInfo:a.vendor,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchModule = async (guid:string) => {
    const res = await send.get(ModuleEndpoint.GET_BY_DEVICE(guid));
    console.log(res);
    if(res.status == HttpStatusCode.Ok){
      res.data.data.map((a:Options) => {
        setModuleOption(prev => ([
          ...prev,
          {
            label:a.label,
            value:a.value
          }
        ]))
      })
    }
  }

  const fetchInputSlot = async (guid:string) => {
    const res = await send.get(ModuleEndpoint.GET_INPUT_SLOT(guid))
    if(res.status == HttpStatusCode.Ok){
      setInputOption(res.data.data)
    }
  }


  {
    /* UseEffect */
  }
  useEffect(() => {
    fetchController();
  }, []);

  return (
    <>
      <div className="grid gap-5 grid-cols-3 md:grid-cols-3 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input
              value={dto.name}
              placeholder="Name"
              name="name"
              type="text"
              id="name"
              onChange={e => setDto(prev => ({
                ...prev,
                name:e.target.value
              }))}
              disabled={isReadOnly}
            />
          </FormField>
          <FormField>
            <Label>Controller</Label>
            <Select
              disabled={isReadOnly}
              isString={true}
              name="deviceId"
              options={controllerOption}
              placeholder="Select Option"
              onChange={(e) => {
                setSelectedController(e.target.value)
                fetchModule(e.target.value)
                setDto(prev => ({
                  ...prev,
                  vendor:controllerOption.find(x => x.value == e.target.value)?.additionalInfo
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={selectedController}
            />
          </FormField>

          <FormField>
            <Label>Module</Label>
            <Select
              disabled={isReadOnly}
              isString
              name="moduleId"
              options={moduleOption}
              placeholder="Select Option"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  deviceModuleGuid:e.target.value
                }));
                fetchInputSlot(e.target.value)
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.deviceModuleGuid}
            />
          </FormField>
          <FormField>
            <Label>Input</Label>
            <Select
              disabled={isReadOnly}
              name="inputNo"
              options={inputOption}
              placeholder="Select Option"
              onChange={e => {
                setDto(prev => ({
                  ...prev,
                  slotNo:Number(e.target.value)
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.slotNo}
            />
          </FormField>
          <FormField>
            <Label>Hold Time</Label>
             <Input
              disabled={isReadOnly}
              value={(dto.metadata as AeroInputMetadata).holdTime}
              min="0"
              max="65535"
              name="holdTime"
              type="number"
              id="holdTime"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    holdTime:Number(e.target.value)
                  }
                }))
              }
            />
          </FormField>
          <FormField>
            <Label>Debounce</Label>
             <Input
              disabled={isReadOnly}
              value={(dto.metadata as AeroInputMetadata).debounce}
              min="0"
              max="65535"
              name="debounce"
              type="number"
              id="debounce"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    debounce:Number(e.target.value)
                  }
                }))
              }
            />
          </FormField>
          <FormField>
            <Label className="pb-3">Monitor Point Mode</Label>
            <Select
              disabled={isReadOnly}
              name="monitorPointMode"
              options={[
                {
                  label:"Normally Close",
                  value:InputMode.NC
                },
                {
                  label:"Normally Open",
                  value:InputMode.NO
                },
                {
                  label:"Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active",
                  value:InputMode.StandardN1A2
                },{
                  label:"Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active",
                  value:InputMode.StandardN2A1
                }
              ]}
              placeholder="Select Option"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    mode:Number(e.target.value)
                  }
                }))
              }
              className="dark:bg-dark-900"
              defaultValue={(dto.metadata as AeroInputMetadata).mode}
            />
          </FormField>
          <FormField>
            <Label className="pb-3">Log Function Mode</Label>
            <Select
              disabled={isReadOnly}
              name="logFunction"
              options={[
                {
                  label:"Normal",
                  value:InputLatchingMode.NORMAL
                },
                {
                  label:"Non Latching",
                  value:InputLatchingMode.NON_LATCHING
                },
                {
                  label:"Latching",
                  value:InputLatchingMode.LATCHING
                }
              ]}
              placeholder="Select Option"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    latchingMode:Number(e.target.value)
                  }
                }))
              }
              className="dark:bg-dark-900"
               defaultValue={(dto.metadata as AeroInputMetadata).latchingMode}
            />
          </FormField>

          <FormField
            className={
              (dto.metadata as AeroInputMetadata).latchingMode == InputLatchingMode.NON_LATCHING || (dto.metadata as AeroInputMetadata).latchingMode == InputLatchingMode.LATCHING
                ? ""
                : "hidden"
            }
          >
            <Label htmlFor="delayEntry">Delay Entry(s)</Label>
            <Input
              disabled={isReadOnly}
              value={(dto.metadata as AeroInputMetadata).delayEntry}
              min="0"
              max="65535"
              name="delayEntry"
              type="number"
              id="delayEntry"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    delayEntry:Number(e.target.value)
                  }
                }))
              }
            />
          </FormField>
           <FormField
            className={
              (dto.metadata as AeroInputMetadata).latchingMode == InputLatchingMode.NON_LATCHING || (dto.metadata as AeroInputMetadata).latchingMode == InputLatchingMode.LATCHING
                ? ""
                : "hidden"
            }
          >
            <Label htmlFor="delayExit">Delay Exit(s)</Label>
            <Input
              disabled={isReadOnly}
              value={(dto.metadata as AeroInputMetadata).delayExit}
              min="0"
              max="65535"
              name="delayExit"
              type="number"
              id="delayExit"
              onChange={e => 
                setDto(prev => ({
                  ...prev,
                  metadata : {
                    ...(prev.metadata as AeroInputMetadata),
                    delayExit:Number(e.target.value)
                  }
                }))
              }
            />
          </FormField>
        </div>
    </>
  );
};

export default InputForm;
