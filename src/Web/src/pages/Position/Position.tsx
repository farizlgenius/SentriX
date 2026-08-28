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
import { useLocation } from "../../context/LocationContext";

var removeTarget: number = 0;

export const HEADER: string[] = ["Name", "Action"];
export const KEY: string[] = ["name"];

export const Position = () => {
  const { locationGuid: locationId } = useLocation();
  const [selectedDepartment, setSelectedDepartment] = useState<number>(-1);
  const [departmentOptions, setDepartmentOptions] = useState<Options[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<number>(-1);
  const [companyOptions, setCompanyOptions] = useState<Options[]>([]);
  const defaultDto: PositionDto = {
    id: 0,
    name: "",
    description: "",
    departmentId: selectedDepartment,
    locationId: locationId,
    isActive: true,
    isDefault: false,
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
    removeTarget = data.id;
    setRemove(true);
    setConfirmRemove(() => async () => {
      const toastId = createPendingToast("Removing position...");
      const res = await api.delete(PositionEndpoint.DELETE(removeTarget));
      if (resolveRequestToast(toastId, res, PositionToast.DELETE)) {
        toggleRefresh();
        removeTarget = 0;
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
            var data: number[] = [];
            select.map(async (a: PositionDto) => {
              data.push(a.id);
            });
            var res = await send.post(PositionEndpoint.DELETE_RANGE, {
              ids: data,
            });
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
          const res = await api.put(PositionEndpoint.UPDATE, positionDto);
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
    locationId?: number,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      PositionEndpoint.PAGINATION_BY_DEPART(
        pageNumber,
        pageSize,
        selectedDepartment,
        locationId,
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

  const fetchDataImd = async (
    pageNumber: number,
    pageSize: number,
    departmentId: number,
    locationId?: number,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      PositionEndpoint.PAGINATION_BY_DEPART(
        pageNumber,
        pageSize,
        departmentId,
        locationId,
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
    var res = await send.get(CompanyEndpoint.GET_BY_LOCATION(locationId));
    if (res.data.success) {
      res.data.data.map((a: CompanyDto) => {
        setCompanyOptions((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.id,
            description: a.description,
          },
        ]);
      });
    }
  };

  const fetchDepartments = async (company: number) => {
    setDepartmentOptions([]);
    const res = await send.get(DepartmentEndpoint.GET_BY_COMPANY(company));
    if (res.data.success) {
      res.data.data.map((a: DepartmentDto) => {
        setDepartmentOptions((prev) => [
          ...prev,
          {
            label: a.name,
            value: a.id,
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
                options={companyOptions}
                name="Company"
                defaultValue={selectedCompany}
                onChange={(e) => {
                  setPositionsDto([]);
                  setSelectedCompany(Number(e));
                  setSelectedDepartment(-1);
                  fetchDepartments(Number(e));
                }}
              />
              <Label>Department Selector</Label>
              <Select
                options={departmentOptions}
                name="Department"
                defaultValue={selectedDepartment}
                onChange={(e) => {
                  setSelectedDepartment(Number(e));
                  setPositionDto((prev) => ({
                    ...prev,
                    departmentId: Number(e),
                  }));
                  if (locationId != -1) {
                    fetchDataImd(1, 10, Number(e), locationId);
                  }
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
            permission={filterPermission(FeatureId.location)}
            onInfo={handleInfo}
            fetchData={fetchData}
            refresh={refresh}
            locationGuid={
              selectedCompany == -1 || selectedDepartment == -1
                ? -1
                : locationId
            }
          />
        </div>
      )}
    </>
  );
};
