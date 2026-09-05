import { PropsWithChildren, useEffect, useState } from "react";
import Badge from "../../ui/badge/Badge";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import { useToast } from "../../../context/ToastContext";
import { DeviceEndpoint } from "../../../endpoint/DeviceEndpoint";
import { send } from "../../../api/api";
import { VerifyHardwareDeviceConfigDto } from "../../../model/Device/VerifyHardwareDeviceConfigDto";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto";
import { FormSection } from "../template/FormTemplate";

interface HardwareComponentFormInterface {
  data: DeviceDto | CreateAeroDeviceDto;
}

export const AeroComponentForm: React.FC<
  PropsWithChildren<HardwareComponentFormInterface>
> = ({ data }) => {
  const { toggleToast } = useToast();
  const [deviceConfig, setDeviceConfig] = useState<
    VerifyHardwareDeviceConfigDto[]
  >([]);

  const fetchData = async () => {
    const res = await send.post(DeviceEndpoint.VERIFY_COM(data.mac));
    if (res && res.data.data) {
      setDeviceConfig(res.data.data);
    }
  };

  // useEffect(() => {
  //   fetchData();
  //   var connection = SignalRService.getConnection();

  //   connection.on("SCP.DEVICE_CONFIGURATION", (status: ScpConfiguration) => {
  //     console.log("Received SCP.DEVICE_CONFIGURATION:", status);
  //     setDeviceConfig(status.configurations);
  //   });
  //   return () => {};
  // }, []);

  useEffect(() => {
    fetchData();
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
              <TableCell className="text-center">Components</TableCell>
              <TableCell className="text-center">Mismatch Record</TableCell>
              <TableCell className="text-center">Status</TableCell>
              <TableCell className="text-center">Action</TableCell>
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            {deviceConfig.map((a: VerifyHardwareDeviceConfigDto, i: number) => (
              <TableRow key={i}>
                <TableCell className="text-center">{a.componentName}</TableCell>
                <TableCell className="text-center">
                  {a.nMismatchRecord}
                </TableCell>
                <TableCell className="text-center">
                  {a.isUpload ? (
                    <Badge color="error">Upload Require</Badge>
                  ) : (
                    <Badge color="success">Sync</Badge>
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
