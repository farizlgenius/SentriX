import { Gender } from "../../enum/Gender";
import { Title } from "../../enum/Title";
import { BaseDto } from "../BaseDto";

export interface OperatorDto {
  guid: string;
  username: string;
  password: string;
  title: Title;
  firstName: string;
  middleName: string;
  lastName: string;
  gender: Gender;
  email: string;
  phone: string;
  joinedDate: Date;
  expiredDate: Date;
  role: BaseDto;
  locations:BaseDto[];
  isActive: boolean;
  isDefault: boolean;
}
