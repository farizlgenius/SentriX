import React, { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";

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
              label:InputMode[InputMode.NC],
              value:InputMode.NC
            },
            {
              label:InputMode[InputMode.NO],
              value:InputMode.NO
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
    </div>
  );
};

export default DoorBGForm;
