import { PropsWithChildren, useEffect } from "react";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { Vendor } from "../../enum/Vendor";
import { RelayMode } from "../../enum/RelayMode";

type ExtraProps = {
  moduleOption:Options[]
  outputOption:Options[]
};

const DoorBuzzerForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  outputOption,
  handleChange
}) => {

  const isReadOnly = type == FormType.INFO || dto.buzzer == null

  //  useEffect(() => {
  //       if (setIsNext) {
  //         let isValid = false;
  //         // 1. Safely find the reader (might be undefined)
  //         const buzzer = dto.buzzer;
    
  //         // 2. Extract values safely, providing fallbacks if undefined
  //         const moduleGuid = buzzer?.deviceModuleGuid || "";
  //         const slotNo = buzzer?.slotNo ?? -1;
    
  //         // 3. Perform the validation check safely
  //         isValid = moduleGuid.trim() !== "" && slotNo !== -1;
    
    
  //         console.log("BuzzerForm isValid:", isValid); 
  //         setIsNext(isValid); // This will now successfully run!
  //       }
  //     }, [dto.buzzer, setIsNext]); // Simplified dependency array

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="buzzer.module">Buzzer - Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="buzzer.module"
          options={moduleOption}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              buzzer:{
                slotNo:-1,
                mode:RelayMode.None,
                metadata:"",
                deviceModuleGuid:e.target.value,
                vendor:Vendor.aero
              }
            }))
            handleChange(e)
          }
            
          }
          className="dark:bg-dark-900"
          defaultValue={dto.buzzer?.deviceModuleGuid ?? ""}
        />
      </FormField>
      <FormField>
        <Label htmlFor="buzzer.slot">Output No</Label>
        <Select
          disabled={isReadOnly}
          name="strk.slot"
          options={outputOption}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              buzzer:prev.buzzer != null ?
               {
                ...prev.buzzer,
                slotNo:Number(e.target.value)
               }
              :
              prev.buzzer
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={dto.buzzer?.slotNo ?? -1}
        />
      </FormField>
    </div>
  );
};

export default DoorBuzzerForm;
