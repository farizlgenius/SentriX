import { ModulePermissionDto } from "./ModulePermissionDto";

export interface CreateRoleDto {
  name: string;
  modules: ModulePermissionDto[];
}
