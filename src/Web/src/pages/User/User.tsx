import { useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { AddIcon, UserIcon } from "../../icons";
import { UserDto } from "../../model/User/UserDto";
import { UserEndpoint } from "../../endpoint/UserEndpoint";
import { useToast } from "../../context/ToastContext";
import Helper from "../../utility/Helper";
import { UserToast as UserToast } from "../../model/ToastMessage";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { BaseTable } from "../UiElements/BaseTable";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { ActionButton } from "../../model/ActionButton";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { usePagination } from "../../context/PaginationContext";
import { FormContent } from "../../model/Form/FormContent";
import { BaseForm } from "../UiElements/BaseForm";
import { TableCell } from "../../components/ui/table";
import { Avatar } from "../UiElements/Avatar";
import { Title } from "../../enum/Title";
import { Gender } from "../../enum/Gender";
import { LicensePlateDto } from "../../model/User/LicensePlateDto";
import { PinDto } from "../../model/User/PinDto";
import { QrCodeDto } from "../../model/User/QrCodeDto";
import { FaceDto } from "../../model/User/FaceDto";
import { PersonalInformationForm } from "../../components/form/user/PersonalInformationForm";
import { OperatorForm } from "../../components/form/user/OperatorForm";
import { LocationForm } from "../../components/form/user/LocationForm";
import { GroupForm } from "../../components/form/user/GroupForm";
import { CredentialForm } from "../../components/form/user/CredentialForm";
import { UserSettingForm } from "../../components/form/user/UserSettingForm";

const CARDHOLDER_HEAD: string[] = [
  "Image",
  "Id",
  "Name",
  "Company",
  "Department",
  "Postion",
  "Enable",
  "Action",
];
const CARDHOLDER_KEY: string[] = [
  "avatar",
  "userCode",
  "name",
  "company",
  "department",
  "position",
];

const User = () => {
  const { locationGuid } = useLocation();
  const { setPagination } = usePagination();
  const { filterPermission } = useAuth();
  const {
    setCreate,
    setUpdate,
    setRemove,
    setConfirmCreate,
    setConfirmRemove,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const { toggleToast } = useToast();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  const [usersDto, setUsersDto] = useState<UserDto[]>([]);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

  const defaultLicensePlate: LicensePlateDto = {
    licensePlate: "",
  };

  const defaultPin: PinDto = {
    pin: "",
  };

  const defaultQrCode: QrCodeDto = {
    qrCode: "",
  };

  const defaultFace: FaceDto = {
    imageName: "",
  };

  const defaultDto: UserDto = {
    guid: "",
    username: "",
    password: "",
    identification: "",
    title: Title.Mr,
    firstname: "",
    middlename: "",
    lastname: "",
    gender: Gender.M,
    dateOfBirth: new Date(),
    email: "",
    phone: "",
    isOperator: false,
    isUser: false,
    role: "",
    company: "",
    department: "",
    position: "",
    address: "",
    joinedDate: new Date(),
    expiredDate: new Date(),
    additionals: [],
    groups: [],
    cards: [],
    licensePlate: defaultLicensePlate,
    pin: defaultPin,
    qrCode: defaultQrCode,
    face: defaultFace,
    locations: [],
    isDefault: false,
    isActive: false,
    roleGuid: "",
    companyGuid: "00000000-0000-0000-0000-000000000000",
    departmentGuid: "00000000-0000-0000-0000-000000000000",
    positionGuid: "00000000-0000-0000-0000-000000000000",
    userCode: "",
  };

  const [userDto, setUserDto] = useState<UserDto>(defaultDto);
  const [image, setImage] = useState<File | undefined>();
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
    console.log(e.currentTarget.value);
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObjects.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          const data: string[] = [];
          selectedObjects.map(async (a: UserDto) => {
            data.push(a.guid);
          });
          const res = await send.deleteBody(UserEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              UserToast.DELETE_RANGE,
              toggleToast,
            )
          ) {
            setSelectedObjects([]);
            toggleRefresh();
          }
        });
        setRemove(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const res1 = await send.post(UserEndpoint.CREATE, userDto);
          if (
            Helper.handleToastByResCode(res1, UserToast.CREATE, toggleToast)
          ) {
            if (image != undefined) {
              const payload = new FormData();
              payload.append("image", image);
              const res2 = await send.postForm(
                UserEndpoint.UPLOAD(res1.data.data),
                payload,
              );
              if (
                Helper.handleToastByResCode(res2, UserToast.CREATE, toggleToast)
              ) {
                setUserDto(defaultDto);
                setForm(false);
                toggleRefresh();
              }
            } else {
              setUserDto(defaultDto);
              setForm(false);
              toggleRefresh();
            }
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(UserEndpoint.UPDATE, userDto);
          if (Helper.handleToastByResCode(res, UserToast.UPDATE, toggleToast)) {
            setUserDto(defaultDto);
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "cancle":
      case "close":
        setForm(false);
        setUserDto(defaultDto);
        break;
      default:
        break;
    }
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: UserDto) => {
    console.log(data);
    setFormType(FormType.UPDATE);
    setUserDto(data);
    setForm(true);
  };

  const handleInfo = (data: UserDto) => {
    setFormType(FormType.INFO);
    setUserDto(data);
    setForm(true);
  };

  const handleRemove = (data: UserDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(UserEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, UserToast.DELETE, toggleToast))
        toggleRefresh();
    });
    setRemove(true);
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
      UserEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res);
    if (res.data.success) {
      setUsersDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<UserDto[]>([]);

  const content: FormContent[] = [
    {
      label: "Personal Information",
      content: (
        <PersonalInformationForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          image={image}
          setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
    {
      label: "Operator Information",
      content: (
        <OperatorForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          // image={image}
          // setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
    {
      label: "Location Information",
      content: (
        <LocationForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          // image={image}
          // setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
    {
      label: "Access Level Information",
      content: (
        <GroupForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          // image={image}
          // setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
    {
      label: "Credential Information",
      content: (
        <CredentialForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          // image={image}
          // setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
    {
      label: "User Setting Information",
      content: (
        <UserSettingForm
          type={formType}
          dto={userDto}
          setDto={setUserDto}
          handleClick={handleClick}
          // image={image}
          // setImage={setImage}
        />
      ),
      icon: <UserIcon />,
    },
  ];

  const action: ActionButton[] = [
    {
      lable: "deactivate",
      buttonName: "Deactivate",
      icon: <AddIcon />,
    },
    {
      lable: "activate",
      buttonName: "Activate",
      icon: <AddIcon />,
    },
    {
      lable: "reset",
      buttonName: "Reset Anti-Passback",
      icon: <AddIcon />,
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Users" />
      {form ? (
        <BaseForm
          handleClick={handleClick}
          type={formType}
          tabContent={content}
          header={""}
          desc={""}
        />
      ) : (
        <BaseTable<UserDto>
          headers={CARDHOLDER_HEAD}
          keys={CARDHOLDER_KEY}
          data={usersDto}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          onClick={handleClick}
          onRemove={handleRemove}
          onEdit={handleEdit}
          onInfo={handleInfo}
          permission={filterPermission(FeatureId.user)}
          action={action}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
          specialDisplay={[
            {
              key: "avatar",
              content: (d, i) => (
                <TableCell
                  key={i}
                  className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                >
                  <div className="cursor-pointer w-11 h-11 overflow-hidden border border-gray-200 rounded-full dark:border-gray-800">
                    <Avatar userId={d.guid} />
                  </div>
                </TableCell>
              ),
            },
            {
              key: "name",
              content: (d, i) => (
                <TableCell
                  key={i}
                  className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                >
                  {d.title} {d.firstname} {d.middlename} {d.lastname}
                </TableCell>
              ),
            },
          ]}
        />
      )}
    </>
  );
};

export default User;
