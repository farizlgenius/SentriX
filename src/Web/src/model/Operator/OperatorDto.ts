import { Gender } from "../../enum/Gender";
import { Title } from "../../enum/Title";

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
  roleGuid: string;
  role: string;
  locationGuids: string[];
  isActive: boolean;
  isDefault: boolean;
}
