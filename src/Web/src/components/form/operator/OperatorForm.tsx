import React, { PropsWithChildren, useEffect, useMemo, useState } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import Label from "../Label";
import Input from "../input/InputField";
import Button from "../../ui/button/Button";
import Select from "../Select";
import { RoleEndpoint } from "../../../endpoint/RoleEndpoint";
import { RoleDto } from "../../../model/Role/RoleDto";
import { Options } from "../../../model/Options";
import Helper from "../../../utility/Helper";
import {
  CamIcon,
  CheckLineIcon,
  CloseLineIcon,
  EnvelopeIcon,
  EyeCloseIcon,
  EyeIcon,
  FileIcon,
  LocationIcon,
} from "../../../icons";
import { send } from "../../../api/api";
import { OperatorEndpoint } from "../../../endpoint/OperatorEndpoint";
import { PasswordRuleDto } from "../../../model/Setting/PasswordRuleDto";
import { OperatorToast } from "../../../model/ToastMessage";
import { useToast } from "../../../context/ToastContext";
import { SettingEndpoint } from "../../../endpoint/SettingEndpoint";
import { FormField, FormSection } from "../template/FormTemplate";
import { OperatorDto } from "../../../model/Operator/OperatorDto";
import { NativeWebcam } from "../../../pages/UiElements/NativeWebcam";
import Modals from "../../../pages/UiElements/Modals";
import DropzoneComponent from "../form-elements/DropZone";
import { Title } from "../../../enum/Title";
import { Gender } from "../../../enum/Gender";
import Radio from "../input/Radio";
import { Avatar } from "../../../pages/UiElements/Avatar";
import PhoneInput from "../group-input/PhoneInput";
import { countries } from "../../../constants/phone-code";
import DatePicker from "../date-picker";
import { LocationDto } from "../../../model/Location/LocationDto";
import { useLocation } from "../../../context/LocationContext";

type PasswordDto = {
  userName: string;
  old: string;
  new: string;
  con: string;
};

export const OperatorForm: React.FC<
  PropsWithChildren<FormProp<OperatorDto>>
> = ({ dto, setDto, type }) => {
  const defaultPassDto: PasswordDto = useMemo(
    () => ({
      userName: dto.username,
      old: "",
      new: "",
      con: "",
    }),
    [dto.username],
  );

  const { toggleToast } = useToast();
  const { locationList } = useLocation();
  const [locationsGuid, setLocationGuid] = useState<string>("");
  const [selectedLocationGuids, setSelectedLocationGuids] = useState<string[]>(
    [],
  );
  const [locations, setLocations] = useState<Options[]>(
    locationList.map((l: LocationDto) => ({
      label: l.name,
      value: l.guid,
      isTaken: false,
    })),
  );

  const [roles, setRoles] = useState<Options[]>([]);
  const [passForm, setPassForm] = useState<boolean>(false);
  const [showOld, setShowOld] = useState<boolean>(false);
  const [showNew, setShowNew] = useState<boolean>(false);
  const [showCon, setShowCon] = useState<boolean>(false);
  const [passDto, setPassDto] = useState<PasswordDto>(defaultPassDto);
  const [passRule, setPassRule] = useState<PasswordRuleDto>({
    len: 4,
    isDigit: false,
    isLower: false,
    isUpper: false,
    isSymbol: false,
    weaks: [],
  });

  const isReadOnly = type === FormType.INFO;
  const [image, setImage] = useState<File | undefined>();
  const [newImage, setNewImage] = useState<File | undefined>();
  const [file, setFile] = useState<boolean>(false);
  const [cam, setCam] = useState<boolean>(false);

  const [selectedValue, setSelectedValue] = useState<Gender | string>(
    Gender.Male.toString(),
  );
  const handleRadioChange = (value: string) => {
    setSelectedValue(value);
    // setDto((prev) => ({ ...prev, gender: value }));
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setDto((prev) => ({ ...prev, [name]: value }));
  };

  const handleChangePassword = () => {
    setPassForm(true);
  };

  const toggleLocationSelection = (data: string) => {
    setSelectedLocationGuids((prev) =>
      prev.includes(data) ? prev.filter((x) => x !== data) : [...prev, data],
    );
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    switch (e.currentTarget.name) {
      case "cancel":
        setPassDto(defaultPassDto);
        setPassForm(false);
        break;
      case "create":
        setDto((prev) => ({ ...prev, password: passDto.new }));
        setPassDto(defaultPassDto);
        setPassForm(false);
        break;
      case "change":
        updatePassword();
        break;
      default:
        break;
    }
  };

  const handleClickInternal = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "file":
        setFile(true);
        break;
      case "cam":
        setCam(true);
        break;
      case "close":
        setCam(false);
        setFile(false);
        break;
      case "cancle":
        setCam(false);
        setFile(false);
        break;
    }
  };

  const updatePassword = async () => {
    const res = await send.put(OperatorEndpoint.PASS, passDto);
    if (
      Helper.handleToastByResCode(res, OperatorToast.UPDATE_PASS, toggleToast)
    ) {
      setPassDto(defaultPassDto);
      setPassForm(false);
    }
  };

  const fetchRole = async () => {
    const res = await send.get(RoleEndpoint.GET);
    if (res) {
      setRoles(
        res.data.data.map((role: RoleDto) => ({
          label: role.name,
          value: role.guid,
          isTaken: false,
        })),
      );
    }
  };

  const addLocation = () => {
    console.log(locationsGuid);
    if (locationsGuid === "" || dto.locationGuids.includes(locationsGuid))
      return;

    setDto((prev) => ({
      ...prev,
      locationGuids: [...prev.locationGuids, locationsGuid],
    }));

    setLocations((prev) =>
      Helper.updateOptionByValue(prev, locationsGuid, true),
    );

    setLocationGuid("");
    console.log(dto.locationGuids);
  };

  const removeSelectedLocations = () => {
    if (selectedLocationGuids.length === 0) return;

    const idsToRemove = [...selectedLocationGuids];
    setDto((prev) => ({
      ...prev,
      locationGuids: prev.locationGuids.filter(
        (id) => !idsToRemove.includes(id),
      ),
    }));
    setLocations((prev) =>
      prev.map((option) =>
        idsToRemove.includes(option.value.toString())
          ? { ...option, isTaken: false }
          : option,
      ),
    );
    setSelectedLocationGuids([]);
  };

  const fetchPasswordRule = async () => {
    const res = await send.get(SettingEndpoint.GET_PASSWORD);
    console.log(res.data);
    if (res) {
      setPassRule({
        len: res.data.data.len,
        isDigit: res.data.data.isDigit,
        isLower: res.data.data.isLower,
        isUpper: res.data.data.isUpper,
        isSymbol: res.data.data.isSymbol,
        weaks: res.data.data.weaks,
      });
    }
  };

  const isRequireLen = (value: string): boolean => value.length >= passRule.len;
  const isRequireUpper = (value: string): boolean =>
    /[A-Z]/.test(value) || !passRule.isUpper;
  const isRequireLower = (value: string): boolean =>
    /[a-z]/.test(value) || !passRule.isLower;
  const isRequireDigit = (value: string): boolean =>
    /[0-9]/.test(value) || !passRule.isDigit;
  const isRequireSymbol = (value: string): boolean =>
    /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(value) || !passRule.isSymbol;
  const isMatch = (value: string, value2: string): boolean =>
    value === value2 && value !== "" && value2 !== "";

  const passwordChecks = [
    {
      label: `At least ${passRule.len} characters`,
      passed: isRequireLen(passDto.new),
    },
    {
      label: "Contains a number",
      passed: isRequireDigit(passDto.new),
      visible: passRule.isDigit,
    },
    {
      label: "Contains an uppercase letter",
      passed: isRequireUpper(passDto.new),
      visible: passRule.isUpper,
    },
    {
      label: "Contains a lowercase letter",
      passed: isRequireLower(passDto.new),
      visible: passRule.isLower,
    },
    {
      label: "Contains a symbol",
      passed: isRequireSymbol(passDto.new),
      visible: passRule.isSymbol,
    },
    {
      label: "Passwords match",
      passed: isMatch(passDto.new, passDto.con),
    },
  ].filter((check) => check.visible ?? true);

  useEffect(() => {
    fetchPasswordRule();
    fetchRole();
  }, []);

  useEffect(() => {
    setPassDto((prev) => ({ ...prev, userName: dto.username }));
  }, [dto.username]);

  return (
    <>
      {passForm ? (
        <div className="mx-auto max-w-3xl rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
          <div className="mb-6">
            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">
              Password
            </p>
            <h2 className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">
              Secure access
            </h2>
            <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
              Set a strong password for this operator account.
            </p>
          </div>

          <div className="grid gap-4 md:grid-cols-3">
            {type === FormType.UPDATE && (
              <div>
                <Label>Old Password</Label>
                <div className="relative">
                  <Input
                    type={showOld ? "text" : "password"}
                    placeholder="Current password"
                    onChange={(e) =>
                      setPassDto((prev) => ({ ...prev, old: e.target.value }))
                    }
                  />
                  <span
                    onClick={() => setShowOld(!showOld)}
                    className="absolute right-4 top-1/2 -translate-y-1/2 cursor-pointer"
                  >
                    {showOld ? (
                      <EyeIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                    ) : (
                      <EyeCloseIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                    )}
                  </span>
                </div>
              </div>
            )}

            <div>
              <Label>New Password</Label>
              <div className="relative">
                <Input
                  type={showNew ? "text" : "password"}
                  placeholder="New password"
                  onChange={(e) =>
                    setPassDto((prev) => ({ ...prev, new: e.target.value }))
                  }
                />
                <span
                  onClick={() => setShowNew(!showNew)}
                  className="absolute right-4 top-1/2 -translate-y-1/2 cursor-pointer"
                >
                  {showNew ? (
                    <EyeIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                  ) : (
                    <EyeCloseIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                  )}
                </span>
              </div>
            </div>

            <div>
              <Label>Confirm Password</Label>
              <div className="relative">
                <Input
                  type={showCon ? "text" : "password"}
                  placeholder="Repeat password"
                  onChange={(e) =>
                    setPassDto((prev) => ({ ...prev, con: e.target.value }))
                  }
                />
                <span
                  onClick={() => setShowCon(!showCon)}
                  className="absolute right-4 top-1/2 -translate-y-1/2 cursor-pointer"
                >
                  {showCon ? (
                    <EyeIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                  ) : (
                    <EyeCloseIcon className="size-5 fill-gray-500 dark:fill-gray-400" />
                  )}
                </span>
              </div>
            </div>
          </div>

          <div className="mt-6 rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/50 p-5">
            <div className="grid gap-3 md:grid-cols-2">
              {passwordChecks.map((check) => (
                <div
                  key={check.label}
                  className="flex items-center gap-3 rounded-xl bg-[var(--app-panel-bg)] px-4 py-3"
                >
                  {check.passed ? (
                    <CheckLineIcon color="green" fontSize={18} />
                  ) : (
                    <CloseLineIcon color="red" fontSize={18} />
                  )}
                  <span className="text-sm text-gray-600 dark:text-gray-300">
                    {check.label}
                  </span>
                </div>
              ))}
            </div>
          </div>

          <div className="mt-6 flex flex-wrap justify-end gap-3">
            <Button
              onClickWithEvent={handleClick}
              name={type === FormType.CREATE ? "create" : "change"}
              size="sm"
            >
              {type === FormType.CREATE ? "Save Password" : "Update Password"}
            </Button>
            <Button
              variant="outline"
              onClickWithEvent={handleClick}
              name="cancel"
              size="sm"
            >
              Cancel
            </Button>
          </div>
        </div>
      ) : (
        <div className="gap-5 grid grid-cols-3">
          <FormSection
            overall="Profile Image"
            title="Photo"
            description="Upload from file or take a live picture with webcam."
          >
            <FormField className="flex flex-col justify-center items-center gap-10">
              {file || cam ? (
                file ? (
                  <Modals
                    handleClickWithEvent={handleClickInternal}
                    body={
                      <DropzoneComponent
                        newImage={newImage}
                        setNewImage={setNewImage}
                        image={image}
                        setImage={setImage}
                        setFile={setFile}
                      />
                    }
                  />
                ) : (
                  <Modals
                    isWide={true}
                    handleClickWithEvent={handleClickInternal}
                    body={
                      <NativeWebcam
                        setNewImage={setNewImage}
                        image={image}
                        setImage={setImage}
                        modelStatus={cam}
                        handleClick={handleClickInternal}
                      />
                    }
                  />
                )
              ) : (
                <>
                  <div className="h-56 w-56 overflow-hidden rounded-full border-4 border-white bg-white shadow-lg ring-1 ring-gray-200 dark:border-gray-900 dark:bg-gray-900 dark:ring-gray-700">
                    <Avatar
                      userId={dto.guid}
                      newImage={newImage}
                      image={image}
                    />
                  </div>

                  <div className="flex flex-wrap justify-center gap-3">
                    {/* <Button
                        disabled={isReadOnly}
                        name="file"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<FileIcon />}
                      >
                        Browse
                      </Button> */}
                    <Button
                      disabled={isReadOnly}
                      variant="outline"
                      onClickWithEvent={handleClickInternal}
                      name="file"
                      startIcon={<FileIcon />}
                      className="justify-center"
                    >
                      Browse
                    </Button>
                    {/* <Button
                        disabled={isReadOnly}
                        name="cam"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<CamIcon />}
                      >
                        Take Picture
                      </Button> */}
                    <Button
                      disabled={isReadOnly}
                      variant="outline"
                      onClickWithEvent={handleClickInternal}
                      name="cam"
                      startIcon={<CamIcon />}
                      className="justify-center"
                    >
                      Take Picture
                    </Button>
                  </div>
                </>
              )}
            </FormField>
          </FormSection>
          <FormSection
            className="col-span-2"
            overall="Operator Details"
            title="Lean and focused form"
            description="Clean inputs for account setup, contact details, role assignment, and location access."
          >
            <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
              <FormField>
                <Label htmlFor="username">Username</Label>
                <Input
                  disabled={type === FormType.INFO || type === FormType.UPDATE}
                  name="username"
                  id="username"
                  onChange={handleChange}
                  value={dto.username}
                  placeholder="operator.account"
                />
              </FormField>
              <div className="w-full max-w-xs">
                <Label>Password</Label>
                {type === FormType.UPDATE || type === FormType.CREATE ? (
                  <Button
                    onClick={handleChangePassword}
                    disabled={isReadOnly}
                    variant={dto.password.length > 0 ? "green" : "primary"}
                    className="w-full justify-center"
                  >
                    {type === FormType.UPDATE
                      ? "Change Password"
                      : dto.password.length === 0
                        ? "Set Password"
                        : "Password Ready"}
                  </Button>
                ) : (
                  <Input
                    disabled
                    name="password"
                    type="password"
                    value={dto.password}
                  />
                )}
              </div>
              <div>
                <Label htmlFor="role">Role</Label>
                <Select
                  disabled={isReadOnly}
                  isString={true}
                  options={roles}
                  defaultValue={dto.roleGuid}
                  onChange={(e) =>
                    setDto((prev) => ({
                      ...prev,
                      roleGuid: e,
                    }))
                  }
                  name="roleGuid"
                  placeholder="Select role"
                />
              </div>

              <div className="flex gap-3 mb-3 w-full col-span-2">
                <FormField className="flex-1">
                  <Label htmlFor="title">Title</Label>
                  <Select
                    options={[
                      {
                        label: Title[Title.Mr],
                        value: Title.Mr,
                      },
                      {
                        label: Title[Title.Miss],
                        value: Title.Miss,
                      },
                      {
                        label: Title[Title.Ms],
                        value: Title.Ms,
                      },
                      {
                        label: Title[Title.Other],
                        value: Title.Other,
                      },
                    ]}
                    onChange={(e) => {
                      setDto((prev) => ({
                        ...prev,
                        title: Number(e),
                      }));
                    }}
                    name={"title"}
                    defaultValue={dto.title}
                  />
                </FormField>
                <FormField className="flex-2">
                  <Label htmlFor="firstName">First Name</Label>
                  <Input
                    disabled={isReadOnly}
                    name="firstName"
                    type="text"
                    id="firstName"
                    onChange={handleChange}
                    value={dto.firstName}
                    placeholder="John"
                  />
                </FormField>
                <FormField className="flex-2">
                  <Label htmlFor="middleName">Middle Name</Label>
                  <Input
                    disabled={isReadOnly}
                    name="middleName"
                    type="text"
                    id="middleName"
                    onChange={handleChange}
                    value={dto.middleName}
                    placeholder="Jr"
                  />
                </FormField>
                <FormField className="flex-2">
                  <Label htmlFor="lastName">Last Name</Label>
                  <Input
                    disabled={isReadOnly}
                    name="lastName"
                    type="text"
                    id="lastName"
                    onChange={handleChange}
                    value={dto.lastName}
                    placeholder="Doh"
                  />
                </FormField>
              </div>
              <div className="col-span-2">
                <FormField>
                  <Label htmlFor="gender">Gender</Label>
                  <div className="flex justify-around gap-3 pb-3">
                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender1"
                        name="gender"
                        value={Gender.Male.toString()}
                        checked={selectedValue === Gender.Male.toString()}
                        onChange={handleRadioChange}
                        label="Male"
                      />
                    </div>

                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender2"
                        name="gender"
                        value={Gender.Female.toString()}
                        checked={selectedValue === Gender.Female.toString()}
                        onChange={handleRadioChange}
                        label="Female"
                      />
                    </div>
                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender3"
                        name="gender"
                        value={Gender.Other.toString()}
                        checked={selectedValue === Gender.Other.toString()}
                        onChange={handleRadioChange}
                        label="Other"
                      />
                    </div>
                  </div>
                </FormField>
              </div>

              <FormField>
                <Label>Email</Label>
                <div className="relative">
                  <Input
                    disabled={isReadOnly}
                    name="email"
                    placeholder="info@gmail.com"
                    type="text"
                    className="pl-[62px]"
                    onChange={handleChange}
                    value={dto.email}
                  />
                  <span className="absolute left-0 top-1/2 -translate-y-1/2 border-r border-gray-200 px-3.5 py-3 text-gray-500 dark:border-gray-800 dark:text-gray-400">
                    <EnvelopeIcon className="size-6" />
                  </span>
                </div>
              </FormField>
              <FormField>
                <Label>Phone</Label>
                <PhoneInput
                  countries={countries}
                  onChange={(e) => setDto((prev) => ({ ...prev, phone: e }))}
                />
              </FormField>
              <FormField>
                <DatePicker
                  isTime={false}
                  id="Date"
                  label="Joined Date"
                  placeholder="Select a date"
                  onChange={(date) =>
                    setDto((prev) => ({
                      ...prev,
                      joinedDate: date[0],
                    }))
                  }
                  value={dto.joinedDate.toString()}
                />
              </FormField>
              <FormField>
                {new Date(dto.expiredDate).getTime() ==
                new Date("9999-01-01T00:00:00Z").getTime() ? (
                  <>
                    <Label>Expired Date</Label>
                    <Input disabled={true} placeholder="No Expiry" />
                  </>
                ) : (
                  <DatePicker
                    isTime={false}
                    id="Date"
                    label="Expired Date"
                    placeholder="Select a date"
                    onChange={(date) =>
                      setDto((prev) => ({
                        ...prev,
                        expiredDate: date[0],
                      }))
                    }
                    value={dto.expiredDate.toString()}
                  />
                )}
              </FormField>
            </div>
          </FormSection>
          <FormSection
            className="col-span-3"
            overall="Location Access"
            title="Manage assigned locations"
            description="Add locations one by one, then tap cards below to mark which ones
            should be removed."
          >
            <div className="flex flex-col gap-3 lg:flex-row">
              <div className="flex-1">
                <Label htmlFor="location">Location</Label>
                <Select
                  disabled={isReadOnly}
                  isString={true}
                  options={locations.filter((x) => x.isTaken == false)}
                  defaultValue={locationsGuid}
                  onChange={(value) => setLocationGuid(value)}
                  name="location"
                  placeholder="Select location"
                />
              </div>
              <div className="flex gap-3 lg:items-end">
                <Button
                  disabled={isReadOnly || locationsGuid === ""}
                  onClick={addLocation}
                  className="min-w-[120px] justify-center"
                >
                  Add
                </Button>
                <Button
                  disabled={isReadOnly || selectedLocationGuids.length === 0}
                  variant="danger"
                  onClick={removeSelectedLocations}
                  className="min-w-[120px] justify-center"
                >
                  Remove
                </Button>
              </div>
            </div>

            <div className="mt-5 grid gap-3 sm:grid-cols-2 xl:grid-cols-3">
              {dto.locationGuids.length > 0 ? (
                dto.locationGuids.map((id, i) => (
                  <button
                    key={i}
                    type="button"
                    onClick={() => toggleLocationSelection(id)}
                    className={`flex items-center gap-4 rounded-[22px] border px-4 py-4 text-left transition ${
                      selectedLocationGuids.includes(id)
                        ? "border-brand-500 bg-brand-50 dark:bg-brand-500/10"
                        : "border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/30 hover:border-brand-300"
                    }`}
                  >
                    <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-[var(--app-panel-bg)] text-brand-500 shadow-sm">
                      <LocationIcon />
                    </div>
                    <div>
                      <p className="text-sm font-semibold text-gray-800 dark:text-white/90">
                        {locations.find((location) => location.value === id)
                          ?.label ?? `Location ${id}`}
                      </p>
                      <p className="text-xs text-gray-500 dark:text-gray-400">
                        {selectedLocationGuids.includes(id)
                          ? "Selected for removal"
                          : "Assigned location"}
                      </p>
                    </div>
                  </button>
                ))
              ) : (
                <div className="col-span-full rounded-[22px] border border-dashed border-[var(--app-panel-border)] px-5 py-10 text-center text-sm text-gray-500 dark:text-gray-400">
                  No locations assigned yet.
                </div>
              )}
            </div>
          </FormSection>
        </div>
      )}
    </>
  );
};
