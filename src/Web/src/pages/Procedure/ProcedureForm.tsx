import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { ProcedureDto } from "../../model/Procedure/ProcedureDto";
import { Options } from "../../model/Options";
import { send } from "../../api/api";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { useLocation } from "../../context/LocationContext";
import { HttpStatusCode } from "axios";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import Input from "../../components/form/input/InputField";


export const ProcedureForm: React.FC<
  PropsWithChildren<FormProp<ProcedureDto>>
> = ({ dto, setDto, type, setIsNext }) => {
  const isReadOnly = type == FormType.INFO;
  const { locationGuid } = useLocation();

  const [deviceOption, setDeviceOption] = useState<Options[]>([]);


  const fetchDevice = async () => {
    var res = await send.get(DeviceEndpoint.GET_OPTION(locationGuid));
    if (res.status == HttpStatusCode.Ok) {
      setDeviceOption(res.data.data);
    }
  };


  useEffect(() => {
    fetchDevice();
  }, []);

  useEffect(() => {
    if (setIsNext && dto.name != "" && dto.deviceGuid != "") {
      setIsNext(true);
    }
  }, [dto.name, dto.deviceGuid])

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input type="text" disabled={isReadOnly}
          placeholder="Procedure name"
          onChange={(e) => {
            setDto(prev => ({
              ...prev,
              name: e.target.value
            }))
          }}
        />
      </FormField>
      <FormField>
        <Label>Device</Label>
        <Select
          isString
          name={""}
          options={deviceOption}
          onChange={e => {
            setDto(prev => ({
              ...prev,
              deviceGuid: e.target.value
            }))

          }}
          defaultValue={dto.deviceGuid}
        />
      </FormField>
    </div>
  );
};
