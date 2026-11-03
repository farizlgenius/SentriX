import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseForm } from "../UiElements/BaseForm";
import { BaseTable } from "../UiElements/BaseTable";
import { FormContent } from "../../model/Form/FormContent";
import { MonitorGroupDto, MonitorPointGroupComponent } from "../../model/MonitorGroup/MonitorGroupDto";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { send } from "../../api/api";
import { DoorIcon, MonitorIcon, SensorOffIcon, SensorOnIcon, UnknownIcon } from "../../icons";
import { MonitorGroupForm } from "../../components/form/monitor-group/MonitorGroupForm";
import { useLocation } from "../../context/LocationContext";
import { MonitorGroupEndpoint } from "../../endpoint/MonitorGroupEndpoint";
import Helper from "../../utility/Helper";
import { useToast } from "../../context/ToastContext";
import { ActionButton } from "../../model/ActionButton";
import { MonitorGroupCommandDto } from "../../model/MonitorGroup/MonitorGroupCommandDto";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { MonitorGroupToast } from "../../model/ToastMessage";
import { usePagination } from "../../context/PaginationContext";
import { HttpStatusCode } from "axios";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../components/ui/table";
import { MonitorType } from "../../enum/MonitorType";
import { MonitorGroupComponentStatus, MonitorGroupStatus } from "../../model/MonitorGroup/MonitorGroupStatus";
import { ChangeOfStateEncode } from "../../enum/ChangeOfStateEncode";
import { StatusHelper } from "../../helper/StatusHelper";
import Badge from "../../components/ui/badge/Badge";
import SignalRService from "../../services/SignalRService";
import { SignalRTopic } from "../../constants/signalr-constant";

export const MP_GP_HEADER: string[] = [
  "Name",
  "Device",
  "Component",
  "Enabled",
  "Action",
];
export const MP_GP_KEY: string[] = ["name","deviceName","components"];

const MONITOR_TYPE_LABELS: Record<MonitorType, string> = {
  [MonitorType.UNKNOWN]: "Unknown",
  [MonitorType.MONITOR_POINT]: "Monitor Point",
  [MonitorType.FORCED_OPEN]: "Forced Open",
  [MonitorType.HELD_OPEN]: "Held Open",
};

export const MonitorGroup = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { setPagination } = usePagination();
  const defaultDto: MonitorGroupDto = {
    guid: "",
    name: "",
    components: [],
    deviceGuid: "",
    isActive: false,
    isDefault: false
  };
  const { filterPermission } = useAuth();
  const [form, setForm] = useState<boolean>(false);
  const {
    setConfirmCreate,
    setConfirmRemove,
    setConfirmUpdate,
    setCreate,
    setRemove,
    setUpdate,
    setMessage,
    setInfo,
  } = usePopup();
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [mpGroupsDto, setMpGroupsDto] = useState<MonitorGroupDto[]>([]);
  const [mpGroupDto, setMpGroupDto] = useState<MonitorGroupDto>(defaultDto);
  const [selectedObject, setSelectedObjects] = useState<MonitorGroupDto[]>([]);
  const [refresh, setRefresh] = useState<boolean>(false);
  const [status, setStatus] = useState<MonitorGroupComponentStatus[]>([]);
  const {token} = useAuth();
  const toggleRefresh = () => setRefresh((prev) => !prev);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setMpGroupDto(defaultDto);
        setForm(true);
        break;
      case "delete":
        if (selectedObject.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          var data: string[] = [];
          selectedObject.map(async (a: MonitorGroupDto) => {
            data.push(a.guid);
          });
          var res = await send.post(MonitorGroupEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.DELETE_RANGE,
              toggleToast,
            )
          ) {
            setRemove(false);
            toggleRefresh();
          }
        });
        setRemove(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const res = await send.post(MonitorGroupEndpoint.CREATE, mpGroupDto);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.CREATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(MonitorGroupEndpoint.UPDATE, mpGroupDto);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.UPDATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "cancel":
      case "close":
        setMpGroupDto(defaultDto);
        setForm(false);
        break;
      case "access":
        selectedObject.map(async (a: MonitorGroupDto) => {
          let object: MonitorGroupCommandDto = {
            macAddress: a.mac,
            componentId: a.componentId,
            command: 1,
            arg: 0,
          };
          var res = await send.post(MonitorGroupEndpoint.COMMAND, object);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.COMMAND,
              toggleToast,
            )
          ) {
          }
        });
        console.log(selectedObject);
        break;
      case "override":
        selectedObject.map(async (a: MonitorGroupDto) => {
          let object: MonitorGroupCommandDto = {
            macAddress: a.mac,
            componentId: a.componentId,
            command: 2,
            arg: 0,
          };
          var res = await send.post(MonitorGroupEndpoint.COMMAND, object);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.COMMAND,
              toggleToast,
            )
          ) {
          }
        });
        console.log(selectedObject);
        break;
      case "force":
        selectedObject.map(async (a: MonitorGroupDto) => {
          let object: MonitorGroupCommandDto = {
            macAddress: a.mac,
            componentId: a.componentId,
            command: 3,
            arg: 0,
          };
          var res = await send.post(MonitorGroupEndpoint.COMMAND, object);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.COMMAND,
              toggleToast,
            )
          ) {
          }
        });
        console.log(selectedObject);
        break;
      case "arm":
        selectedObject.map(async (a: MonitorGroupDto) => {
          let object: MonitorGroupCommandDto = {
            macAddress: a.mac,
            componentId: a.componentId,
            command: 4,
            arg: 0,
          };
          var res = await send.post(MonitorGroupEndpoint.COMMAND, object);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.COMMAND,
              toggleToast,
            )
          ) {
          }
        });
        console.log(selectedObject);
        break;
      case "override-arm":
        selectedObject.map(async (a: MonitorGroupDto) => {
          let object: MonitorGroupCommandDto = {
            macAddress: a.mac,
            componentId: a.componentId,
            command: 5,
            arg: 0,
          };
          var res = await send.post(MonitorGroupEndpoint.COMMAND, object);
          if (
            Helper.handleToastByResCode(
              res,
              MonitorGroupToast.COMMAND,
              toggleToast,
            )
          ) {
          }
        });
        console.log(selectedObject);
        break;
      default:
        break;
    }
  };
  const handleEdit = (data: MonitorGroupDto) => {
    setMpGroupDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleRemove = (data: MonitorGroupDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(
        MonitorGroupEndpoint.DELETE(data.guid),
      );
      if (
        Helper.handleToastByResCode(res, MonitorGroupToast.DELETE, toggleToast)
      ) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (data: MonitorGroupDto) => {
    setMpGroupDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      MonitorGroupEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res?.data.data);
    if (res.status == HttpStatusCode.Ok) {
      setMpGroupsDto(res.data.data.items);
      setPagination(res.data.data);

      const newStatuses = res.data.data.items.map((a:MonitorGroupDto) => {
        const status:MonitorGroupStatus = {
          guid: a.guid,
          maskCount: 0,
          numActive: 0,
          activeList: []
        }
        return status;
      })

      setStatus((prev) => [...prev,...newStatuses])

      // Fetch status for each
            res.data.data.items.forEach((a: MonitorGroupDto) => {
              fetchStatus(a.guid);
            });
    }
  };

  const fetchStatus = async (guid:string) => {
    await send.get(MonitorGroupEndpoint.STATUS(guid))
  }

  const tabContent: FormContent[] = [
    {
      label: "Monitor Group",
      icon: <MonitorIcon />,
      content: (
        <MonitorGroupForm
          setDto={setMpGroupDto}
          dto={mpGroupDto}
          handleClick={handleClick}
          type={formType} handleChange={()=>{}}        />
      ),
    },
  ];
  const action: ActionButton[] = [
    {
      lable: "access",
      buttonName: "Access",
      icon: <MonitorIcon />,
    },
    {
      lable: "override",
      buttonName: "Override",
      icon: <MonitorIcon />,
    },
    {
      lable: "force",
      buttonName: "Froce",
      icon: <MonitorIcon />,
    },
    {
      lable: "arm",
      buttonName: "Arm",
      icon: <MonitorIcon />,
    },
    {
      lable: "override-arm",
      buttonName: "Override",
      icon: <MonitorIcon />,
    },
  ];

   const renderOptionalComponent = (data: MonitorPointGroupComponent) => {
    // 1. Look up the matched record once
    const matchedItem = status.some((b) => b.guid === data.componentGuid);

    return [
      <TableCell
        key={`status-badge-${data.componentGuid}`}
        className="px-4 py-3 text-start text-theme-sm"
      >
        <div className="flex flex-wrap items-center gap-1.5">
          {
            matchedItem ? 
             <Badge
            color="success"
            size="sm"
            startIcon={<SensorOnIcon className="h-3.5 w-3.5" />}
          >
            Active
          </Badge>
            :
            <Badge
            color="error"
            size="sm"
            startIcon={<SensorOffIcon className="h-3.5 w-3.5" />}
          >
            Inactive
          </Badge>

          }
          
        </div>
      </TableCell>,
    ];
  };

  useEffect(() => {
      const initSignalR = async () => {
        if (!token) return;
  
        await SignalRService.startConnection();
        const connection = SignalRService.getConnection();
        if (!connection) return;
  
        connection.on(SignalRTopic.MONITOR_GROUP_STATUS, (status: MonitorGroupStatus) => {
          console.log(status);
          setStatus((prev) =>
            prev.map((item) =>
              item.guid === status.guid
                ? {
                  ...item,
                  maskCount:status.maskCount,
                  numActive:status.numActive,
                  activeList:status.activeList
                }
                : item,
            ),
          );
        });
  
  
  
        try {
          await SignalRService.joinGroup(SignalRTopic.MONITOR_GROUP_STATUS);
        } catch (err) {
          console.error("Subscribe error:", err);
        }
  
      };
  
      initSignalR();
  
      return () => {
        const connection = SignalRService.getConnection();
        connection?.off(SignalRTopic.MONITOR_GROUP_STATUS);
      };
    }, [refresh, locationGuid, token]);

  return (
    <>
      <PageBreadcrumb pageTitle="Monitor Group" />
      {form ? (
        <BaseForm tabContent={tabContent} type={formType} handleClick={handleClick}  />
      ) : (
        <BaseTable<MonitorGroupDto>
          headers={MP_GP_HEADER}
          keys={MP_GP_KEY}
          onClick={handleClick}
          data={mpGroupsDto}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          select={selectedObject}
          setSelect={setSelectedObjects}
          subTable={(data) => (
            <TableRow
              key={`${data.guid}-components`}
              className="bg-gray-50/70 dark:bg-white/[0.02]"
            >
              <TableCell
                colspan={MP_GP_HEADER.length + 1}
                className="px-5 py-4"
              >
                <section className="overflow-hidden rounded-xl border border-gray-200 bg-white shadow-sm dark:border-white/[0.08] dark:bg-gray-900">
                  <div className="flex items-center justify-between gap-4 border-b border-gray-100 px-4 py-3 dark:border-white/[0.05]">
                    <div>
                      <h3 className="text-sm font-semibold text-gray-800 dark:text-white/90">
                        Components
                      </h3>
                      <p className="mt-0.5 text-xs text-gray-500 dark:text-gray-400">
                        Components assigned to this monitor group
                      </p>
                    </div>
                    <span className="rounded-full bg-brand-50 px-2.5 py-1 text-xs font-medium text-brand-600 dark:bg-brand-500/10 dark:text-brand-400">
                      {data.components.length}
                    </span>
                  </div>

                  {data.components.length === 0 ? (
                    <p className="px-4 py-6 text-center text-sm text-gray-500 dark:text-gray-400">
                      No components assigned.
                    </p>
                  ) : (
                    <div className="overflow-x-auto">
                      <Table>
                        <TableHeader className="bg-gray-50 dark:bg-white/[0.03]">
                          <TableRow>
                            <TableCell
                              isHeader
                              className="px-4 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
                            >
                              Component
                            </TableCell>
                            <TableCell
                              isHeader
                              className="px-4 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
                            >
                              Type
                            </TableCell>
                            <TableCell
                              isHeader
                              className="px-4 py-3 text-start text-xs font-medium text-gray-500 dark:text-gray-400"
                            >
                              Status
                            </TableCell>
                          </TableRow>
                        </TableHeader>
                        <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                          {data.components.map((component, index) => {
                            const isMonitorPoint =
                              component.type === MonitorType.MONITOR_POINT;
                            const componentType =
                              MONITOR_TYPE_LABELS[component.type];
                            const ComponentIcon = isMonitorPoint
                              ? MonitorIcon
                              : DoorIcon;

                            return (
                              <TableRow
                                key={`${component.type}-${component.componentGuid}-${index}`}
                                className="hover:bg-gray-50 dark:hover:bg-white/[0.02]"
                              >
                                <TableCell className="px-4 py-3 text-sm text-gray-700 dark:text-gray-300">
                                  <span className="flex min-w-0 items-center gap-2">
                                    <ComponentIcon className="h-4 w-4 shrink-0 text-gray-500" />
                                    <span className="break-all">
                                      {component.componentGuid}
                                    </span>
                                  </span>
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-500 dark:text-gray-400">
                                  {componentType}
                                </TableCell>
                                <TableCell className="px-4 py-3 text-sm text-gray-500 dark:text-gray-400">
                                  {renderOptionalComponent(component)}
                                </TableCell>
                              </TableRow>
                            );
                          })}
                        </TableBody>
                      </Table>
                    </div>
                  )}
                </section>
              </TableCell>
            </TableRow>
          )}
          permission={filterPermission(FeatureId.monitorgroup)}
          action={action}
          fetchData={fetchData}
          locationGuid={locationGuid}
          specialDisplay={[
            {
              key:"components",
              content:(data,i) =>(
              <TableCell
                                  key={i}
                                  className="flex items-center gap-2 px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                                >
                                  <MonitorIcon className="text-xl"/>
                                  {data.components.length}
                                  </TableCell>
            )
            }
          ]}
        />
      )}
    </>
  );
};
