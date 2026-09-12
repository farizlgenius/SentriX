import React, { PropsWithChildren } from "react";
import { FormProp } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";

const DoorMonitorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  return (
    <>
      <FormField>
        <Label htmlFor="sensor.sensorModuleComponentId">Sensor Module</Label>
        <Select
          disabled={type == FormType.INFO}
          name="sensor.sensorModuleComponentId"
          options={moduleOption}
          onChange={(value: string) => {
            if (
              (dto.metadata as AeroDoorMetadata).rex.rex0ModuleComponentId !=
              Number(value)
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
                sensor: {
                  ...(prev.metadata as AeroDoorMetadata).sensor,
                  sensorModuleComponentId: Number(value),
                  sensorModuleId: moduleOption.find(
                    (x) => x.value == Number(value),
                  )?.additionalInfo,
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).sensor
              ?.sensorModuleComponentId ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="sensor.sensorNumber">Input No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="sensor.sensorNumber"
          options={inputOption.filter((x) => x.isTaken == false)}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                sensor: {
                  ...(prev.metadata as AeroDoorMetadata).sensor,
                  sensorNumber: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).sensor?.sensorNumber ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="sensor.sensorMode">Input Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="sensor.sensorMode"
          options={inputModeOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                sensor: {
                  ...(prev.metadata as AeroDoorMetadata).sensor,
                  sensorMode: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).sensor?.sensorMode ?? ""
          }
        />
      </FormField>
    </>
  );
};

export default DoorMonitorForm;
