import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { send } from "../../api/api";
import { EventEndpoint } from "../../endpoint/EventEndpoint";

import SignalRService from "../../services/SignalRService";

import { PageProp } from "../../model/PageProp";
import { useLocation } from "../../context/LocationContext";
import { TableCell } from "../../components/ui/table";

import { SignalRTopic } from "../../constants/signalr-constant";
import { useAuth } from "../../context/AuthContext";
import {
  AlertIcon,
  AmicoIcon,
  CalenderIcon,
  CancelCircleIcon,
  CheckCircleIcon,
  ModuleIcon,
} from "../../icons";
import { useTheme } from "../../context/ThemeContext";
import { AdapterEventDto } from "../../model/Event/AdapterEventDto";
import { EventCommandStatus } from "../../enum/CommandStatus";
import { Vendor } from "../../enum/Vendor";
import Modals from "../UiElements/Modals";
import { FormField, FormSection } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import { UtilityEndpoint } from "../../endpoint/UitlityEndpoint";
import { CommandLine } from "../../model/Uitlity/AeroCommandDecode";
import { EventTable } from "../../components/tables/Tables/EventTable";
import { usePagination } from "../../context/PaginationContext";

// Define header Table
const headers: string[] = [
  "Send",
  "Name",
  "Mac",
  "Command",
  "Response",
  "Reason",
  "Status",
];

// Define kwy Table
const keys: string[] = [
  "send",
  "name",
  "mac",
  "command",
  "response",
  "reason",
  "status",
];

const AdapterEvent = () => {
  {
    /* Pagination */
  }
  const { locationGuid } = useLocation();
  const { accentColor } = useTheme();
  const { token } = useAuth();
  const [popup, setPopup] = useState<boolean>(false);
  const [decode, setDecode] = useState<CommandLine[]>([]);
  const [body, setBody] = useState<string>("");
  const { pageSize, search, startDate, endDate, setPagination } = usePagination();


  const modalHandleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "close":
        setPopup(false);
        break;
      default:
        break;
    }
  }

  const handleClick = (item: AdapterEventDto) => {
    // const metadata = JSON.parse(item.metadata as string) as AeroMetadata;
    // item.metadata = metadata;
    // setFormType(FormType.UPDATE);
    // setDeviceDto(item);
    fetchCommandDetail(item.body);
    setBody(item.body);
    setPopup(true);
  };


  const switchModuleIcon = (mode: Vendor) => {
    switch (mode) {
      case Vendor.aero:
        return <ModuleIcon className="w-6 h-6" style={{ color: accentColor }} />
      case Vendor.amico:
        return <AmicoIcon className="w-6 h-6" style={{ color: accentColor }} />
      default:
        return <ModuleIcon className="w-6 h-6" style={{ color: accentColor }} />
    }
  }

  {
    /* Event Data */
  }
  const [tableDatas, setTablesData] = useState<AdapterEventDto[]>([]);
  async function fetchData(
    pageNumber: number,
    pageSize: number,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) {
    const res = await send.get(
      EventEndpoint.GET_ADAPTER_PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res);
    if (res.data) {
      setTablesData(res.data.data.items);
      setPagination(res.data.data);
    }
  }

  const fetchCommandDetail = async (command: string) => {
    const res = await send.post(UtilityEndpoint.DECODE_AERO, {
      command: command
    });
    console.log(res);
    setDecode(res.data.data);
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

  const columnWidths: Record<string, string> = {
    timestamp: "w-[20%]",
    name: "w-[15%]",
    code: "w-[15%]",
    remarks: "w-[50%]",
  };

  const getRowStatusClass = (status: EventCommandStatus) => {
    switch (status) {
      case EventCommandStatus.SUCCESSED:
        return `
        border-l-4 border-b-0 border-green-500
        bg-green-50 hover:bg-green-100
        dark:bg-green-500/10 dark:hover:bg-green-500/20
      `;

      case EventCommandStatus.FAILED:
        return `
        border-l-4 border-red-500
        bg-red-50 hover:bg-red-100
        dark:bg-red-500/10 dark:hover:bg-red-500/20
      `;

      default:
        return `
        border-l-4 border-yellow-500
        bg-yellow-50 hover:bg-yellow-100
        dark:bg-yellow-500/10 dark:hover:bg-yellow-500/20
      `;
    }
  };

  return (
    <>

      <PageBreadcrumb pageTitle="Events" />
      {
        popup && (
          <Modals header="Command Detail" handleClickWithEvent={modalHandleClickWithEvent} isWide={false} body={
            <FormSection className="flex flex-col gap-5">
              <FormField>
                <Label>Row</Label>
                <div className={`px-4  text-start text-theme-sm  whitespace-pre-wrap  dark:text-gray-400 text-gray-500`} >
                  {body}
                </div>
              </FormField>
              <FormField>

                <Label>Detail</Label>
                <div className="h-full w-full" style={{ overflowY: 'auto', padding: '8px' }}>
                  {decode.map((item, index) => (
                    <div key={index} className={`px-4  text-start text-theme-sm  whitespace-pre-wrap ${item.color.name.toLocaleLowerCase() == "red" ? "dark:text-red-400 text-red-500" : item.color.name.toLocaleLowerCase() == "green" ? "dark:text-green-400 text-green-500" : "dark:text-gray-400 text-gray-500"}`} >
                      {item.text}
                    </div>
                  ))}
                </div>
              </FormField>
            </FormSection>} />
        )
      }

      <EventTable<AdapterEventDto> tableHeaders={headers} tableDatas={tableDatas} tableKeys={keys} fetchData={fetchData} columnWidths={columnWidths} getRowStatusClass={getRowStatusClass} handleClick={handleClick}
        specialDisplay={[
          {
            key: "name",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                <span className="flex gap-2">
                  {switchModuleIcon(data.vendor)}
                  {/* {<HardwareIcon/>} */}
                  {data.name}
                </span>
              </TableCell>
            ),
          },
          {
            key: "send",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                <span className="flex gap-2">
                  {
                    <CalenderIcon
                      className="w-5 h-5"
                      style={{ color: accentColor }}
                    />
                  }
                  {<>{data.sendAt}</>}
                  {/* {new Intl.DateTimeFormat("en-GB").format(
                          new Date(data.sendAt),
                        )}{" "}
                        {new Date(data.sendAt).toTimeString().split(" ")[0]} */}
                </span>
              </TableCell>
            ),
          },
          {
            key: "received",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                <span className="flex gap-2">
                  {
                    <CalenderIcon
                      className="w-5 h-5"
                      style={{ color: accentColor }}
                    />
                  }
                  {data.receivedAt == null ? <></> : <>{data.receivedAt}</>}
                  {/* {new Intl.DateTimeFormat("en-GB").format(
                          new Date(data.receivedAt),
                        )}{" "}
                        {new Date(data.receivedAt).toTimeString().split(" ")[0]} */}
                </span>
              </TableCell>
            ),
          },
          {
            key: "status",
            content: (data, i) => (
              <TableCell
                key={i}
                className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
              >
                <span className="flex gap-2">
                  {data.status == EventCommandStatus.SUCCESSED ? (
                    <CheckCircleIcon
                      className="w-5 h-5"
                      style={{ color: "green" }}
                    />
                  ) : data.status == EventCommandStatus.FAILED ? (
                    <CancelCircleIcon
                      className="w-5 h-5"
                      style={{ color: "red" }}
                    />
                  ) : (
                    <AlertIcon
                      className="w-5 h-5"
                      style={{ color: accentColor }}
                    />
                  )}
                </span>
              </TableCell>
            ),
          },
        ]}
      />



    </>
  );
};

export default AdapterEvent;
