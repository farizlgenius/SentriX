import { PropsWithChildren } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";

const DoorRexOutForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  return (
    <>
      <FormField>
        <Label htmlFor="rex.rex0ModuleComponentId">REX - Module</Label>
        <Select
          disabled={type == FormType.INFO}
          name="rex.rex0ModuleComponentId"
          options={moduleOption}
          onChange={(value: string) => {
            if (
              (dto.metadata as AeroDoorMetadata).rex.rex0ModuleComponentId !=
                Number(value) &&
              inputOption.length == 0
            ) {
              fetchInput(
                moduleOption.find((x) => x.value == Number(value))
                  ?.additionalInfo,
              );
            }
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                rex: {
                  ...(prev.metadata as AeroDoorMetadata).rex,
                  rex0ModuleComponentId: Number(value),
                  rex0ModuleId: moduleOption.find(
                    (x) => x.value == Number(value),
                  )?.additionalInfo,
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).rex?.rex0ModuleComponentId ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputNo">REX - Input No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="rex0.inputNo"
          options={inputOption.filter((x) => x.isTaken == false)}
          onChange={(value: string) => {
            setInputOption((prev) =>
              Helper.updateOptionByValue(prev, Number(value), true),
            );
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                rex: {
                  ...(prev.metadata as AeroDoorMetadata).rex,
                  rex0Number: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).rex?.rex0Number ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.inputMode">REX - Input Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="rex0.inputMode"
          options={inputModeOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                rex: {
                  ...(prev.metadata as AeroDoorMetadata).rex,
                  rex0SensorMode: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).rex?.rex0SensorMode ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="rex0.MaskTimeZone">REX - Mask Time Zone</Label>
        <Select
          disabled={type == FormType.INFO}
          name="rex0.MaskTimeZone"
          options={timeZoneOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                rex: {
                  ...(prev.metadata as AeroDoorMetadata).rex,
                  disableRex0Timezone: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).rex?.disableRex0Timezone ?? ""
          }
        />
      </FormField>
    </>
  );
};

export default DoorRexOutForm;
