import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { OutputDto } from "../../model/ControlPoint/OutputDto";
import { DeviceEndpoint } from "../../endpoint/HardwareEndpoint";
import { ControlPointEndpoint } from "../../endpoint/ControlPointEndpoint";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import api, { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormActions, FormField, FormSection } from "../../components/form/template/FormTemplate";
import { DeviceType } from "../../enum/DeviceType";



const ControlPointForm: React.FC<PropsWithChildren<FormProp<OutputDto>>> = ({ handleClick, dto, setDto, type }) => {
  const { locationId } = useLocation();
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto(prev => ({ ...prev, [e.target.name]: e.target.value }))
  }

  {/* Select */ }
  const [controllerOption, setControllerOption] = useState<Options[]>([])
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const [relayOption, setRelayOption] = useState<Options[]>([]);
  const [relayModeOption, setRelyModeOption] = useState<Options[]>([]);
  const [controller,setController] = useState<number>(-1);

  const handleSelect = async (value: string, e: React.ChangeEvent<HTMLSelectElement>) => {
    switch (e.target.name) {
      case "driverId":
        fetchModuleByDeviceId(controllerOption.find(a => a.value == Number(value))?.additionalInfo)
        setDto((prev) => ({ ...prev, deviceComponentId: Number(value), mac: controllerOption.find(a => a.value == Number(value))?.description ?? "" }))
        break;
      case "moduleId":
        fetchOutput(moduleOption.find(a => a.value == Number(value))?.additionalInfo);
        setDto((prev) => ({ ...prev, moduleComponentId: Number(value), model: moduleOption.find(a => a.value == Number(value))?.label ?? "" }))
        break;
      case "relayMode":
        console.log(value);
        setDto(prev => ({ ...prev, relayMode: Number(value) }))
        // setOfflineModeOption()
        break;
      default:
        setDto((prev) => ({ ...prev, [e.target.name]: value }));
        break;
    }
  }

  {/* Controller Data */ }
  const fetchDevice = async () => {
    const res = await send.get(DeviceEndpoint.GET(locationId));
    console.log(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setControllerOption(prev => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description,
          isTaken:a.isTaken
        }])
      })
    }
  }

  const fetchRelayMode = async () => {

    let res = await api.get(ControlPointEndpoint.GET_RELAY_OP_MODE(DeviceType.AERO));
    if (res.data) {
      res.data.map((a: Options) => {
        setRelyModeOption((prev) => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description
        }]);
      });
    }


  }

  const fetchModuleByDeviceId = async (value: number) => {
    const res = await send.get(ModuleEndpoint.GET_BY_DEVICE_ID(value));
    console.log(res)
    if (res.data) {
      res.data.map((a: Options) => {
        setModuleOption((prev) => [...prev, {
          label: a.label,
          value: a.value,
          additionalInfo:a.additionalInfo,
          description:a.description,
          isTaken:a.isTaken
        }])
      })
    }
  }

  const fetchOutput = async (value: number) => {
    var res = await send.get(ControlPointEndpoint.OUTPUT(value));
    if (res) {
      res.data.map((a: number) => {
        setRelayOption((prev) => [...prev, {
          label: `Relay ${a + 1}`,
          value: a.toString()
        }]);
      });
    }
  }

  {/* UseEffect */ }
  useEffect(() => {
    fetchDevice();
    fetchRelayMode();
    if (type == FormType.INFO || type == FormType.UPDATE) {
      // fetchModuleByDeviceId(dto.moduleId);
      // fetchOutput(dto.moduleId);
    }
  }, []);

  return (
    <>
      <FormSection title="Control Point Details" description="Name the location, assign its country, and add a short description." className="pb-10 mb-5">
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Control Point Name</Label>
            <Input disabled={type == FormType.INFO} name="name" value={dto.name} type="text" id="name" onChange={handleChange} />
          </FormField>
          <FormField>
            <Label>Controller</Label>
            <Select
              isString={false}
              name="driverId"
              options={controllerOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelect}
              className="dark:bg-dark-900"
              defaultValue={dto.deviceComponentId}
              disabled={type == FormType.INFO}
            />
          </FormField>
          <FormField>
            <Label>Module</Label>
            <Select
              isString={false}
              name="moduleId"
              options={moduleOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelect}
              className="dark:bg-dark-900"
              defaultValue={dto.moduleComponentId}
              disabled={type == FormType.INFO}
            />
          </FormField>
          <FormField>
            <Label>Relay</Label>
            <Select
              name="outputNo"
              options={relayOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelect}
              className="dark:bg-dark-900"
              defaultValue={dto.outputNo}
              disabled={type == FormType.INFO}
            />
          </FormField>
          <FormField>
            <Label>Relay Mode</Label>
            <Select
              name="relayMode"
              options={relayModeOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelect}
              className="dark:bg-dark-900"
              defaultValue={dto.relayMode}
              disabled={type == FormType.INFO}
            />

          </FormField>
          <FormField>
            <Label htmlFor="defaultPulseTime">Pulse Time (second)</Label>
            <Input disabled={type == FormType.INFO} defaultValue={0} value={dto.defaultPulse} min="0" max="500" name="defaultPulse" type="number" id="defaultPulse" onChange={handleChange} />
          </FormField>
        </div>

      </FormSection>
      <FormActions
        // disabled={isReadOnly}
        onSubmit={handleClick}
        onCancel={handleClick}
        cancelName="close"
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      />


    </>


  );
}

export default ControlPointForm;