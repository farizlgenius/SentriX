import { ReactNode, useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import Button from "../../components/ui/button/Button";
import { HardwareIcon, ResetIcon, ScanIcon, ToggleTranIcon, TransferIcon, UploadIcon } from "../../icons";
import Modals from "../UiElements/Modals";
import Helper from "../../utility/Helper";
import DeviceForm from "../../components/form/device/DeviceForm";
import { HardwareDto } from "../../model/Device/HardwareDto";
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
import { Table, TableBody, TableCell, TableHeader, TableRow } from "../../components/ui/table";
import { HardwareMemAllocForm } from "../../components/form/device/HardwareMemAllocForm";
import { useLoading } from "../../context/LoadingContext";
import { HardwareComponentForm } from "../../components/form/device/HardwareComponentForm";
import { TranStatusDto } from "../../model/Device/TranStatusDto";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { SetTranDto } from "../../model/Device/SetTranDto";
import { usePagination } from "../../context/PaginationContext";
import { CreateDeviceDto } from "../../model/Device/CreateDeviceDto";
import { useIdReport } from "../../context/IdReportContext";
import { SignalRTopic } from "../../constants/signalr-constant";
import AeroCreateDeviceForm from "../../components/form/device/AeroCreateDeviceForm";
import SelectDeviceForm from "../../components/form/device/SelectDeviceForm";
import { DeviceType } from "../../enum/DeviceType";
import AmicoCreateDeviceForm from "../../components/form/device/AmicoCreateDeviceForm";


const HEADER = ["Name", "Type", "Mac","Firmware", "IP","Port", "Transction", "Configuration", "Status", "Action"];
const KEY = ["name", "hardwareTypeDetail", "mac","firmware", "ip","port", "tranStatus"];
// Hardware Page
const ID_REPORT_KEY = [ "scpId",'mac','fw','serialNumber'];
const ID_REPORT_TABLE_HEADER = ["Id", "Mac", "Firmware","Serial No", "Action"];


const Device = () => {
  const { FlashLoading } = useLoading();
  const { idReports,setIdReports } = useIdReport();
  const {setPagination} = usePagination();
  const { locationId } = useLocation();
  const { toggleToast } = useToast();
  const { filterPermission,token } = useAuth();
  const { setCreate,setRemove,setUpdate,setConfirmCreate,setConfirmRemove,setConfirmUpdate,setMessage,setInfo  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);

  let ScanTableTemplate: ReactNode;



  const defaultDto: HardwareDto = {
    // Base
    locationId: locationId,
    isActive: true,

    // Define
    name: "",
    ip: "",
    serialNumber: "",
    isUpload: false,
    isReset: false,
    hardwareType: 1,
    hardwareTypeDetail: "",
    firmware: "",
    port: "",
    modules: [],
    portOne: false,
    portTwo: false,
    protocolOne: 0,
    protocolOneDetail: "",
    baudRateOne: -1,
    protocolTwo: 0,
    protocolTwoDetail: "",
    baudRateTwo: -1,
    id: 0,
    scpId: 0,
    mac: "",
    lastSync: new Date()
  }

   const defaultCreateDto: CreateDeviceDto = {
     // Base
     locationId: locationId,

     // Define
     name: "",
     serialNumber: "",
     fw: "",
     componentId: 0,
     mac: "",
     syncedAt: new Date(),
     type: "",
     status: "PENDING"
   }
  


  {/* Modal Handler */ }
  const [scan, setScan] = useState<boolean>(false);
  const [selectType,setSelectType] = useState<boolean>(false);
  const [form,setForm] = useState<boolean>(false);
  const [formType,setFormType] = useState<FormType>(FormType.CREATE);

  // Upload Modal
  const [deviceType,setDeviceType] = useState<string>("");
  const handleCloseModal = () => setScan(false);
  const handleCloseSelect = () => setSelectType(false);



  {/* IdReport */ }
  const handleAddIdReport = async (data: IdReport) => {
    setCreateDto({
      name:"",
      componentId:data.scpId,
      mac:data.mac,
      serialNumber:data.serialNumber,
      fw:data.fw,
      type:"aero",
      syncedAt:new Date(),
      status:"PENDING",
      locationId:locationId
    });
    console.log(data);
    setScan(false);
    setForm(true);
  }


  ScanTableTemplate = (
    <>
      <div className="max-h-[70vh] overflow-y-auto hidden-scroll">
        <Table>
          {/* Table Header */}
          <TableHeader className="border-b border-gray-100 dark:border-white/[0.05] bg-white dark:bg-gray-900 sticky top-0 z-10">
            <TableRow>
              {ID_REPORT_TABLE_HEADER.map((head: string, i: number) =>
                <TableCell
                  key={i}
                  isHeader
                  className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                >
                  {head}
                </TableCell>
              )}
            </TableRow>
          </TableHeader>
          <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
            { 
            
             idReports.map((data: any, i: number) => (
              <TableRow key={i}>
                {ID_REPORT_KEY.map((key: string, i: number) =>
                  <TableCell key={i} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
                    {String(data[key as keyof typeof data])}
                  </TableCell>
                )}
                <TableCell>
                  <Button onClick={() => handleAddIdReport(data)} size="sm" variant="primary">
                    Add
                  </Button>
                </TableCell>
              </TableRow>
            ))
            }

          </TableBody>
        </Table>
      </div>

    </>
  );



  {/* Hardware Data */ }
  const [deviceDto,setDeviceDto] = useState<HardwareDto>(defaultDto);
  const [createDto,setCreateDto] = useState<CreateDeviceDto>(defaultCreateDto);
  const [data, setData] = useState<HardwareDto[]>([]);
  const [status, setStatus] = useState<StatusDto[]>([]);
  const [tranStatus, setTranStatus] = useState<TranStatusDto[]>([]);
  const fetchData = async (pageNumber: number, pageSize: number,locationId?:number,search?: string, startDate?: string, endDate?: string) => {
    const res = await send.get(DeviceEndpoint.PAGINATION(pageNumber,pageSize,locationId,search, startDate, endDate))
    if (res && res.data.data) {
      setData(res.data.data.data);
      setPagination(res.data.data.page);
      // Batch set state
      const newStatuses = res.data.data.data.map((a: HardwareDto) => ({
        scpId: a.scpId,
        driverId: a.scpId,
        status: -1,
        tamper: -1,
        ac: -1,
        batt: -1
      }));

      const newTranStatuses = res.data.data.data.map((a: HardwareDto) => ({
        scpId: a.scpId,
        capacity: 0,
        oldest: 0,
        lastReport: 0,
        lastLog: 0,
        disabled: 0,
      }));

      console.log(newStatuses);

      setTranStatus((prev) => [...prev, ...newTranStatuses])
      setStatus((prev) => [...prev, ...newStatuses]);
      console.log(res.data.data)
      // Fetch status for each
      res.data.data.data.forEach((a: HardwareDto) => {
        fetchStatus(a.id);
        fetchTransactionStatus(a.mac);
      });
    }

  }

  const setTran = async (data:SetTranDto[]) => {
    var res = await send.post(DeviceEndpoint.TRAN_RANGE,data);
    if(Helper.handleToastByResCode(res,HardwareToast.TOGGLE_TRAN,toggleToast)){
      toggleRefresh();
    }
  }
  const fetchStatus = async (id: number) => {
    const res = await send.get(DeviceEndpoint.STATUS(id));
    console.log(res)
    if (res && res.data.data) {
      setStatus((prev) => prev.map((a) =>
        a.scpId == res.data.data.scpId
          ? {
            ...a,
            status: res.data.data.status,
          }
          : {
            ...a
          }
      )
      );
    }
  }

  const fetchTransactionStatus = async (mac: string) => {
    const res = await send.get(DeviceEndpoint.TRAN(mac));
    if (res && res.data.data) {
      setTranStatus((prev) => prev.map((a: TranStatusDto) =>
        a.scpId == res.data.data.scpId ? {
          ...a,
        } : {
          ...a
        }
      ))
    }
  }

  const resetDevice = async (ScpMac: string) => {
    const res = await send.post(DeviceEndpoint.RESET(ScpMac))
    if (Helper.handleToastByResCode(res, HardwareToast.RESET, toggleToast)) {
      toggleRefresh();
    }
  }

  const uploadConfig = async (id: number) => {
    const res = await send.post(DeviceEndpoint.UPLOAD(id))
    if (Helper.handleToastByResCode(res, HardwareToast.UPLOAD, toggleToast)) {
      toggleRefresh();
    }
  }


  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDeviceDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  }



  {/* Handle Action Table*/ }
  const handleEdit = (data: HardwareDto) => {
    setFormType(FormType.UPDATE)
    setDeviceDto({
      // Base
      id :data.id,
      scpId: data.scpId,
      name: data.name,
      hardwareType: data.hardwareType,
      hardwareTypeDetail: data.hardwareTypeDetail,
      mac: data.mac,
      ip: data.ip,
      firmware: data.firmware,
      port: data.port,
      modules: data.modules,
      serialNumber: data.serialNumber,
      isUpload: data.isUpload,
      isReset: data.isReset,
      portOne: data.portOne,
      portTwo: data.portTwo,
      protocolOne:data.protocolOne,
      baudRateOne:data.baudRateOne,
      protocolOneDetail:data.protocolOneDetail,
      protocolTwo:data.protocolTwo,
      protocolTwoDetail:data.protocolTwoDetail,
      baudRateTwo:data.baudRateTwo,
      lastSync:new Date(),

      locationId: data.locationId,
      isActive: true,
    })
    setForm(true);
  }

  const handleRemove = (data: HardwareDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(DeviceEndpoint.DELETE(data.id));
      if(Helper.handleToastByResCode(res,HardwareToast.DELETE,toggleToast)){
        setDeviceDto(defaultDto)
        toggleRefresh();
      }
    })
    setRemove(true);

  }
  const handleInfo = (data:HardwareDto) => {
    setFormType(FormType.INFO);
    setDeviceDto(data);
    setForm(true);
  }
  {/* Handle Click */ }
  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setSelectType(true);
        break;
      case "report":
        if (select.length == 0) {
          setMessage("Please select object")
          setInfo(true);
        } else {
          let data:SetTranDto[] = []
          select.map((a:HardwareDto) => {
            data.push({
              macAddress:a.mac,
              param:1
            });
          })
          setTran(data);
          
        }
        break;
      case "delete":
        if (select.length == 0) {
          setMessage("Please select object")
          setInfo(true);
        } else {
          setConfirmRemove(() => async () => {
            var data: number[] = [];
            select.map(async (a: HardwareDto) => {
              data.push(a.id)
            })
            var res = await send.post(DeviceEndpoint.DELETE_RANGE, data)
            if (Helper.handleToastByResCode(res, HardwareToast.DELETE_RANGE, toggleToast)) {
              setRemove(false);
              toggleRefresh();
            }
          })
          setRemove(true);
        }
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(DeviceEndpoint.UPDATE,deviceDto);
          if(Helper.handleToastByResCode(res,HardwareToast.UPDATE,toggleToast)){
            setForm(false);
            toggleRefresh();
            setDeviceDto(defaultDto);
          }
        })
        setUpdate(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const res = await send.post(DeviceEndpoint.CREATE,createDto);
          if(Helper.handleToastByResCode(res,HardwareToast.CREATE,toggleToast)){
            toggleRefresh();
            setForm(false);
            setCreateDto(defaultCreateDto);
          }
        })
        setCreate(true);
        break;
      case "type":
        setForm(true)
        break;
      case "scan":
        setScan(true);
        // fetchIdReport();
        break;
      case "close":
        setForm(false)
        setCreateDto(defaultCreateDto)
        setDeviceDto(defaultDto)
        break;
      case "reset":
        if (select.length != 0) {
          select.map((a: HardwareDto) => {
            resetDevice(a.mac);
          })

        } else {
          alert("No selected object")
        }
        break;
      case "upload":
        if (select.length != 0) {
          select.map((a: HardwareDto) => {
            uploadConfig(a.id);
          })

        } else {
          alert("No selected object")
        }
        break;
      default:
        break;
    }
  }

  {/* checkBox */ }
  const [select, setSelect] = useState<HardwareDto[]>([]);

  const fetchIdReport = async () => {
    var res = await send.get(DeviceEndpoint.ID_REPORT);
   setIdReports(res.data);
  }


  {/* UseEffect */ }
  useEffect(() => {
    const initSignalR = async () => {


      if (!token)
        return;

      // ⭐ ensure connection is started
      await SignalRService.startConnection();

      const connection = SignalRService.getConnection();
      if (!connection) return;

      // ⭐ register handlers FIRST
      connection.on(SignalRTopic.IDREPORT, (idreports: IdReport[]) => {
        console.log("Received realtime update:", idreports);
        setIdReports(idreports);
      });

      // ⭐ THEN join group
      try {
        await SignalRService.joinGroup(SignalRTopic.IDREPORT);
        console.log("Joined group:");
      } catch (err) {
        console.error("Subscribe error:", err);
      }


      // initial load
      fetchIdReport();
    };

    initSignalR();


    return () => {
      const connection = SignalRService.getConnection();
      connection?.off(SignalRTopic.IDREPORT);
    };
  }, [refresh, locationId]);


  const actionBtn: ActionButton[] = [
    {
      buttonName: "Reset",
      lable: "reset",
      icon: <ResetIcon />
    }, {
      buttonName: "Upload",
      lable: "upload",
      icon: <UploadIcon />
    },
    {
      buttonName: "Transfer",
      lable: "transfer",
      icon: <TransferIcon />
    },{
      buttonName:"Report Toggle",
      lable:"report",
      icon:<ToggleTranIcon/>
    },
    {
      buttonName: "Scan",
      lable: "scan",
      icon: <>
        <ScanIcon className={idReports.length != 0 ? "animate-ping" : ""} />
      </>
    }
  ];
  const renderOptional = (data: HardwareDto, statusDto: StatusDto[],index:number) => {
    console.log(data)
    console.log(statusDto)
    console.log(statusDto.find(b => b.scpId == data.scpId)?.status)
    return [
      <TableCell key={index} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        <>
          {
            data.isReset == true && statusDto.find(b => b.scpId == data.scpId && b.driverId == data.id)?.status == 0 ?
              <FlashLoading />
              :
              data.isReset == true ?
                <Badge
                  variant="solid"
                  size="sm"
                  color="error"
                >
                  Reset Require
                </Badge>
                : data.isUpload == true ?
                  <Badge
                    variant="solid"
                    size="sm"
                    color="warning"
                  >
                    Upload Require
                  </Badge>
                  :
                  <Badge
                    variant="solid"
                    size="sm"
                    color="success"
                  >
                    Synced
                  </Badge>
          }

        </>
      </TableCell>,
      <TableCell key={index+1} className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">
        {data.isReset || data.isUpload ?
          <Badge
            size="sm"
            color="error"
          >
            Error
          </Badge>
          :
          <Badge
            size="sm"
            color={
              statusDto.find(b => b.scpId == data.scpId)?.status == 1
                ? "success"
                : statusDto.find(b => b.scpId == data.scpId)?.status == 0
                  ? "error"
                  : "warning"
            }
          >
            {statusDto.find(b => b.scpId == data.scpId)?.status == 1 ? "Online" : statusDto.find(b => b.scpId == data.scpId)?.status == 0 ? "Offline" : statusDto.find(b => b.scpId == data.scpId)?.status}
          </Badge>

        }
      </TableCell>
    ]
  }

  {/* Form */ }
  const tabContent: FormContent[] = [
    {
      icon: <HardwareIcon />,
      label: "Hardware",
      content: <DeviceForm handleClick={handleClickWithEvent}  dto={deviceDto} setDto={setDeviceDto} type={formType} />
    }, {
      icon: <HardwareIcon />,
      label: "Memory Allocate",
      content: <HardwareMemAllocForm data={deviceDto} />
    }, {
      icon: <HardwareIcon />,
      label: "Component",
      content: <HardwareComponentForm data={deviceDto} />
    }
  ];

  const createAmicoContent: FormContent[] = [
    {
      icon: <HardwareIcon />,
      label: "AMICO",
      content: <AmicoCreateDeviceForm handleClick={handleClickWithEvent}  dto={createDto} setDto={setCreateDto} type={formType} />
    }
  ]

  const createAeroContent: FormContent[] = [
    {
      icon: <HardwareIcon />,
      label: "AERO",
      content: <AeroCreateDeviceForm handleClick={handleClickWithEvent}  dto={createDto} setDto={setCreateDto} type={formType} />
    }
  ]


  return (
    <>

      {/* {select &&ด
        <Modals header="Hardware Select" body={<SelectDeviceForm setDto={setHardwareType} handleClick={handleClickWithEvent} />} handleClickWithEvent={handleCloseSelectDevice} />
      } */}
      {scan &&
        <Modals header="Host List" body={ScanTableTemplate} handleClickWithEvent={handleCloseModal} />
      }

      {
        selectType && 

        <Modals body={<SelectDeviceForm setDeviceType={setDeviceType} setSelectType={setSelectType} setForm={setForm} />} handleClickWithEvent={handleCloseSelect}  />
      }

      <PageBreadcrumb pageTitle="Hardware" />
      <div className="space-y-6">
        {form ?
          <>

            {deviceType == DeviceType.AERO ?
              <BaseForm tabContent={createAeroContent} header="Aero Form" desc="Form used for create device." />
             :
              <BaseForm tabContent={createAmicoContent} header="Amico Form" desc="Form used for create device." />
             }
            
          </>

          :
          <BaseTable<HardwareDto> refresh={refresh} headers={HEADER} keys={KEY} data={data} onEdit={handleEdit} onRemove={handleRemove} onInfo={handleInfo} onClick={handleClickWithEvent} select={select} setSelect={setSelect} permission={filterPermission(FeatureId.device)} action={actionBtn} renderOptionalComponent={renderOptional} status={status} locationId={locationId} fetchData={fetchData} specialDisplay={[
            {
              key: "tranStatus",
              content: (a, i) => <TableCell key={i} className="px-4 py-3 text-gray-500 text-center text-theme-sm dark:text-gray-400">
                <Badge size="sm" color={tranStatus.find(x => x.scpId == a.scpId)?.disabled == 0 && tranStatus.find(x => x.scpId == a.scpId)?.status ? "success" : "error"}>
                  {tranStatus.find(x => x.scpId == a.scpId)?.status ?? "Unknown"}
                </Badge>
              </TableCell>
            }
          ]} />
        }
      </div>
    </>
  );
}


export default Device;