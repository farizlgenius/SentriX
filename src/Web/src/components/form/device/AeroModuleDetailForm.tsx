import { PropsWithChildren, SetStateAction, useEffect, useState } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { StatusDto } from "../../../model/StatusDto";
import SignalRService from "../../../services/SignalRService";
import { SignalRTopic } from "../../../constants/signalr-constant";
import Badge from "../../ui/badge/Badge";
import { DeviceModuleDto } from "../../../model/Device/DeviceModuleDto";
import { Status } from "../../../enum/Status";
import { BaseTable } from "../../../pages/UiElements/BaseTable";
import { DeviceModuleModel } from "../../../enum/DeviceModuleModel";
import { useAuth } from "../../../context/AuthContext";
import { FeatureId } from "../../../enum/FeatureId";
import { ModuleEndpoint } from "../../../endpoint/ModuleEndpoint";
import { send } from "../../../api/api";
import { InputStatus } from "../../../enum/InputStatus";

interface AeroModuleDetailFormInterface {
  data: DeviceDto;
}

const headers = [
  "Name",
  "Model",
  "Address",
  "Serial Number",
  "Port",
  "Batt",
  "AC",
  "Tamper",
  "Status",
  "Enable",
  "Action"
]

const keys = [
  "name",
  "model",
  "address",
  "serialNumber",
  "port",


]


export const AeroModuleDetailForm: React.FC<
  PropsWithChildren<AeroModuleDetailFormInterface>
> = ({ data }) => {
  const statusDefault:StatusDto[] = data.deviceModules.map((a:DeviceModuleDto) => ({
    guid:a.guid,
    status:Status.Unknown,
    batt:InputStatus.Unknown,
    ac:InputStatus.Unknown,
    tamper:InputStatus.Unknown
  }))
  const [status, setStatus] = useState<StatusDto[]>(statusDefault);
  const [refresh, setRefresh] = useState<boolean>(false);
  const toggleRefresh = () => setRefresh(!refresh);
  const [select, setSelect] = useState<DeviceModuleDto[]>([]);
  const { filterPermission, token } = useAuth();
  

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {

  };

  const fetchStatus = async (guid: string) => {
    var res = await send.get(ModuleEndpoint.STATUS(guid));
    console.log(res);
  };

  const handleEdit = (item: DeviceModuleDto) => { }
  const handleInfo = (item: DeviceModuleDto) => { }
  const handleRemove = (item: DeviceModuleDto) => { }

  const renderOptional = (
    item: DeviceDto,
    statusDto: StatusDto[],
    index: number,
  ) => {
    return [
      <TableCell
      key={index + 1}
      className="text-center">

        <Badge
          size="sm"
          color={
            status.find((x) => x.guid == item.guid)?.batt == InputStatus.Active
              ? "success"
              : "error"
          }
        >
          {status.find((x) => x.guid == item.guid)?.batt == InputStatus.Active ? "Active" :
           InputStatus[status.find((x) => x.guid == item.guid)?.batt ?? InputStatus.Unknown]}
        </Badge>
      </TableCell>,
      <TableCell
      key={index + 2} className="text-center">

        <Badge
          size="sm"
          color={
            status.find((x) => x.guid == item.guid)?.ac == InputStatus.Active
              ? "success"
              : "error"
          }
        >
          {status.find((x) => x.guid == item.guid)?.ac == InputStatus.Active ? "Active" :
            InputStatus[status.find((x) => x.guid == item.guid)?.ac ?? InputStatus.Unknown]}
        </Badge>
      </TableCell>,
      <TableCell key={index + 3} className="text-center">

        <Badge
          size="sm"
          color={
            status.find((x) => x.guid == item.guid)?.tamper == InputStatus.Active
              ? "success"
              : "error"
          }
        >
          {status.find((x) => x.guid == item.guid)?.tamper == InputStatus.Active ? "Active" :
            InputStatus[status.find((x) => x.guid == item.guid)?.tamper ?? InputStatus.Unknown]}
        </Badge>
      </TableCell>,

      <TableCell
        key={index + 4}
        className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
      >
        <Badge
          size="sm"
          color={
            statusDto.find((statusItem) => statusItem.guid === item.guid)
              ?.status == Status.Online
              ? "success"
              : "error"
          }
        >
          {statusDto.find((statusItem) => statusItem.guid === item.guid)?.status == Status.Online
            ? "Online"
            : Status[statusDto.find((statusItem) => statusItem.guid === item.guid)?.status ?? Status.Unknown]}
        </Badge>
      </TableCell>,
    ];
  };

  {
    /* UseEffect */
  }
  useEffect(() => {
    const initSignalR = async () => {
      const connection = SignalRService.getConnection();
      if (!connection) return;

      connection.on(SignalRTopic.MODULE_STATUS, (status: StatusDto) => {
        console.log("Received realtime update:", status);
        setStatus((prev) =>
          prev.map((a) =>
            a.guid == status.guid
              ? {
                ...a,
                status: status.status,
                ac: status.ac,
                batt: status.batt,
                tamper: status.tamper,
              }
              : {
                // scpIp:ScpIp,
                // cpNumber:first,
                // status:status[0]
                ...a,
              },
          ),
        );
      });
      try{
        await SignalRService.joinGroup(SignalRTopic.MODULE_STATUS);
      }catch(err){
        console.error("Subscribe error:", err);
      }
      
      
    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.MODULE_STATUS);
    };
  }, [refresh, token]);

  useEffect(() => {
    data.deviceModules.map((a:DeviceModuleDto) => {
      fetchStatus(a.guid);
    })
  },[])

  


  return (
    <>
      <BaseTable<DeviceModuleDto>
        headers={headers}
        keys={keys}
        status={status}
        data={data.deviceModules}
        permission={filterPermission(FeatureId.device)}
        onInfo={handleInfo}
        onEdit={handleEdit}
        onRemove={handleRemove}
        onClick={function (e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
          throw new Error("Function not implemented.");
        }}
        select={select}
        setSelect={setSelect}
        fetchData={fetchData}
        locationGuid={data.locationGuid}
        renderOptionalComponent={renderOptional}
        specialDisplay={[
          {
            key:"model",
            content: (item, index) => (
                  <TableCell
                    key={index}
                    className="px-4 py-3 text-gray-500 text-center text-theme-sm dark:text-gray-400"
                  >
                    {DeviceModuleModel[item.model]}
                  </TableCell>
                ),
          }
        ]}
      />

  
    </>
  );
};
