import { PropsWithChildren, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import AeroDoorForm from "./AeroDoorForm";
import AmicoDoorForm from "./AmicoDoorForm";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import Select from "../../components/form/Select";

const DoorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  const selectedType = Vendor.aero;

  const FormTypeSwitcher = (value: Vendor) => {
    switch (value) {
      case Vendor.aero:
        return (
          <AeroDoorForm
            handleClick={handleClick}
            dto={dto}
            setDto={setDto}
            type={type}
          />
        );
      case Vendor.amico:
        return (
          <AmicoDoorForm
            handleClick={handleClick}
            dto={dto}
            setDto={setDto}
            type={type}
          />
        );
      default:
        return <></>;
    }
  };
  return (
    <div className="grid grid-cols-2 gap-5">
      <FormSection
        title="Door setup"
        description="Provide a name for this door before configuring its access components."
        className="col-span-2"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input
              id="name"
              name="name"
              placeholder="Main lobby door"
              value={dto.name}
              disabled={type === FormType.INFO}
              onChange={(event) =>
                setDto((previous) => ({
                  ...previous,
                  name: event.target.value,
                }))
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="type">Type</Label>
            <Select name={"type"} options={[]} />
          </FormField>
        </div>
      </FormSection>

      {FormTypeSwitcher(selectedType)}
    </div>
  );
};

export default DoorForm;
