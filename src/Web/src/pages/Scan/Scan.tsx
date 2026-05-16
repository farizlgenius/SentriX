import { ReactNode, useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { IdReport } from "../../model/IdReport/IdReport";
import { DeviceEndpoint } from "../../endpoint/HardwareEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { Table, TableBody, TableCell, TableHeader, TableRow } from "../../components/ui/table";
import { useIdReport } from "../../context/IdReportContext";
import Button from "../../components/ui/button/Button";
import { BaseForm } from "../UiElements/BaseForm";
import { HardwareIcon } from "../../icons";
import { FormContent } from "../../model/Form/FormContent";
import AeroCreateDeviceForm from "../../components/form/device/AeroCreateDeviceForm";
import { FormType } from "../../model/Form/FormProp";
import { CreateAeroDeviceDto } from "../../model/Device/CreateAeroDeviceDto";
import Helper from "../../utility/Helper";
import { HardwareToast } from "../../model/ToastMessage";
import { usePopup } from "../../context/PopupContext";
import { useToast } from "../../context/ToastContext";
import { mapFields } from "../../utility/Mapper";
import { CreateDeviceStrMetadataDto } from "../../model/Device/CreateDeviceStrMetadataDto";




// Hardware Page
const ID_REPORT_KEY = ["scpId", 'mac', 'fw', 'serialNumber'];
const ID_REPORT_TABLE_HEADER = ["Id", "Mac", "Firmware", "Serial No", "Action"];


const Scan = () => {
      const { idReports,setIdReports } = useIdReport();
      const { locationId } = useLocation();
      const { setCreate, setConfirmCreate } = usePopup();
      const toggleRefresh = () => setRefresh(!refresh);
      const { toggleToast } = useToast();
      const [refresh, setRefresh] = useState(false);
      const [form, setForm] = useState(false);

      const defaultCreateDto: CreateAeroDeviceDto = {
            // Base
            locationId: locationId,

            // Define
            name: "",
            serialNumber: "",
            fw: "",
            componentId: 0,
            mac: "",
            syncedAt: new Date(),
            type: "AERO",
            status: "PENDING",
            ip: "",
            port: 0,
            metadata: {
                  portOne: false,
                  protocolOne: -1    ,
                  baudRateOne: -1,
                  portTwo: false,
                  protocolTwo: -1,
                  baudRateTwo: -1
            }
      }

      const [dto, setDto] = useState<CreateAeroDeviceDto>(defaultCreateDto);

       function mapDto(api: CreateAeroDeviceDto): CreateDeviceStrMetadataDto {
            const { metadata, ...rest } = api
            return mapFields(rest, { metadata: JSON.stringify(metadata) })
      }



      const handleAdd = (data: IdReport) => {
            setForm(true);
            setDto({
                  locationId: locationId,
                  name: "",
                  serialNumber: data.serialNumber,
                  fw: data.fw,
                  componentId: data.scpId,
                  mac: data.mac,
                  syncedAt: new Date(),
                  type: "AERO",
                  status: "PENDING",
                  ip: "",
                  port: 0,
                  metadata: {
                        portOne: false,
                        protocolOne: -1,
                        baudRateOne: -1,
                        portTwo: false,
                        protocolTwo: -1,
                        baudRateTwo: -1
                  }     
            })
      }

      const handleClick = async (e: React.MouseEvent<HTMLButtonElement>) => {
            switch (e.currentTarget.name) {
                  case "create":
                        setConfirmCreate(() => async () => {
                              var req:CreateDeviceStrMetadataDto = mapDto(dto);
                              const res = await send.post(DeviceEndpoint.CREATE, req);
                              if (Helper.handleToastByResCode(res, HardwareToast.CREATE, toggleToast)) {
                                    toggleRefresh();
                                    setForm(false);
                                    setDto(defaultCreateDto);
                                    setIdReports(prev => prev.filter(r => r.scpId !== dto.componentId));
                              }
                        })
                        setCreate(true);
                        break;
                  case "close":
                        setForm(false);
                        setDto(defaultCreateDto);
                        break;
                  default:
                        break;
            }
      }

      const createAeroContent: FormContent[] = [
            {
                  icon: <HardwareIcon />,
                  label: "AERO",
                  content: <AeroCreateDeviceForm handleClick={handleClick} dto={dto} setDto={setDto} type={FormType.CREATE} />
            }
      ]




      return (
            <>
                  <PageBreadcrumb pageTitle="Scan" />
                  {
                        form ?
                              <BaseForm tabContent={createAeroContent} header="Aero Form" desc="Form used for create device." />

                              :
                              <div className="space-y-6">
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
                                                                                    <Button onClick={() => handleAdd(data)} size="sm" variant="primary">
                                                                                          Add
                                                                                    </Button>
                                                                              </TableCell>
                                                                        </TableRow>
                                                                  ))
                                                            }
                                                            <></>

                                                      </TableBody>
                                                </Table>
                                          </div>

                                    </>
                              </div>

                  }

            </>
      );
}


export default Scan;