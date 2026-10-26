import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import Pagination from "../../components/ui/table/Pagination";
import { send } from "../../api/api";
import { EventEndpoint } from "../../endpoint/EventEndpoint";
import { EventDto } from "../../model/Event/EventDto";
import SignalRService from "../../services/SignalRService";
import DatePicker from "../../components/form/date-picker";
import { PageProp } from "../../model/PageProp";
import { useLocation } from "../../context/LocationContext";
import { TableCell } from "../../components/ui/table";
import { Avatar } from "../UiElements/Avatar";
import { SignalRTopic } from "../../constants/signalr-constant";
import { useAuth } from "../../context/AuthContext";
import {
  AmicoIcon,
  CalenderIcon,
  CamIcon,
  ControlIcon,
  DoorIcon,
  EyeIcon,
  ImageIcon,
  ModuleIcon,
  MonitorIcon,
  NotiIcon,
  TimezonIcon,
  UserIcon,
  VideoIcon,
} from "../../icons";
import { EventModule } from "../../enum/EventModule";
import { useTheme } from "../../context/ThemeContext";
import Modals from "../UiElements/Modals";
import { CaptureModalData } from "../../model/Event/CaptureModalData";

// Define header Table
const headers: string[] = ["Date","Name","Actor", "Status", "Remark", "Capture"];

// Define kwy Table
const keys: string[] = ["timestamp","componentName","actor", "eventCode", "remarks", "capture"];

const AlertEvent = () => {
  {
    /* Pagination */
  }
  const defaultValue: CaptureModalData = {
    name: "",
    location: "",
    time: new Date().toString(),
  };
  const { locationGuid, locationList } = useLocation();
  const { accentColor } = useTheme();
  const { token } = useAuth();
  const [search, setSearch] = useState<string | undefined>();
  const [captureModalData, setCaptureModalData] =
    useState<CaptureModalData>(defaultValue);
  const [startDate, setStartDate] = useState<string | undefined>();
  const [endDate, setEndDate] = useState<string | undefined>();
  const [pageSize, setPageSize] = useState<number>(10);
  const [capture, setCapture] = useState<string>("");
  const [captureModal, setCaptureModal] = useState<boolean>(false);
  const [pagination, setPagination] = useState<PageProp>({
    page: 0,
    pageSize: 0,
    totalItems: 0,
    totalPages: 0,
  });
  

 

  const switchModuleIcon = (mod: string) =>
    ({
      [EventModule.Device.toString()]: (
        <ModuleIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Module.toString()]: (
        <ModuleIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.User.toString()]: (
        <UserIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Door.toString()]: (
        <DoorIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Input.toString()]: (
        <MonitorIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Output.toString()]: (
        <ControlIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Timezone.toString()]: (
        <TimezonIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
      [EventModule.Amico.toString()]: (
        <AmicoIcon className="w-6 h-6" style={{ color: accentColor }} />
      ),
    })[mod] ?? null;

  {
    /* Event Data */
  }
  const [tableDatas, setTablesData] = useState<EventDto[]>([]);
  async function fetchData(
    pageNumber: number,
    pageSize: number,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) {
    const res = await send.get(
      EventEndpoint.GET_PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setTablesData(res.data.data.items);
      setPagination(res.data.data);
    }
  }

  useEffect(() => {
    console.log(accentColor);
    const initSignalR = async () => {
      if (!token) return;

      await SignalRService.startConnection();
      const connection = SignalRService.getConnection();
      if (!connection) return;

      connection.on(SignalRTopic.EVENT, () => {
        fetchData(1, pageSize);
      });

      try {
        await SignalRService.joinGroup(SignalRTopic.EVENT);
      } catch (err) {
        console.error("Subscribe error:", err);
      }

      fetchData(1, pageSize);
    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.EVENT);
    };
  }, [locationGuid]);

  useEffect(() => {
    fetchData(1, pageSize, search, startDate, endDate);
  }, [pageSize, search, startDate, endDate]);

  return (
    <>
      <PageBreadcrumb pageTitle="Alerts" />
      {/* <EventTarget<AlertEve> /> */}
      
    </>
  );
};

export default AlertEvent;
