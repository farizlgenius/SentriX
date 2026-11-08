import { PropsWithChildren, useEffect } from "react";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { Vendor } from "../../enum/Vendor";
import { DriveMode } from "../../enum/DriveMode";
import { OfflineMode } from "../../enum/OfflineMode";
import { AeroBuzzerMetadata } from "../../model/Door/AeroBuzzerMetadata";
import Button from "../../components/ui/button/Button";

type ExtraProps = {
  moduleOption:Options[]
};

const DoorBuzzerForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  handleClick,
  handleChange
}) => {
  const isFinished = dto.buzzer?.slotNo == undefined ;
      const isReadOnly = type == FormType.INFO;
      const isModuleSelected = dto.buzzer?.deviceModuleGuid  == "" || dto.buzzer?.deviceModuleGuid  == undefined;

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="buzzer.module">Buzzer - Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="buzzer.module"
          options={moduleOption}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              buzzer:{
                slotNo:-1,
                metadata:{
                  driveMode:DriveMode.Normal,
                  offlineMode:OfflineMode.NoChange,
                  defaultPulse:1
                },
                deviceModuleGuid:e.target.value,
                vendor:Vendor.aero
              }
            }))
            handleChange(e)
          }
            
          }
          className="dark:bg-dark-900"
          defaultValue={dto.buzzer?.deviceModuleGuid ?? ""}
        />
      </FormField>
      <FormField>
        <Label htmlFor="buzzer.slot">Slot No</Label>
        {/* <Select
          disabled={isReadOnly}
          name="strk.slot"
          options={outputOption}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              buzzer:prev.buzzer != null ?
               {
                ...prev.buzzer,
                slotNo:Number(e.target.value)
               }
              :
              prev.buzzer
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={dto.buzzer?.slotNo ?? -1}
        /> */}
        <Button disabled={isReadOnly || isModuleSelected} name="select.slot" onClick={handleClick} variant={isFinished ? "primary" : "green" }>
          {isFinished ? "Select Slot" : "Selected"}
        </Button>
      </FormField>
       <FormField>
        <Label htmlFor="relay.relayMode">Drive Mode</Label>
        <Select
          disabled={isReadOnly}
          name="strkMode"
          options={[
            {
              label: DriveMode[DriveMode.Inverted],
              value: DriveMode.Inverted
            },
            {
              label: DriveMode[DriveMode.Normal],
              value: DriveMode.Normal
            }
          ]}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              buzzer: prev.buzzer != null ?
                {
                  ...prev.buzzer,
                  metadata: {
                    ...(prev.buzzer.metadata as AeroBuzzerMetadata),
                    driveMode: Number(e.target.value)
                  }
                }
                :
                prev.buzzer
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.buzzer?.metadata as AeroBuzzerMetadata)?.driveMode ?? -1
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.relayMode">Offline Mode</Label>
        <Select
          disabled={isReadOnly}
          name="strkMode"
          options={[
            {
              label: OfflineMode[OfflineMode.Active],
              value: OfflineMode.Active
            },
            {
              label: OfflineMode[OfflineMode.Inactive],
              value: OfflineMode.Inactive
            },
            {
              label: OfflineMode[OfflineMode.NoChange],
              value: OfflineMode.NoChange
            }
          ]}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              buzzer: prev.buzzer != null ?
                {
                  ...prev.buzzer,
                  metadata: {
                    ...(prev.buzzer.metadata as AeroBuzzerMetadata),
                    offlineMode: Number(e.target.value)
                  }
                }
                :
                prev.buzzer
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.buzzer?.metadata as AeroBuzzerMetadata)?.offlineMode ?? -1
          }
        />
      </FormField>
    </div>
  );
};

export default DoorBuzzerForm;
