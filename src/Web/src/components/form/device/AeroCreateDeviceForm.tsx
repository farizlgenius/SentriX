import { ChangeEvent, PropsWithChildren } from "react";
import Label from "../Label.tsx";
import Input from "../input/InputField.tsx";
import { FormProp, FormType } from "../../../model/Form/FormProp.ts";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto.ts";
import { FormActions, FormField, FormSection } from "../template/FormTemplate.tsx";
import Switch from "../switch/Switch.tsx";
import Select from "../Select.tsx";







const AeroCreateDeviceForm: React.FC<PropsWithChildren<FormProp<CreateAeroDeviceDto>>> = ({ dto, type, handleClick, setDto }) => {
      const isReadOnly = type == FormType.INFO;
      const protocolOptions = [
            { label: "Aero", value: 0 },
            { label: "VertX", value: 15 },
            { label: "Aperio", value: 16 }
      ]
      const baudRateOptions = [
            { label: "9600", value: 9600 },
            { label: "19200", value: 19200 },
            { label: "38400", value: 38400 },
            { label: "115200", value: 115200 },
      ]
      const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
            setDto(prev => ({ ...prev, [e.target.name]: e.target.value }));
      }


      return (
            <>

                  <FormSection title="Device Details" description="Name the location, assign its country, and add a short description." className="pb-10 mb-5">
                        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
                              <FormField>
                                    <Label htmlFor="name">Name</Label>
                                    <Input disabled={isReadOnly} placeholder="Device Name" name="name" type="text" id="name" onChange={handleChange} value={dto.name} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="name">Component Id</Label>
                                    <Input disabled={isReadOnly} placeholder="ComponentId" name="componentId" type="text" id="componentId" onChange={handleChange} value={dto.componentId} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="name">Mac</Label>
                                    <Input disabled={isReadOnly} placeholder="Mac Address" name="mac" type="text" id="mac" onChange={handleChange} value={dto.mac} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="name">Firmware</Label>
                                    <Input disabled={isReadOnly} placeholder="Firmware" name="fw" type="text" id="fw" onChange={handleChange} value={dto.fw} />
                              </FormField>
                              <FormField>
                                    <Label htmlFor="name">Serial Number</Label>
                                    <Input disabled={isReadOnly} placeholder="Serial Number" name="serialNumber" type="text" id="fw" onChange={handleChange} value={dto.serialNumber} />
                              </FormField>
                        </div>
                        
                  </FormSection>
                  <FormSection title="Connection Settings" description="Configure the connection settings for the device." className="pb-10">
                        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 p-5 ">
                              <FormField className="col-span-2">
                                    <Switch label={"Port 1"} defaultChecked={dto.metadata.portOne} onChange={(checked) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, portOne: checked } }))} />
                              </FormField>
                              {
                                    dto.metadata.portOne &&
                                    <>
                                          <FormField>
                                                <Label htmlFor="name">Protocol</Label>
                                                <Select disabled={isReadOnly} defaultValue={dto.metadata.protocolOne} name={"protocolOne"} options={protocolOptions} onChange={(v) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, protocolOne: Number(v) } }))} />
                                          </FormField>
                                          <FormField>
                                                <Label htmlFor="name">Baudrate</Label>
                                                <Select disabled={isReadOnly} defaultValue={dto.metadata.baudRateOne} name={"baudrateOne"} options={baudRateOptions} onChange={(v) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, baudRateOne: Number(v) } }))} />
                                          </FormField>
                                    </>
                              }

                        </div>
                        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 p-5 ">
                              <FormField className="col-span-2">
                                    <Switch label={"Port 2"} defaultChecked={dto.metadata.portTwo} onChange={(checked) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, portTwo: checked } }))} />
                              </FormField>
                              {
                                    dto.metadata.portTwo &&
                                    <>
                                          <FormField>
                                                <Label htmlFor="name">Protocol</Label>
                                                <Select disabled={isReadOnly} defaultValue={dto.metadata.protocolTwo} name={"protocolTwo"} options={protocolOptions} onChange={(v) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, protocolTwo: Number(v) } }))} />
                                          </FormField>
                                          <FormField>
                                                <Label htmlFor="name">Baudrate</Label>
                                                <Select disabled={isReadOnly} defaultValue={dto.metadata.baudRateTwo} name={"baudrateTwo"} options={baudRateOptions} onChange={(v) => setDto(prev => ({ ...prev, metadata: { ...prev.metadata, baudRateTwo: Number(v) } }))} />
                                          </FormField>
                                    </>
                              }

                        </div>
                  </FormSection>
                  <FormActions
                        disabled={isReadOnly}
                        onSubmit={handleClick}
                        onCancel={handleClick}
                        cancelName="close"
                        submitName={type == FormType.UPDATE ? "update" : "create"}
                        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
                  />


            </>);
}

export default AeroCreateDeviceForm;