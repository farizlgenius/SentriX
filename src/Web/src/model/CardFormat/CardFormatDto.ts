import { BaseDto } from "../BaseDto";

export interface CardFormatDto extends BaseDto {
    id:number;
    name: string;
    fac: number;
    offset:number;
    functionId:number;
    flag:number;
    bits: number;
    peLn: number;
    peLoc: number;
    poLn: number;
    poLoc: number;
    fcLn: number;
    fcLoc: number;
    chLn: number;
    chLoc: number;
    icLn: number;
    icLoc: number;
    locationId:number;
    isActive:boolean;
}