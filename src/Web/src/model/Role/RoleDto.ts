import { PermissionDto } from "./PermissionDto";

export interface RoleDto{
    id:number;
    name:string;
    permissions:PermissionDto[];
    locationName:string;
}