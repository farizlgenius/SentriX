export interface StatusDto {
  id: number;
  componentId: number;
  status: number | string;
  tamper: number | string;
  ac: number | string;
  batt: number | string;
}
