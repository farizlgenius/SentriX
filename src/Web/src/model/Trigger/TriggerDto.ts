import { BaseDto } from "../BaseDto";
import { TransactionCodeDto } from "../Event/TransactionCodeDto";

export interface TriggerDto extends BaseDto {
    command:number;
    procedureId:number;
    sourceType:number;
    sourceNumber:number;
    tranType:number;
    codeMap:TransactionCodeDto[];
    timeZone:number;
}