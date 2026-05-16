import { ChangeEvent, PropsWithChildren, useEffect, useState } from "react";
import Label from "../Label.tsx";
import Input from "../input/InputField.tsx";
import Button from "../../ui/button/Button.tsx";
import { FormProp, FormType } from "../../../model/Form/FormProp.ts";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto.ts";
import { CheckCircleIcon, ErrorIcon, LoadIcon } from "../../../icons/index.ts";
import { FormActions, FormField, FormSection } from "../template/FormTemplate.tsx";
import { DeviceDto } from "../../../model/Device/DeviceDto.ts";







const AmicoCreateDeviceForm: React.FC<PropsWithChildren<FormProp<DeviceDto>>> = ({ dto, type, handleClick, setDto }) => {

      type ConnectionStatus = "idle" | "loading" | "success" | "error";

      const [connectionStatus, setConnectionStatus] = useState<ConnectionStatus>("idle");
      const [info, setInfo] = useState(false);
      const isReadOnly = type == FormType.INFO;

      const onConnectClick = async (e: any) => {
            setConnectionStatus("loading");

            try {
                  // await handleClick(e); // call parent API
                  setInfo(true);
                  setConnectionStatus("success");
            } catch {
                  setConnectionStatus("error");
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
            setDto(prev => ({ ...prev, [e.target.name]: e.target.value }));
      }


      return (
            <>
                  <FormSection title="Amico Details" description="Name the location, assign its country, and add a short description." className="pb-10">
                        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
                              <FormField>
                                    <Label htmlFor="username">Username</Label>
                                    <Input disabled={isReadOnly} placeholder="Username" name="username" type="text" id="ip" onChange={handleChange} value={dto.name} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="password">Password</Label>
                                    <Input disabled={isReadOnly} placeholder="Password" name="password" type="password" id="passowrd" onChange={handleChange} value={dto.name} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="ip">IP Address</Label>
                                    <Input disabled={isReadOnly} placeholder="IP Address" name="ip" type="text" id="ip" onChange={handleChange} value={dto.name} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="ip">Port</Label>
                                    <Input disabled={isReadOnly} placeholder="Port" name="ip" type="text" id="port" onChange={handleChange} value={dto.name} />
                              </FormField>
                              <FormField className="col-span-2">
                                    {/* CONNECT BUTTON + STATUS */}
                                    <div className="flex items-center gap-4 pb-1">
                                          <Button
                                                onClickWithEvent={onConnectClick}
                                                disabled={type === FormType.INFO || connectionStatus === "loading"}
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
                        {
                              info &&

                              <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 p-5 pt-6 border-t border-gray-200 dark:border-gray-800">
                                    <FormField>
                                          <Label htmlFor="username">Username</Label>
                                          <Input disabled={isReadOnly} placeholder="Username" name="username" type="text" id="ip" onChange={handleChange} value={dto.name} />
                                    </FormField>
                                    <FormField>
                                          <Label htmlFor="password">Password</Label>
                                          <Input disabled={isReadOnly} placeholder="Password" name="password" type="password" id="passowrd" onChange={handleChange} value={dto.name} />
                                    </FormField>
                                    <FormField>
                                          <Label htmlFor="ip">IP Address</Label>
                                          <Input disabled={isReadOnly} placeholder="IP Address" name="ip" type="text" id="ip" onChange={handleChange} value={dto.name} />
                                    </FormField>
                                    <FormField>
                                          <Label htmlFor="ip">Port</Label>
                                          <Input disabled={isReadOnly} placeholder="Port" name="ip" type="text" id="port" onChange={handleChange} value={dto.name} />
                                    </FormField>
                                   
                              </div>
                        }

                  </FormSection>
                  {
                        info &&
                        <FormActions
                              disabled={isReadOnly}
                              onSubmit={handleClick}
                              onCancel={handleClick}
                              cancelName="close"
                              submitName={type == FormType.UPDATE ? "update" : "create"}
                              typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
                        />

                  }


            </>);
}

export default AmicoCreateDeviceForm;