import React, { PropsWithChildren } from "react";
import { ActionButton } from "../../../model/ActionButton";
import { usePagination } from "../../../context/PaginationContext";

interface SearchProp {
  onClick?: (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  altrAction?: ActionButton[];
}

const Search: React.FC<PropsWithChildren<SearchProp>> = ({
  onClick,
  altrAction,
}) => {
  const { setSearch } = usePagination();

  return (
    <div className="flex flex-col gap-4 py-1 sm:flex-row sm:items-center sm:justify-between">
      <div className="flex flex-wrap items-center gap-2">
        <div className="inline-flex flex-wrap items-center gap-2">
          {altrAction?.map((item) => (
            <button
              key={item.lable}
              onClick={onClick}
              name={item.lable}
              type="button"
              className="inline-flex items-center gap-2 rounded-lg border border-gray-200 bg-[var(--app-panel-bg)] px-3.5 py-2.5 text-sm font-medium text-gray-700 shadow-theme-xs transition hover:border-brand-300 hover:bg-brand-50 hover:text-brand-700 dark:border-gray-700 dark:text-gray-300 dark:hover:border-brand-500/40 dark:hover:bg-brand-500/10 dark:hover:text-brand-300"
            >
              {item.icon}
              {item.buttonName}
            </button>
          ))}
        </div>
      </div>
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
        <div className="relative w-full sm:w-auto">
          <span aria-hidden="true" className="absolute left-3.5 top-1/2 -translate-y-1/2 text-gray-400 dark:text-gray-500">
            <svg
              className="h-[18px] w-[18px] fill-current"
              viewBox="0 0 20 20"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                fillRule="evenodd"
                clipRule="evenodd"
                d="M3.04199 9.37363C3.04199 5.87693 5.87735 3.04199 9.37533 3.04199C12.8733 3.04199 15.7087 5.87693 15.7087 9.37363C15.7087 12.8703 12.8733 15.7053 9.37533 15.7053C5.87735 15.7053 3.04199 12.8703 3.04199 9.37363ZM9.37533 1.54199C5.04926 1.54199 1.54199 5.04817 1.54199 9.37363C1.54199 13.6991 5.04926 17.2053 9.37533 17.2053C11.2676 17.2053 13.0032 16.5344 14.3572 15.4176L17.1773 18.238C17.4702 18.5309 17.945 18.5309 18.2379 18.238C18.5308 17.9451 18.5309 17.4703 18.238 17.1773L15.4182 14.3573C16.5367 13.0033 17.2087 11.2669 17.2087 9.37363C17.2087 5.04817 13.7014 1.54199 9.37533 1.54199Z"
                fill=""
              ></path>
            </svg>
          </span>
          <input
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Search..."
            className="h-10 w-full rounded-lg border border-gray-200 bg-transparent py-2 pl-10 pr-3.5 text-sm text-gray-800 shadow-none placeholder:text-gray-400 transition-colors focus:border-brand-400 focus:outline-hidden focus:ring-2 focus:ring-brand-500/10 dark:border-gray-700 dark:bg-transparent dark:text-white/90 dark:placeholder:text-white/30 dark:focus:border-brand-500 sm:w-[260px] xl:w-[300px]"
            type="text"
          />
        </div>
      </div>
    </div>
  );
};

export default Search;
