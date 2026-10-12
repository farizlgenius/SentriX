import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseTable } from "../UiElements/BaseTable";
import { ProcedureDto } from "../../model/Procedure/ProcedureDto";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import RemoveModal from "../UiElements/RemoveModal";
import { send } from "../../api/api";
import { ProcedureEndpoint } from "../../endpoint/ProcedureEndpoint";
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
import { ActionDto } from "../../model/Action/ActionDto";
import { ActionEndpoint } from "../../endpoint/ActionEndpoint";
import ActionForm from "./ActionForm";
import { ActionType } from "../../enum/ActionType";

const HEADERS: string[] = ["Name", "Action"];
const KEYS: string[] = ["name"];

export const Action = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [selectedObject, setSelectedObjects] = useState<ActionDto[]>([]);
  const [actionDtos, setActionDtos] = useState<ActionDto[]>([]);
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
  const defaultDto: ActionDto = {
    name: "",
    guid: "",
    metadata: "",
    isActive: true,
    isDefault: false,
    type: ActionType.DeleteAll
  };
  const [dto, setDto] = useState<ActionDto>(defaultDto);

  const toggleRefresh = () => setRefresh((prev) => !prev);

  const handleRemove = (data: ActionDto) => {
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
  const handleEdit = (data: ActionDto) => {
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: ActionDto) => {
    setDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
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
          selectedObject.map((a: ActionDto) => {
            data.push(a.guid);
          });
          var res = await send.post(ProcedureEndpoint.DLETE_RANGE, data);
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
          const res = await send.post(ProcedureEndpoint.CREATE, dto);
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
          const res = await send.put(ProcedureEndpoint.UPDATE, dto);
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
    const res = await send.get(
      ActionEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res && res.data.data) {
      setActionDtos(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  const tabContent: FormContent[] = [
    {
      label: "Action",
      icon: <TriggerIcon />,
      content: (
        <ActionForm
          handleClick={handleClick}
          dto={dto}
          setDto={setDto}
          type={formType}
          handleChange={()=>{}}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Actions" />

      {form ? (
        <BaseForm tabContent={tabContent} type={formType} />
      ) : (
        <BaseTable<ActionDto>
          refresh={refresh}
          keys={KEYS}
          headers={HEADERS}
          data={actionDtos}
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
