import { PropsWithChildren, useEffect, useState } from "react";
import { ModuleIcon, TrashBinIcon } from "../../../icons";
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
import { FormSection } from "../template/FormTemplate";
import { DeviceModuleDto } from "../../../model/Device/DeviceModuleDto";
import { DeviceModuleModel } from "../../../enum/DeviceModuleModel";

interface AeroModuleDetailFormInterface {
  data: DeviceDto;
}

export const AeroModuleDetailForm: React.FC<
  PropsWithChildren<AeroModuleDetailFormInterface>
> = ({ data }) => {
  const [status, setStatus] = useState<StatusDto[]>([]);
  const [refresh, setRefresh] = useState<boolean>(false);
  const toggleRefresh = () => setRefresh(!refresh);

  // const fetchStatus = async (moduleId: number) => {
  //   await send.get(ModuleEndpoint.STATUS(moduleId));
  //   //Helper.handlePopup(res, PopUpMsg.GET_MODULE_STATUS, showPopup)
  // };

  {
    /* UseEffect */
  }
  useEffect(() => {
    const setup = async () => {
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
        toggleRefresh();
      });

      await SignalRService.joinGroup(SignalRTopic.MODULE_STATUS);
      // fetchModule();
    };

    setup();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.MODULE_STATUS);
    };
  }, []);

  useEffect(() => {}, [refresh]);

  return (
    <>
      <FormSection
        overall="Modules Detail"
        title="Modules Information"
        description="Module detail and information that connected to device."
      >
        <>
          <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)]">
            <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] ">
              <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
                <TableRow>
                  <TableCell className="text-center">Type</TableCell>
                  <TableCell className="text-center">Name</TableCell>
                  <TableCell className="text-center">Model</TableCell>
                  <TableCell className="text-center">Address</TableCell>
                  <TableCell className="text-center">Firmware</TableCell>
                  <TableCell className="text-center">Serial Number</TableCell>
                  <TableCell className="text-center">Port</TableCell>
                  <TableCell className="text-center">Batt</TableCell>
                  <TableCell className="text-center">AC</TableCell>
                  <TableCell className="text-center">Tamper</TableCell>
                  <TableCell className="text-center">Status</TableCell>
                  <TableCell className="text-center">Action</TableCell>
                </TableRow>
              </TableHeader>
              <TableBody>
                {data.deviceModules.map((m: DeviceModuleDto) => (
                  <TableRow>
                    <TableCell className="flex justify-center">
                      <div className="flex h-14 w-14 items-center justify-center rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] text-gray-700 dark:text-gray-200">
                        <ModuleIcon className="text-2xl" />
                      </div>
                    </TableCell>
                    <TableCell className="text-center">{m.name}</TableCell>
                    <TableCell className="text-center">
                      {DeviceModuleModel[m.model]}
                    </TableCell>
                    <TableCell className="text-center">{m.address}</TableCell>
                    <TableCell className="text-center">{m.firmware}</TableCell>
                    <TableCell className="text-center">
                      {m.serialNumber}
                    </TableCell>
                    <TableCell className="text-center">
                      {m.port == "0"
                        ? "Internal"
                        : m.port == "1"
                          ? "PORT 1"
                          : m.port == "2"
                            ? "PORT 2"
                            : "NONE"}
                    </TableCell>
                    <TableCell className="text-center">
                      {" "}
                      <Badge
                        size="sm"
                        color={
                          status.find((x) => x.guid == m.guid)?.batt == "Active"
                            ? "success"
                            : "error"
                        }
                      >
                        {status.find((x) => x.guid == m.guid)?.batt ??
                          "Offline"}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-center">
                      {" "}
                      <Badge
                        size="sm"
                        color={
                          status.find((x) => x.guid == m.guid)?.ac == "Active"
                            ? "success"
                            : "error"
                        }
                      >
                        {status.find((x) => x.guid == m.guid)?.ac ?? "Offline"}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-center">
                      <Badge
                        size="sm"
                        color={
                          status.find((x) => x.guid == m.guid)?.tamper ==
                          "Active"
                            ? "success"
                            : "error"
                        }
                      >
                        {status.find((x) => x.guid == m.guid)?.tamper ??
                          "Offline"}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-center">
                      <Badge
                        size="sm"
                        color={
                          status.find((x) => x.guid == m.guid)?.status ==
                          "Online"
                            ? "success"
                            : "error"
                        }
                      >
                        {status.find((x) => x.guid == m.guid)?.status ??
                          "Offline"}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-center">
                      <button
                        type="button"
                        onClick={(e) => {
                          e.stopPropagation();
                          // onRemove(data);
                        }}
                        className={`inline-flex items-center justify-center rounded-lg p-1 transition-all duration-200 cursor-pointer text-red-600 hover:bg-red-50 hover:text-red-700 active:scale-95`}
                      >
                        <TrashBinIcon className="h-5 w-5" />
                      </button>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </div>
        </>
      </FormSection>
    </>
  );
};
