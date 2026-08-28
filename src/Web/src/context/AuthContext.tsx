import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import { AuthEndpoint } from "../endpoint/AuthEndpoint";
import { useLoading } from "./LoadingContext";
import { useToast } from "./ToastContext";
import { LocationEndpoint } from "../endpoint/LocationEndpoint";
import {
  clearAccessToken,
  getAccessToken,
  send,
  setAccessToken,
} from "../api/api";
import Helper from "../utility/Helper";
import { useLocation } from "./LocationContext";
import { LocationDto } from "../model/Location/LocationDto";
import { AuthToast } from "../model/ToastMessage";
import SignalRService from "../services/SignalRService";
import { ModulePermissionDto } from "../model/Role/ModulePermissionDto";
import { FeaturePermissionDto } from "../model/Role/FeaturePermissionDto";
import { Options } from "../model/Options";

interface AuthContextType {
  isAuthenticated: boolean;
  loading: boolean;
  signIn: (username: string, password: string) => Promise<boolean>;
  signOut: () => Promise<boolean>;
  filterPermission: (featureId: number) => FeaturePermissionDto | undefined;
  isAllowedPermission: (moduleId: number, featureId: number) => boolean;
  fetchMeTrigger: () => void;
  token: string;
  isAuthReady: boolean;
  selectedModule: number;
  setSelectedModule: React.Dispatch<React.SetStateAction<number>>;
  modules: Options[];
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

let isRefreshing = false;
let refreshPromise: Promise<boolean> | null = null;
// let permissions: PermissionDto[] = [];

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const { loading, setLoading } = useLoading();
  const [fetch, setFetch] = useState<boolean>(false);
  const {
    setLocationGuid,
    setLocationList,
    setLocationName,
    SetLocationOption,
  } = useLocation();
  const { toggleToast } = useToast();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isAuthReady, setIsAuthReady] = useState(false);
  const [token, setToken] = useState<string>("");
  const [permissions, setPermission] = useState<ModulePermissionDto[]>([]);
  const [modules, setModules] = useState<Options[]>([]);
  const [selectedModule, setSelectedModule] = useState<number>(1);

  const fetchMeTrigger = () => setFetch(!fetch);

  const doRefresh = async () => {
    if (isRefreshing && refreshPromise) return refreshPromise;
    isRefreshing = true;
    refreshPromise = (async () => {
      try {
        const res = await send.post(AuthEndpoint.REFRESH, {
          refresh: "",
        });
        if (res?.status !== 200) return false;
        setAccessToken(res.data.data.accessToken);
        setToken(res.data.data.accessToken);
        SignalRService.setToken(res.data.data.accessToken);

        await SignalRService.stopConnection();
        await SignalRService.startConnection();
        return true;
      } catch {
        return false;
      } finally {
        isRefreshing = false;
        refreshPromise = null;
        setIsAuthReady(true);
      }
    })();
    return refreshPromise;
  };

  const fetchMe = useCallback(async () => {
    if (!getAccessToken()) return false;
    const res = await send.get(AuthEndpoint.ME);
    if (res?.status !== 200) return false;
    setModules([]);
    fetchLocation(res.data.data.locationGuids); // [1]
    setPermission(res.data.data.permissions);
    setSelectedModule(res.data.data.permissions[0].id);
    res.data.data.permissions.map((p: ModulePermissionDto) => {
      setModules((prev) =>
        p.isEnabled
          ? [
              ...prev,
              {
                value: p.id,
                label: p.name,
              },
            ]
          : [...prev],
      );
    });
    setIsAuthenticated(true);
    return true;
  }, [fetch]);

  const fetchLocation = useCallback(async (locationGuids: string[]) => {
    if (!getAccessToken()) return false;
    const dto = locationGuids;
    const res = await send.post(LocationEndpoint.GET_RANGE, dto);
    console.log(res);
    const locs: LocationDto[] = res.data.data;
    setLocationList(locs);
    SetLocationOption(
      locs.map((d) => ({
        label: d.name,
        value: d.guid,
        description: d.description,
        isTaken: false,
      })),
    );
    if (locs.length > 0) {
      setLocationName(locs[0].name);
      setLocationGuid(locs[0].guid);
    }
  }, []);

  useEffect(() => {
    (async () => {
      const ok = await doRefresh();
      console.log(ok);
      if (ok) {
        await fetchMe();
      } else {
        console.log(1);
        setIsAuthenticated(false);
      }
      setLoading(false);
    })();
  }, [fetchMe]);

  const signIn = useCallback(
    async (username: string, password: string) => {
      setLoading(true);
      const body = new FormData();
      body.append("username", username);
      body.append("password", password);
      const res = await send.post(AuthEndpoint.LOGIN, body);
      setLoading(false);
      if (!Helper.handleToastByResCode(res, AuthToast.LOGIN, toggleToast)) {
        return false;
      }
      console.log(res);
      setAccessToken(res.data.data.accessToken);
      setToken(res.data.data.accessToken);
      SignalRService.setToken(res.data.data.accessToken);
      await fetchMe();
      return true;
    },
    [fetchMe],
  );

  const signOut = useCallback(async () => {
    const res = await send.post(AuthEndpoint.LOGOUT, {
      refresh: "",
    });
    console.log(res);
    if (res.status == 200) {
      clearAccessToken();
      setIsAuthenticated(false);
    }
    return res.status == 200;
  }, []);

  const filterPermission = useCallback(
    (featureId: number) => {
      console.log(selectedModule);
      console.log(permissions);
      return permissions
        .find((s) => s.id == selectedModule)
        ?.featurePermission.find((x) => x.id == featureId);
    },
    [permissions],
  );

  const isAllowedPermission = useCallback(
    (moduleId: number, featureId: number) => {
      return !(
        permissions
          .find((x) => x.id == moduleId)
          ?.featurePermission?.find((x) => x.id == featureId)?.isEnabled ??
        false
      );
    },
    [permissions],
  );

  useEffect(() => {
    if (!token) return;

    const startSignalR = async () => {
      await SignalRService.startConnection();
      console.log("✅ Global SignalR connected");
    };

    startSignalR();
  }, [token]);

  return (
    <AuthContext.Provider
      value={{
        isAuthReady,
        token,
        isAuthenticated,
        isAllowedPermission,
        loading,
        signIn,
        signOut,
        filterPermission,
        fetchMeTrigger,
        selectedModule,
        setSelectedModule,
        modules,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
}
