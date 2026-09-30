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

type ExtraProps = {
  moduleOption:Options[]
  inputOption:Options[]
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
  setIsNext
}) => {
  const {locationGuid} = useLocation();

  useEffect(() => {
      if (setIsNext) {
        let isValid = false;
        // 1. Safely find the reader (might be undefined)
        const rex = dto.rex;
  
        // 2. Extract values safely, providing fallbacks if undefined
        const moduleGuid = rex?.deviceModuleGuid || "";
        const slotNo = rex?.slotNo ?? -1;
  
        // 3. Perform the validation check safely
        isValid = moduleGuid.trim() !== "" && slotNo !== -1;
  
  
        console.log("RexForm isValid:", isValid); 
        setIsNext(isValid); // This will now successfully run!
      }
    }, [dto.rex, setIsNext]); // Simplified dependency array

  useEffect(() => {
    fetchTime(locationGuid);
  },[])
  const isReadOnly = type == FormType.INFO || dto.rex == null  
  return (
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
              rex:{
                slotNo:-1,
                mode:InputMode.NC,
                metadata:"",
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value
              } 
            }))
            handleChange(e)
          }
             
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rex?.deviceModuleGuid ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex.slot">Slot *</Label>
        <Select
          disabled={isReadOnly}
          name="rex.slot"
          options={inputOption}
          onChange={(e) =>
            setDto((prev) => ({
              ...prev,
              rex: prev.rex != null ?
                {
                  ...prev.rex,
                  slotNo: Number(e.target.value)
                }
                :
                prev.rex
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.rex?.slotNo ?? -1
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
              label:"NO",
              value:InputMode.NO
            },
            {
              label:"NC",
              value:InputMode.NC
            }
          ]}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              rex: prev.rex != null ? 
              {
                ...prev.rex,
                mode:Number(e)
              }
              :
              prev.rex
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={ dto.rex?.mode ?? InputMode.NC}
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
            setDto(prev => ({
              ...prev,
              rex: prev.rex != null ? 
              {
                ...prev.rex,
                metadata:{
                  maskTimeGuid:e.target.value
                }
              }
              :
              prev.rex
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
           (dto.rex?.metadata as AeroRexMetadata)?.maskTimeGuid ?? ""
          }
        />
      </FormField>
    </div>
  );
};

export default DoorRexOutForm;
