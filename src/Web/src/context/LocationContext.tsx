import { createContext, useContext, useState } from "react";
import { LocationDto } from "../model/Location/LocationDto";
import { Options } from "../model/Options";
interface LocationContextInterface {
  locationGuid: string;
  locationName: string;
  setLocationGuid: React.Dispatch<React.SetStateAction<string>>;
  setLocationName: React.Dispatch<React.SetStateAction<string>>;
  locationList: LocationDto[];
  locationOption: Options[];
  SetLocationOption: React.Dispatch<React.SetStateAction<Options[]>>;
  setLocationList: React.Dispatch<React.SetStateAction<LocationDto[]>>;
}

const LocationContext = createContext<LocationContextInterface | null>(null);

export const LocationProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [locationGuid, setLocationGuid] = useState<string>("");
  const [locationName, setLocationName] = useState<string>("");
  const [locationList, setLocationList] = useState<LocationDto[]>([]);
  const [locationOption, SetLocationOption] = useState<Options[]>([]);

  return (
    <LocationContext.Provider
      value={{
        locationOption,
        SetLocationOption,
        locationGuid,
        setLocationGuid,
        locationName,
        setLocationName,
        locationList,
        setLocationList,
      }}
    >
      {children}
    </LocationContext.Provider>
  );
};

export const useLocation = () => {
  const ctx = useContext(LocationContext);
  if (!ctx) throw new Error("useLocation must be used inside LocationProvider");
  return ctx;
};
