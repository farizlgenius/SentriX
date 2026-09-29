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
import { Components, DeviceComponentDto } from "../../../model/Device/DeviceComponentDto";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { UploadIcon } from "../../../icons";
import { EntityType } from "../../../enum/EntityType";

interface HardwareComponentFormInterface {
  data: DeviceDto;
}

const defaultDeviceComponentConfig:Components[] = [
  {
    type:EntityType.DeviceModule,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
  {
    type:EntityType.User,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.TimeZone,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Holiday,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Door,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Turnstile,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Group,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Input,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Output,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Procedure,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Trigger,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.MonitorGroup,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  },
   {
    type:EntityType.Area,
    total:0,
    uploaded:0,
    remain:0,
    isSynced:false
  }
]



export const AeroComponentForm: React.FC<
  PropsWithChildren<HardwareComponentFormInterface>
> = ({ data }) => {
  const { toggleToast } = useToast();
  const [deviceConfig, setDeviceConfig] = useState<
    Components[]
  >(defaultDeviceComponentConfig);

  const fetchData = async () => {
  const res = await send.get(DeviceEndpoint.GET_COMPONENT(data.guid));
  const fetchedComponents = res.data.data.components || [];

  setDeviceConfig(prev => 
    prev.map(item => {
      // Look for a matching component in the API response based on 'type'
      const match = fetchedComponents.find((c:Components) => c.type === item.type);
      
      // If a match is found, merge the existing item with the new data
      if (match) {
        return {
          ...item,
          ...match // The properties from 'match' will overwrite the old ones
        };
      }
      
      // If no match is found in the API response, leave the item as is
      return item;
    })
  );
};

  useEffect(() => {
    fetchData();
    return () => {};
  }, []);

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)]  bg-[var(--app-panel-bg)] ">
          <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
            <TableRow>
              <TableCell className="text-center">Components</TableCell>
              <TableCell className="text-center">Total</TableCell>
              <TableCell className="text-center">Uploaded</TableCell>
              <TableCell className="text-center">Remain</TableCell>
              <TableCell className="text-center">Status</TableCell>
              <TableCell className="text-center">Action</TableCell>
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            {deviceConfig.map((a: Components, i: number) => (
              <TableRow key={i}>
                <TableCell className="text-center">{EntityType[a.type]}</TableCell>
                <TableCell className="text-center">
                  {a.total}
                </TableCell>
                 <TableCell className="text-center">
                  {a.uploaded}
                </TableCell>
                <TableCell className="text-center">
                  {a.remain}
                </TableCell>
                <TableCell className="text-center">
                  {a.isSynced ? (
                    <Badge color="success">Synced</Badge>
                  ) : (
                    <Badge color="error">Upload</Badge>
                  )}
                </TableCell>
                 <TableCell className="text-center">
                      <button
                        disabled={a.isSynced}
                        type="button"
                        onClick={(e) => {
                          e.stopPropagation();
                          // onRemove(data);
                        }}
                        className={`${a.isSynced ? "cursor-not-allowed text-red-600" : "text-brand-600 cursor-pointer hover:bg-brand-50 hover:text-brand-700 active:scale-95"} inline-flex items-center justify-center rounded-lg p-1 transition-all duration-200`}
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
