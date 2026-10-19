
export interface AlertDto {
      guid:string;
      name:string;
      advice:string;
      locationGuid:string;
      locationName:string;
      alertLevelGuid:string;
      alertLevelColor:string;
      alertLevelName:string;
      coreEntityGuid:string;
      coreEventGuid:string;
      isActive:boolean;
      isDefault:boolean;
}