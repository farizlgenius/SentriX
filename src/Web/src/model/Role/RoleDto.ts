import { ModulePermissionDto } from "./ModulePermissionDto";

export interface RoleDto {
  guid: string;
  name: string;
  modules: ModulePermissionDto[];
  isActive: boolean;
  isDefault: boolean;
}
