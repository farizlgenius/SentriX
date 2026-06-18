import { PropsWithChildren, useState } from "react";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { FormProp } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import AeroDoorForm from "./AeroDoorForm";
import { DeviceType } from "../../enum/DeviceType";
import AmicoDoorForm from "./AmicoDoorForm";

const typeOption:Options[] = [
      {
                  label:"Aero",
                  value:DeviceType.AERO
            },{
                  label:"Amico",
                  value:DeviceType.AMICO
            }
]

const DoorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({ handleClick, dto, setDto, type }) => {
      const [selectedType, setSelectedType] = useState("");
      const FormTypeSwitcher = (value:string) => {
            switch(value){
                  case DeviceType.AERO:
                        return <AeroDoorForm handleClick={handleClick} dto={dto} setDto={setDto} type={type} />
                  case DeviceType.AMICO:
                        return <AmicoDoorForm handleClick={handleClick} dto={dto} setDto={setDto} type={type}/>
                  default:
                  return <></>
            }
      }
      return (
            <>
                  {
                        selectedType == "AERO" ?
                              FormTypeSwitcher(selectedType)
                              :
                              <div className="rounded-xl border border-gray-200 p-6 dark:border-gray-800 border border-gray-200 bg-white dark:border-gray-800 dark:bg-white/[0.03] " >
                                    <div className="gap-3">
                                          <Label>Door Type Selector</Label>
                                          <Select isString={true} options={typeOption} name="Type" defaultValue={selectedType} onChange={e => {
                                                setSelectedType(e);
                                          }} />
                                    </div>
                              </div>
                  }

            </>

      )
}

export default DoorForm;