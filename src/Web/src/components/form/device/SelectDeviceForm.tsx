import { ChangeEvent, PropsWithChildren, useEffect, useState } from "react";
import Label from "../Label.tsx";
import Select from "../Select.tsx";
import { HardwareIcon } from "../../../icons/index.ts";
import { DeviceType } from "../../../enum/DeviceType.ts";



interface SelectDeviceFormProp {
      setForm:React.Dispatch<React.SetStateAction<boolean>>
      setDeviceType:React.Dispatch<React.SetStateAction<string>>
      setSelectType:React.Dispatch<React.SetStateAction<boolean>>

}



const SelectDeviceForm: React.FC<PropsWithChildren<SelectDeviceFormProp>> = ({setForm,setDeviceType,setSelectType}) => {

      return (
            <div>
                  <Label htmlFor="type">Device Type</Label>

                        <Select
                              icon={<HardwareIcon/>}
                              name="name"
                              id="name"
                              defaultValue={-1}
                              options={[
                              {
                                    label:DeviceType.AERO.toString(),
                                    value:DeviceType.AERO
                              },{
                                    label:DeviceType.AMICO.toString(),
                                    value:DeviceType.AMICO
                              }]}
                              onChange={(v) => {
                                    setDeviceType(v);
                                    setForm(true);
                                    setSelectType(false);
                              }}
                        />
            </div>);
}

export default SelectDeviceForm;