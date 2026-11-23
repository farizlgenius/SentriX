import { PropsWithChildren, useEffect, useState } from "react";
import Input from "../../components/form/input/InputField";
import Label from "../../components/form/Label";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { TriggerDto } from "../../model/Trigger/TriggerDto";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { send } from "../../api/api";
import { TriggerEndpoint } from "../../endpoint/TriggerEndpoint";
import { useLocation } from "../../context/LocationContext";
import { FormField } from "../../components/form/template/FormTemplate";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { HttpStatusCode } from "axios";
import { TriggerCommand } from "../../enum/TriggerCommand";
import ListTransfer, { ListOptionsTransfer } from "../../components/form/list-transfer/ListTransfer";
import { EntityType } from "../../enum/EntityType";
import { TimezoneEndPoint } from "../../endpoint/TimezoneEndpoint";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { ProcedureEndpoint } from "../../endpoint/ProcedureEndpoint";
import { ControlEndpoint } from "../../endpoint/ControlPointEndpoint";
import { MonitorPointEndpoint } from "../../endpoint/MonitorPointEndpoint";

export const TriggerForm: React.FC<PropsWithChildren<FormProp<TriggerDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  const { locationGuid } = useLocation();
  const [deviceOption,setDeviceOption] = useState<Options[]>([])
  const [triggerSourceOption, setTriggerSourceOption] = useState<Options[]>([]);
  const [triggerTypeOption, setTriggerTypeOption] = useState<Options[]>([]);
  
  const [triggerCodeOption,setTriggerCodeOption] = useState<Options[]>([]);
  const [selectedTriggerCodes,setSelectedTriggerCodes] = useState<Options[]>([]);
  const [availableTriggerCodes,setAvailableTriggerCodes] = useState<Options[]>([]);

  const [triggerArgOption,setTriggerArgOption] = useState<Options[]>([]);
  const [selectedTriggerArgs,setSelectedTriggerArgs] = useState<Options[]>([]);
  const [availableTriggerArgs,setAvailableTriggerArgs] = useState<Options[]>([]);

  const [sourceOption,setSourceOption] = useState<Options[]>([]);
  const [tzOption,setTzOption] = useState<Options[]>([]);

  const [procedureOption,setProcedureOption] = useState<Options[]>([]);



  const isReadOnly = type == FormType.INFO 

  const fetchDevice = async () => {
    const res = await send.get(DeviceEndpoint.GET_OPTION(locationGuid))
    if(res.status == HttpStatusCode.Ok){
      setDeviceOption(res.data.data);
    }
  }

  const fetchTriggerSource = async () => {
    const res = await send.get(TriggerEndpoint.GET_TRIGGER_SOURCE)
    if(res.status == HttpStatusCode.Ok){
      setTriggerSourceOption(res.data.data);
    }
  }

  const fetchTriggerType = async (source:string) => {
    const res = await send.get(TriggerEndpoint.GET_TRIGGER_TYPE(source))
    if(res.status == HttpStatusCode.Ok){
      setTriggerTypeOption(res.data.data);
    }
  }

  const fetchTriggerCode = async (type:string) => {
    const res = await send.get(TriggerEndpoint.GET_TRIGGER_CODE(type))
    if(res.status == HttpStatusCode.Ok){
      setTriggerCodeOption(res.data.data);
      setSelectedTriggerCodes(res.data.data.filter((x:Options) => dto.triggerCodes.includes(x.value.toString())))
      setAvailableTriggerCodes(res.data.data.filter((x:Options) => !dto.triggerCodes.includes(x.value.toString())));
    }
  }

  const fetchTriggerArg = async (code:string[]) => {
   const res = await send.post(TriggerEndpoint.GET_TRIGGER_ARGS,code)
    if(res.status == HttpStatusCode.Ok){
      setTriggerArgOption(res.data.data);
      setSelectedTriggerArgs(res.data.data.filter((x:Options) => dto.triggerArgs.includes(x.value.toString())))
      setAvailableTriggerArgs(res.data.data.filter((x:Options) => !dto.triggerArgs.includes(x.value.toString())));
    }
  }

  const getSourceSwitcher = async (type:EntityType) => {
    switch(type){
      case EntityType.DEVICE_MODULE:
        return await send.get(ModuleEndpoint.GET_OPTION(dto.device));
      case EntityType.DOOR:
        return await send.get(DoorEndpoint.GET_BY_DEVICE(dto.device));
      case EntityType.OUTPUT:
        return await send.get(ControlEndpoint.GET_OPTION_BY_DEVICE(dto.device));
      case EntityType.INPUT:
        return await send.get(MonitorPointEndpoint.GET_OPTION_BY_DEVICE(dto.device));
      default:
        break;
    }
  }

  const fetchSourceDevice = async (type:EntityType) => {
    if(type == EntityType.DEVICE){
      setDto(prev => ({
        ...prev,
        src:prev.device
      }))
      return;
    }
    var res = await getSourceSwitcher(type);
    if(res.status == HttpStatusCode.Ok){
      setSourceOption(res.data.data);
    }
  }

  const fetchTimeZone = async () => {
    const res = await send.get(TimezoneEndPoint.GET_OPTION_BY_LOCATION(locationGuid))
    if(res.status == HttpStatusCode.Ok){
      setTzOption(res.data.data);
    }
  }

  const fetchProcedure = async (device:string) => {
    const res = await send.get(ProcedureEndpoint.GET_OPTION_BY_DEVICE(device))
    if(res.status == HttpStatusCode.Ok){
      setProcedureOption(res.data.data);
    }
  }


  const handleTriggerCodeListChange = (data: Options[]) => {
      setDto((prev) => ({
        ...prev,
        triggerCodes: data.map((x) => x.value.toString()),
      }));
      fetchTriggerArg(data.map(x => x.value.toString()));
    };

    const handleTriggerArgListChange = (data: Options[]) => {
      setDto((prev) => ({
        ...prev,
        triggerArgs: data.map((x) => x.value.toString()),
      }));
    };

  

  useEffect(() => {
    fetchDevice();
    fetchTriggerSource();
    fetchTimeZone();
  }, []);
  return (
    <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
       
      <FormField>
        <Label>Name</Label>
        <Input 
        name="name" 
        type="text" 
        placeholder="Trigger name" 
        onChange={e => setDto(prev => ({...prev,name:e.target.value}))}
        />
      </FormField>
      <FormField>
        <Label>Device</Label>
        <Select
          isString
          name="device"
          options={deviceOption}
          defaultValue={dto.device}
          onChange={e => {
            setDto(prev => ({
              ...prev,
              device:e.target.value
            }))
            fetchProcedure(e.target.value);
          }
            
          }
        />
      </FormField>
      <FormField>
        <Label>Command</Label>
        <Select
          name="command"
          options={
            [
              {
                label:"Abort Delay",
                value:TriggerCommand.ABORT_DELAY
              },{
                label:"Execute Action Prefix 0",
                value:TriggerCommand.EXE_ACTION_PREFIX_0,
              },
              {
                label:"Execute Action Prefix 256",
                value:TriggerCommand.EXE_ACTION_PREFIX_256,
              },
              {
                label:"Execute Action Prefix 512",
                value:TriggerCommand.EXE_ACTION_PREFIX_512,
              },
              {
                label:"Execute Action Prefix 1024",
                value:TriggerCommand.EXE_ACTION_PREFIX_1024,
              },
              {
                label:"Resume Delay And Execute Action Prefix 0",
                value:TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_0,
              },
              {
                label:"Resume Delay And Execute Action Prefix 256",
                value:TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_256,
              },
              {
                label:"Resume Delay And Execute Action Prefix 512",
                value:TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_512,
              },
              {
                label:"Resume Delay And Execute Action Prefix 1024",
                value:TriggerCommand.RESUME_DELAY_AND_EXE_ACTION_PREFIX_1024,
              },
            ]
          }
          defaultValue={dto.command}
          onChange={e => 
            setDto(prev => ({
              ...prev,
              command:Number(e.target.value)
            }))
          }
        />
      </FormField>
      <FormField>
        <Label>Trigger Source</Label>
        <Select
          disabled={isReadOnly || dto.device == "" || dto.device == undefined}
          isString
          name="triggerSource"
          options={triggerSourceOption}
          defaultValue={dto.triggerSource}
          onChange={e => {
            setDto(prev => ({
              ...prev,
              triggerSource:e.target.value,
              srcType: Number(triggerSourceOption.find(x => x.value == e.target.value)?.description) ?? EntityType.UNKNOWN,
              triggerType:"",
              triggerCodes:[],
              src:""
            }));
            fetchTriggerType(e.target.value)
            setSourceOption([])
            fetchSourceDevice(Number(triggerSourceOption.find(x => x.value == e.target.value)?.description) ?? EntityType.UNKNOWN);
            setTriggerCodeOption([])
          }
          }
        />
      </FormField>
      <FormField>
        <Label>Trigger Type</Label>
        <Select
          isString
          name="triggerType"
          options={triggerTypeOption}
          defaultValue={dto.triggerType}
          onChange={e => {
             setDto(prev => ({
              ...prev,
              triggerType:e.target.value,
              triggerCodes:[]
            }));
            fetchTriggerCode(e.target.value)

          } }
        />
      </FormField>
     <FormField>
        <Label>Source Device</Label>
        <Select
          disabled={isReadOnly || dto.triggerSource == ""}
          isString
          name="src"
          options={sourceOption}
          defaultValue={dto.src}
           onChange={e =>
            setDto(prev => ({
              ...prev,
              src:e.target.value
            }))
          }
        />
      </FormField>
      <FormField className="col-span-2">
        <Label>Trigger Code</Label>
        {/* {codeLoading ? (
                <p>Loading trigger codes...</p>
              ) : 
              triggerCodeOption.length == 0 ? 
                <p>No trigger code available for selected type.</p>
              :
              (
                <ListOptionsTransfer<Options>
                  availableItems={availableTriggerCodes}
                  selectedItems={selectedTriggerCodes}
                  onChange={handleTriggerCodeListChange}
                  disabled={isReadOnly}
                />
              )} */}

           <ListOptionsTransfer<Options>
                  availableItems={availableTriggerCodes}
                  selectedItems={selectedTriggerCodes}
                  onChange={handleTriggerCodeListChange}
                  disabled={isReadOnly}
                />
        
      </FormField>
      <FormField className="col-span-2">
        <Label>Trigger Args</Label>
        {/* {argLoading ? (
                <p>Loading trigger args...</p>
              ) : 
              triggerArgOption.length == 0 ? 
                <p>No trigger code available for selected type.</p>
              :
              (
                <ListOptionsTransfer<Options>
                  availableItems={availableTriggerArgs}
                  selectedItems={selectedTriggerArgs}
                  onChange={handleTriggerArgListChange}
                  disabled={isReadOnly}
                />
              )} */}
               <ListOptionsTransfer<Options>
                  availableItems={availableTriggerArgs}
                  selectedItems={selectedTriggerArgs}
                  onChange={handleTriggerArgListChange}
                  disabled={isReadOnly}
                />
      </FormField>
      
      <FormField>
        <Label>Time Zone</Label>
        <Select
          isString
          name="timeZone"
          options={tzOption}
          defaultValue={dto.timeZone}
          onChange={e =>
            setDto(prev => ({
              ...prev,
              timeZone:e.target.value
            }))
          }
        />
      </FormField>
       <FormField>
        <Label>Procedure to trigger</Label>
        <Select
          isString
          name="procedure"
          options={procedureOption}
          defaultValue={dto.procedure}
          onChange={e =>
            setDto(prev => ({
              ...prev,
              procedure:e.target.value
            }))
          }
        />
      </FormField>
    </div>
  );
};
