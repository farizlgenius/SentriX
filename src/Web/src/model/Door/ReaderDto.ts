import { ReaderDirection } from "../../enum/DoorDirection";
import { ReaderMode } from "../../enum/ReaderMode";
import { Vendor } from "../../enum/Vendor";
import { AeroReaderMetadata } from "./AeroReaderMetadata";

export interface ReaderDto {
  guid: string;
  slotNo: number;
  mode: ReaderMode;
  metadata: string | AeroReaderMetadata;
  vendor: Vendor;
  readerDirection: ReaderDirection;
  deviceModuelGuid: string;
}
