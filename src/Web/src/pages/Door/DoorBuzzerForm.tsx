import { PropsWithChildren } from "react";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import { FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";

const DoorBuzzerForm: React.FC<PropsWithChildren<DoorDto>> = ({
  dto,
  setDto,
  type,
}) => {
  return (
    <>
      <FormField>
        <Label htmlFor="relay.relayModuleComponentId">Relay - Module</Label>
        <Select
          disabled={type == FormType.INFO}
          name="relay.relayModuleComponentId"
          options={moduleOption}
          onChange={(value: string) => {
            fetchOutput(
              moduleOption.find((x) => x.value == Number(value))
                ?.additionalInfo,
            );
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayModuleComponentId: Number(value),
                  relayModuleId: moduleOption.find(
                    (x) => x.value == Number(value),
                  )?.additionalInfo,
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).relay?.relayModuleComponentId ??
            ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="strk.outputNo">Relay No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="strk.outputNo"
          options={outputOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayNumber: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).relay?.relayNumber ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.relayMin">Minimum Strike Active Time</Label>
        <Input
          disabled={type == FormType.INFO}
          defaultValue={1}
          value={(dto.metadata as AeroDoorMetadata).relay?.relayMin}
          name="relayMin"
          type="number"
          id="strikeMinActiveTime"
          onChange={(e: ChangeEvent<HTMLInputElement>) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayMin: Number(e.target.value),
                },
              },
            }));
          }}
        />
      </FormField>
      <FormField>
        <Label htmlFor="strkMax">Maximum Strike Active Time</Label>
        <Input
          disabled={type == FormType.INFO}
          defaultValue={5}
          value={(dto.metadata as AeroDoorMetadata).relay?.relayMax}
          name="relayMax"
          type="number"
          id="strikeMaxActiveTime"
          onChange={(e: ChangeEvent<HTMLInputElement>) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayMax: Number(e.target.value),
                },
              },
            }));
          }}
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.relayMode">Drive Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="strkMode"
          options={relayDriveOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayDriveMode: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).relay?.relayDriveMode ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="relay.relayMode">Offline Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="strkMode"
          options={relayOfflineOption}
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                relay: {
                  ...(prev.metadata as AeroDoorMetadata).relay,
                  relayOfflineMode: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).relay?.relayOfflineMode ?? ""
          }
        />
      </FormField>
    </>
  );
};

export default DoorBuzzerForm;
