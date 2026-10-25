import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../components/ui/table";
import Search from "../../components/ui/table/Search";
import { TableProp } from "../../model/TableProp";
import { AddIcon, EditIcon, TrashBinIcon } from "../../icons";
import { useEffect, useMemo, useRef, useState } from "react";
import Pagination from "../../components/ui/table/Pagination";
import { usePagination } from "../../context/PaginationContext";
import React from "react";
import Switch from "../../components/form/switch/Switch";
import { createPortal } from "react-dom";
import { useTheme } from "../../context/ThemeContext";

export const BaseTable = <
  T extends { guid: string; isDefault: boolean; isActive: boolean },
>({
  headers,
  keys,
  data,
  onEdit,
  onRemove,
  setSelect,
  renderOptionalComponent,
  specialDisplay,
  onClick: handleClick,
  permission,
  action,
  status,
  select,
  subTable,
  fetchData,
  refresh,
  altrAction,
  locationGuid,
  onInfo
}: TableProp<T>) => {
  const { accentColor } = useTheme();
  const { search, startDate, endDate, pageSize, pagination, setPageSize } =
    usePagination();
  const [show, setShow] = useState<number>(-1);
  const [sortKey, setSortKey] = useState<string>(keys?.[0] ?? "");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const addButtonRef = useRef<HTMLButtonElement | null>(null);
  const selectedCount = select.length;
  const hasMoreActions = Boolean(permission?.isEnabled);
  const showActionIsland = Boolean(permission?.isCreated || selectedCount > 0);

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
    fetchData(1, 10, locationGuid, search, startDate, endDate);
  };

  const handleClickPrevious = () => {
    fetchData(
      pagination.page - 1,
      pageSize,
      locationGuid,
      search,
      startDate,
      endDate,
    );
  };

  const handleClickNext = () => {
    fetchData(
      pagination.page + 1,
      pageSize,
      locationGuid,
      search,
      startDate,
      endDate,
    );
  };

  const handleClickLast = () => {
    fetchData(
      pagination.totalPages,
      pageSize,
      locationGuid,
      search,
      startDate,
      endDate,
    );
  };

  const handlePageSizeSelect = (data: string) => {
    setPageSize(Number(data));
  };

  const handleCheckAll = (
    data: T[],
    e: React.ChangeEvent<HTMLInputElement>,
  ) => {
    if (e.target.checked) {
      setSelect(data.filter((x) => !x.isDefault));
    } else {
      setSelect([]);
    }
  };

  const handleCheck = (data: T, e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.checked) {
      setSelect((prev) => [...prev, data]);
    } else {
      setSelect((prev) => prev.filter((item) => item.guid !== data.guid));
    }
  };

  useEffect(() => {
    console.log(permission);
    if (locationGuid != "") {
      fetchData(1, pageSize, locationGuid, search, startDate, endDate);
    }
  }, [refresh, pageSize, search, startDate, endDate, locationGuid]);

  useEffect(() => {
    if (!permission?.isCreated || selectedCount > 0) return;

    const handleAddShortcut = (event: KeyboardEvent) => {
      const target = event.target;
      if (
        event.defaultPrevented ||
        event.altKey ||
        event.ctrlKey ||
        event.metaKey ||
        (target instanceof HTMLElement &&
          (target.isContentEditable ||
            ["INPUT", "TEXTAREA", "SELECT"].includes(target.tagName)))
      ) {
        return;
      }

      if (event.key.toLowerCase() === "a" || event.key === "+") {
        event.preventDefault();
        addButtonRef.current?.click();
      }
    };

    window.addEventListener("keydown", handleAddShortcut);
    return () => window.removeEventListener("keydown", handleAddShortcut);
  }, [permission?.isCreated, selectedCount]);

  return (
    <>
      <div className="flex flex-col gap-4">
        <Search
          altrAction={altrAction}
          onClick={handleClick}
        />
        <div className="overflow-hidden rounded-xl border border-gray-200 bg-white shadow-theme-xs dark:border-white/[0.05] dark:bg-white/[0.03]">
          <div className="max-w-full overflow-x-auto">
          <div className="max-h-[70vh] overflow-y-auto hidden-scroll">
            <Table>
              {/* Table Header */}
              <TableHeader className="border-b border-gray-100 dark:border-white/[0.05] bg-white dark:bg-gray-900 sticky top-0 z-10">
                <TableRow>
                  <TableCell
                    isHeader
                    className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                  >
                    <input
                      type="checkbox"
                      onChange={(e) => handleCheckAll(sortedData, e)}
                    />
                  </TableCell>
                  {headers &&
                    headers.map((head: string, i: number) => {
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
                            <span
                              className={`inline-flex flex-col leading-none ${isActive ? "text-brand-500" : "text-gray-400 group-hover:text-brand-500"}`}
                            >
                              <svg
                                className={`h-2 w-2 ${isActive && sortDirection === "asc" ? "opacity-100" : "opacity-40"}`}
                                viewBox="0 0 10 10"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path d="M5 2L8 6H2L5 2Z" fill="currentColor" />
                              </svg>
                              <svg
                                className={`h-2 w-2 ${isActive && sortDirection === "desc" ? "opacity-100" : "opacity-40"}`}
                                viewBox="0 0 10 10"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                              >
                                <path d="M5 8L2 4H8L5 8Z" fill="currentColor" />
                              </svg>
                            </span>
                          </button>
                        </TableCell>
                      );
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
                {sortedData &&
                  sortedData.map((data: T, i: number) => (
                    <React.Fragment key={i}>
                      <TableRow
                        key={i}
                        className="cursor-pointer hover:bg-gray-900 active:bg-gray-800"
                        onClickWithEvent={() => {
                          console.log(data);
                          onInfo(data);
                        }}
                      >
                        <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                          {!data.isDefault && (
                            <input
                              onClick={(e) => e.stopPropagation()}
                              checked={select?.includes(data)}
                              type="checkbox"
                              onChange={(e) => handleCheck(data, e)}
                            />
                          )}
                        </TableCell>

                        {keys &&
                          keys.map((key: string, i: number) =>
                            specialDisplay?.some((a) => a.key == key) ? (
                              specialDisplay
                                .find((a) => a.key == key)
                                ?.content(data, i)
                            ) : (
                              <TableCell
                                key={i}
                                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                              >
                                {String(data[key as keyof typeof data])}
                              </TableCell>
                            ),
                          )}
                        {status &&
                          renderOptionalComponent &&
                          renderOptionalComponent(data, status, i)}

                        {/* Status */}
                        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                          {
                            !data.isDefault &&
                            <Switch
                              label={""}
                              defaultChecked={data.isActive}
                              onChange={(checked) => {
                                console.log(checked);
                              }}
                            />
                          }

                        </TableCell>

                        {/* Action */}
                        <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                          {
                            !data.isDefault &&

                            <div className="flex gap-2">
                               <button
                                type="button"
                                onClick={(e) => {
                                  e.stopPropagation();
                                  onEdit(data);
                                }}

                                className={`
                                                                    inline-flex items-center justify-center
                                                                    rounded-lg p-1
                                                                    transition-all duration-200
                                                                    cursor-pointer text-brand-600 hover:bg-brand-50 hover:text-brand-700 active:scale-95
  `}
                              >
                                <EditIcon className="h-5 w-5" />
                              </button>
                              <button
                                type="button"
                                onClick={(e) => {
                                  e.stopPropagation();
                                  onRemove(data);
                                }}

                                className={`
                                                                    inline-flex items-center justify-center
                                                                    rounded-lg p-1
                                                                    transition-all duration-200
                                                                    cursor-pointer text-red-600 hover:bg-red-50 hover:text-red-700 active:scale-95
  `}
                              >
                                <TrashBinIcon className="h-5 w-5" />
                              </button>
                              
                            </div>

                          }

                        </TableCell>
                      </TableRow>
                      {/* {show == i && subTable && subTable(i + 1)} */}
                    </React.Fragment>
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
          {/* <PaginationNew /> */}
          </div>
        </div>
      </div>
      {showActionIsland &&
        typeof document !== "undefined" &&
        createPortal(
          <div
            className={`fixed bottom-6 left-1/2 z-[10000] -translate-x-1/2 transition-[width,transform] duration-300 ease-out ${
              selectedCount > 0
                ? "w-[min(720px,calc(100vw-1.5rem))]"
                : "w-[168px]"
            }`}
          >
            <div
              className="rounded-full border border-gray-200 bg-[var(--app-panel-bg)]/95 p-2.5 shadow-[0_20px_70px_rgba(0,0,0,0.3)] backdrop-blur-md dark:border-gray-700 dark:bg-gray-900/95"
              style={{
                boxShadow: `0 20px 70px rgba(0,0,0,0.3), 0 0 28px color-mix(in srgb, ${accentColor} 10%, transparent)`,
              }}
            >
              {selectedCount === 0 ? (
                <div className="group relative">
                  <button
                    ref={addButtonRef}
                    type="button"
                    name="add"
                    onClick={handleClick}
                    disabled={!locationGuid}
                    className="inline-flex w-full items-center justify-center gap-2 rounded-full bg-brand-500 px-5 py-3 text-sm font-semibold text-white shadow-lg transition duration-200 hover:bg-brand-600 active:scale-[0.98] disabled:cursor-not-allowed disabled:opacity-50"
                    style={{
                      boxShadow: `0 0 18px color-mix(in srgb, ${accentColor} 25%, transparent)`,
                    }}
                  >
                    <AddIcon />
                    Add Point
                  </button>
                  <span
                    aria-hidden="true"
                    className="pointer-events-none absolute bottom-full left-1/2 mb-3 -translate-x-1/2 whitespace-nowrap rounded-lg border border-gray-700 bg-gray-900 px-3 py-1.5 text-xs text-gray-300 opacity-0 shadow-lg transition-opacity group-hover:opacity-100 group-focus-within:opacity-100"
                  >
                    Press A or +
                  </span>
                </div>
              ) : (
                <div className="flex min-w-0 items-center gap-2 overflow-x-auto whitespace-nowrap rounded-full border border-brand-500/30 bg-[var(--app-panel-bg)] px-2 py-1.5 [scrollbar-width:none] [&::-webkit-scrollbar]:hidden">
                  <span
                    aria-live="polite"
                    className="inline-flex shrink-0 items-center gap-2 rounded-full bg-brand-500/10 px-3 py-2 text-xs font-semibold text-brand-700 dark:text-brand-300"
                  >
                    <svg
                      aria-hidden="true"
                      className="h-4 w-4"
                      viewBox="0 0 20 20"
                      fill="none"
                    >
                      <path
                        d="m4.5 10.5 3.5 3.5 7.5-8"
                        stroke="currentColor"
                        strokeWidth="1.8"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      />
                    </svg>
                    {selectedCount} Selected
                  </span>
                  <span aria-hidden="true" className="h-7 w-px shrink-0 bg-gray-200 dark:bg-white/15" />

                  {permission?.isDeleted && (
                    <button
                      type="button"
                      name="delete"
                      onClick={handleClick}
                      className="inline-flex shrink-0 items-center gap-2 rounded-xl border border-red-500/20 bg-red-500/10 px-3 py-2 text-xs font-medium text-red-300 transition hover:border-red-500/40 hover:bg-red-600/30 hover:text-red-200"
                    >
                      <TrashBinIcon className="h-4 w-4" />
                      Delete
                    </button>
                  )}

                  {hasMoreActions &&
                    action?.map((item) => (
                      <button
                        key={item.lable}
                        type="button"
                        name={item.lable}
                        onClick={handleClick}
                        className="inline-flex shrink-0 items-center gap-2 rounded-xl border border-gray-200 bg-[var(--app-panel-bg)] px-3 py-2 text-xs font-medium text-gray-700 transition hover:border-brand-300 hover:bg-brand-50 hover:text-brand-700 dark:border-gray-700 dark:text-gray-300 dark:hover:border-brand-500/40 dark:hover:bg-brand-500/10 dark:hover:text-brand-300"
                      >
                        {item.icon}
                        {item.buttonName}
                      </button>
                    ))
                  }

                  <button
                    type="button"
                    onClick={() => setSelect([])}
                    className="inline-flex shrink-0 items-center gap-2 rounded-xl px-3 py-2 text-xs font-medium text-gray-600 transition hover:bg-brand-500/10 hover:text-brand-700 dark:text-gray-300 dark:hover:text-brand-300"
                  >
                    <svg
                      aria-hidden="true"
                      className="h-4 w-4"
                      viewBox="0 0 20 20"
                      fill="none"
                    >
                      <path
                        d="m5 5 10 10M15 5 5 15"
                        stroke="currentColor"
                        strokeWidth="1.6"
                        strokeLinecap="round"
                      />
                    </svg>
                    Deselect
                  </button>
                </div>
              )}
            </div>
          </div>,
          document.body,
        )}
    </>
  );
};
