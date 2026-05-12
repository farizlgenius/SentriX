import { useState } from "react";
import { ScanIcon } from "../../icons";
import { useNavigate } from "react-router";
import { useIdReport } from "../../context/IdReportContext";

export const ScanButton: React.FC = () => {
  const {idReports} = useIdReport();
  const navigate = useNavigate();

  return (
    <label
      onClick={() => navigate("/scan")}
      title="Choose accent color"
      aria-label="Choose accent color"
      className="cursor-pointer relative flex h-11 w-11 items-center justify-center rounded-lg border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] text-gray-500 shadow-theme-xs transition-colors hover:border-brand-200 hover:text-brand-500"
    >
      <span
          className={`absolute right-0 top-0.5 z-10 h-2 w-2 rounded-full bg-orange-400 ${
            idReports.length > 0 ? "flex" : "hidden"
          }`}
        >
          <span className="absolute inline-flex w-full h-full bg-orange-400 rounded-full opacity-75 animate-ping"></span>
        </span>
      <ScanIcon className={idReports.length > 0 ? "animate-ping" : ""}/>
    </label>
  );
};
