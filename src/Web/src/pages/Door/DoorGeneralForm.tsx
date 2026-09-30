import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import { Options } from "../../model/Options";


type ExtraProps = {
  deviceOption:Options[];
  fetchDevice:() => Promise<void>;
};

const DoorGeneralForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  type,
  deviceOption,
   handleChange,
   fetchDevice,
   setDto,
   setIsNext
}) => {
  const isReadOnly = type == FormType.INFO;
  //const [deviceOption,setDeviceOption] = useState<Options[]>([]);

  useEffect(() => {
    fetchDevice();
  },[])

  // NEW: Automatically validate whenever name or deviceGuid changes
  useEffect(() => {
    if (setIsNext) {
      // .trim() ensures spaces aren't counted as valid input
      const isValid = dto.name.trim() !== "" && dto.deviceGuid.trim() !== "";
      setIsNext(isValid);
    }
  }, [dto.name, dto.deviceGuid, setIsNext]);

  
  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input
          name="name"
          placeholder="Door name"
          disabled={isReadOnly}
          type="text"
          onChange={(e) => {

            setDto(prev => ({
              ...prev,
              name: e.target.value
            }))
            
            
          }
          }
          value={dto.name}
        />
      </FormField>
      <FormField>
        <Label>Vendor</Label>
        <Select
          disabled={isReadOnly}
          name="vendor"
          options={[
            {
              label: "Aero",
              value: Vendor.aero,
            },
            {
              label: "Amico",
              value: Vendor.amico,
            },
          ]}
          onChange={(e) => {
            setDto(prev => ({
          ...prev,
          vendor: Number(e.target.value)
        }))

          }
            
          }
          defaultValue={dto.vendor}
        />
      </FormField>
      <FormField>
        <Label>Type</Label>
        <Select
          disabled={isReadOnly}
          name={"type"}
          options={[
            {
              label: "In/Out",
              value: DoorType.Dual,
            },
            {
              label: "Single",
              value: DoorType.Single,
            },
          ]}
          onChange={(e) => {
             setDto(prev => ({
          ...prev,
          type: Number(e.target.value)
        }))

  
          }
           

          }
          defaultValue={dto.type}
        />
      </FormField>
      <FormField>
        <Label>Device</Label>
        <Select
          isString={true}
          disabled={isReadOnly}
          name={"deviceGuid"}
          options={deviceOption}
          onChange={(e) => {
            setDto(prev => ({
              ...prev,
              deviceGuid: e.target.value
            }))
            handleChange(e)
         
          }
          }
          defaultValue={dto.deviceGuid}
        />
      </FormField>
    </div>
  );
};

export default DoorGeneralForm;
