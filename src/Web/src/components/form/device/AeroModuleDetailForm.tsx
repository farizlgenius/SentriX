import { PropsWithChildren, useEffect, useState } from "react";
import { ModuleIcon } from "../../../icons";
import { Table, TableBody, TableCell, TableHeader, TableRow } from "../../ui/table";
import { send } from "../../../api/api";
import { ModuleEndpoint } from "../../../endpoint/ModuleEndpoint";
import { DeviceDto } from "../../../model/Device/DeviceDto";
import { ModuleDto } from "../../../model/Module/ModuleDto";
import { StatusDto } from "../../../model/StatusDto";
import SignalRService from "../../../services/SignalRService";
import { SignalRTopic } from "../../../constants/signalr-constant";
import Badge from "../../ui/badge/Badge";

interface AeroModuleDetailFormInterface {
      data: DeviceDto;
}


export const AeroModuleDetailForm: React.FC<PropsWithChildren<AeroModuleDetailFormInterface>> = ({ data }) => {
      const [modules, setModules] = useState<ModuleDto[]>([]);
      const [status, setStatus] = useState<StatusDto[]>([]);
      const [refresh, setRefresh] = useState<boolean>(false);
      const toggleRefresh = () => setRefresh(!refresh);

      const fetchModule = async () => {
            const res = await send.get(ModuleEndpoint.GET(data.id));
            setModules(res.data);
            const newStatuses = res.data.map((a: ModuleDto) => ({
                  id: a.id,
                  status: "",
                  tamper: "",
                  ac: "",
                  batt: ""
            }));

            console.log(newStatuses);

            setStatus((prev) => [...prev, ...newStatuses]);

            // Fetch status for each
            res.data.forEach((a: ModuleDto) => {
                  fetchStatus(a.id);
            });
      }

      const fetchStatus = async (moduleId: number) => {
            await send.get(ModuleEndpoint.STATUS(moduleId))
            //Helper.handlePopup(res, PopUpMsg.GET_MODULE_STATUS, showPopup)
      };



      {/* UseEffect */ }
      useEffect(() => {
            const setup = async () => {
                  const connection = SignalRService.getConnection();
                  if (!connection) return;

                  connection.on(SignalRTopic.MODULE_STATUS, (status: StatusDto) => {
                        console.log("Received realtime update:", status);
                        setStatus((prev) =>
                              prev.map((a) =>
                                    a.id == status.id
                                          ? {
                                                ...a,
                                                status: status.status,
                                                ac: status.ac,
                                                batt: status.batt,
                                                tamper: status.tamper
                                          }
                                          : {
                                                // scpIp:ScpIp,
                                                // cpNumber:first,
                                                // status:status[0]
                                                ...a
                                          }
                              )
                        );
                        toggleRefresh();

                  });

                  await SignalRService.joinGroup(SignalRTopic.MODULE_STATUS);
                  fetchModule();
            };

            setup();


            return () => {
                  const connection = SignalRService.getConnection();
                  connection?.off(SignalRTopic.MODULE_STATUS);
            };
      }, []);

      useEffect(() => {

      }, [refresh])

      return (
            <>
                  <div className="flex flex-col gap-4 rounded-[20px]  bg-[var(--app-panel-bg)] p-5 lg:flex-row lg:items-center lg:justify-between">
                        <div>
                              <h3 className="text-lg font-semibold text-gray-900 dark:text-white">Modules Detail</h3>
                              <p className="mt-1 max-w-2xl text-sm text-gray-500 dark:text-gray-400">Name the location, assign its country, and add a short description.</p>
                        </div>

                  </div>
                  <>
                        <div className="rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)]">
                              <Table className="border-separate border-spacing-y-4 overflow-hidden rounded-2xl border border-[var(--app-panel-border)] ">
                                    <TableHeader className="h-10 items-center gap-3 bg-[var(--app-panel-muted)] px-4 py-3 text-[11px] font-semibold uppercase tracking-[0.12em] text-gray-400">
                                          <TableRow >
                                                <TableCell className="text-center">Type</TableCell>
                                                <TableCell className="text-center">Name</TableCell>
                                                <TableCell className="text-center">Model</TableCell>
                                                <TableCell className="text-center">Firmware</TableCell>
                                                <TableCell className="text-center">Serial Number</TableCell>
                                                <TableCell className="text-center">Port</TableCell>
                                                <TableCell className="text-center">BATT</TableCell>
                                                <TableCell className="text-center">AC</TableCell>
                                                <TableCell className="text-center">TAMPER</TableCell>
                                                <TableCell className="text-center">Status</TableCell>
                                                <TableCell className="text-center">Action</TableCell>
                                          </TableRow>
                                    </TableHeader>
                                    <TableBody >
                                          {modules.map((m: ModuleDto) => (
                                                <TableRow >
                                                      <TableCell className="flex justify-center">
                                                            <div className="flex h-14 w-14 items-center justify-center rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] text-gray-700 dark:text-gray-200">
                                                                  <ModuleIcon className="text-2xl" />
                                                            </div>
                                                      </TableCell>
                                                      <TableCell className="text-center">{m.name}</TableCell>
                                                      <TableCell className="text-center">{m.model}</TableCell>
                                                      <TableCell className="text-center">{m.fw}</TableCell>
                                                      <TableCell className="text-center">{m.serialNumber}</TableCell>
                                                      <TableCell className="text-center">{m.port == 0 ? "Internal" : m.port == 1 ? "PORT 1" : m.port == 2 ? "PORT 2" : "NONE"}</TableCell>
                                                      <TableCell className="text-center"> <Badge
                                                            size="sm"
                                                            color={status.find(x => x.id == m.id)?.batt == "Active"
                                                                  ? "success"
                                                                  : "error"}
                                                      >
                                                            {status.find(x => x.id == m.id)?.batt}
                                                      </Badge></TableCell>
                                                      <TableCell className="text-center"> <Badge
                                                            size="sm"
                                                            color={status.find(x => x.id == m.id)?.ac == "Active"
                                                                  ? "success"
                                                                  : "error"}
                                                      >
                                                            {status.find(x => x.id == m.id)?.ac}
                                                      </Badge></TableCell>
                                                      <TableCell className="text-center">
                                                            <Badge
                                                                  size="sm"
                                                                  color={status.find(x => x.id == m.id)?.tamper == "Active"
                                                                        ? "success"
                                                                        : "error"}
                                                            >
                                                                  {status.find(x => x.id == m.id)?.tamper}
                                                            </Badge>
                                                      </TableCell>
                                                      <TableCell className="text-center">
                                                            <Badge
                                                                  size="sm"
                                                                  color={status.find(x => x.id == m.id)?.status == "Online"
                                                                        ? "success"
                                                                        : "error"}
                                                            >
                                                                  {status.find(x => x.id == m.id)?.status}
                                                            </Badge>
                                                      </TableCell>
                                                </TableRow>


                                          ))}

                                    </TableBody>
                              </Table>
                        </div>

                  </>
            </>
      )
}

