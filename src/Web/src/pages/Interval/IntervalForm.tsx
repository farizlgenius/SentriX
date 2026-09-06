import { PropsWithChildren } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { IntervalDto } from "../../model/Interval/IntervalDto";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Switch from "../../components/form/switch/Switch";
import Helper from "../../utility/Helper";
import { DaysInWeekDto } from "../../model/Interval/DaysInWeekDto";
import Input from "../../components/form/input/InputField";

const daysInWeek = [
  "sunday",
  "monday",
  "tuesday",
  "wednesday",
  "thursday",
  "friday",
  "saturday",
];

export const IntervalForm: React.FC<
  PropsWithChildren<FormProp<IntervalDto>>
> = ({ type, setDto, dto }) => {
  return (
    <FormSection
      overall="Interval Detail"
      title="Interval Informations"
      description="Enter the interval details manually for the timezone."
    >
      <div className="grid gap-4 sm:grid-cols-2">
        <FormField className="grid grid-cols-3 col-span-2 gap-5">
          {daysInWeek.map((d: string, i: number) => (
            <div key={i} className="flex gap-10 justify-around items-center">
              <div className="flex-1">
                <Switch
                  label={Helper.toCapitalCase(d)}
                  defaultChecked={dto.days[d as keyof DaysInWeekDto] as boolean}
                  onChange={(e) => {
                    setDto((prev) => ({
                      ...prev,
                      days: {
                        ...prev.days,
                        [d]: e,
                      },
                    }));
                  }}
                />
              </div>
            </div>
          ))}
        </FormField>
        <FormField>
          <Label>Start Time</Label>
          <Input
            type="time"
            id="tm"
            name="start"
            onChange={(e) =>
              setDto((prev) => ({
                ...prev,
                start: e.target.value,
              }))
            }
            defaultValue={"00:00"}
            value={dto.start}
            min="00:00"
            placeholder={dto.start}
            disabled={type == FormType.INFO}
          />
        </FormField>
        <FormField>
          <Label>End Time</Label>
          <Input
            type="time"
            id="tm"
            name="end"
            onChange={(e) =>
              setDto((prev) => ({
                ...prev,
                end: e.target.value,
              }))
            }
            defaultValue={"00:00"}
            value={dto.end}
            min="00:00"
            placeholder={dto.end}
            disabled={type == FormType.INFO}
          />
        </FormField>
      </div>
    </FormSection>
  );
};
