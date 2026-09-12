import { PropsWithChildren } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";

const DoorGeneralForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const isReadOnly = type == FormType.INFO;
  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input
          disabled={isReadOnly}
          type="text"
          onChange={(e) => setDto((prev) => ({ ...prev, name: e.target.name }))}
        />
      </FormField>
      <FormField>
        <Label>Door control</Label>
        <Select
          disabled={isReadOnly}
          name={"vendor"}
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
          onChange={(e) => setDto((prev) => ({ ...prev, vendor: Number(e) }))}
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
          onChange={(e) => setDto((prev) => ({ ...prev, type: Number(e) }))}
          defaultValue={dto.type}
        />
      </FormField>
    </div>
  );
};

export default DoorGeneralForm;
