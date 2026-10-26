import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { send } from "../../api/api";
import { EventEndpoint } from "../../endpoint/EventEndpoint";
import { EventDto } from "../../model/Event/EventDto";
import SignalRService from "../../services/SignalRService";
import { useLocation } from "../../context/LocationContext";
import { TableCell } from "../../components/ui/table";
import { Avatar } from "../UiElements/Avatar";
import { SignalRTopic } from "../../constants/signalr-constant";
import { useAuth } from "../../context/AuthContext";
import {
  AmicoIcon,
  CalenderIcon,
  ControlIcon,
  DoorIcon,
  ImageIcon,
  ModuleIcon,
  MonitorIcon,
  TimezonIcon,
  UserIcon,
} from "../../icons";
import { EventModule } from "../../enum/EventModule";
import { useTheme } from "../../context/ThemeContext";
import Modals from "../UiElements/Modals";
import { CaptureModalData } from "../../model/Event/CaptureModalData";
import { usePagination } from "../../context/PaginationContext";
import { EventTable } from "../../components/tables/Tables/EventTable";

// Define header Table
const headers: string[] = ["Date", "Name", "Actor", "Status", "Remark", "Capture"];

// Define kwy Table
const keys: string[] = ["timestamp", "componentName", "actor", "eventCode", "remarks", "capture"];

const Event = () => {
  {
    /* Pagination */
  }
  const defaultValue: CaptureModalData = {
    name: "",
    location: "",
    time: new Date().toString(),
  };
  const { locationGuid, locationList } = useLocation();
  const { pageSize, setPagination } = usePagination();
  const { accentColor } = useTheme();
  const { token } = useAuth();
  const [captureModalData, setCaptureModalData] =
    useState<CaptureModalData>(defaultValue);
  const [capture, setCapture] = useState<string>("");
  const [captureModal, setCaptureModal] = useState<boolean>(false);


  const handleClickSeeImage = (capture: EventDto) => {
    fetchCapture(capture.captureImageName);
    setCaptureModalData({
      name: capture.componentName,
      location:
        locationList.find((x) => x.guid == capture.locationGuid)?.name ?? "",
      // time: formatDate(capture.timestamp),
      time: capture.timestamp.toISOString()
    });
  };

  function formatDate(dateString: string): string {
    return new Date(dateString)
      .toLocaleString("en-GB", {
        day: "2-digit",
        month: "short",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: false,
      })
      .replace(",", "");
  }



  const fetchCapture = async (time: string) => {
    var res = await send.getImage(EventEndpoint.CAPTURE(time));
    console.log(res);
    if (res.status === 200 && res.data != null) {
      console.log("image found, using default.");
      const blob = new Blob([res.data], {
        type: res.headers["content-type"] || "image/png",
      });
      setCapture(URL.createObjectURL(blob));
    } else {
      console.log("No image found, using default.");
      setCapture("");
    }
    setCaptureModal(true);
  };

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

  // useEffect(() => {
  //   fetchData(1, pageSize, search, startDate, endDate);
  // }, [pageSize, search, startDate, endDate]);

  return (
    <>
      <PageBreadcrumb pageTitle="Events" />
      {captureModal && (
        <Modals
          isWide
          header="Unidentified Person"
          handleClickWithEvent={() => setCaptureModal(false)}
          body={
            <Modals
              isWide
              header="📷 Unidentified Person"
              handleClickWithEvent={() => setCaptureModal(false)}
              body={
                <div className="space-y-6">
                  {/* Image + Detail */}
                  <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                    {/* Image */}
                    <div>
                      <img
                        src={capture}
                        className="w-full h-[480px] object-cover rounded-2xl border border-gray-200 shadow"
                      />
                    </div>

                    {/* Detail */}
                    <div className="flex flex-col justify-between">
                      <div>
                        <div className="mb-6">
                          <span className="inline-flex items-center rounded-full bg-red-100 text-red-700 px-3 py-1 text-sm font-medium">
                            🔴 Not Identified
                          </span>
                        </div>

                        <div className="space-y-4">
                          <div className="flex justify-between pb-2">
                            <span className="text-gray-500">Device</span>
                            <span className="font-semibold">
                              {captureModalData.name}
                            </span>
                          </div>

                          <div className="flex justify-between pb-2">
                            <span className="text-gray-500">
                              Location
                            </span>
                            <span>{captureModalData.location}</span>
                          </div>

                          <div className="flex justify-between pb-2">
                            <span className="text-gray-500">
                              Captured At
                            </span>
                            <span>{captureModalData.time}</span>
                          </div>
                        </div>
                      </div>

                      <div className="mt-8 rounded-xl bg-yellow-50 border border-yellow-200 p-4">
                        <div className="flex gap-3">
                          <div className="text-yellow-600 text-2xl">
                            ⚠️
                          </div>

                          <div>
                            <div className="font-semibold text-yellow-800">
                              Unknown Person Detected
                            </div>

                            <div className="text-sm text-yellow-700 mt-1">
                              This captured face does not match any
                              registered person in the system.
                            </div>
                          </div>
                        </div>
                      </div>

                      <div className="flex justify-end gap-3 mt-8">
                        <button
                          onClick={() => setCaptureModal(false)}
                          className="px-5 py-2 rounded-xl border border-gray-300 hover:bg-gray-100"
                        >
                          Close
                        </button>

                        <button className="px-5 py-2 rounded-xl bg-blue-600 text-white hover:bg-blue-700">
                          Register Person
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              }
            />
          }
        />
      )}
      {/* Header */}

      <EventTable<EventDto>
        tableHeaders={headers}
        fetchData={fetchData}
        tableDatas={tableDatas}
        tableKeys={keys}
        specialDisplay={[
          {
            key: "componentName",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                <span className="flex gap-2">
                  {switchModuleIcon(data.module)}
                  {/* {<HardwareIcon/>} */}
                  {data.componentName}
                </span>
              </TableCell>
            ),
          },
          {
            key: "timestamp",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                {/* <span className='flex gap-2'>
                    {<CalenderIcon className="w-5 h-5" />} {new Intl.DateTimeFormat("en-GB").format(new Date(data.dateTime))}  {<TimeIcon className="w-5 h-5" />}  {new Date(data.dateTime).toTimeString().split(" ")[0]}
                  </span> */}
                {/* <span className='flex gap-2'>
                    {<TimeIcon className="w-5 h-5" />} {new Date(data.timestamp).toTimeString().split(" ")[0]}
                  </span> */}
                <span className="flex gap-2">
                  {
                    <CalenderIcon
                      className="w-5 h-5"
                      style={{ color: accentColor }}
                    />
                  }
                  {new Intl.DateTimeFormat("en-GB").format(
                    new Date(data.timestamp),
                  )}{" "}
                  {new Date(data.timestamp).toTimeString().split(" ")[0]}
                </span>
              </TableCell>
            ),
          },
          {
            key: "actor",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                {!data.imageName ||
                  (data.imageName != "" && (
                    <div className="flex items-center gap-2">
                      <div className="cursor-pointer w-7 h-7 overflow-hidden border border-gray-200 rounded-full dark:border-gray-800">
                        <Avatar userId={data.imageName} />
                      </div>
                      {data.actor}
                    </div>
                  ))}
              </TableCell>
            ),
          },
          {
            key: "capture",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                {!data.captureImageName ||
                  (data.captureImageName != "" && (
                    <div
                      onClick={() => handleClickSeeImage(data)}
                      className="cursor-pointer flex items-center gap-2"
                    >
                      <ImageIcon
                        className="w-6 h-6"
                        style={{ color: accentColor }}
                      />
                    </div>
                  ))}
              </TableCell>
            ),
          },
        ]}
      />
    </>
  );
};

export default Event;
