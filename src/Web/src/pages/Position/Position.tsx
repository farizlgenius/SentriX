import { useEffect, useState } from "react";
import { BaseForm } from "../UiElements/BaseForm";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { FormContent } from "../../model/Form/FormContent";
import { LocationIcon } from "../../icons";
import { useToast } from "../../context/ToastContext";
import Helper from "../../utility/Helper";
import { PositionToast } from "../../model/ToastMessage";
import { BaseTable } from "../UiElements/BaseTable";
import api, { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { usePopup } from "../../context/PopupContext";
import { FeatureId } from "../../enum/FeatureId";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { PositionDto } from "../../model/Position/PositionDto";
import { PositionEndpoint } from "../../endpoint/PositionEndpoint";
import { PositionForm } from "./PositionForm";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { DepartmentEndpoint } from "../../endpoint/DepartmentEndpoint";
import { CompanyDto } from "../../model/Company/CompanyDto";
import { CompanyEndpoint } from "../../endpoint/CompanyEndpoint";
import { DepartmentDto } from "../../model/Department/DepartmentDto";

const HEADER: string[] = ["Name", "Action"];
const KEY: string[] = ["name"];

export const Position = () => {
  const [selectedDepartment, setSelectedDepartment] = useState<string>("");
  const [departmentOptions, setDepartmentOptions] = useState<Options[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<string>("");
  const [companyOptions, setCompanyOptions] = useState<Options[]>([]);
  const defaultDto: PositionDto = {
    guid: "",
    name: "",
    description: "",
    departmentGuid: selectedDepartment,
    isActive: true,
    isDefault: false,
    companyGuid: "",
    company: "",
    department: "",
  };

  const { toggleToast, updateToast } = useToast();
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
  const [positionDto, setPositionDto] = useState<PositionDto>(defaultDto);
  const [positionsDto, setPositionsDto] = useState<PositionDto[]>([]);
  const [select, setSelect] = useState<PositionDto[]>([]);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  const createPendingToast = (message: string) =>
    toggleToast("pending", message);

  const resolveRequestToast = (
    toastId: string,
    res: any,
    successMessage: string,
  ) =>
    Helper.handleToastByResCode(
      res,
      successMessage,
      toggleToast,
      updateToast,
      toastId,
    );

  const handleRemove = (data: PositionDto) => {
    setRemove(true);
    setConfirmRemove(() => async () => {
      const toastId = createPendingToast("Removing position...");
      const res = await api.delete(PositionEndpoint.DELETE(data.guid));
      if (resolveRequestToast(toastId, res, PositionToast.DELETE)) {
        toggleRefresh();
      }
    });
  };

  const handleInfo = (data: PositionDto) => {
    setFormType(FormType.INFO);
    setPositionDto(data);
    setForm(true);
  };

  const handleEdit = (data: PositionDto) => {
    setFormType(FormType.UPDATE);
    setPositionDto(data);
    setForm(true);
  };

  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
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
            const toastId = createPendingToast(
              "Removing selected positions...",
            );
            const data: string[] = [];
            select.map(async (a: PositionDto) => {
              data.push(a.guid);
            });
            const res = await send.deleteBody(
              PositionEndpoint.DELETE_RANGE,
              data,
            );
            if (resolveRequestToast(toastId, res, PositionToast.DELETE_RANGE)) {
              setRemove(false);
              toggleRefresh();
            }
          });
          setRemove(true);
        }
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const toastId = createPendingToast("Creating position...");
          const res = await send.post(PositionEndpoint.CREATE, positionDto);
          if (resolveRequestToast(toastId, res, PositionToast.CREATE)) {
            setForm(false);
            toggleRefresh();
            setPositionDto(defaultDto);
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const toastId = createPendingToast("Updating position...");
          const res = await send.put(PositionEndpoint.UPDATE, positionDto);
          if (resolveRequestToast(toastId, res, PositionToast.UPDATE)) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setPositionDto(defaultDto);
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
      PositionEndpoint.PAGINATION_BY_DEPART(
        pageNumber,
        pageSize,
        locationGuid ?? "",
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setPositionsDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  const tabContent: FormContent[] = [
    {
      icon: <LocationIcon />,
      label: "Intevals",
      content: (
        <PositionForm
          type={formType}
          dto={positionDto}
          setDto={setPositionDto}
          handleClick={handleClickWithEvent}
        />
      ),
    },
  ];

  const fetchCompany = async () => {
    setCompanyOptions([]);
    const res = await send.get(CompanyEndpoint.GET);
    if (res.data.success) {
      res.data.data.map((a: CompanyDto) => {
        setCompanyOptions((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.guid,
            description: a.description,
          },
        ]);
      });
    }
  };

  const fetchDepartments = async (company: string) => {
    setDepartmentOptions([]);
    const res = await send.get(DepartmentEndpoint.GET_BY_COMPANY(company));
    if (res.data.success) {
      res.data.data.map((a: DepartmentDto) => {
        setDepartmentOptions((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.guid,
            description: a.description,
          },
        ]);
      });
    }
  };

  useEffect(() => {
    fetchCompany();
  }, []);

  return (
    <>
      <PageBreadcrumb pageTitle="Positions" />
      {form ? (
        <BaseForm tabContent={tabContent} header={""} desc={""} />
      ) : (
        <div className="space-y-6">
          <div className="rounded-xl border border-gray-200 p-6 dark:border-gray-800 border border-gray-200 bg-white dark:border-gray-800 dark:bg-white/[0.03] ">
            <div className="flex gap-10">
              <Label>Company Selector</Label>
              <Select
                isString={true}
                options={companyOptions}
                name="Company"
                defaultValue={selectedCompany}
                onChange={(e) => {
                  setPositionsDto([]);
                  setSelectedCompany(e);
                  setSelectedDepartment("");
                  fetchDepartments(e);
                  setPositionDto((prev) => ({
                    ...prev,
                    companyGuid: e,
                    company:
                      companyOptions.find((x) => x.value == e)?.label ?? "",
                  }));
                }}
              />
              <Label>Department Selector</Label>
              <Select
                isString={true}
                options={departmentOptions}
                name="Department"
                defaultValue={selectedDepartment}
                onChange={(e) => {
                  setSelectedDepartment(e);
                  setPositionDto((prev) => ({
                    ...prev,
                    departmentGuid: e,
                    department:
                      departmentOptions.find((x) => x.value == e)?.label ?? "",
                  }));
                  fetchData(1, 10, e);
                }}
              />
            </div>
          </div>
          <BaseTable<PositionDto>
            headers={HEADER}
            keys={KEY}
            data={positionsDto}
            select={select}
            setSelect={setSelect}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onClick={handleClickWithEvent}
            permission={filterPermission(FeatureId.user)}
            onInfo={handleInfo}
            fetchData={fetchData}
            refresh={refresh}
            locationGuid={selectedDepartment}
          />
        </div>
      )}
    </>
  );
};
