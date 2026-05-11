import { PermissionDto } from "./PermissionDto";

export interface CreateRoleDto{
      name:string;
      permissions:PermissionDto[];
      locationId:number;
}