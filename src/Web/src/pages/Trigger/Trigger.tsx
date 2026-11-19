import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { FormContent } from "../../model/Form/FormContent";
import { BaseForm } from "../UiElements/BaseForm";
import { BaseTable } from "../UiElements/BaseTable";
import { TriggerDto } from "../../model/Trigger/TriggerDto";
import { useLocation } from "../../context/LocationContext";
import { TriggerIcon } from "../../icons";
import { TriggerForm } from "./TriggerForm";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { send } from "../../api/api";
import { TriggerEndpoint } from "../../endpoint/TriggerEndpoint";
import Helper from "../../utility/Helper";
import { useToast } from "../../context/ToastContext";
import { usePagination } from "../../context/PaginationContext";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { IntervalToast, TriggerToast } from "../../model/ToastMessage";
import { EntityType } from "../../enum/EntityType";
import { TriggerCommand } from "../../enum/TriggerCommand";
import { HttpStatusCode } from "axios";

const KEYS: string[] = ["name"];
const HEADERS: string[] = ["Name", "Action"];

export const Trigger = () => {
  const { setPagination } = usePagination();
  const { locationGuid: locationId } = useLocation();
  const {
    setCreate,
    setUpdate,
    setInfo,
    setRemove,
    setConfirmRemove,
    setConfirmCreate,
    setConfirmUpdate,
    setMessage,
  } = usePopup();
  const { toggleToast } = useToast();
  const { filterPermission } = useAuth();
  const [selectedObject, setSelectedObjects] = useState<TriggerDto[]>([]);
  const [triggerDtos, setTriggerDtos] = useState<TriggerDto[]>([]);
  const [refresh, setRefresh] = useState<boolean>(false);
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const toggleRefresh = () => setRefresh((prev) => !prev);
  const defaultDto: TriggerDto = {
    guid: "",
    name: "",
    command: TriggerCommand.ABORT_DELAY,
    device: "",
    triggerSource: "",
    triggerType: "",
    triggerCodes: [],
    triggerArgs: [],
    firstTriggerVariable: null,
    secondTriggerVariable: null,
    thirdTriggerVariable: null,
    forthTriggerVariable: null,
    timeZone: "",
    procedure: "",
    src: "",
    srcType: EntityType.AUTH,
    isActive: false,
    isDefault: false
  };
  const [dto, setDto] = useState<TriggerDto>(defaultDto);

  {
    /* handle Table Action */
  }
  const handleEdit = (data: TriggerDto) => {
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleRemove = (data: TriggerDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(TriggerEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, IntervalToast.DELETE, toggleToast)) {
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (data: TriggerDto) => {
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
          selectedObject.map(async (a: TriggerDto) => {
            data.push(a.guid);
          });
          var res = await send.post(TriggerEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              TriggerToast.DELETE_RANGE,
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
          const res = await send.post(TriggerEndpoint.CREATE, dto);
          if (
            Helper.handleToastByResCode(res, TriggerToast.CREATE, toggleToast)
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
          const res = await send.put(TriggerEndpoint.UPDATE, dto);
          if (
            Helper.handleToastByResCode(res, TriggerToast.UPDATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
          } else {
            setForm(false);
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

  const tabContent: FormContent[] = [
    {
      label: "Trigger",
      icon: <TriggerIcon />,
      content: (
        <TriggerForm
          handleClick={handleClick}
          dto={dto}
          setDto={setDto}
          type={formType} handleChange={() => {}}        />
      ),
    },
  ];

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      TriggerEndpoint.PAGINATION(
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
      console.log(res.data.data);
      setTriggerDtos(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  return (
    <>
      <PageBreadcrumb pageTitle="Trigger" />
      {form ? (
        <BaseForm tabContent={tabContent} type={formType} handleClick={handleClick} />
      ) : (
        <BaseTable<TriggerDto>
          keys={KEYS}
          headers={HEADERS}
          data={triggerDtos}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onClick={handleClick}
          select={selectedObject}
          setSelect={setSelectedObjects}
          permission={filterPermission(FeatureId.trigger)}
          fetchData={fetchData}
          locationGuid={locationId}
        />
      )}
    </>
  );
};
