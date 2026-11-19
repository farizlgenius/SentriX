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

  const [tran, setTran] = useState<Options[]>([]);
  const [code, setCode] = useState<Options[]>([]);
  const [device, setDevice] = useState<Options[]>([]);
  const [tz, setTz] = useState<Options[]>([]);
  const [procedure, setProcedure] = useState<Options[]>([]);
  const [loading, setLoading] = useState(true);

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
    try{
      setLoading(false);
      const res = await send.get(TriggerEndpoint.GET_TRIGGER_CODE(type))
    if(res.status == HttpStatusCode.Ok){
      setTriggerCodeOption(res.data.data);
      setSelectedTriggerCodes(res.data.data.filter((x:Options) => dto.triggerCodes.includes(x.value.toString())))
      setAvailableTriggerCodes(res.data.data.filter((x:Options) => !dto.triggerCodes.includes(x.value.toString())));
    }
    }catch (err){
      console.error("Failed to load trigger code",err);
    }finally{
      setLoading(false);
    }
  }


  const handleTriggerCodeListChange = (data: Options[]) => {
      setDto((prev) => ({
        ...prev,
        triggerCodes: data.map((x) => x.value.toString()),
      }));
    };

  

  useEffect(() => {
    fetchDevice();
    fetchTriggerSource();
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
          onChange={e => 
            setDto(prev => ({
              ...prev,
              device:e.target.value
            }))
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
          isString
          name="triggerSource"
          options={triggerSourceOption}
          defaultValue={dto.triggerSource}
          onChange={e => {
            setDto(prev => ({
              ...prev,
              triggerSource:e.target.value,
              triggerType:"",
              triggerCodes:[]
            }));
            fetchTriggerType(e.target.value)
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
      <div></div>
      <FormField className="col-span-2">
        <Label>Trigger Code</Label>
        {/* <Select
          isString
          name="triggerCode"
          options={triggerTypeOption}
          defaultValue={dto.triggerType}
          onChange={e =>
            setDto(prev => ({
              ...prev,
              triggerType:e.target.value
            }))
          }
        /> */}
        {loading ? (
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
              )}
        
      </FormField>
      <FormField>
        <Label>Source Device</Label>
        <Select
          name="sourceNumber"
          options={device}
          defaultValue={dto.src}
           onChange={e =>
            setDto(prev => ({
              ...prev,
              src:e.target.value
            }))
          }
        />
      </FormField>
      <FormField>
        <Label>Tran Type</Label>
        <Select
          name="tranType"
          options={tran}
          defaultValue={dto.triggerType}
           onChange={e =>
            setDto(prev => ({
              ...prev,
              srcType:Number(e.target.value)
            }))
          }
        />
      </FormField>
      <FormField>
        <Label>Tran Code</Label>
        <Select
          name="codeMap"
          options={code}
          defaultValue={-1}
        
        />
      </FormField>
      <FormField>
        <Label>Time Zone</Label>
        <Select
          name="timeZone"
          options={tz}
          defaultValue={dto.timeZone}
          onChange={e =>
            setDto(prev => ({
              ...prev,
              timeZone:e.target.value
            }))
          }
        />
      </FormField>
    </div>
  );
};
