import { useState } from "react";
import { BaseForm } from "../UiElements/BaseForm";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { FormContent } from "../../model/Form/FormContent";
import { LocationIcon } from "../../icons";
import { useToast } from "../../context/ToastContext";
import Helper from "../../utility/Helper";
import { CompanyToast } from "../../model/ToastMessage";
import { BaseTable } from "../UiElements/BaseTable";
import api, { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { usePopup } from "../../context/PopupContext";
import { FeatureId } from "../../enum/FeatureId";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { useLocation } from "../../context/LocationContext";
import { CompanyDto } from "../../model/Company/CompanyDto";
import { CompanyEndpoint } from "../../endpoint/CompanyEndpoint";
import { CompanyForm } from "./CompanyForm";

const HEADER: string[] = ["Name", "Address", "Action"];
const KEY: string[] = ["name", "address"];

export const Company = () => {
  const { locationGuid } = useLocation();
  const { toggleToast } = useToast();
  const { setPagination } = usePagination();
  const { filterPermission } = useAuth();
  const {
    setRemove,
    setConfirmRemove,
    setConfirmCreate,
    setInfo,
    setMessage,
    setCreate,
    setUpdate,
    setConfirmUpdate,
  } = usePopup();
  const [form, setForm] = useState<boolean>(false);
  const [refresh, setRefresh] = useState<boolean>(false);
  const toggleRefresh = () => setRefresh(!refresh);
  const defaultDto: CompanyDto = {
    guid: "",
    name: "",
    description: "",
    address: "",
    isActive: true,
    isDefault: false,
  };

  const [dto, setDto] = useState<CompanyDto>(defaultDto);
  const [dtos, setDtos] = useState<CompanyDto[]>([]);
  const [select, setSelect] = useState<CompanyDto[]>([]);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  const handleRemove = (data: CompanyDto) => {
    setRemove(true);
    setConfirmRemove(() => async () => {
      const res = await api.delete(CompanyEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, CompanyToast.DELETE, toggleToast)) {
        toggleRefresh();
      }
    });
  };

  const handleInfo = (data: CompanyDto) => {
    setFormType(FormType.INFO);
    setDto(data);
    setForm(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: CompanyDto) => {
    setFormType(FormType.UPDATE);
    setDto(data);
    setForm(true);
  };

  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (select.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          setConfirmRemove(() => async () => {
            const data: string[] = [];
            select.map(async (a: CompanyDto) => {
              data.push(a.guid);
            });
            const res = await send.deleteBody(
              CompanyEndpoint.DELETE_RANGE,
              data,
            );
            if (
              Helper.handleToastByResCode(
                res,
                CompanyToast.DELETE_RANGE,
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
      case "create":
        setConfirmCreate(() => async () => {
          const res = await send.post(CompanyEndpoint.CREATE, dto);
          if (
            Helper.handleToastByResCode(res, CompanyToast.CREATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
            setDto(defaultDto);
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await api.put(CompanyEndpoint.UPDATE, dto);
          if (
            Helper.handleToastByResCode(res, CompanyToast.UPDATE, toggleToast)
          ) {
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

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      CompanyEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setDtos(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* Form */
  }
  const tabContent: FormContent[] = [
    {
      icon: <LocationIcon />,
      label: "Intevals",
      content: (
        <CompanyForm
          type={formType}
          dto={dto}
          setDto={setDto}
          handleClick={handleClickWithEvent}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Companies" />
      {form ? (
        <BaseForm tabContent={tabContent} header={""} desc={""} />
      ) : (
        <div className="space-y-6">
          <BaseTable<CompanyDto>
            headers={HEADER}
            keys={KEY}
            data={dtos}
            select={select}
            setSelect={setSelect}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onClick={handleClickWithEvent}
            permission={filterPermission(FeatureId.location)}
            onInfo={handleInfo}
            fetchData={fetchData}
            refresh={refresh}
            locationGuid={locationGuid}
          />
        </div>
      )}
    </>
  );
};
