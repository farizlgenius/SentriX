import { ReactNode, useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import {
  AddIcon,
  AmicoIcon,
  ControlIcon,
  HardwareIcon,
  Info2Icon,
  ModuleIcon,
  ResetIcon,
  ScanIcon,
  ToggleTranIcon,
  TransferIcon,
  TrashBinIcon,
  UploadIcon
} from "../../icons";
import Modals from "../UiElements/Modals";
import Helper from "../../utility/Helper";
import DeviceForm from "../../components/form/device/AeroDeviceForm";
import { DeviceDto } from "../../model/Device/DeviceDto";
import { IdReport } from "../../model/IdReport/IdReport";
import SignalRService from "../../services/SignalRService";
import { StatusDto } from "../../model/StatusDto";
import { DeviceEndpoint } from "../../endpoint/HardwareEndpoint";
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
import { HardwareMemAllocForm } from "../../components/form/device/HardwareMemAllocForm";
import { HardwareComponentForm } from "../../components/form/device/HardwareComponentForm";
import { TranStatusDto } from "../../model/Device/TranStatusDto";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { SetTranDto } from "../../model/Device/SetTranDto";
import { usePagination } from "../../context/PaginationContext";
import { useIdReport } from "../../context/IdReportContext";
import { SignalRTopic } from "../../constants/signalr-constant";
import SelectDeviceForm from "../../components/form/device/SelectDeviceForm";
import { DeviceType } from "../../enum/DeviceType";
import { AeroModuleDetailForm } from "../../components/form/device/AeroModuleDetailForm";
import AeroDeviceForm from "../../components/form/device/AeroDeviceForm";
import { DeviceDtoMetadata as AeroDtoMetadata } from "../../model/Device/DeviceDtoStrMetadata";
import { mapFields } from "../../utility/Mapper";

const HEADER = ["Type", "Name", "Mac", "Firmware", "IP", "Port", "Configuration", "Status", "Action"];
const KEY = ["type", "name", "mac", "fw", "ip", "port"];






const Device = () => {
  const { idReports, setIdReports } = useIdReport();
  const { setPagination } = usePagination();
  const { locationId } = useLocation();
  const { toggleToast } = useToast();
  const { filterPermission, token } = useAuth();
  const {
    setCreate,
    setRemove,
    setUpdate,
    setConfirmRemove,
    setConfirmUpdate,
    setMessage,
    setInfo
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);

  const defaultDto:DeviceDto = {
    id: 0,
    componentId: 0,
    name: "",
    serialNumber: "",
    mac: "",
    ip: "",
    port: "",
    fw: "",
    type: "",
    status: "",
    syncedAt: new Date(),
    locationId: 0,
    metadata: "",
  }

  const aeroDefault:AeroDtoMetadata = {
    id: 0,
    componentId: 0,
    name: "",
    serialNumber: "",
    mac: "",
    ip: "",
    port: "",
    fw: "",
    type: "",
    status: "",
    syncedAt: new Date(),
    locationId: 0,
    metadata: {
      portOne: false,
      protocolOne: -1,
      baudRateOne: -1,
      portTwo: false,
      protocolTwo: -1,
      baudRateTwo: -1
    }
  }


  const [scan, setScan] = useState<boolean>(false);
  const [selectType, setSelectType] = useState<boolean>(false);
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [currentDeviceType, setCurrentDeviceType] = useState<string>("");
  const [deviceDto, setDeviceDto] = useState<DeviceDto>(defaultDto);
  const [aeroDto, setAeroDto] = useState<AeroDtoMetadata>(aeroDefault);
  const [data, setData] = useState<DeviceDto[]>([]);
  const [status, setStatus] = useState<StatusDto[]>([]);
  const [tranStatus, setTranStatus] = useState<TranStatusDto[]>([]);
  const [select, setSelect] = useState<DeviceDto[]>([]);

  const handleCloseModal = () => setScan(false);
  const handleCloseSelect = () => setSelectType(false);

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    fetchLocationId?: number,
    search?: string,
    startDate?: string,
    endDate?: string
  ) => {
    const res = await send.get(DeviceEndpoint.PAGINATION(pageNumber, pageSize, fetchLocationId, search, startDate, endDate));
    if (res && res.data) {
      setData(res.data.items);
      setPagination(res.data);

      const newStatuses = res.data.items.map((item: StatusDto) => ({
        id: item.id,
        componentId: item.componentId,
        status: -1,
        tamper: -1,
        ac: -1,
        batt: -1
      }));

      const newTranStatuses = res.data.items.map((item: StatusDto) => ({
        scpId: item.componentId,
        capacity: 0,
        oldest: 0,
        lastReport: 0,
        lastLog: 0,
        disabled: 0,
        status: ""
      }));

      setStatus(newStatuses);
      setTranStatus(newTranStatuses);

      res.data.items.forEach((item: DeviceDto) => {
        fetchStatus(item.id);
      });
    }
  };

  const setTran = async (tranData: SetTranDto[]) => {
    const res = await send.post(DeviceEndpoint.TRAN_RANGE, tranData);
    if (Helper.handleToastByResCode(res, HardwareToast.TOGGLE_TRAN, toggleToast)) {
      toggleRefresh();
    }
  };

  const fetchStatus = async (id: number) => {
    const res = await send.get(DeviceEndpoint.STATUS(id));
    if (res.data) {
      setStatus((prev) =>
        prev.map((item) =>
          item.id === res.data.id
            ? {
                ...item,
                status: res.data.status
              }
            : item
        )
      );
    }
  };

  const resetDevice = async (id: number) => {
    const res = await send.post(DeviceEndpoint.RESET(id));
    if (Helper.handleToastByResCode(res, HardwareToast.RESET, toggleToast)) {
      toggleRefresh();
    }
  };

  const uploadConfig = async (id: number) => {
    const res = await send.post(DeviceEndpoint.UPLOAD(id));
    if (Helper.handleToastByResCode(res, HardwareToast.UPLOAD, toggleToast)) {
      toggleRefresh();
    }
  };

  const handleFormSelection = (type:string,data:any) => {
    switch(type){
      case DeviceType.AERO :
       const {metadata,...rest} = data;
        setAeroDto(mapFields(rest, { metadata: JSON.parse(metadata) }));
      break;
      case DeviceType.AMICO:
        break;
        default:
          break;
    }
  }



  const handleEdit = (item: DeviceDto) => {

    setFormType(FormType.UPDATE);
    setCurrentDeviceType(item.type);
    handleFormSelection(item.type,item);
    setForm(true);
  };

  const handleRemove = (item: DeviceDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(DeviceEndpoint.DELETE(item.id));
      if (Helper.handleToastByResCode(res, HardwareToast.DELETE, toggleToast)) {
        setDeviceDto(defaultDto);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (item: DeviceDto) => {
    setFormType(FormType.INFO);
    setCurrentDeviceType(item.type);
    handleFormSelection(item.type,item);
    setForm(true);
  };

  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setDeviceDto(defaultDto);
        setSelectType(true);
        break;
      case "report":
        if (select.length === 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          const tranData = select.map((item: DeviceDto) => ({
            macAddress: item.mac,
            param: 1
          }));
          setTran(tranData);
        }
        break;
      case "delete":
        if (select.length === 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          setConfirmRemove(() => async () => {
            const ids = select.map((item: DeviceDto) => item.id);
            const res = await send.post(DeviceEndpoint.DELETE_RANGE, ids);
            if (Helper.handleToastByResCode(res, HardwareToast.DELETE_RANGE, toggleToast)) {
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
          if (Helper.handleToastByResCode(res, HardwareToast.UPDATE, toggleToast)) {
            setForm(false);
            toggleRefresh();
            setDeviceDto(defaultDto);
            setCurrentDeviceType("");
          }
        });
        setUpdate(true);
        break;
      case "create":
        setCreate(true);
        break;
      case "type":
        setForm(true);
        break;
      case "scan":
        setScan(true);
        break;
      case "close":
        setForm(false);
        setDeviceDto(defaultDto);
        setCurrentDeviceType("");
        break;
      case "reset":
        if (select.length !== 0) {
          select.forEach((item: DeviceDto) => resetDevice(item.id));
        } else {
          setMessage("No selected object");
          setInfo(true);
        }
        break;
      case "upload":
        if (select.length !== 0) {
          select.forEach((item: DeviceDto) => uploadConfig(item.id));
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

      try {
        await SignalRService.joinGroup(SignalRTopic.IDREPORT);
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
  }, [refresh, locationId, token, setIdReports]);

  const actionBtn: ActionButton[] = [
    {
      buttonName: "Reset",
      lable: "reset",
      icon: <ResetIcon />
    },
    {
      buttonName: "Upload",
      lable: "upload",
      icon: <UploadIcon />
    },
    {
      buttonName: "Transfer",
      lable: "transfer",
      icon: <TransferIcon />
    },
    {
      buttonName: "Report Toggle",
      lable: "report",
      icon: <ToggleTranIcon />
    },
    {
      buttonName: "Scan",
      lable: "scan",
      icon: <ScanIcon className={idReports.length !== 0 ? "animate-ping" : ""} />
    }
  ];

  const renderOptional = (item: DeviceDto, statusDto: StatusDto[], index: number) => {
    return [
      <TableCell key={index} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <Badge variant="solid" size="sm" color={item.status == "RESET" ? "error" : item.status == "UPLOAD" ? "warning" : "success" }>
          {item.status === "RESET" ? "Reset" : item.status === "UPLOAD" ? "Upload" : "Synced"}
        </Badge>
      </TableCell>,
      <TableCell key={index + 1} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <Badge
          size="sm"
          color={statusDto.find((statusItem) => statusItem.id === item.id)?.status ? "success" : "error"}
        >
          {statusDto.find((statusItem) => statusItem.id === item.id)?.status
            ? "Online"
            : "Offline"}
        </Badge>
      </TableCell>
    ];
  };



  const aeroContent: FormContent[] = [
    {
      icon: <Info2Icon />,
      label: "Device Information",
      content:  <AeroDeviceForm handleClick={handleClickWithEvent} dto={aeroDto} setDto={setAeroDto} type={formType} />
    },
    {
      icon: <ModuleIcon />,
      label: "Module",
      content:<AeroModuleDetailForm/> 
    },
    {
      icon: <TransferIcon />,
      label: "Memory Allocate Detail",
      content: <HardwareMemAllocForm data={deviceDto} />
    },
    {
      icon: <ControlIcon />,
      label: "ComponentSync Detail",
      content: <HardwareComponentForm data={deviceDto} />
    }
  ];

  const amicoContent: FormContent[] = [
    {
      icon: <HardwareIcon />,
      label: "Show Detail",
      content: (
        // <DeviceInfoSummary device={deviceDto} />
        <></>
      )
    }
  ];

  return (
    <>
      {selectType && (
        <Modals
          body={<SelectDeviceForm setDeviceType={setCurrentDeviceType} setSelectType={setSelectType} setForm={setForm} />}
          handleClickWithEvent={handleCloseSelect}
        />
      )}

      {scan && <Modals header="Scan" body={<div className="text-sm text-gray-500">Scanning workspace is not changed in this screen yet.</div>} handleClickWithEvent={handleCloseModal} />}

      <PageBreadcrumb pageTitle="Device" />
      <div className="space-y-6">
        {form ? (
          <BaseForm
            tabContent={currentDeviceType === DeviceType.AMICO ? amicoContent : aeroContent}
            header={currentDeviceType === DeviceType.AMICO ? "AMICO Device Detail" : "AERO Device Detail"}
            desc={
              currentDeviceType === DeviceType.AMICO
                ? "Single-tab AMICO detail with the existing form and a graphical module split."
                : "Split detail workspace for AERO with information, module, memory allocation, and component sync tabs."
            }
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
            locationId={locationId}
            fetchData={fetchData}
            specialDisplay={[
              {
                key: "tranStatus",
                content: (item, index) => (
                  <TableCell key={index} className="px-4 py-3 text-gray-500 text-center text-theme-sm dark:text-gray-400">
                    <Badge
                      size="sm"
                      color={tranStatus.find((tranItem) => tranItem.scpId === item.componentId)?.disabled === 0 &&
                        tranStatus.find((tranItem) => tranItem.scpId === item.componentId)?.status
                        ? "success"
                        : "error"}
                    >
                      {tranStatus.find((tranItem) => tranItem.scpId === item.componentId)?.status ?? "Unknown"}
                    </Badge>
                  </TableCell>
                )
              },
              {
                key: "type",
                content: (item, index) => (
                  <TableCell key={index} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                    {item.type === "AERO" ? <ModuleIcon className="text-2xl" /> : <AmicoIcon className="text-2xl"/>}
                  </TableCell>
                )
              }
            ]}
          />
        )}
      </div>
    </>
  );
};

export default Device;
