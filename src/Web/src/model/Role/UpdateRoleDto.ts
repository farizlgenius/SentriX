import { ModulePermissionDto } from "./ModulePermissionDto";

export interface UpdateRoleDto {
  guid: string;
  name: string;
  modules: ModulePermissionDto[];
}
