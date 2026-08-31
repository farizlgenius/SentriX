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
  CheckLineIcon,
  CloseLineIcon,
  EyeCloseIcon,
  EyeIcon,
} from "../../../icons";
import { send } from "../../../api/api";
import { OperatorEndpoint } from "../../../endpoint/OperatorEndpoint";
import { PasswordRuleDto } from "../../../model/Setting/PasswordRuleDto";
import { OperatorToast } from "../../../model/ToastMessage";
import { useToast } from "../../../context/ToastContext";
import { UserDto } from "../../../model/User/UserDto";
import { SettingEndpoint } from "../../../endpoint/SettingEndpoint";
import { FormField, FormSection } from "../template/FormTemplate";

type PasswordDto = {
  userName: string;
  old: string;
  new: string;
  con: string;
};

export const OperatorForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({
  dto,
  setDto,
  type,
}) => {
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setDto((prev) => ({ ...prev, [name]: value }));
  };

  const handleChangePassword = () => {
    setPassForm(true);
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
        <div>
          <section className="space-y-6">
            <FormSection
              overall="Operator Details"
              title="Lean and focused form"
              description="Clean inputs for account setup, contact details, role assignment, and location access."
              >
                <div className="grid gap-5 md:grid-cols-2">
                <FormField>
                  <Label htmlFor="username">Username</Label>
                  <Input
                    disabled={
                      type === FormType.INFO || type === FormType.UPDATE
                    }
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
                        role:
                          roles.find((option) => option.value === e)?.label ??
                          "",
                      }))
                    }
                    name="roleId"
                    placeholder="Select role"
                  />
                </div>
              </div>
              </FormSection>

          </section>
        </div>
      )}
    </>
  );
};
