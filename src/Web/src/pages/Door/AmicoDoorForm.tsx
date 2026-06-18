import { PropsWithChildren } from "react";
import { FormProp } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";

const AmicoDoorForm:React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({ handleClick,  dto, setDto ,type}) => {
      return (
            <></>
      );
}

export default AmicoDoorForm;