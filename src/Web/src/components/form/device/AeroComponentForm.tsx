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
import { DeviceComponentConfigurationDto } from "../../../model/Device/DeviceComponentConfigurationDto";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { CreateAeroDeviceDto } from "../../../model/Device/CreateAeroDeviceDto";
import { FormSection } from "../template/FormTemplate";
import Button from "../../ui/button/Button";
import { UploadIcon } from "../../../icons";

interface HardwareComponentFormInterface {
  data: DeviceDto | CreateAeroDeviceDto;
}

const defaultDeviceComponentConfig:DeviceComponentConfigurationDto[] = [
  {
    id:1,
    component:"Module",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:2,
    component:"Door",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:3,
    component:"Input",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:4,
    component:"Output",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:5,
    component:"MonitorGroup",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:6,
    component:"Area",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:7,
    component:"TimeZone",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:8,
    component:"AccessGroup",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
   {
    id:9,
    component:"Holiday",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:10,
    component:"Trigger",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  },
  {
    id:1,
    component:"Prcedure",
    total:0,
    uploaded:0,
    pending:0,
    isSynced:true
  }
]



export const AeroComponentForm: React.FC<
  PropsWithChildren<HardwareComponentFormInterface>
> = ({ data }) => {
  const { toggleToast } = useToast();
  const [deviceConfig, setDeviceConfig] = useState<
    DeviceComponentConfigurationDto[]
  >(defaultDeviceComponentConfig);

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
    // <FormSection
    //   overall="Components Detail"
    //   title="Device Component Information"
    //   description="Module detail and information that connected to device."
    // >
    //   <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)]">
    //     <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] ">
    //       <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
    //         <TableRow>
    //           <TableCell className="text-center">Components</TableCell>
    //           <TableCell className="text-center">Total</TableCell>
    //           <TableCell className="text-center">Uploaded</TableCell>
    //           <TableCell className="text-center">Pending</TableCell>
    //           <TableCell className="text-center">Status</TableCell>
    //           <TableCell className="text-center">Action</TableCell>
    //         </TableRow>
    //       </TableHeader>
    //       <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
    //         {deviceConfig.map((a: DeviceComponentConfigurationDto, i: number) => (
    //           <TableRow key={i}>
    //             <TableCell className="text-center">{a.component}</TableCell>
    //             <TableCell className="text-center">
    //               {a.total}
    //             </TableCell>
    //              <TableCell className="text-center">
    //               {a.uploaded}
    //             </TableCell>
    //             <TableCell className="text-center">
    //               {a.pending}
    //             </TableCell>
    //             <TableCell className="text-center">
    //               {a.isSynced ? (
    //                 <Badge color="error">Upload Require</Badge>
    //               ) : (
    //                 <Badge color="success">Sync</Badge>
    //               )}
    //             </TableCell>
    //             <TableCell className="flex justify-center"> 
    //               <Button size="sm" >Upload</Button>
    //             </TableCell>
    //           </TableRow>
    //         ))}
    //       </TableBody>
    //     </Table>
    //   </div>
    // </FormSection>
    <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)]  bg-[var(--app-panel-bg)] ">
          <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
            <TableRow>
              <TableCell className="text-center">Components</TableCell>
              <TableCell className="text-center">Total</TableCell>
              <TableCell className="text-center">Uploaded</TableCell>
              <TableCell className="text-center">Pending</TableCell>
              <TableCell className="text-center">Status</TableCell>
              <TableCell className="text-center">Action</TableCell>
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            {deviceConfig.map((a: DeviceComponentConfigurationDto, i: number) => (
              <TableRow key={i}>
                <TableCell className="text-center">{a.component}</TableCell>
                <TableCell className="text-center">
                  {a.total}
                </TableCell>
                 <TableCell className="text-center">
                  {a.uploaded}
                </TableCell>
                <TableCell className="text-center">
                  {a.pending}
                </TableCell>
                <TableCell className="text-center">
                  {a.isSynced ? (
                    <Badge color="error">Upload</Badge>
                  ) : (
                    <Badge color="success">Synced</Badge>
                  )}
                </TableCell>
                 <TableCell className="text-center">
                      <button
                        type="button"
                        onClick={(e) => {
                          e.stopPropagation();
                          // onRemove(data);
                        }}
                        className={`inline-flex items-center justify-center rounded-lg p-1 transition-all duration-200 cursor-pointer text-brand-600 hover:bg-brand-50 hover:text-brand-700 active:scale-95`}
                      >
                        <UploadIcon className="h-5 w-5" />
                      </button>
                    </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
  );
};
