import React, { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { AeroBgMetadata } from "../../model/Door/AeroBgMetadata";
import Input from "../../components/form/input/InputField";

type ExtraProps = {
  moduleOption:Options[]
  inputOption:Options[]
};

const DoorBGForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  handleChange,
  moduleOption,
  inputOption
}) => {
  const isReadOnly = type == FormType.INFO || dto.bg == null

  //  useEffect(() => {
  //         if (setIsNext) {
  //           let isValid = false;
  //           // 1. Safely find the reader (might be undefined)
  //           const bg = dto.bg;
      
  //           // 2. Extract values safely, providing fallbacks if undefined
  //           const moduleGuid = bg?.deviceModuleGuid || "";
  //           const slotNo = bg?.slotNo ?? -1;
      
  //           // 3. Perform the validation check safely
  //           isValid = moduleGuid.trim() !== "" && slotNo !== -1;
      
      
  //           console.log("BgForm isValid:", isValid); 
  //           setIsNext(isValid); // This will now successfully run!
  //         }
  //       }, [dto.buzzer, setIsNext]); // Simplified dependency array
        
  return (
     <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="bg.module">Break Glass Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="bg.module"
          options={moduleOption}
          onChange={(e) => {
             setDto((prev) => ({
              ...prev,
              bg:{
                slotNo:-1,
                mode:InputMode.NC,
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value,
                metadata:""
              }
            }))
            handleChange(e)
          }
          }
          className="dark:bg-dark-900"
          defaultValue={dto.bg?.deviceModuleGuid ?? ""}
        />
      </FormField>
      <FormField>
        <Label htmlFor="bg.slot">Input No</Label>
        <Select
          disabled={isReadOnly}
          name="bg.slot"
          options={inputOption}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              bg:prev.bg != null 
              ?
              {
                ...prev.bg,
                slotNo:Number(e.target.value)
              }
               :
               prev.bg
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={dto.bg?.slotNo ?? -1}
        />
      </FormField>
   
       <FormField>
        <Label htmlFor="bg.mode">Input Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="bg.mode"
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
              bg:prev.bg != null ? 
              {
                ...prev.bg,
                mode:Number(e.target.value)
              }
              :
              prev.bg
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.bg?.mode ?? InputMode.NC
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.debounce">Debounce</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="0" max="15" 
        value={ (dto.bg?.metadata as AeroBgMetadata)?.debounce ?? ""}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              bg: prev.bg != null ? 
              {
                ...prev.bg,
                metadata:{
                  ...(prev.bg.metadata as AeroBgMetadata),
                  debounce:Number(e.target.value)
                }
              }
              :
              prev.bg
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.holdTime">Hold Time</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="2" max="15" 
        defaultValue={2}
        value={(dto.bg?.metadata as AeroBgMetadata)?.holdTime ?? ""}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              bg: prev.bg != null ? 
              {
                ...prev.bg,
                metadata:{
                  ...(prev.bg.metadata as AeroBgMetadata),
                  holdTime:Number(e.target.value)
                }
              }
              :
              prev.bg
            }))
          }
        />
      </FormField>
    </div>
  );
};

export default DoorBGForm;
