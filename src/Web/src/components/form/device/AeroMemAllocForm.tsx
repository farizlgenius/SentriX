import { PropsWithChildren, useEffect, useState } from "react";
import Badge from "../../ui/badge/Badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import { MemoryDto as MemoryDto } from "../../../model/Device/MemoryDto";
import { send } from "../../../api/api";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { useToast } from "../../../context/ToastContext";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto";
import { FormSection } from "../template/FormTemplate";
import SignalRService from "../../../services/SignalRService";
import { SignalRTopic } from "../../../constants/signalr-constant";
import { useAuth } from "../../../context/AuthContext";

interface HardwareMemAllocFormInterface {
  data: DeviceDto;
}



export const AeroMemAllocForm: React.FC<
  PropsWithChildren<HardwareMemAllocFormInterface>
> = ({ data }) => {
  const { token } = useAuth();
  const [memAllocs, setMemAllocs] = useState<MemoryDto[]>([]);

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
          setMemAllocs(reports);
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
    <FormSection
      overall="Components Detail"
      title="Device Component Information"
      description="Module detail and information that connected to device."
    >
      <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)]">
        <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] ">
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
                  {a.record == a.driverRecord ? (
                    <Badge color="success">Sync</Badge>
                  ) : (
                    <Badge color="error">Not Sync</Badge>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </FormSection>
  );
};
