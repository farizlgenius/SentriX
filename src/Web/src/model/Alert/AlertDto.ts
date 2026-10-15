export interface AlertDto {
      guid:string;
      name:string;
      advice:string;
      locationGuid:string;
      locationName:string;
      alertLevelGuid:string;
      alertLevel:string;
      baseEventGuids:string[]
      isActive:boolean;
      isDefault:boolean;
}