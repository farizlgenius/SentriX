import { ChangeEvent, PropsWithChildren } from "react";
import Label from "../Label.tsx";
import Input from "../input/InputField.tsx";
import Button from "../../ui/button/Button.tsx";
import { FormProp, FormType } from "../../../model/Form/FormProp.ts";
import { CreateDeviceDto } from "../../../model/Device/CreateDeviceDto.ts";
import { FormActions, FormField, FormSection } from "../template/FormTemplate.tsx";







const AeroCreateDeviceForm: React.FC<PropsWithChildren<FormProp<CreateDeviceDto>>> = ({ dto, type, handleClick, setDto }) => {
      const isReadOnly = type == FormType.INFO;
      const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
            setDto(prev => ({ ...prev, [e.target.name]: e.target.value }));
      }


      return (
            <>

                  <FormSection title="Device Details" description="Name the location, assign its country, and add a short description." className="pb-10">
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