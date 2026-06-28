import { BaseDto } from "../BaseDto";
import { CredentialDto } from "./CredentialDto";

export interface UserDto extends BaseDto {
    userId:string;
    title:string;
    firstName:string;
    middleName:string;
    lastName:string;
    gender:string;
    dateOfBirth:string;
    email:string;
    phone:string;
    companyId:number;
    company:string;
    departmentId:number;
    department:string;
    positionId:number;
    position:string;
    address:string;
    flag:number;
    additionals:string[];
    image:string;
    credentials:CredentialDto[];
    groups:number[];
    vacationId:number;
    locationId:number;
}