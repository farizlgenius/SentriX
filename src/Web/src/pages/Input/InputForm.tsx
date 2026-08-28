import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Select from "../../components/form/Select";
import Button from "../../components/ui/button/Button";
import { Options } from "../../model/Options";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { InputDto } from "../../model/MonitorPoint/InputDto";
import { ModeDto } from "../../model/ModeDto";
import { ModuleDto } from "../../model/Module/ModuleDto";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { MonitorPointEndpoint } from "../../endpoint/MonitorPointEndpoint";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import {
  FormActions,
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import { DeviceType } from "../../enum/DeviceType";

const InputForm: React.FC<PropsWithChildren<FormProp<InputDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  const { locationGuid: locationId } = useLocation();
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  {
    /* Select */
  }
  const [moduleOption, setModuleOption] = useState<Options[]>([]);
  const [controllerOption, setControllerOption] = useState<Options[]>([]);
  const [logFunctionOption, setLogFunctionOption] = useState<Options[]>([]);
  const [inputOption, setInputOption] = useState<Options[]>([]);
  const [inputModeOption, setInputModeOption] = useState<Options[]>([]);
  const [monitorPointModeOption, setMonitorPointModeOption] = useState<
    Options[]
  >([]);

  const handleSelectChange = async (
    value: string,
    e: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    switch (e.target.name) {
      case "deviceId":
        setDto((prev) => ({ ...prev, scpId: Number(value) }));
        const res1 = await send.get(
          ModuleEndpoint.GET_BY_DEVICE_ID(Number(value)),
        );
        if (res1.data) {
          res1.data.map((a: Options) => {
            setModuleOption((prev) => [
              ...prev,
              {
                label: a.label,
                value: a.value,
                description: a.description,
                additionalInfo: a.additionalInfo,
                isTaken: false,
              },
            ]);
          });
        }
        break;
      case "moduleId":
        setDto((prev) => ({
          ...prev,
          moduleId: Number(value),
          moduleDescription:
            moduleOption.find((x) => x.value == Number(value))?.label ?? "",
          moduleDriverId:
            moduleOption.find((x) => x.value == Number(value))
              ?.additionalInfo ?? -1,
        }));
        const res2 = await send.get(
          MonitorPointEndpoint.IP_LIST(Number(value)),
        );
        if (res2?.data.data) {
          res2.data.data.map((a: number) => {
            setInputOption((prev) => [
              ...prev,
              {
                label: `Input ${a + 1}`,
                value: a.toString(),
              },
            ]);
          });
        }
        break;
      case "monitorPointMode":
        setDto((prev) => ({
          ...prev,
          monitorPointMode: Number(value),
          monitorPointModeDescription:
            monitorPointModeOption.find((x) => x.value == Number(value))
              ?.label ?? "",
        }));
        break;
      case "inputMode":
        setDto((prev) => ({
          ...prev,
          inputMode: Number(value),
          inputModeDescription:
            inputModeOption.find((x) => x.value == Number(value))?.label ?? "",
        }));
        break;
      case "logFunction":
        setDto((prev) => ({
          ...prev,
          logFunction: Number(value),
          logFunctionDescription:
            logFunctionOption.find((x) => x.value == Number(value))?.label ??
            "",
        }));
        break;
      default:
        setDto((prev) => ({ ...prev, [e.target.name]: value }));
        break;
    }
  };

  {
    /* Controller Data */
  }
  const fetchController = async () => {
    let res = await send.get(
      DeviceEndpoint.GET_OPTION_BY_TYPE(locationId, DeviceType.AERO),
    );
    if (res.data) {
      res.data.map((a: Options) => {
        setControllerOption((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchInputMode = async () => {
    let res = await send.get(MonitorPointEndpoint.IP_MODE);
    if (res?.data.data) {
      res.data.data.map((a: ModeDto) => {
        setInputModeOption((prev) => [
          ...prev,
          { label: a.name, value: a.value },
        ]);
      });
    }
  };

  const fetchMonitorPointMode = async () => {
    let res = await send.get(MonitorPointEndpoint.MP_MODE);
    if (res?.data.data) {
      res.data.data.map((a: ModeDto) => {
        setMonitorPointModeOption((prev) => [
          ...prev,
          { label: a.name, value: a.value },
        ]);
      });
    }
  };

  const fetchLFMode = async () => {
    let res = await send.get(MonitorPointEndpoint.LOG_FUNCTION);
    if (res.data.data) {
      res.data.data.map((a: ModeDto) => {
        setLogFunctionOption((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.value,
            description: a.description,
          },
        ]);
      });
    }
  };

  {
    /* UseEffect */
  }
  useEffect(() => {
    fetchController();
    fetchInputMode();
    fetchMonitorPointMode();
    fetchLFMode();
  }, []);

  return (
    <>
      <FormSection title="Monitor Point Detail" description="">
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Monitor Point Name</Label>
            <Input
              value={dto.name}
              name="name"
              type="text"
              id="name"
              onChange={handleChange}
              disabled={type == FormType.INFO}
            />
          </FormField>
          <FormField>
            <Label>Controller</Label>
            <Select
              disabled={type == FormType.INFO}
              isString={false}
              name="deviceId"
              options={controllerOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelectChange}
              className="dark:bg-dark-900"
              defaultValue={dto.scpId}
            />
          </FormField>

          <FormField>
            <Label>Module</Label>
            <Select
              disabled={type == FormType.INFO}
              name="moduleId"
              options={moduleOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelectChange}
              className="dark:bg-dark-900"
              defaultValue={dto.moduleId}
            />
          </FormField>
          <FormField>
            <Label>Input</Label>
            <Select
              disabled={type == FormType.INFO}
              name="inputNo"
              options={inputOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelectChange}
              className="dark:bg-dark-900"
              defaultValue={dto.inputNo}
            />
          </FormField>
          <FormField>
            <Label className="pb-3">Monitor Point Mode</Label>
            <Select
              disabled={type == FormType.INFO}
              name="monitorPointMode"
              options={monitorPointModeOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelectChange}
              className="dark:bg-dark-900"
              defaultValue={dto.monitorPointMode}
            />
          </FormField>
          <FormField>
            <Label className="pb-3">Log Function Mode</Label>
            <Select
              disabled={type == FormType.INFO}
              name="logFunction"
              options={logFunctionOption}
              placeholder="Select Option"
              onChangeWithEvent={handleSelectChange}
              className="dark:bg-dark-900"
              defaultValue={dto.logFunction}
            />
          </FormField>

          <FormField
            className={
              dto.monitorPointMode == 1 || dto.monitorPointMode == 2
                ? ""
                : "hidden"
            }
          >
            <Label htmlFor="delayEntry">Delay Entry(s)</Label>
            <Input
              disabled={type == FormType.INFO}
              value={dto.delayEntry}
              min="0"
              max="65535"
              name="delayEntry"
              type="number"
              id="delayEntry"
              onChange={handleChange}
            />
          </FormField>
          <FormField
            className={
              dto.monitorPointMode == 1 || dto.monitorPointMode == 2
                ? ""
                : "hidden"
            }
          >
            <Label htmlFor="delayExit">Delay Exit(s)</Label>
            <Input
              disabled={type == FormType.INFO}
              value={dto.delayExit}
              min="0"
              max="65535"
              name="delayExit"
              type="number"
              id="delayExit"
              onChange={handleChange}
            />
          </FormField>
        </div>
      </FormSection>
      <FormActions
        onSubmit={handleClick}
        onCancel={handleClick}
        cancelName="close"
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      />
    </>
  );
};

export default InputForm;
