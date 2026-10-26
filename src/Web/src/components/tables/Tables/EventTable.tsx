
import {  useMemo, useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import { TableSpecialDisplay } from "../../../model/TableSpecialDisplay";
import DatePicker from "../../form/date-picker";
import { usePagination } from "../../../context/PaginationContext";
import Pagination from "../../ui/table/Pagination";


type SortDirection = "asc" | "desc";

interface TableContents<T extends {guid:string,status:number}> {
  tableHeaders: string[];
  tableDatas: T[];
  tableKeys: string[];
  specialDisplay?: TableSpecialDisplay<T>[];
  columnWidths?:Record<string, string>;
  handleClick?:(data:T) => void; 
  getRowStatusClass?:(status:number) => string;
  fetchData: (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string | undefined,
    startDate?: string | undefined,
    endDate?: string | undefined,
  ) => Promise<void>;
}

export const EventTable = < T extends {guid:string,status:number} >({
  tableHeaders, tableDatas, tableKeys, specialDisplay, fetchData,columnWidths,getRowStatusClass,handleClick
}:TableContents<T>) => {
  const [sortKey, setSortKey] = useState<string>("dateTime");
  const [sortDirection, setSortDirection] = useState<SortDirection>("desc");
  const { search, startDate, endDate, pageSize, pagination, setPageSize, setEndDate, setStartDate, setSearch } = usePagination();

  const sortedDatas = useMemo(() => {
    const normalizedValue = (value: unknown) => {
      if (value === null || value === undefined) return "";
      if (typeof value === "number") return value;
      return String(value).toLowerCase();
    };



    const data = [...tableDatas];
    data.sort((a, b) => {
      const aValue = normalizedValue(a[sortKey as keyof T]);
      const bValue = normalizedValue(b[sortKey as keyof T]);

      if (aValue < bValue) return sortDirection === "asc" ? -1 : 1;
      if (aValue > bValue) return sortDirection === "asc" ? 1 : -1;
      return 0;
    });
    return data;
  }, [tableDatas, sortKey, sortDirection]);

  const handleSort = (key: string) => {
    if (sortKey === key) {
      setSortDirection((prev) => (prev === "asc" ? "desc" : "asc"));
      return;
    }
    setSortKey(key);
    setSortDirection("asc");
  };

  // const columnWidths: Record<string, string> = {
  //   timestamp: "w-[20%]",
  //   name: "w-[15%]",
  //   code: "w-[15%]",
  //   remarks: "w-[40%]",
  //   capture: "w-[10%]",
  // };

  const handleClickFirst = () => {
    fetchData(1, 10, search, startDate, endDate);
  };

  const handleClickPrevious = () => {
    fetchData(pagination.page - 1, pageSize, search, startDate, endDate);
  };

  const handleClickNext = () => {
    fetchData(pagination.page + 1, pageSize, search, startDate, endDate);
  };

  const handleClickLast = () => {
    fetchData(pagination.totalPages, pageSize, search, startDate, endDate);
  };

  const handlePageSizeSelect = (data: string) => {
    setPageSize(Number(data));
  };

  return (

    <>
      <div className="flex flex-col gap-2 px-4  sm:flex-row sm:items-center sm:justify-between">
        <div className="flex flex-wrap items-end gap-3 p-3  ">
          <div className="min-w-[220px]">
            <DatePicker
              id="startDate"
              placeholder="Select start date"
              onChange={(dates) => {
                if (!dates?.[0]) {
                  setStartDate(undefined);
                  return;
                }
                setStartDate(new Date(dates[0]).toISOString());
              }}
              replaceClass="h-10 w-full rounded-none border-b border-gray-300 bg-transparent py-2 pl-5 pr-2 text-sm text-gray-800 transition-colors placeholder:text-gray-400 focus:border-brand-500 focus:outline-none dark:border-gray-700 dark:text-white/90 dark:placeholder:text-white/30 dark:focus:border-brand-400 xl:w-[260px]"
            />
          </div>
          <div className="min-w-[220px]">
            <DatePicker
              id="endDate"
              placeholder="Select end date"
              onChange={(dates) => {
                if (!dates?.[0]) {
                  setEndDate(undefined);
                  return;
                }
                setEndDate(new Date(dates[0]).toISOString());
              }}
              replaceClass="h-10 w-full rounded-none border-b border-gray-300 bg-transparent py-2 pl-5 pr-2 text-sm text-gray-800 transition-colors placeholder:text-gray-400 focus:border-brand-500 focus:outline-none dark:border-gray-700 dark:text-white/90 dark:placeholder:text-white/30 dark:focus:border-brand-400 xl:w-[260px]"
            />
          </div>
          <button
            type="button"
            onClick={() => {
              setStartDate(undefined);
              setEndDate(undefined);
              const startInput = document.getElementById(
                "startDate",
              ) as HTMLInputElement | null;
              const endInput = document.getElementById(
                "endDate",
              ) as HTMLInputElement | null;
              if (startInput) startInput.value = "";
              if (endInput) endInput.value = "";
            }}
            className="h-11 rounded-lg border border-gray-300 bg-white px-3.5 text-sm font-medium text-gray-600 hover:bg-gray-100 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-300 dark:hover:bg-white/[0.03]"
          >
            Clear
          </button>
        </div>
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
          <div className="relative">
            <button className="absolute -translate-y-1/2 left-1 top-1/2 text-gray-500 dark:text-gray-400">
              <svg
                className="fill-current"
                width="18"
                height="18"
                viewBox="0 0 20 20"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  fillRule="evenodd"
                  clipRule="evenodd"
                  d="M3.04199 9.37363C3.04199 5.87693 5.87735 3.04199 9.37533 3.04199C12.8733 3.04199 15.7087 5.87693 15.7087 9.37363C15.7087 12.8703 12.8733 15.7053 9.37533 15.7053C5.87735 15.7053 3.04199 12.8703 3.04199 9.37363ZM9.37533 1.54199C5.04926 1.54199 1.54199 5.04817 1.54199 9.37363C1.54199 13.6991 5.04926 17.2053 9.37533 17.2053C11.2676 17.2053 13.0032 16.5344 14.3572 15.4176L17.1773 18.238C17.4702 18.5309 17.945 18.5309 18.2379 18.238C18.5308 17.9451 18.5309 17.4703 18.238 17.1773L15.4182 14.3573C16.5367 13.0033 17.2087 11.2669 17.2087 9.37363C17.2087 5.04817 13.7014 1.54199 9.37533 1.54199Z"
                />
              </svg>
            </button>
            <input
              onChange={(e) => setSearch(e.target.value)}
              placeholder="Search..."
              className="h-10 w-full rounded-none border-b border-gray-300 bg-transparent py-2 pl-8 pr-2 text-sm text-gray-800 transition-colors placeholder:text-gray-400 focus:border-brand-500 focus:outline-none dark:border-gray-700 dark:text-white/90 dark:placeholder:text-white/30 dark:focus:border-brand-400 xl:w-[260px]"
              type="text"
            />
          </div>
          <button className="inline-flex items-center justify-center gap-2 rounded-lg transition  px-4 py-3 text-sm bg-white text-gray-700 ring-1 ring-inset ring-gray-300 hover:bg-gray-50 dark:bg-gray-800 dark:text-gray-400 dark:ring-gray-700 dark:hover:bg-white/[0.03] dark:hover:text-gray-300 ">
            Export
            <svg
              className="fill-current"
              width="20"
              height="20"
              viewBox="0 0 20 20"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fillRule="evenodd"
                clipRule="evenodd"
                d="M10.0018 14.083C9.7866 14.083 9.59255 13.9924 9.45578 13.8472L5.61586 10.0097C5.32288 9.71688 5.32272 9.242 5.61552 8.94902C5.90832 8.65603 6.3832 8.65588 6.67618 8.94868L9.25182 11.5227L9.25182 3.33301C9.25182 2.91879 9.5876 2.58301 10.0018 2.58301C10.416 2.58301 10.7518 2.91879 10.7518 3.33301L10.7518 11.5193L13.3242 8.94866C13.6172 8.65587 14.0921 8.65604 14.3849 8.94903C14.6777 9.24203 14.6775 9.7169 14.3845 10.0097L10.5761 13.8154C10.4385 13.979 10.2323 14.083 10.0018 14.083ZM4.0835 13.333C4.0835 12.9188 3.74771 12.583 3.3335 12.583C2.91928 12.583 2.5835 12.9188 2.5835 13.333V15.1663C2.5835 16.409 3.59086 17.4163 4.8335 17.4163H15.1676C16.4102 17.4163 17.4176 16.409 17.4176 15.1663V13.333C17.4176 12.9188 17.0818 12.583 16.6676 12.583C16.2533 12.583 15.9176 12.9188 15.9176 13.333V15.1663C15.9176 15.5806 15.5818 15.9163 15.1676 15.9163H4.8335C4.41928 15.9163 4.0835 15.5806 4.0835 15.1663V13.333Z"
                fill="currentColor"
              ></path>
            </svg>
          </button>
        </div>
      </div>
      <div className="overflow-hidden rounded-xl border border-gray-200 bg-white dark:border-white/[0.05] dark:bg-white/[0.03]">
        <div className="max-w-full overflow-x-auto">

          <div className="max-h-[70vh] overflow-auto scrollbar-thin scrollbar-transparent">

            <Table className="w-full table-fixed">
              {/* Table Header */}
              <TableHeader className="border-b border-gray-100 dark:border-white/[0.05] bg-white dark:bg-gray-900 sticky top-0 z-10">
                <TableRow>
                  {tableHeaders.map((head: string, i: number) => {
                    const key = tableKeys[i];
                    const isActive = sortKey === key;
                    return (
                      <TableCell
                        key={i}
                        isHeader
                        className={`px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400 ${columnWidths == undefined ? "" : columnWidths[key] ?? ""}`}
                      >
                        <button
                          type="button"
                          onClick={() => handleSort(key)}
                          className="group inline-flex items-center gap-1.5 transition-colors hover:text-brand-500"
                        >
                          <span>{head}</span>
                          <span className={`inline-flex flex-col leading-none ${isActive ? "text-brand-500" : "text-gray-400 group-hover:text-brand-500"}`}>
                            <svg className={`h-2 w-2 ${isActive && sortDirection === "asc" ? "opacity-100" : "opacity-40"}`} viewBox="0 0 10 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                              <path d="M5 2L8 6H2L5 2Z" fill="currentColor" />
                            </svg>
                            <svg className={`h-2 w-2 ${isActive && sortDirection === "desc" ? "opacity-100" : "opacity-40"}`} viewBox="0 0 10 10" fill="none" xmlns="http://www.w3.org/2000/svg">
                              <path d="M5 8L2 4H8L5 8Z" fill="currentColor" />
                            </svg>
                          </span>
                        </button>
                      </TableCell>
                    )
                  })}
                </TableRow>
              </TableHeader>
              <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                {sortedDatas.length === 0 && (
                  <TableRow>
                    <TableCell className="px-4 py-8 text-center text-gray-500 text-theme-sm dark:text-gray-400" colspan={tableHeaders.length}>
                      No event records found
                    </TableCell>
                  </TableRow>
                )}
                {sortedDatas && 
                sortedDatas.map((data: T, i: number) => (
                  <TableRow 
                  onClickWithEvent={() => {
                    console.log(data)
                    if(handleClick != undefined)
                      handleClick(data)
                  }}
                  key={i} className={`transition-colors ${handleClick != undefined ? "cursor-pointer" : ""} hover:bg-gray-50 dark:hover:bg-white/[0.02] ${getRowStatusClass == undefined ? "" : getRowStatusClass(data.status)}`}>
                    {tableKeys.map((key: string, i: number) =>
                      specialDisplay?.some(a => a.key == key) ?
                        specialDisplay.find(a => a.key == key)?.content(data, i)
                        :
                        <TableCell
                          key={i}
                          className={`px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400 ${key === "remarks" ? "break-all" : ""
                            }`}
                        >
                          {String(data[key as keyof typeof data])}
                        </TableCell>

                    )}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
          <Pagination
            onSelectPageSize={handlePageSizeSelect}
            pageNumber={pagination.page}
            pageSize={pagination.pageSize}
            totalCount={pagination.totalItems}
            totalPage={pagination.totalPages}
            onClickFirst={handleClickFirst}
            onClickPrevious={handleClickPrevious}
            onClickLast={handleClickLast}
            onClickNext={handleClickNext}
          />

        </div>
      </div>
    </>

  );
}



