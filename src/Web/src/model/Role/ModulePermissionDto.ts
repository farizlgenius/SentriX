import { FeaturePermissionDto } from "./FeaturePermissionDto";

export interface ModulePermissionDto {
  id: number;
  name: string;
  isEnabled: boolean;
  features: FeaturePermissionDto[];
}
