import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import { Options } from "../../model/Options";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";

type ExtraProps = {
  setModuleOption: React.Dispatch<React.SetStateAction<Options[]>>;
  setDeviceOption: React.Dispatch<React.SetStateAction<Options[]>>;
  deviceOption:Options[]
};

const DoorGeneralForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  setModuleOption,
  setDeviceOption,
  deviceOption
}) => {
  const isReadOnly = type == FormType.INFO;
  const { locationGuid } = useLocation();
  //const [deviceOption,setDeviceOption] = useState<Options[]>([]);

  const fetchDevice = async () => {
    var res = await send.get(DeviceEndpoint.GET_LOCATION(locationGuid))
    var option = res.data.data.map((a:DeviceDto) => ({
      value:a.guid,
      label:a.name,
      description:a.mac,
      isTaken:false
    }))

    setDeviceOption(option)

  }

  const fetchModule = async (guid:string) => {
    var res = await send.get(ModuleEndpoint.GET_BY_GUID(guid))
    var option = res.data.data.map((a:DeviceDto) => ({
      value:a.guid,
      label:a.name,
      description:a.mac,
      isTaken:false
    }))

    setModuleOption(option)
  }

  useEffect(() => {
    fetchDevice();
  },[])
  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input
          placeholder="Door name"
          disabled={isReadOnly}
          type="text"
          onChange={(e) => setDto((prev) => ({ ...prev, name: e.target.name }))}
        />
      </FormField>
      <FormField>
        <Label>Vendor</Label>
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
      <FormField>
        <Label>Device</Label>
        <Select
          isString={true}
          disabled={isReadOnly}
          name={"device"}
          options={deviceOption}
          onChange={(e) => {
             setDto((prev) => ({ ...prev, deviceGuid: e }))
             fetchModule(e);
          }}
          defaultValue={dto.deviceGuid}
        />
      </FormField>
    </div>
  );
};

export default DoorGeneralForm;
