import { ReaderDirection } from "../../enum/DoorDirection";
import { ReaderMode } from "../../enum/ReaderMode";
import { Vendor } from "../../enum/Vendor";

export interface ReaderDto {
  guid: string;
  slotNo: number;
  mode: ReaderMode;
  metadata: string;
  vendor: Vendor;
  readerDirection: ReaderDirection;
}
