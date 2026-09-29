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
  deviceOption:Options[]
};

const DoorGeneralForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  type,
  deviceOption,
   handleChange
}) => {
  const isReadOnly = type == FormType.INFO;
  //const [deviceOption,setDeviceOption] = useState<Options[]>([]);

  
  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input
        name="name"
          placeholder="Door name"
          disabled={isReadOnly}
          type="text"
          onChange={handleChange}
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
          onChange={handleChange}
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
          onChange={handleChange}
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
          onChange={handleChange}
          defaultValue={dto.deviceGuid}
        />
      </FormField>
    </div>
  );
};

export default DoorGeneralForm;
