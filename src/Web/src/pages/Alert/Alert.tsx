import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseTable } from "../UiElements/BaseTable";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { send } from "../../api/api";
import { AlertEndpoint } from "../../endpoint/AlertEndpoint";
import Helper from "../../utility/Helper";
import { useToast } from "../../context/ToastContext";
import { useLocation } from "../../context/LocationContext";
import { TriggerIcon } from "../../icons";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { FormType } from "../../model/Form/FormProp";
import { ProcedureToast } from "../../model/ToastMessage";
import { usePopup } from "../../context/PopupContext";
import { usePagination } from "../../context/PaginationContext";
import { ActionEndpoint } from "../../endpoint/ActionEndpoint";
import AlertForm from "./AlertForm";
import { AlertDto } from "../../model/Alert/AlertDto";
import { TableCell } from "../../components/ui/table";


const HEADERS: string[] = ["Name", "Level", "Enabled", "Action"];
const KEYS: string[] = ["name", "level"];

export const AlertPage = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [selectedObject, setSelectedObjects] = useState<AlertDto[]>([]);
  const [alertDtos, setAlertDtos] = useState<AlertDto[]>([]);
  const {
    setRemove,
    setConfirmRemove,
    setConfirmCreate,
    setCreate,
    setUpdate,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const { setPagination } = usePagination();
  const [refresh, setRefresh] = useState<boolean>(false);
  const defaultDto: AlertDto = {
    guid: "",
    name: "",
    advice: "",
    locationGuid: locationGuid,
    locationName: "",
    alertLevelGuid: "",
    coreEventGuid: "",
    isActive: false,
    isDefault: false,
    alertLevelColor: "",
    coreEntityGuid: "",
    alertLevelName: ""
  };
  const [dto, setDto] = useState<AlertDto>(defaultDto);

  const toggleRefresh = () => setRefresh((prev) => !prev);

  const handleRemove = (data: AlertDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(AlertEndpoint.DELETE(data.guid));
      if (
        Helper.handleToastByResCode(res, ProcedureToast.DELETE, toggleToast)
      ) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: AlertDto) => {
    console.log(data)
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: AlertDto) => {
    console.log(data)
    setDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObject.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          var data: string[] = [];
          selectedObject.map((a: AlertDto) => {
            data.push(a.guid);
          });
          var res = await send.post(AlertEndpoint.DLETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              ProcedureToast.DELETE_RANGE,
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
          dto.locationGuid = locationGuid;
          const res = await send.post(AlertEndpoint.CREATE, dto);
          if (
            Helper.handleToastByResCode(res, ProcedureToast.CREATE, toggleToast)
          ) {
            setForm(false);
            setDto(defaultDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(AlertEndpoint.UPDATE, dto);
          if (
            Helper.handleToastByResCode(res, ProcedureToast.UPDATE, toggleToast)
          ) {
            setForm(false);
            setDto(defaultDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setDto(defaultDto);
        setForm(false);
        break;
      default:
        break;
    }
  };

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(AlertEndpoint.PAGINATION(pageNumber, pageSize, locationGuid, search, startDate, endDate));
    if (res.data) {
      setAlertDtos(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  const tabContent: FormContent[] = [
    {
      label: "Alerts",
      icon: <TriggerIcon />,
      content: (
        <AlertForm
          handleClick={handleClick}
          dto={dto}
          setDto={setDto}
          type={formType}
          handleChange={() => { }}
        />

      ),
    },
  ];



  return (
    <>
      <PageBreadcrumb pageTitle="Alerts" />

      {form ? (
        <BaseForm handleClick={handleClick} tabContent={tabContent} type={formType} desc="" />
      ) : (
        <BaseTable<AlertDto>
          refresh={refresh}
          keys={KEYS}
          headers={HEADERS}
          data={alertDtos}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onClick={handleClick}
          select={selectedObject}
          setSelect={setSelectedObjects}
          permission={filterPermission(FeatureId.trigger)}
          fetchData={fetchData}
          locationGuid={locationGuid}
          specialDisplay={[
            {
              key: "level",
              content: (data, i) => (
                <TableCell className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400">

                  <label className="relative flex h-9 w-20 shrink-0 cursor-pointer items-center justify-center rounded-md border border-gray-200 shadow-inner dark:border-gray-700">
                    <div
                      className="flex h-full w-full items-center justify-center rounded-md text-white transition-transform hover:scale-105"
                      style={{ backgroundColor: data.alertLevelColor }}
                    >
                      {data.alertLevelName}
                    </div>
                  </label>
                </TableCell>
              ),
            }
          ]}
        />
      )}
    </>
  );
};
