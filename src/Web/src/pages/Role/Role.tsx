import { useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { useToast } from "../../context/ToastContext";
import { RoleDto } from "../../model/Role/RoleDto";
import { RoleToast } from "../../model/ToastMessage";
import { FormContent } from "../../model/Form/FormContent";
import { BaseForm } from "../UiElements/BaseForm";
import { RoleIcon } from "../../icons";
import { BaseTable } from "../UiElements/BaseTable";
import { RoleEndpoint } from "../../endpoint/RoleEndpoint";
import Helper from "../../utility/Helper";
import { RoleForm } from "../../components/form/role/RoleForm";
import { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { useLocation } from "../../context/LocationContext";
import { usePagination } from "../../context/PaginationContext";
import { CreateRoleDto } from "../../model/Role/CreateRoleDto";
import { UpdateRoleDto } from "../../model/Role/UpdateRoleDto";

const LOCATION_HEADER: string[] = ["Name", "Status", "Action"];
const LOCATION_KEY: string[] = ["name"];

export const Role = () => {
  const { toggleToast } = useToast();
  const { locationGuid: locationId } = useLocation();
  const { setPagination } = usePagination();
  const { filterPermission } = useAuth();
  const {
    setCreate,
    setConfirmCreate,
    setUpdate,
    setConfirmUpdate,
    setRemove,
    setConfirmRemove,
    setInfo,
    setMessage,
  } = usePopup();
  const [form, setForm] = useState<boolean>(false);
  const [refresh, setRefresh] = useState<boolean>(false);
  const defaultDto: RoleDto = {
    guid: "00000000-0000-0000-0000-000000010000",
    name: "",
    modules: [
      {
        id: 1,
        name: "Access Control",
        isEnabled: false,
        features: [
          {
            id: 1,
            name: "dashboard",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 2,

            name: "events",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 3,

            name: "location",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 4,

            name: "alert",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 5,

            name: "operator",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 6,
            name: "device",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 7,
            name: "control",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 8,
            name: "monitor",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 9,
            name: "monitorgroup",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 10,
            name: "acr",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 11,
            name: "user",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 12,
            name: "group",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 13,
            name: "area",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 14,
            name: "time",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 15,
            name: "trigger",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 16,
            name: "map",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 17,
            name: "report",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 18,
            name: "setting",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 19,
            name: "tools",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
        ],
      },
      {
        id: 2,
        name: "Visitor Management",
        isEnabled: false,
        features: [
          {
            id: 20,
            name: "register",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
          {
            id: 21,
            name: "appointment",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
        ],
      },
      {
        id: 3,
        name: "Time Attendance",
        isEnabled: false,
        features: [
          {
            id: 22,
            name: "report",
            isEnabled: false,
            isCreated: false,
            isUpdated: false,
            isDeleted: false,
          },
        ],
      },
    ],
    isActive: true,
    isDefault: false,
  };

  const [roleDto, setRoleDto] = useState<RoleDto>(defaultDto);
  const [rolesDto, setRolesDto] = useState<RoleDto[]>([]);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const toggleRefresh = () => setRefresh(!refresh);

  const handleRemove = (data: RoleDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(RoleEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, RoleToast.DELETE, toggleToast)) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (data: RoleDto) => {
    setFormType(
      filterPermission(FeatureId.operator)?.isUpdated && !data.isDefault
        ? FormType.UPDATE
        : FormType.INFO,
    );
    setRoleDto(data);
    setForm(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: RoleDto) => {
    setFormType(
      filterPermission(FeatureId.operator)?.isUpdated && !data.isDefault
        ? FormType.UPDATE
        : FormType.INFO,
    );
    setRoleDto(data);
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
            selectedObjects.map(async (a: RoleDto) => {
              data.push(a.guid);
            });
            const res = await send.deleteBody(RoleEndpoint.DELETE_RANGE, data);
            if (
              Helper.handleToastByResCode(
                res,
                RoleToast.DELETE_RANGE,
                toggleToast,
              )
            ) {
              setRemove(false);
              toggleRefresh();
              setSelectedObjects([]);
            }
          });
          setRemove(true);
        }

        break;
      case "create":
        setConfirmCreate(() => async () => {
          const createDto: CreateRoleDto = {
            name: roleDto.name,
            modules: roleDto.modules,
          };
          const res = await send.post(RoleEndpoint.CREATE, createDto);
          if (Helper.handleToastByResCode(res, RoleToast.CREATE, toggleToast)) {
            setForm(false);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const updateDto: UpdateRoleDto = {
            name: roleDto.name,
            guid: roleDto.guid,
            modules: roleDto.modules,
          };
          const res = await send.put(RoleEndpoint.UPDATE, updateDto);
          if (Helper.handleToastByResCode(res, RoleToast.UPDATE, toggleToast)) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setRoleDto(defaultDto);
        setForm(false);
        break;
      default:
        break;
    }
  };

  const [selectedObjects, setSelectedObjects] = useState<RoleDto[]>([]);

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      RoleEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setRolesDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* Form */
  }
  const tabContent: FormContent[] = [
    {
      icon: <RoleIcon />,
      label: "Role",
      content: (
        <RoleForm
          type={formType}
          dto={roleDto}
          setDto={setRoleDto}
          handleClick={handleClick}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Roles" />
      {form ? (
        <BaseForm
          handleClick={handleClick}
          tabContent={tabContent}
          header={""}
          desc={""}
          type={formType}
        />
      ) : (
        <div className="space-y-6">
          <BaseTable<RoleDto>
            headers={LOCATION_HEADER}
            keys={LOCATION_KEY}
            data={rolesDto}
            select={selectedObjects}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onClick={handleClick}
            permission={filterPermission(FeatureId.operator)}
            onInfo={handleInfo}
            setSelect={setSelectedObjects}
            fetchData={fetchData}
            locationGuid={locationId}
            refresh={refresh}
          />
        </div>
      )}
    </>
  );
};
