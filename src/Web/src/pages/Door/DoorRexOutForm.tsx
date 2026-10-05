import { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { useLocation } from "../../context/LocationContext";
import { AeroRexMetadata } from "../../model/Door/AeroRexMetadata";
import Input from "../../components/form/input/InputField";
import Helper from "../../utility/Helper";

type ExtraProps = {
  moduleOption:Options[]
  inputOption:Options[],
  setInputOption:React.Dispatch<React.SetStateAction<Options[]>>
  timeOption:Options[]
  fetchTime:(guid:string) => Promise<void>;
};


const DoorRexOutForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  inputOption,
  timeOption,
  handleChange,
  fetchTime,
  setIsNext,
  setInputOption
}) => {
  const {locationGuid} = useLocation();

  useEffect(() => {
      if (setIsNext) {
        let isValid = false;
        // 1. Safely find the reader (might be undefined)
        const rex = dto.rexes;
  
        // 2. Extract values safely, providing fallbacks if undefined
        const moduleGuid = rex[0]?.deviceModuleGuid || "";
        const slotNo = rex[0]?.slotNo ?? -1;

        if(moduleGuid !== ""){
          handleChange({
            target: {
              name: "rex.module",
              value: moduleGuid
            }
          } as React.ChangeEvent<HTMLInputElement>);
        }
  
        // 3. Perform the validation check safely
        isValid = moduleGuid.trim() !== "" && slotNo !== -1;
  
  
        console.log("RexForm isValid:", isValid); 
        setIsNext(isValid); // This will now successfully run!
      }
    }, [dto.rexes, setIsNext]); // Simplified dependency array

  useEffect(() => {
    fetchTime(locationGuid);
  },[])
  const isReadOnly = type == FormType.INFO || dto.rexes.length == 0
  return (
   <>
   <h6 className="mt-2 mb-2 text-xl font-semibold text-gray-900 dark:text-white">Rex 1</h6>
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="rex.module">Module *</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="rex.module"
          options={moduleOption}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              rexes:[
                {
                slotNo:-1,
                mode:InputMode.NC,
                maskGuid:null,
                metadata:{
                  debounce:2,
                  holdTime:0,
                  mode:InputMode.NC
                },
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value
              }
              ]
            }))
            handleChange(e)
          }
             
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rexes[0]?.deviceModuleGuid ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex.slot">Slot *</Label>
        <Select
          disabled={isReadOnly}
          name="rex.slot"
          options={inputOption}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 0 ? 
              {
                ...d,
                slotNo:Number(e.target.value)
              }
              :
              d
              )
            }))
            setInputOption(prev => 
              Helper.updateOptionByValue(prev,e.target.name,true)
            )
          }
            
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rexes[0]?.slotNo ?? -1
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputMode">Mode</Label>
        <Select
          disabled={isReadOnly}
          name="rex0.inputMode"
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
              label:"Standard Noraml 1K Active 2K",
              value:InputMode.StandardN1A2
            },
            {
              label:"Standard Noraml 2K Active 1K",
              value:InputMode.StandardN2A1
            }
          ]}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 0 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  mode:Number(e.target.value)
                }
              } : d
              )
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={(dto.rexes[0]?.metadata as AeroRexMetadata)?.mode ?? InputMode.NC}
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputMode">Debounce</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="0" max="15" 
        value={(dto.rexes[0]?.metadata as AeroRexMetadata)?.debounce ?? ""}
        onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 0 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  debounce:Number(e.target.value)
                }
              } : d
              )
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="rex0.inputMode">Hold Time</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="2" max="15" 
        defaultValue={2}
        value={ (dto.rexes[0]?.metadata as AeroRexMetadata)?.holdTime ?? ""}
        onChange={(e) => 
           setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 0 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  holdTime:Number(e.target.value)
                }
              } : d
              )
            }))
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.MaskTimeZone">Mask Time (Optional)</Label>
        <Select
          isString={true}
          disabled={isReadOnly}
          name="rex0.MaskTimeZone"
          options={timeOption}
          onChange={(e) => 
           setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 0 ? {
                ...d,
                maskGuid:e.target.value
              } : d
              )
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
           dto.rexes[0]?.maskGuid ?? ""
          }
        />
      </FormField>
    </div>
     <div className="mt-5 mb-5 flex-grow border-t border-gray-600"></div>
     <h6 className="mt-2 mb-2 text-xl font-semibold text-gray-900 dark:text-white">Rex 2 (Option)</h6>
    <div className="grid grid-cols-2 gap-5">
       <FormField>
        <Label htmlFor="rex.module">Module *</Label>
        <Select
        isString={true}
          disabled={isReadOnly && dto.rexes[0]?.deviceModuleGuid !== ""}
          name="rex.module"
          options={moduleOption}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              rexes:[
               ...prev.rexes
                ,{
                slotNo:-1,
                mode:InputMode.NC,
                maskGuid:null,
                metadata:{
                  debounce:2,
                  holdTime:0,
                  mode:InputMode.NC
                },
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value
              }] 
            }))
            handleChange(e)
          }
             
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rexes[1]?.deviceModuleGuid ?? ""
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="rex.slot">Slot *</Label>
        <Select
          disabled={isReadOnly}
          name="rex.slot"
          options={inputOption.filter(x => x.value !== dto.rexes[0]?.slotNo)}
            onChange={(e) =>
              setDto((prev) => ({
                ...prev,
                rexes: prev.rexes.map((d, i) =>
                  i == 1 ?
                    {
                      ...d,
                      slotNo: Number(e.target.value)
                    }
                    :
                    d
                )
              }))
            }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rexes[1]?.slotNo ?? -1
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputMode">Mode</Label>
        <Select
          disabled={isReadOnly}
          name="rex0.inputMode"
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
              label:"Standard Noraml 1K Active 2K",
              value:InputMode.StandardN1A2
            },
            {
              label:"Standard Noraml 2K Active 1K",
              value:InputMode.StandardN2A1
            }
          ]}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 1 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  mode:Number(e.target.value)
                }
              } : d
              )
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={(dto.rexes[1]?.metadata as AeroRexMetadata)?.mode ?? InputMode.NC}
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputMode">Debounce</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="0" max="15" 
        value={ (dto.rexes[1]?.metadata as AeroRexMetadata)?.debounce ?? ""}
        onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 1 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  debounce:Number(e.target.value)
                }
              } : d
              )
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="rex0.inputMode">Hold Time</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="2" max="15" 
        defaultValue={2}
        value={ (dto.rexes[1]?.metadata as AeroRexMetadata)?.holdTime ?? ""}
        onChange={(e) => 
           setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 1 ? {
                ...d,
                metadata:{
                  ...(d.metadata as AeroRexMetadata),
                  holdTime:Number(e.target.value)
                }
              } : d
              )
            }))
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.MaskTimeZone">Mask Time (Optional)</Label>
        <Select
          isString={true}
          disabled={isReadOnly}
          name="rex0.MaskTimeZone"
          options={timeOption}
          onChange={(e) => 
           setDto((prev) => ({
              ...prev,
              rexes: prev.rexes.map((d,i) => 
              i == 1 ? {
                ...d,
                maskGuid:e.target.value
              } : d
              )
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
           dto.rexes[1]?.maskGuid ?? ""
          }
        />
      </FormField>
    </div>
    
   </>
  );
};

export default DoorRexOutForm;
