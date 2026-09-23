import { PropsWithChildren, useEffect, useState } from "react";
import Badge from "../../ui/badge/Badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import { MemoryDto } from "../../../model/Device/MemoryDto";
import { send } from "../../../api/api";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import SignalRService from "../../../services/SignalRService";
import { SignalRTopic } from "../../../constants/signalr-constant";
import { useAuth } from "../../../context/AuthContext";
import { DeviceConfigurationStatus } from "../../../enum/DeviceConfigurationStatus";

interface HardwareMemAllocFormInterface {
  data: DeviceDto;
}


const defaultMemoryDto:MemoryDto[] = [
  {
    id:1,
    type:"Transaction",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:2,
    type:"TimeZone",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:3,
    type:"Holiday",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:4,
    type:"Msp1 port",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:5,
    type:"SIOs",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:6,
    type:"Monitor Point",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:7,
    type:"Control Point",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:8,
    type:"Access control readers",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:9,
    type:"Access levels",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:10,
    type:"Triggers",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:11,
    type:"Procedures",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:12,
    type:"Monitor point groups",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:13,
    type:"Access areas",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:14,
    type:"Elevator access levels",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:15,
    type:"Cardholder DB",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:20,
    type:"Flash specs",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:21,
    type:"Build sequence number",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:22,
    type:"Flash status",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:23,
    type:"Host config free memory",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:24,
    type:"Card DB free memory",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:26,
    type:"Access Request buffer",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:27,
    type:"Parition Memory free",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
  {
    id:33,
    type:"Web logins",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  },
   {
    id:35,
    type:"File System",
    driverRecord:0,
    record:0,
    recordSize:0,
    active:0,
    status:DeviceConfigurationStatus.unknown
  }
]


export const AeroMemAllocForm: React.FC<
  PropsWithChildren<HardwareMemAllocFormInterface>
> = ({ data }) => {
  const { token } = useAuth();
  const [memAllocs, setMemAllocs] = useState<MemoryDto[]>(defaultMemoryDto);

  const fetchData = async () => {
    await send.get(DeviceEndpoint.VERIFY_MEM(data.guid));
  };

  // useEffect(() => {
  //   fetchData();
  //   var connection = SignalRService.getConnection();
  //   connection.on("SCP.MEMORY_ALLOCATE", (status: MemoryAllocateDto) => {
  //     console.log(status);
  //     setMemAllocs(status.memories);
  //   });
  //   return () => {};
  // }, []);

   useEffect(() => {
      const initSignalR = async () => {
        if (!token) return;
  
        await SignalRService.startConnection();
        const connection = SignalRService.getConnection();
        if (!connection) return;

        connection.on(SignalRTopic.CONFIG, (reports: MemoryDto[]) => {
          console.log(reports);
          setMemAllocs(prev =>
            prev.map(memAlloc => {
              // Look for a matching report with the same ID
              const matchingReport = reports.find(report => report.id === memAlloc.id);

              // If we found a match, merge the old item with the new report data
              if (matchingReport) {
                return { ...memAlloc, ...matchingReport };
              }

              // If no match is found, return the item unchanged
              return memAlloc;
            })
          );
        });

       
        try {
          await SignalRService.joinGroup(SignalRTopic.CONFIG);
        } catch (err) {
          console.error("Subscribe error:", err);
        }
  
  
        fetchData();
      };
  
      initSignalR();
  
      return () => {
        const connection = SignalRService.getConnection();
        connection?.off(SignalRTopic.CONFIG);
      };
    }, []);

  return (
    <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] ">
          <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
            <TableRow>
              <TableCell className="text-center">Type</TableCell>
              <TableCell className="text-center">Device Allocate</TableCell>
              <TableCell className="text-center">Record Size</TableCell>
              <TableCell className="text-center">Active Record</TableCell>
              <TableCell className="text-center">Driver Allocate</TableCell>
              <TableCell className="text-center">Status</TableCell>
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            {memAllocs.map((a: MemoryDto, i: number) => (
              <TableRow key={i}>
                <TableCell className="text-center">{a.type}</TableCell>
                <TableCell className="text-center">{a.record}</TableCell>
                <TableCell className="text-center">{a.recordSize}</TableCell>
                <TableCell className="text-center">{a.active}</TableCell>
                <TableCell className="text-center">{a.driverRecord}</TableCell>
                <TableCell className="text-center">
                  {a.status == DeviceConfigurationStatus.sync ? (
                    <Badge color="success">Sync</Badge>
                  ) : a.status == DeviceConfigurationStatus.info ? (
                    <Badge color="info">Info</Badge>
                  ) : (
                    <Badge color="error">Not Sync</Badge>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
  );
};
