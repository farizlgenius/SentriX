import { useEffect, useState } from "react";
import { BaseForm } from "../UiElements/BaseForm";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { FormContent } from "../../model/Form/FormContent";
import { LocationIcon } from "../../icons";
import { useToast } from "../../context/ToastContext";
import Helper from "../../utility/Helper";
import { DepartmentToast } from "../../model/ToastMessage";
import { BaseTable } from "../UiElements/BaseTable";
import api, { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { DepartmentDto } from "../../model/Department/DepartmentDto";
import { DepartmentEndpoint } from "../../endpoint/DepartmentEndpoint";
import { DepartmentForm } from "./DepartmentForm";
import Select from "../../components/form/Select";
import Label from "../../components/form/Label";
import { Options } from "../../model/Options";
import { CompanyEndpoint } from "../../endpoint/CompanyEndpoint";
import { CompanyDto } from "../../model/Company/CompanyDto";
import { FeatureId } from "../../enum/FeatureId";

const HEADER: string[] = ["Name", "Action"];
const KEY: string[] = ["name"];

export const Department = () => {
  const [selectedCompany, setSelectedCompany] = useState<string>("");
  const [companyOptions, setCompanyOptions] = useState<Options[]>([]);

  const defaultDto: DepartmentDto = {
    guid: "",
    name: "",
    description: "",
    companyGuid: selectedCompany,
    company: "",
    isActive: true,
    isDefault: false,
  };

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
  const [departmentDto, setDepartmentDto] = useState<DepartmentDto>(defaultDto);
  const [departmentsDto, setDepartmentsDto] = useState<DepartmentDto[]>([]);
  const [select, setSelect] = useState<DepartmentDto[]>([]);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  const handleRemove = (data: DepartmentDto) => {
    setRemove(true);
    setConfirmRemove(() => async () => {
      const res = await api.delete(DepartmentEndpoint.DELETE(data.guid));
      if (
        Helper.handleToastByResCode(res, DepartmentToast.DELETE, toggleToast)
      ) {
        toggleRefresh();
      }
    });
  };

  const handleInfo = (data: DepartmentDto) => {
    setFormType(FormType.INFO);
    setDepartmentDto(data);
    setForm(true);
  };

  const handleEdit = (data: DepartmentDto) => {
    setFormType(FormType.UPDATE);
    setDepartmentDto(data);
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
            const data: string[] = [];
            select.map(async (a: DepartmentDto) => {
              data.push(a.guid);
            });
            const res = await send.deleteBody(
              DepartmentEndpoint.DELETE_RANGE,
              data,
            );
            if (
              Helper.handleToastByResCode(
                res,
                DepartmentToast.DELETE_RANGE,
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
          const res = await send.post(DepartmentEndpoint.CREATE, departmentDto);
          if (
            Helper.handleToastByResCode(
              res,
              DepartmentToast.CREATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
            setDepartmentDto(defaultDto);
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await api.put(DepartmentEndpoint.UPDATE, departmentDto);
          if (
            Helper.handleToastByResCode(
              res,
              DepartmentToast.UPDATE,
              toggleToast,
            )
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setDepartmentDto(defaultDto);
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
      DepartmentEndpoint.PAGINATION_BY_COMPANY(
        pageNumber,
        pageSize,
        locationGuid ?? "",
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setDepartmentsDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  const tabContent: FormContent[] = [
    {
      icon: <LocationIcon />,
      label: "Intevals",
      content: (
        <DepartmentForm
          type={formType}
          dto={departmentDto}
          setDto={setDepartmentDto}
          handleClick={handleClickWithEvent}
        />
      ),
    },
  ];

  const fetchCompany = async () => {
    const res = await send.get(CompanyEndpoint.GET);
    res.data.data.map((a: CompanyDto) => {
      setCompanyOptions((prev) => [
        ...prev,
        {
          label: a.name,
          value: a.guid,
          description: a.description,
          additionalInfo: a.address,
        },
      ]);
    });
  };

  useEffect(() => {
    fetchCompany();
  }, []);

  return (
    <>
      <PageBreadcrumb pageTitle="Departments" />
      {form ? (
        <BaseForm tabContent={tabContent} header={""} desc={""} />
      ) : (
        <div className="space-y-6">
          <div className="rounded-xl border border-gray-200 p-6 dark:border-gray-800 border border-gray-200 bg-white dark:border-gray-800 dark:bg-white/[0.03] ">
            <div className="gap-3">
              <Label>Company Selector</Label>
              <Select
                options={companyOptions}
                isString={true}
                name="Company"
                defaultValue={selectedCompany}
                onChange={(e) => {
                  setSelectedCompany(e);
                  setDepartmentDto((prev) => ({
                    ...prev,
                    companyGuid: e,
                    company:
                      companyOptions.find((x) => x.value == e)?.label ?? "",
                  }));
                  fetchData(1, 10, e);
                }}
              />
            </div>
          </div>
          <BaseTable<DepartmentDto>
            headers={HEADER}
            keys={KEY}
            data={departmentsDto}
            select={select}
            setSelect={setSelect}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onClick={handleClickWithEvent}
            permission={filterPermission(FeatureId.user)}
            onInfo={handleInfo}
            fetchData={fetchData}
            refresh={refresh}
            locationGuid={selectedCompany}
          />
        </div>
      )}
    </>
  );
};
