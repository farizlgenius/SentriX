export interface ModuleDto
{
    id:number,
    componentId:number;
    deviceComponentId:number;
    name:string,
    fw:string,
    serialNumber:string,
    port:number,
    address:number,
    mac:string,
    model:string,
    locationId:number;
    type:string;
    isActive:boolean;
}