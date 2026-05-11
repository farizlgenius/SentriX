import { createContext, useContext, useState } from "react"
import { IdReport } from "../model/IdReport/IdReport";


interface IdReportContextInterface {
  idReports:IdReport[];
  setIdReports:React.Dispatch<React.SetStateAction<IdReport[]>>;
  addIdeReport:(data:IdReport[]) => void;
  clearIdReport:() => void;
}

// Create Context 
const IdReportContext = createContext<IdReportContextInterface | null>(null);

// Provider
export const IdReportProvider:React.FC<{children:React.ReactNode}> = ({children}) => {
    const [idReports,setIdReports] = useState<IdReport[]>([]);
    const addIdeReport = (data:IdReport[]) => {
      setIdReports(data)
    }
    const clearIdReport = () => {
      setIdReports([]);
    }
    return (
        <IdReportContext.Provider value={{idReports,setIdReports,addIdeReport,clearIdReport}}>
            {children}
        </IdReportContext.Provider>
    );
}

// Custom hook for easy usage
export const useIdReport = () => {
  const ctx = useContext(IdReportContext);
  if (!ctx) throw new Error("useIdReport must be used inside IdReportProvider");
  return ctx;
};