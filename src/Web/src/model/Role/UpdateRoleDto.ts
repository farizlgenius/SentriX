import { PermissionDto } from "./PermissionDto";

export interface UpdateRoleDto {
      id: number;
      name: string;
      permissions: PermissionDto[];
      locationId: number;
}