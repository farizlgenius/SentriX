import React, { PropsWithChildren } from "react";

import DatePicker from "../../components/form/date-picker";
import { HolidayDto } from "../../model/Holiday/HolidayDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Input from "../../components/form/input/InputField";
import Label from "../../components/form/Label";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";

const HolidayForm: React.FC<PropsWithChildren<FormProp<HolidayDto>>> = ({
  type,
  setDto,
  dto,
}) => {
  const isReadOnly = type == FormType.INFO;
  // Alert Modal
  return (
    <>
      <FormSection
        overall="Holday Details"
        title="Holiday Informations."
        description="Name the holiday and select the date it applies to."
      >
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField className="col-span-3">
            <Label>Name</Label>
            <Input
              placeholder="Holiday name"
              disabled={isReadOnly}
              defaultValue={dto.name}
              value={dto.name}
              onChange={(e) =>
                setDto((prev) => ({ ...prev, name: e.target.value }))
              }
            />
          </FormField>
          <FormField>
            <DatePicker
              isTime={false}
              id="startDate"
              label="Start Date"
              placeholder="Select a date"
              value={dto.start.toISOString()}
              onChange={(date) => {
                console.log(date[0]);
                setDto((prev) => ({
                  ...prev,
                  start: date[0],
                }));
              }}
            />
          </FormField>
          <FormField>
            <DatePicker
              isTime={false}
              id="endDate"
              label="End Date"
              placeholder="Select a date"
              value={dto.end.toISOString()}
              onChange={(date) => {
                console.log(date[0]);
                setDto((prev) => ({
                  ...prev,
                  end: date[0],
                }));
              }}
            />
          </FormField>
        </div>
      </FormSection>
    </>
  );
};

export default HolidayForm;
