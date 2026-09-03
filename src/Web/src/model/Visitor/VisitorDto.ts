import { Gender } from "../../enum/Gender";
import { Title } from "../../enum/Title";

export interface VisitorDto {
  guid: string;
  identification: string;
  title: Title;
  firstname: string;
  middlename: string;
  lastname: string;
  gender: Gender;
  email: string;
  phone: string;
  address: string;
  joinedDate: Date;
  expiredDate: Date;
  additionals: string[];
  groups: string;
  locations: string;
  isActive: boolean;
}
