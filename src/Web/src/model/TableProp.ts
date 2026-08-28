import React, { JSX } from "react";
import { TableSpecialDisplay } from "./TableSpecialDisplay";
import { ActionButton } from "./ActionButton";
import { StatusDto } from "./StatusDto";
import { FeaturePermissionDto } from "./Role/FeaturePermissionDto";

export interface TableProp<T extends { guid: string }> {
  headers: string[];
  keys: string[];
  data: T[];
  onInfo: (data: T) => void;
  onEdit: (data: T) => void;
  onRemove: (data: T) => void;
  onClick: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  select: T[];
  setSelect: React.Dispatch<React.SetStateAction<T[]>>;
  renderOptionalComponent?: (
    data: any,
    statusDto: StatusDto[],
    index: number,
  ) => JSX.Element[];
  specialDisplay?: TableSpecialDisplay<T>[];
  permission?: FeaturePermissionDto;
  status?: StatusDto[];
  action?: ActionButton[];
  subTable?: (index: number) => JSX.Element;
  fetchData: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) => Promise<void>;
  refresh?: boolean;
  locationGuid: string;
}
