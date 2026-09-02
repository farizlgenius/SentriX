import { Gender } from "../../enum/Gender";
import { Title } from "../../enum/Title";


export interface OperatorDto  {
    guid:string;
    username:string;
    password:string;
    title:Title;
    firstname:string;
    middlename:string;
    lastname:string;
    gender:Gender;
    email:string;
    mobile:string;
    roleGuid:string; 
    locationGuids:string[]; 
    isActive:boolean;
    isDefault:boolean;
}
