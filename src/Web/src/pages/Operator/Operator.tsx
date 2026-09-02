import { useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { useToast } from "../../context/ToastContext";
import { BaseForm } from "../UiElements/BaseForm";
import { BaseTable } from "../UiElements/BaseTable";
import { OperatorIcon } from "../../icons";
import { OperatorDto } from "../../model/Operator/OperatorDto";
import Helper from "../../utility/Helper";
import { OperatorToast } from "../../model/ToastMessage";
import { FormContent } from "../../model/Form/FormContent";
import { OperatorEndpoint } from "../../endpoint/OperatorEndpoint";
import { useLocation } from "../../context/LocationContext";
import { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { Title } from "../../enum/Title";
import { Gender } from "../../enum/Gender";
import { OperatorForm } from "../../components/form/operator/OperatorForm";

const defaultDto: OperatorDto = {
  guid: "",
  username: "",
  password: "",
  email: "",
  title: Title.Mr,
  firstName: "",
  middleName: "",
  lastName: "",
  phone: "",
  gender: Gender.Male,
  joinedDate: new Date(),
  expiredDate: new Date(new Date().setFullYear(new Date().getFullYear() + 10)),
  roleGuid: "",
  role: "",
  locationGuids: [],
  isActive: true,
  isDefault: false,
};

const HEADER: string[] = ["Username", "Email", "Enable", "Action"];
const KEY: string[] = ["username", "email"];

export const Operator = () => {
  const { setPagination } = usePagination();
  const { filterPermission } = useAuth();
  const { toggleToast } = useToast();
  const { locationGuid: locationId } = useLocation();
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
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [refresh, setRefresh] = useState<boolean>(false);
  const [operatorDto, setOperatorDto] = useState<OperatorDto>(defaultDto);
  const [operatorsDto, setOperatorsDto] = useState<OperatorDto[]>([]);
  const toggleRefresh = () => setRefresh(!refresh);

  const handleRemove = (data: OperatorDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(OperatorEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, OperatorToast.DELETE, toggleToast)) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: OperatorDto) => {
    data.password = "";
    setOperatorDto(data);
    console.log(data);
    setFormType(
      filterPermission(FeatureId.operator)?.isUpdated && !data.isDefault
        ? FormType.UPDATE
        : FormType.INFO,
    );
    setForm(true);
  };

  const handleInfo = (data: OperatorDto) => {
    data.password = "";
    setOperatorDto(data);
    setFormType(
      filterPermission(FeatureId.operator)?.isUpdated && !data.isDefault
        ? FormType.UPDATE
        : FormType.INFO,
    );
    setForm(true);
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    console.log(e.currentTarget.name);
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObjects.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        } else {
          setConfirmRemove(() => async () => {
            const data: string[] = [];
            selectedObjects.map(async (a: OperatorDto) => {
              data.push(a.guid);
            });
            const res = await send.post(OperatorEndpoint.DELETE_RANGE, data);
            if (
              Helper.handleToastByResCode(
                res,
                OperatorToast.DELETE_RANGE,
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
          const res = await send.post(OperatorEndpoint.CREATE, operatorDto);
          if (
            Helper.handleToastByResCode(res, OperatorToast.CREATE, toggleToast)
          ) {
            setForm(false);
            setOperatorDto(defaultDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(OperatorEndpoint.UPDATE, operatorDto);
          if (
            Helper.handleToastByResCode(res, OperatorToast.UPDATE, toggleToast)
          ) {
            setForm(false);
            setOperatorDto(defaultDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setOperatorDto(defaultDto);
        setForm(false);
        break;
      default:
        break;
    }
  };

  const [selectedObjects, setSelectedObjects] = useState<OperatorDto[]>([]);

  {
    /* Form */
  }
  const tabContent: FormContent[] = [
    {
      icon: <OperatorIcon />,
      label: "Operator",
      content: (
        <OperatorForm
          type={formType}
          dto={operatorDto}
          setDto={setOperatorDto}
          handleClick={handleClick}
        />
      ),
    },
  ];

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string | undefined,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      OperatorEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setOperatorsDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  return (
    <>
      <PageBreadcrumb pageTitle="Operators" />
      {form ? (
        <BaseForm
          type={formType}
          handleClick={handleClick}
          tabContent={tabContent}
          header={""}
          desc={""}
        />
      ) : (
        <div className="space-y-6">
          <BaseTable<OperatorDto>
            refresh={refresh}
            headers={HEADER}
            keys={KEY}
            data={operatorsDto}
            select={selectedObjects}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onInfo={handleInfo}
            onClick={handleClick}
            permission={filterPermission(FeatureId.operator)}
            setSelect={setSelectedObjects}
            fetchData={fetchData}
            locationGuid={locationId}
          />
        </div>
      )}
    </>
  );
};
