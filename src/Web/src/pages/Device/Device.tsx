import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import {
  AmicoIcon,
  CancelCircleIcon,
  CheckCircleIcon,
  ModuleIcon,
  ResetIcon,
  ScanIcon,
  ToggleTranIcon,
  TransferIcon,
  UploadIcon,
} from "../../icons";
import Helper from "../../utility/Helper";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { IdReport } from "../../model/IdReport/IdReport";
import SignalRService from "../../services/SignalRService";
import { StatusDto } from "../../model/StatusDto";
import { DeviceEndpoint } from "../../endpoint/DeviceEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { BaseTable } from "../UiElements/BaseTable";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { ActionButton } from "../../model/ActionButton";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { useToast } from "../../context/ToastContext";
import { HardwareToast } from "../../model/ToastMessage";
import Badge from "../../components/ui/badge/Badge";
import { TableCell } from "../../components/ui/table";
import { EventStatusDto } from "../../model/Device/TranStatusDto";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { SetTranDto } from "../../model/Device/SetTranDto";
import { usePagination } from "../../context/PaginationContext";
import { useIdReport } from "../../context/IdReportContext";
import { SignalRTopic } from "../../constants/signalr-constant";
import AeroDeviceForm from "../../components/form/device/AeroDeviceForm";
import { CreateDeviceStrMetadataDto } from "../../model/Device/CreateDeviceStrMetadataDto";
import { Vendor } from "../../enum/Vendor";
import { AeroMetadata } from "../../model/Device/AeroMetadata";
import { AmicoMetadata } from "../../model/Device/AmicoMetadata";
import { FormSection } from "../../components/form/template/FormTemplate";
import AmicoDeviceForm from "../../components/form/device/AmicoDeviceForm";
import { AeroModuleDetailForm } from "../../components/form/device/AeroModuleDetailForm";
import { AeroComponentForm } from "../../components/form/device/AeroComponentForm";
import { AeroMemAllocForm } from "../../components/form/device/AeroMemAllocForm";

const HEADER = [
  "Type",
  "Name",
  "Mac",
  "Firmware",
  "IP",
  "Port",
  "Event",
  "Configuration",
  "Status",
  "Enable",
  "Action",
];
const KEY = ["type", "name", "mac", "firmware", "ip", "port", "tranStatus"];

const Device = () => {
  const { idReports, setIdReports } = useIdReport();
  const { setPagination } = usePagination();
  const { locationGuid } = useLocation();
  const { toggleToast } = useToast();
  const { filterPermission, token } = useAuth();
  const {
    setCreate,
    setRemove,
    setUpdate,
    setConfirmRemove,
    setConfirmUpdate,
    setConfirmCreate,
    setMessage,
    setInfo,
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);

  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [currentDeviceType, setCurrentDeviceType] = useState<string>("");

  const [data, setData] = useState<DeviceDto[]>([]);
  const [status, setStatus] = useState<StatusDto[]>([]);
  const [tranStatus, setTranStatus] = useState<EventStatusDto[]>([]);
  const [select, setSelect] = useState<DeviceDto[]>([]);

  const aeroMetadata: AeroMetadata = {
    portOne: false,
    protocolOne: 0,
    baudRateOne: 0,
    portTwo: false,
    protocolTwo: 0,
    baudRateTwo: 0,
  };

  const amicoMetadata: AmicoMetadata = {
    deviceId: "",
  };

  const defaultDto: DeviceDto = {
    guid: "",
    name: "",
    serialNumber: "",
    mac: "",
    ip: "",
    port: "",
    firmware: "",
    vendor:
      currentDeviceType == Vendor[Vendor.aero] ? Vendor.aero : Vendor.amico,
    syncedAt: new Date(),
    locationGuid: locationGuid,
    metadata: aeroMetadata,
    isDefault: false,
    isActive: false,
    configurationStatus: "",
    deviceModules: [],
  };

  const [deviceDto, setDeviceDto] = useState<DeviceDto>(defaultDto);

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      DeviceEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setData(res.data.data.items);
      setPagination(res.data.data);

      const newStatuses = res.data.data.items.map((item: DeviceDto) => ({
        guid: item.guid,
        mac: item.mac,
        status: false,
        tamper: -1,
        ac: -1,
        batt: -1,
      }));

      const newTranStatuses = res.data.data.items.map((item: DeviceDto) => ({
        deviceGuid: item.guid,
        capacity: 0,
        oldest: 0,
        lastReport: 0,
        lastLog: 0,
        disabled: 0,
        status: "",
      }));

      setStatus(newStatuses);
      setTranStatus(newTranStatuses);

      res.data.data.items.forEach((item: DeviceDto) => {
        fetchStatus(item.guid);
        fetchTranStatus(item.guid);
      });
    }
  };

  const fetchSetTran = async (tranData: SetTranDto) => {
    const res = await send.post(DeviceEndpoint.SET_TRAN, tranData);
    if (
      Helper.handleToastByResCode(res, HardwareToast.TOGGLE_TRAN, toggleToast)
    ) {
      toggleRefresh();
    }
  };

  const fetchTranStatus = async (guid: string) => {
    const res = await send.get(DeviceEndpoint.GET_EVENT_STATUS(guid));
    if (res.data.success) {
      setTranStatus((prev) =>
        prev.map((item) =>
          item.deviceGuid === res.data.data.guid
            ? {
                ...item,
                status: res.data.data.status,
              }
            : item,
        ),
      );
    }
  };

  const fetchStatus = async (guid: string) => {
    const res = await send.get(DeviceEndpoint.STATUS(guid));
    if (res.data.success) {
      setStatus((prev) =>
        prev.map((item) =>
          item.guid === res.data.data.guid
            ? {
                ...item,
                status: res.data.data.status,
              }
            : item,
        ),
      );
    }
  };

  const resetDevice = async (guid: string) => {
    const res = await send.post(DeviceEndpoint.RESET(guid));
    if (Helper.handleToastByResCode(res, HardwareToast.RESET, toggleToast)) {
      toggleRefresh();
    }
  };

  const uploadConfig = async (guid: string) => {
    const res = await send.post(DeviceEndpoint.UPLOAD(guid));
    if (Helper.handleToastByResCode(res, HardwareToast.UPLOAD, toggleToast)) {
      toggleRefresh();
    }
  };

  // const handleFormSelection = (type: string, data: any) => {
  //   switch (type) {
  //     case DeviceType.AERO:
  //       const { metadata, ...rest } = data;
  //       setAeroDto(mapFields(rest, { metadata: JSON.parse(metadata) }));
  //       break;
  //     case DeviceType.AMICO:
  //       break;
  //     default:
  //       break;
  //   }
  // };

  const handleEdit = (item: DeviceDto) => {
    const metadata = JSON.parse(item.metadata as string) as AeroMetadata;
    item.metadata = metadata;
    setFormType(FormType.UPDATE);
    setDeviceDto(item);
    setForm(true);
  };

  const handleRemove = (item: DeviceDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(DeviceEndpoint.DELETE(item.guid));
      if (Helper.handleToastByResCode(res, HardwareToast.DELETE, toggleToast)) {
        setDeviceDto(defaultDto);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (item: DeviceDto) => {
    setFormType(FormType.INFO);
    setDeviceDto(item);
    setForm(true);
  };

  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        setDeviceDto(defaultDto);
        break;
      case "report":
        if (select.length === 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          select.forEach((item: DeviceDto) =>
            fetchSetTran({
              deviceGuid: item.guid,
              type: item.type,
              isEnable: true,
            }),
          );
        }
        break;
      case "delete":
        if (select.length === 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          setConfirmRemove(() => async () => {
            const ids = select.map((item: DeviceDto) => item.guid);
            const res = await send.post(DeviceEndpoint.DELETE_RANGE, ids);
            if (
              Helper.handleToastByResCode(
                res,
                HardwareToast.DELETE_RANGE,
                toggleToast,
              )
            ) {
              setRemove(false);
              toggleRefresh();
            }
          });
          setRemove(true);
        }
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(DeviceEndpoint.UPDATE, deviceDto);
          if (
            Helper.handleToastByResCode(res, HardwareToast.UPDATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
            setDeviceDto(defaultDto);
            setCurrentDeviceType("");
          }
        });
        setUpdate(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const req: CreateDeviceStrMetadataDto = {
            ...deviceDto,
            metadata: JSON.stringify(deviceDto.metadata),
          };
          const res = await send.post(DeviceEndpoint.CREATE, req);
          if (
            Helper.handleToastByResCode(res, HardwareToast.CREATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "type":
        setForm(true);
        break;
      case "close":
        setForm(false);
        setDeviceDto(defaultDto);
        setCurrentDeviceType("");
        break;
      case "reset":
        if (select.length !== 0) {
          select.forEach((item: DeviceDto) => resetDevice(item.guid));
        } else {
          setMessage("No selected object");
          setInfo(true);
        }
        break;
      case "upload":
        if (select.length !== 0) {
          select.forEach((item: DeviceDto) => uploadConfig(item.guid));
        } else {
          setMessage("No selected object");
          setInfo(true);
        }
        break;
      default:
        break;
    }
  };

  useEffect(() => {
    const initSignalR = async () => {
      if (!token) return;

      await SignalRService.startConnection();
      const connection = SignalRService.getConnection();
      if (!connection) return;

      connection.on(SignalRTopic.IDREPORT, (reports: IdReport[]) => {
        setIdReports(reports);
      });

      connection.on(SignalRTopic.EVENT_STATUS, (status: EventStatusDto) => {
        setTranStatus((prev) =>
          prev.map((item) =>
            item.deviceGuid === status.deviceGuid
              ? {
                  ...item,
                  isEnable: status.isEnable,
                }
              : item,
          ),
        );
      });

      try {
        await SignalRService.joinGroup(SignalRTopic.IDREPORT);
      } catch (err) {
        console.error("Subscribe error:", err);
      }

      try {
        await SignalRService.joinGroup(SignalRTopic.EVENT_STATUS);
      } catch (err) {
        console.error("Subscribe error:", err);
      }

      const res = await send.get(DeviceEndpoint.ID_REPORT);
      setIdReports(res.data);
    };

    initSignalR();

    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.IDREPORT);
    };
  }, [refresh, locationGuid, token, setIdReports]);

  const actionBtn: ActionButton[] = [
    {
      buttonName: "Reset",
      lable: "reset",
      icon: <ResetIcon />,
    },
    {
      buttonName: "Upload",
      lable: "upload",
      icon: <UploadIcon />,
    },
    {
      buttonName: "Transfer",
      lable: "transfer",
      icon: <TransferIcon />,
    },
    {
      buttonName: "Report Toggle",
      lable: "report",
      icon: <ToggleTranIcon />,
    },
    {
      buttonName: "Scan",
      lable: "scan",
      icon: (
        <ScanIcon className={idReports.length !== 0 ? "animate-ping" : ""} />
      ),
    },
  ];

  const renderOptional = (
    item: DeviceDto,
    statusDto: StatusDto[],
    index: number,
  ) => {
    return [
      <TableCell
        key={index}
        className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
      >
        <Badge
          variant="solid"
          size="sm"
          color={
            item.configurationStatus == "RESET"
              ? "error"
              : item.configurationStatus == "UPLOAD"
                ? "warning"
                : "success"
          }
        >
          {item.configurationStatus === "RESET"
            ? "Reset"
            : item.configurationStatus === "UPLOAD"
              ? "Upload"
              : "Synced"}
        </Badge>
      </TableCell>,
      <TableCell
        key={index + 1}
        className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
      >
        <Badge
          size="sm"
          color={
            statusDto.find((statusItem) => statusItem.guid === item.guid)
              ?.status
              ? "success"
              : "error"
          }
        >
          {statusDto.find((statusItem) => statusItem.guid === item.guid)?.status
            ? "Online"
            : "Offline"}
        </Badge>
      </TableCell>,
    ];
  };

  const deviceTypes = [
    {
      vendor: Vendor.aero,
      name: "HID Aero X1100",
      description: "Configure an Aero controller and its connected ports.",
      icon: ModuleIcon,
    },
    {
      vendor: Vendor.amico,
      name: "HID Amico",
      description: "Set up an Amico controller for streamlined access control.",
      icon: AmicoIcon,
    },
  ];

  const handleDeviceTypeSelect = (vendor: Vendor) => {
    setDeviceDto(defaultDto);
    setCurrentDeviceType(Vendor[vendor]);
    setDeviceDto((prev) => ({ ...prev, vendor: vendor }));
  };

  const aeroContent: FormContent[] = [
    {
      icon: <ModuleIcon />,
      label: "Device Information",
      content: (
        <AeroDeviceForm
          handleClick={handleClickWithEvent}
          type={formType}
          setDto={setDeviceDto}
          dto={deviceDto}
        />
      ),
    },
    {
      icon: <ModuleIcon />,
      label: "Sub-device Information",
      content: <AeroModuleDetailForm data={deviceDto} />,
    },
    {
      icon: <ModuleIcon />,
      label: "Device Component",
      content: <AeroComponentForm data={deviceDto} />,
    },
    {
      icon: <ModuleIcon />,
      label: "Device Memory Alloc",
      content: <AeroMemAllocForm data={deviceDto} />,
    },
  ];

  const amicoContent: FormContent[] = [
    {
      icon: <ModuleIcon />,
      label: "Device Information",
      content: (
        <AmicoDeviceForm
          handleClick={handleClickWithEvent}
          type={formType}
          setDto={setDeviceDto}
          dto={deviceDto}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Device" />
      <div className="flex flex-col gap-5">
        {form && formType !== FormType.INFO && (
          <FormSection
            overall="Device setup"
            title="Choose device type"
            description="Select the controller you want to add before completing its details."
          >
            <div className="grid gap-4 sm:grid-cols-2">
              {deviceTypes.map((deviceType) => {
                const Icon = deviceType.icon;
                const isSelected = deviceDto.vendor === deviceType.vendor;

                return (
                  <button
                    key={deviceType.vendor}
                    type="button"
                    aria-pressed={isSelected}
                    onClick={() => handleDeviceTypeSelect(deviceType.vendor)}
                    className={`group relative flex min-h-36 items-center gap-4 rounded-2xl border p-5 text-left transition-all duration-200 focus:outline-hidden focus:ring-4 focus:ring-brand-500/15 ${
                      isSelected
                        ? "border-brand-500 bg-brand-50 shadow-theme-xs dark:border-brand-400 dark:bg-brand-500/10"
                        : "border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] hover:-translate-y-0.5 hover:border-brand-300 hover:shadow-theme-xs dark:hover:border-brand-700"
                    }`}
                  >
                    <span
                      className={`flex size-14 shrink-0 items-center justify-center rounded-2xl transition-colors ${
                        isSelected
                          ? "bg-brand-500 text-white"
                          : "bg-brand-50 text-brand-500 dark:bg-brand-500/10 dark:text-brand-300"
                      }`}
                    >
                      <Icon className="size-7" />
                    </span>
                    <span className="min-w-0 pr-5">
                      <span className="block text-base font-semibold text-gray-900 dark:text-white">
                        {deviceType.name}
                      </span>
                      <span className="mt-1 block text-sm leading-5 text-gray-500 dark:text-gray-400">
                        {deviceType.description}
                      </span>
                    </span>
                    <span
                      className={`absolute right-4 top-4 flex size-5 items-center justify-center rounded-full border transition-colors ${
                        isSelected
                          ? "border-brand-500 bg-brand-500 text-white"
                          : "border-gray-300 bg-transparent dark:border-gray-600"
                      }`}
                    >
                      {isSelected && <CheckCircleIcon className="size-3.5" />}
                    </span>
                  </button>
                );
              })}
            </div>
          </FormSection>
        )}
        {form ? (
          <BaseForm
            type={formType}
            handleClick={handleClickWithEvent}
            tabContent={
              deviceDto.vendor == Vendor.aero ? aeroContent : amicoContent
            }
            header={""}
            desc={""}
          />
        ) : (
          <BaseTable<DeviceDto>
            refresh={refresh}
            headers={HEADER}
            keys={KEY}
            data={data}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onInfo={handleInfo}
            onClick={handleClickWithEvent}
            select={select}
            setSelect={setSelect}
            permission={filterPermission(FeatureId.device)}
            action={actionBtn}
            renderOptionalComponent={renderOptional}
            status={status}
            locationGuid={locationGuid}
            fetchData={fetchData}
            specialDisplay={[
              {
                key: "tranStatus",
                content: (item, index) => (
                  <TableCell
                    key={index}
                    className="px-4 py-3 text-gray-500 text-center text-theme-sm dark:text-gray-400"
                  >
                    {tranStatus.find(
                      (tranItem) => tranItem.deviceGuid === item.guid,
                    )?.isEnable ? (
                      <CheckCircleIcon className="text-2xl" />
                    ) : (
                      <CancelCircleIcon className="text-2xl" />
                    )}
                  </TableCell>
                ),
              },
              {
                key: "type",
                content: (item, index) => (
                  <TableCell
                    key={index}
                    className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                  >
                    {item.vendor == Vendor.aero ? (
                      <ModuleIcon className="text-2xl" />
                    ) : (
                      <AmicoIcon className="text-2xl" />
                    )}
                  </TableCell>
                ),
              },
            ]}
          />
        )}
      </div>
    </>
  );
};

export default Device;

// function mapDto(amicoDto: AmicoMetadata): CreateDeviceStrMetadataDto {
//   const { metadata, ...rest } = amicoDto;
//   return mapFields(rest, { metadata: JSON.stringify(metadata) });
// }
