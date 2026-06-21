import { PaginationData } from "./PaginationData";
import { EventDto } from "./EventDto";

export interface PaginationDto{
    page:PaginationData;
    data:EventDto[];
}