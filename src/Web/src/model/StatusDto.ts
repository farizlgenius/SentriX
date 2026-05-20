export interface StatusDto {
  deviceComponentId: number;
  componentId: number;
  status: number | string;
  tamper: number | string;
  ac: number | string;
  batt: number | string;
}
