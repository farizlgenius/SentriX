import { PropsWithChildren, useEffect, useState } from "react";
import Badge from "../../ui/badge/Badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import SignalRService from "../../../services/SignalRService";
import { MemoryDto as MemoryDto } from "../../../model/Device/MemoryDto";
import { send } from "../../../api/api";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { useToast } from "../../../context/ToastContext";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { MemoryAllocateDto } from "../../../model/Device/MemoryAllocateDto";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto";
import { FormSection } from "../template/FormTemplate";

interface HardwareMemAllocFormInterface {
  data: DeviceDto | CreateAeroDeviceDto;
}

export const AeroMemAllocForm: React.FC<
  PropsWithChildren<HardwareMemAllocFormInterface>
> = ({ data }) => {
  const { toggleToast } = useToast();
  const [memAllocs, setMemAllocs] = useState<MemoryDto[]>([]);

  const fetchData = async () => {
    const res = await send.post(DeviceEndpoint.VERIFY_MEM(data.mac));
    // if(Helper.handleToastByResCode(res,ToastMessage.GET_SCP_STRUCTURE,toggleToast)){}
  };

  useEffect(() => {
    fetchData();
    var connection = SignalRService.getConnection();
    connection.on("SCP.MEMORY_ALLOCATE", (status: MemoryAllocateDto) => {
      console.log(status);
      setMemAllocs(status.memories);
    });
    return () => {};
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
              <TableCell className="text-center">Structure Type</TableCell>
              <TableCell className="text-center">HW Record Allocate</TableCell>
              <TableCell className="text-center">nRecSize</TableCell>
              <TableCell className="text-center">HW Active Record</TableCell>
              <TableCell className="text-center">SW Record Allocate</TableCell>
              <TableCell className="text-center">Status</TableCell>
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            {memAllocs.map((a: MemoryDto, i: number) => (
              <TableRow key={i}>
                <TableCell className="text-center">{a.strType}</TableCell>
                <TableCell className="text-center">{a.nRecord}</TableCell>
                <TableCell className="text-center">{a.nRecSize}</TableCell>
                <TableCell className="text-center">{a.nActive}</TableCell>
                <TableCell className="text-center">{a.nSwAlloc}</TableCell>
                <TableCell className="text-center">
                  {a.isSync ? (
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
