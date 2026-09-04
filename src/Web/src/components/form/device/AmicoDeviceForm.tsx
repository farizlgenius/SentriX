import { ChangeEvent, PropsWithChildren, useState } from "react";
import Label from "../Label.tsx";
import Input from "../input/InputField.tsx";
import Button from "../../ui/button/Button.tsx";
import { FormProp, FormType } from "../../../model/Form/FormProp.ts";
import { CheckCircleIcon, ErrorIcon, LoadIcon } from "../../../icons/index.ts";
import { FormField, FormSection } from "../template/FormTemplate.tsx";
import { send } from "../../../api/api.ts";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint.ts";
import { AmicoConnect } from "../../../model/Device/AmicoConnect.ts";
import { DeviceDto } from "../../../model/Device/DeviceDto.ts";

const AmicoDeviceForm: React.FC<PropsWithChildren<FormProp<DeviceDto>>> = ({
  dto,
  type,
  setDto,
}) => {
  const defaultConnect: AmicoConnect = {
    ip: "",
  };

  const [connect, setConnect] = useState<AmicoConnect>(defaultConnect);

  type ConnectionStatus = "idle" | "loading" | "success" | "error";

  const [connectionStatus, setConnectionStatus] =
    useState<ConnectionStatus>("idle");
  const [info, setInfo] = useState(true);
  const isReadOnly = type == FormType.INFO;

  // const onSubmit = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
  //       var res = await c
  // }

  const onConnectClick = async (e: any) => {
    setConnectionStatus("loading");

    try {
      const res = await send.post(DeviceEndpoint.CHECK_AMICO_CONNECT, connect);
      console.log(res.data);
      if (res.data.success) {
        setDto((prev) => ({
          ...prev,
          serialNumber: res.data.data.serial,
          mac: res.data.data.network.mac,
          ip: res.data.data.network.ip,
          port: res.data.data.network.web_server_port,
          fw: res.data.data.version,
          type: "amico",
          metadata: {
            deviceId: res.data.data.device_id,
          },
        }));
        setInfo(true);
        setConnectionStatus("success");
      } else {
        setConnectionStatus("error");
        setInfo(false);
      }
    } catch {
      setConnectionStatus("error");
      setInfo(false);
    }
  };

  const renderConnectionIcon = () => {
    switch (connectionStatus) {
      case "loading":
        return <LoadIcon className="animate-spin text-blue-500 text-2xl" />;

      case "success":
        return <CheckCircleIcon className="text-green-500 text-2xl" />;

      case "error":
        return <ErrorIcon className="text-red-500 text-2xl" />;

      default:
        return null; // hide when idle
    }
  };

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    switch (e.target.name) {
      case "ip":
        setConnect((prev) => ({ ...prev, [e.target.name]: e.target.value }));
        setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
        break;
      default:
        setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
        break;
    }
  };

  return (
    <>
      <FormSection
        title="Amico Details"
        description="Name the location, assign its country, and add a short description."
        className="pb-10"
      >
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="ip">IP Address</Label>
            <Input
              disabled={isReadOnly}
              placeholder="IP Address"
              name="ip"
              type="text"
              id="ip"
              onChange={handleChange}
              value={connect.ip}
            />
          </FormField>
          <FormField className="col-span-2">
            {/* CONNECT BUTTON + STATUS */}
            <div className="flex items-center gap-4 pb-1">
              <Button
                onClickWithEvent={onConnectClick}
                disabled={
                  type === FormType.INFO || connectionStatus === "loading"
                }
                name="connect"
                className="w-40"
                size="sm"
              >
                {connectionStatus === "loading" ? "Connecting..." : "Connect"}
              </Button>

              {/* Status Icon */}
              <div className="w-8 h-8 flex items-center justify-center">
                {renderConnectionIcon()}
              </div>
            </div>
          </FormField>
        </div>
        {info && (
          <div className="grid gap-5 grid-cols-3 md:grid-cols-3 gap-x-10 gap-y-6 p-5 pt-6 border-t border-gray-200 dark:border-gray-800">
            <FormField>
              <Label htmlFor="mac">Name</Label>
              <Input
                placeholder="Name"
                name="name"
                type="text"
                id="name"
                onChange={handleChange}
                value={dto.name}
              />
            </FormField>
            <FormField>
              <Label htmlFor="mac">Mac</Label>
              <Input
                disabled={isReadOnly}
                placeholder="Mac"
                name="mac"
                type="text"
                id="mac"
                onChange={handleChange}
                value={dto.mac}
              />
            </FormField>
            <FormField>
              <Label htmlFor="password">Serial Number</Label>
              <Input
                disabled={isReadOnly}
                placeholder="Serial Nunber"
                name="serialNumber"
                type="text"
                id="serialNumber"
                onChange={handleChange}
                value={dto.serialNumber}
              />
            </FormField>
            <FormField>
              <Label htmlFor="ip">IP Address</Label>
              <Input
                disabled={isReadOnly}
                placeholder="IP Address"
                name="ip"
                type="text"
                id="ip"
                onChange={handleChange}
                value={dto.ip}
              />
            </FormField>
            <FormField>
              <Label htmlFor="ip">Port</Label>
              <Input
                disabled={isReadOnly}
                placeholder="Port"
                name="port"
                type="text"
                id="port"
                onChange={handleChange}
                value={dto.port}
              />
            </FormField>
            <FormField>
              <Label htmlFor="ip">Fw</Label>
              <Input
                disabled={isReadOnly}
                placeholder="Fw"
                name="firmware"
                type="text"
                id="fw"
                onChange={handleChange}
                value={dto.firmware}
              />
            </FormField>
          </div>
        )}
      </FormSection>
    </>
  );
};

export default AmicoDeviceForm;
