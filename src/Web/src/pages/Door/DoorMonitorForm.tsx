import React, { PropsWithChildren } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";

type ExtraProps = {
  moduleOption:Options[]
  inputOption:Options[]
};

const DoorMonitorForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  handleChange,
  moduleOption,
  inputOption
}) => {
  const isReadOnly = type == FormType.INFO || dto.sensor == null
  return (
    <>
      <FormField>
        <Label htmlFor="sensor.sensorModuleComponentId">Sensor Module</Label>
        <Select
          disabled={type == FormType.INFO}
          name="sensor.sensorModuleComponentId"
          options={moduleOption}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              sensor:{
                guid:"",
                slotNo:-1,
                mode:InputMode.NC,
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value,
                metadata:""
              }
            }))
          }
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
          disabled={isReadOnly}
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
