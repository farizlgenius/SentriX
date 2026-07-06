import { Table, TableBody, TableCell, TableHeader, TableRow } from "../../components/ui/table";
import Search from "../../components/ui/table/Search";
import { TableProp } from "../../model/TableProp";
import {  ToggleOffIcon, ToggleOnIcon, TrashBinIcon } from "../../icons";
import { useEffect, useMemo, useState } from "react";
import Pagination from "../../components/ui/table/Pagination";
import { usePagination } from "../../context/PaginationContext";
import React from "react";
import Switch from "../../components/form/switch/Switch";



export const BaseTable = <T extends { id: number | string, isDefault: boolean, isActive: boolean }>({ id, headers, keys, data, onEdit, onInfo, onRemove, setSelect, renderOptionalComponent, specialDisplay, onClick: handleClick, permission, action, status, select, subTable, fetchData, refresh, locationId }: TableProp<T>) => {
    const { search, startDate, endDate, pageSize, pagination, setPageSize } = usePagination();
    const [show, setShow] = useState<number>(-1)
    const [sortKey, setSortKey] = useState<string>(keys?.[0] ?? "");
    const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");


    const toSortableValue = (value: unknown): number | string => {
        if (value === null || value === undefined) return "";
        if (typeof value === "number") return value;
        if (typeof value === "boolean") return value ? 1 : 0;
        if (value instanceof Date) return value.getTime();

        const str = String(value).trim();
        const parsedDate = Date.parse(str);
        if (!Number.isNaN(parsedDate) && /[\d]/.test(str)) {
            return parsedDate;
        }
        return str.toLowerCase();
    };

    const sortedData = useMemo(() => {
        if (!sortKey) return data;
        const next = [...data];
        next.sort((a, b) => {
            const aValue = toSortableValue(a[sortKey as keyof T]);
            const bValue = toSortableValue(b[sortKey as keyof T]);

            if (typeof aValue === "number" && typeof bValue === "number") {
                return sortDirection === "asc" ? aValue - bValue : bValue - aValue;
            }

            const result = String(aValue).localeCompare(String(bValue));
            return sortDirection === "asc" ? result : -result;
        });
        return next;
    }, [data, sortKey, sortDirection]);

    const handleSort = (key: string) => {
        if (!key) return;
        if (sortKey === key) {
            setSortDirection((prev) => (prev === "asc" ? "desc" : "asc"));
        } else {
            setSortKey(key);
            setSortDirection("asc");
        }
        setShow(-1);
    };

    const handleClickFirst = () => {
        fetchData(1, 10, locationId, search, startDate, endDate);
    }

    const handleClickPrevious = () => {
        fetchData(pagination.page - 1, pageSize, locationId, search, startDate, endDate);
    }

    const handleClickNext = () => {

        fetchData(pagination.page + 1, pageSize, locationId, search, startDate, endDate);
    }

    const handleClickLast = () => {

        fetchData(pagination.totalPages, pageSize, locationId, search, startDate, endDate);
    }

    const handlePageSizeSelect = (data: string) => {
        setPageSize(Number(data));
    }


    const handleCheckAll = (data: T[], e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.checked) {
            setSelect(data.filter(x => !x.isDefault));
        } else {
            setSelect([]);
        }

    }

    const handleCheck = (data: T, e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.checked) {
            setSelect((prev) => [...prev, data]);
        } else {
            setSelect((prev) =>
                prev.filter((item) => item.id !== data.id)
            );
        }

    }

    useEffect(() => {
        if (locationId != -1) {
            fetchData(1, pageSize, locationId, search, startDate, endDate);
        }
    }, [refresh, pageSize, search, startDate, endDate, locationId])


    return (
        <>
            <div className="overflow-visible rounded-xl border border-gray-200 bg-white dark:border-white/[0.05] dark:bg-white/[0.03]">
                <div className="max-w-full overflow-x-auto">
                    <Search action={action} onClick={handleClick} permission={permission} locationId={locationId} />
                    <div className="max-h-[70vh] overflow-y-auto hidden-scroll">
                        <Table>
                            {/* Table Header */}
                            <TableHeader className="border-b border-gray-100 dark:border-white/[0.05] bg-white dark:bg-gray-900 sticky top-0 z-10">
                                <TableRow>
                                    <TableCell isHeader className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                        <input type="checkbox" onChange={(e) => handleCheckAll(sortedData, e)} />
                                    </TableCell>
                                    {headers && headers.map((head: string, i: number) => {
                                        const key = keys?.[i] ?? "";
                                        const isActive = sortKey === key;
                                        return (
                                            <TableCell
                                                key={i}
                                                isHeader
                                                className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
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
                                {sortedData.length === 0 && (
                                    <TableRow>
                                        <TableCell
                                            colspan={headers.length + 2}
                                            className="px-4 py-8 text-center text-gray-500 text-theme-sm dark:text-gray-400"
                                        >
                                            No record
                                        </TableCell>
                                    </TableRow>
                                )}
                                {sortedData && sortedData.map((data: T, i: number) => (
                                    <React.Fragment key={i} >
                                        <TableRow key={i} className="cursor-pointer hover:bg-gray-900 active:bg-gray-800" onClickWithEvent={() => {
                                            onInfo(data)
                                        }}>
                                            <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                                {
                                                    !data.isDefault && <input onClick={(e) => e.stopPropagation()} checked={select?.includes(data)} type="checkbox" onChange={(e) => handleCheck(data, e)} />
                                                }

                                            </TableCell  >

                                            {keys && keys.map((key: string, i: number) =>
                                                specialDisplay?.some(a => a.key == key) ?
                                                    specialDisplay.find(a => a.key == key)?.content(data, i)
                                                    :
                                                    <TableCell key={i} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                                        {String(data[key as keyof typeof data])}
                                                    </TableCell>
                                            )}
                                            {status && renderOptionalComponent && renderOptionalComponent(data, status, i)}

                                            {
                                                /* Status */
                                            }
                                            <TableCell  className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                                <Switch label={""} defaultChecked={data.isActive}  onChange={(checked) => {
                                                    console.log(checked)
                                                }} />
                                            </TableCell>



                                            {/* Action */}
                                            <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                                                <div className="flex gap-2">
                                                    {


                                                        <button
                                                            type="button"
                                                            onClick={(e) => {
                                                                e.stopPropagation()
                                                                onRemove(data)
                                                            }}
                                                            disabled={!permission?.isDeleted || data.isDefault}
                                                            className={`
    inline-flex items-center justify-center
    rounded-lg p-1
    transition-all duration-200
    ${permission?.isDeleted || !data.isDefault
        ?
        "cursor-pointer text-red-600 hover:bg-red-50 hover:text-red-700 active:scale-95"
                                                                    : "cursor-not-allowed bg-gray-100 text-gray-400 opacity-60"
                                                                    
                                                                }
  `}
                                                        >
                                                            <TrashBinIcon className="h-5 w-5" />
                                                        </button>

                                                    }


                                                </div>
                                            </TableCell>
                                        </TableRow>
                                        {/* {show == i && subTable && subTable(i + 1)} */}
                                    </React.Fragment>


                                ))}
                            </TableBody>
                        </Table>
                    </div>
                    <Pagination onSelectPageSize={handlePageSizeSelect} pageNumber={pagination.page} pageSize={pagination.pageSize} totalCount={pagination.totalItems} totalPage={pagination.totalPages} onClickFirst={handleClickFirst} onClickPrevious={handleClickPrevious} onClickLast={handleClickLast} onClickNext={handleClickNext} />
                    {/* <PaginationNew /> */}
                </div>
            </div>


        </>
    )
}
