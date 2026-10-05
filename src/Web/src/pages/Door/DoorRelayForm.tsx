import React, { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { RelayMode } from "../../enum/RelayMode";
import { Vendor } from "../../enum/Vendor";
import Input from "../../components/form/input/InputField";
import { AeroRelayMetadata } from "../../model/Door/AeroRelayMetadata";
import { DriveMode } from "../../enum/DriveMode";
import { OfflineMode } from "../../enum/OfflineMode";
import { StrikeMode } from "../../enum/StrikeMode";

type ExtraProps = {
  moduleOption: Options[]
  outputOption: Options[]
};


const DoorRelayForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  outputOption,
  handleChange,
  setIsNext
}) => {
  const isReadOnly = type == FormType.INFO || dto.relay == null

  // NEW: Automatically validate whenever name or deviceGuid changes
  useEffect(() => {
    if (setIsNext) {

      // 2. Extract values safely, providing fallbacks if undefined
      const moduleGuid = dto.relay?.deviceModuleGuid || "";
      const slotNo = dto.relay?.slotNo ?? -1;

      if (moduleGuid !== "") {
        if (moduleGuid !== "") {
          handleChange({
            target: {
              name: "relay.module",
              value: moduleGuid
            }
          } as React.ChangeEvent<HTMLInputElement>);
        }
      }

      // 3. Perform the validation check safely
      const isValid = moduleGuid.trim() !== "" && slotNo !== -1;

      console.log("DoorOutForm isValid:", isValid);
      setIsNext(isValid); // This will now successfully run!
    }
  }, [dto.relay, setIsNext]); // Simplified dependency array


  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="relay.module">Relay - Module</Label>
        <Select
          isString={true}
          disabled={type == FormType.INFO}
          name="relay.module"
          options={moduleOption}
          onChange={(e) => {
            console.log(e.target.value)
            handleChange(e)
            setDto((prev) => ({
              ...prev,
              relay: {
                slotNo: -1,
                mode: RelayMode.None,
                vendor: Vendor.aero,
                deviceModuleGuid: e.target.value,
                metadata: {
                  strikeMin: 1,
                  strikeMax: 5,
                  driveMode: DriveMode.Normal,
                  offlineMode: OfflineMode.NoChange,
                  strikeMode: StrikeMode.None
                }
              }
            }))
          }

          }
          className="dark:bg-dark-900"
          defaultValue={dto.relay?.deviceModuleGuid ?? ""}
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.slot">Relay No</Label>
        <Select
          disabled={isReadOnly}
          name="relay.slot"
          options={outputOption}
          onChange={(e) =>
            setDto((prev) => ({
              ...prev,
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  slotNo: Number(e.target.value)
                } :
                prev.relay
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={dto.relay?.slotNo ?? -1}
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.minStrk">Minimum Strike Time</Label>
        <Input
          disabled={isReadOnly}
          defaultValue={1}
          value={(dto.relay?.metadata as AeroRelayMetadata)?.strikeMin ?? 1}
          name="relay.minStrk"
          type="number"
          id="relay.minStrk"
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  metadata: {
                    ...(prev.relay.metadata as AeroRelayMetadata),
                    strikeMin: Number(e.target.value)
                  }
                }
                :
                prev.relay
            }));
          }}
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.maxStrk">Maximum Strike Active Time</Label>
        <Input
          disabled={isReadOnly}
          defaultValue={5}
          value={(dto.relay?.metadata as AeroRelayMetadata)?.strikeMax ?? 5}
          name="relay.maxStrk"
          type="number"
          id="relay.maxStrk"
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  metadata: {
                    ...(prev.relay.metadata as AeroRelayMetadata),
                    strikeMax: Number(e.target.value)
                  }
                }
                :
                prev.relay
            }));
          }}
        />
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
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  metadata: {
                    ...(prev.relay.metadata as AeroRelayMetadata),
                    driveMode: Number(e.target.value)
                  }
                }
                :
                prev.relay
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.relay?.metadata as AeroRelayMetadata)?.driveMode ?? -1
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
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  metadata: {
                    ...(prev.relay.metadata as AeroRelayMetadata),
                    offlineMode: Number(e.target.value)
                  }
                }
                :
                prev.relay
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.relay?.metadata as AeroRelayMetadata)?.offlineMode ?? -1
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.relayMode">Strike Mode</Label>
        <Select
          disabled={isReadOnly}
          name="strkMode"
          options={[
            {
              label: "None",
              value: StrikeMode.None
            },
            {
              label: "Deactivate Open",
              value: StrikeMode.DeactOpen
            },
            {
              label: "Deactivate Close",
              value: StrikeMode.DeactClose
            },
            {
              label: "Tailgate",
              value: StrikeMode.Tailgate
            }
          ]}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              relay: prev.relay != null ?
                {
                  ...prev.relay,
                  metadata: {
                    ...(prev.relay.metadata as AeroRelayMetadata),
                    offlineMode: Number(e.target.value)
                  }
                }
                :
                prev.relay
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.relay?.metadata as AeroRelayMetadata)?.offlineMode ?? -1
          }
        />
      </FormField>
    </div>
  );
};

export default DoorRelayForm;
