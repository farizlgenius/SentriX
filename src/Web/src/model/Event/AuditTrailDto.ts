
export interface AuditTrailDto{
    timestamp:string;
    username:string;
    module:string;
    action:number;
    ip:string;
    guid:string;
    name:string;
    detail:string | EntityDiffResponse;
}

export interface EntityDiffResponse {
  entityName: string;
  entityState: 'Added' | 'Modified' | 'Deleted' | 'Unchanged';
  changes: Record<string, PropertyDiff>;
}

export interface PropertyDiff {
  old: any;
  new: any;
}