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
import { Options } from "../../model/Options";
import { EventEndpoint } from "../../endpoint/TransactionEndpoint";

const HEADERS: string[] = ["Name", "Action"];
const KEYS: string[] = ["name"];

export const AlertPage = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [selectedObject, setSelectedObjects] = useState<AlertDto[]>([]);
  const [alertDto, setAlertDto] = useState<AlertDto[]>([]);
  const [events,setEvents] = useState<Options[]>([]);
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
    locationGuid: "",
    locationName: "",
    alertLevelGuid: "",
    alertLevel: "",
    baseEventGuids: [],
    isActive: false,
    isDefault: false
  };
  const [dto, setDto] = useState<AlertDto>(defaultDto);

  const toggleRefresh = () => setRefresh((prev) => !prev);

  const handleRemove = (data: AlertDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(ActionEndpoint.DELETE(data.guid));
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
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: AlertDto) => {
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
    const res = await send.get(EventEndpoint.GET_BASE);
    console.log(res);
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
          handleChange={() => {}}
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
          data={alertDto}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onClick={handleClick}
          select={selectedObject}
          setSelect={setSelectedObjects}
          permission={filterPermission(FeatureId.trigger)}
          fetchData={fetchData}
          locationGuid={locationGuid}
        />
      )}
    </>
  );
};
