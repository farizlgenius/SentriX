import React, { PropsWithChildren, useEffect, useMemo, useRef, useState } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import Label from "../Label";
import Input from "../input/InputField";
import Button from "../../ui/button/Button";
import Select from "../Select";
import { RoleEndpoint } from "../../../endpoint/RoleEndpoint";
import { RoleDto } from "../../../model/Role/RoleDto";
import { Options } from "../../../model/Options";
import { LocationDto } from "../../../model/Location/LocationDto";
import { LocationEndpoint } from "../../../endpoint/LocationEndpoint";
import Helper from "../../../utility/Helper";
import {
    CamIcon,
    CheckLineIcon,
    ClearIcon,
    CloseLineIcon,
    EyeCloseIcon,
    EyeIcon,
    FileIcon,
    LocationIcon,
    UploadIcon,
    UserCircleIcon
} from "../../../icons";
import { send } from "../../../api/api";
import { OperatorEndpoint } from "../../../endpoint/OperatorEndpoint";
import { SettingEndpoint } from "../../../endpoint/SettingEndpoint";
import { PasswordRuleDto } from "../../../model/Setting/PasswordRuleDto";
import { OperatorToast } from "../../../model/ToastMessage";
import { useToast } from "../../../context/ToastContext";
import { OperatorDto } from "../../../model/Operator/OperatorDto";
import { useLocation } from "../../../context/LocationContext";
import { TITLE } from "../../../enum/Title";

type PasswordDto = {
    userName: string;
    old: string;
    new: string;
    con: string;
}

const IMAGE_TYPES = ["image/png", "image/jpeg", "image/webp"];

export const OperatorForm: React.FC<PropsWithChildren<FormProp<OperatorDto>>> = ({ handleClick: handleClickWithEvent, dto, setDto, type }) => {
    const defaultPassDto: PasswordDto = useMemo(() => ({
        userName: dto.username,
        old: "",
        new: "",
        con: ""
    }), [dto.username]);

    const { toggleToast } = useToast();
    const {locationId} = useLocation();
    const browseInputRef = useRef<HTMLInputElement | null>(null);
    const videoRef = useRef<HTMLVideoElement | null>(null);
    const canvasRef = useRef<HTMLCanvasElement | null>(null);

    const [roles, setRoles] = useState<Options[]>([]);
    const [locationsId, setLocationId] = useState<number>(-1);
    const [selectedLocationIds, setSelectedLocationIds] = useState<number[]>([]);
    const [locations, setLocations] = useState<Options[]>([]);
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
        weaks: []
    });

    const [imageFile, setImageFile] = useState<File | undefined>();
    const [imagePreview, setImagePreview] = useState<string>("");
    const [isDragging, setIsDragging] = useState(false);
    const [cameraOpen, setCameraOpen] = useState(false);
    const [cameraError, setCameraError] = useState<string>("");
    const [stream, setStream] = useState<MediaStream | null>(null);
    const titleOptions = Object.values(TITLE)
  .filter(v => typeof v === "number")
  .map(v => ({
    label: TITLE[v], // "Mr"
    value: v         // 0
  }));
    const isReadOnly = type === FormType.INFO;

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setDto((prev) => ({ ...prev, [name]: value }));
    };

    const stopCamera = (targetStream?: MediaStream | null) => {
        const activeStream = targetStream ?? stream ?? (videoRef.current?.srcObject as MediaStream | null);
        if (activeStream) {
            activeStream.getTracks().forEach((track) => track.stop());
        }
        if (videoRef.current) {
            videoRef.current.srcObject = null;
        }
        setStream(null);
    };

    const applyImage = (file?: File) => {
        if (!file) return;
        if (!IMAGE_TYPES.includes(file.type)) {
            setCameraError("Please use PNG, JPG, or WebP image files.");
            return;
        }
        setCameraError("");
        setImageFile(file);
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
        if (Helper.handleToastByResCode(res, OperatorToast.UPDATE_PASS, toggleToast)) {
            setPassDto(defaultPassDto);
            setPassForm(false);
        }
    };

    const fetchRole = async () => {
        const res = await send.get(RoleEndpoint.GET_BY_LOCATION(locationId));
        console.log(res);
        if (res?.data) {
            setRoles(res.data.map((role: RoleDto) => ({
                label: role.name,
                value: role.id,
                isTaken: false
            })));
        }
    };

    const fetchLocation = async () => {
        const res = await send.get(LocationEndpoint.GET);
        console.log(res)
        if (res?.data) {
            setLocations(res.data.map((location: LocationDto) => ({
                label: location.name,
                value: location.id,
                isTaken: false
            })));
        }
    };

    const fetchPasswordRule = async () => {
        const res = await send.get(SettingEndpoint.GET_PASSWORD);
        console.log(res.data)
        if (res?.data) {
            setPassRule({
                len: res.data.len,
                isDigit: res.data.isDigit,
                isLower: res.data.isLower,
                isUpper: res.data.isUpper,
                isSymbol: res.data.isSymbol,
                weaks: res.data.weaks
            });
        }
    };

    const isRequireLen = (value: string): boolean => value.length >= passRule.len;
    const isRequireUpper = (value: string): boolean => /[A-Z]/.test(value) || !passRule.isUpper;
    const isRequireLower = (value: string): boolean => /[a-z]/.test(value) || !passRule.isLower;
    const isRequireDigit = (value: string): boolean => /[0-9]/.test(value) || !passRule.isDigit;
    const isRequireSymbol = (value: string): boolean => /[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/.test(value) || !passRule.isSymbol;
    const isMatch = (value: string, value2: string): boolean => value === value2 && value !== "" && value2 !== "";

    const passwordChecks = [
        {
            label: `At least ${passRule.len} characters`,
            passed: isRequireLen(passDto.new)
        },
        {
            label: "Contains a number",
            passed: isRequireDigit(passDto.new),
            visible: passRule.isDigit
        },
        {
            label: "Contains an uppercase letter",
            passed: isRequireUpper(passDto.new),
            visible: passRule.isUpper
        },
        {
            label: "Contains a lowercase letter",
            passed: isRequireLower(passDto.new),
            visible: passRule.isLower
        },
        {
            label: "Contains a symbol",
            passed: isRequireSymbol(passDto.new),
            visible: passRule.isSymbol
        },
        {
            label: "Passwords match",
            passed: isMatch(passDto.new, passDto.con)
        }
    ].filter((check) => check.visible ?? true);

    const toggleLocationSelection = (data: number) => {
        setSelectedLocationIds((prev) => prev.includes(data) ? prev.filter((x) => x !== data) : [...prev, data]);
    };

    const addLocation = () => {
        if (locationsId === -1 || dto.locationId.includes(locationsId)) return;

        setDto((prev) => ({ ...prev, locationId: [...prev.locationId, locationsId] }));
        setLocations((prev) => Helper.updateOptionByValue(prev, locationsId, true));
        setLocationId(-1);
    };

    const removeSelectedLocations = () => {
        if (selectedLocationIds.length === 0) return;

        const idsToRemove = [...selectedLocationIds];
        setDto((prev) => ({ ...prev, locationId: prev.locationId.filter((id) => !idsToRemove.includes(id)) }));
        setLocations((prev) => prev.map((option) => idsToRemove.includes(Number(option.value)) ? { ...option, isTaken: false } : option));
        setSelectedLocationIds([]);
    };

    const startCamera = async () => {
        if (isReadOnly) return;

        setCameraError("");
        try {
            const nextStream = await navigator.mediaDevices.getUserMedia({
                video: { width: 1280, height: 720, facingMode: "user" },
                audio: false
            });
            setCameraOpen(true);
            setStream(nextStream);
            if (videoRef.current) {
                videoRef.current.srcObject = nextStream;
            }
        } catch (error: any) {
            setCameraError(error?.message ?? "Camera access was denied.");
            setCameraOpen(false);
        }
    };

    const capturePhoto = () => {
        const video = videoRef.current;
        const canvas = canvasRef.current;
        if (!video || !canvas) return;

        canvas.width = video.videoWidth || 1280;
        canvas.height = video.videoHeight || 720;

        const ctx = canvas.getContext("2d");
        if (!ctx) return;

        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
        canvas.toBlob((blob) => {
            if (!blob) return;
            const file = new File([blob], `operator_${Date.now()}.png`, { type: "image/png" });
            applyImage(file);
            setCameraOpen(false);
            stopCamera();
        }, "image/png");
    };

    const clearImage = () => {
        setImageFile(undefined);
        setImagePreview("");
        setCameraError("");
        browseInputRef.current?.value && (browseInputRef.current.value = "");
    };

    useEffect(() => {
        fetchPasswordRule();
        fetchRole();
        fetchLocation();
    }, []);

    useEffect(() => {
        setPassDto((prev) => ({ ...prev, userName: dto.username }));
    }, [dto.username]);

    useEffect(() => {
        if (!imageFile) {
            setImagePreview("");
            return;
        }

        const objectUrl = URL.createObjectURL(imageFile);
        setImagePreview(objectUrl);

        return () => URL.revokeObjectURL(objectUrl);
    }, [imageFile]);

    useEffect(() => {
        return () => stopCamera();
    }, []);

    return (
        <>
            {passForm ? (
                <div className="mx-auto max-w-3xl rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
                    <div className="mb-6">
                        <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">Password</p>
                        <h2 className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">Secure access</h2>
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
                                        onChange={(e) => setPassDto((prev) => ({ ...prev, old: e.target.value }))}
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
                                    onChange={(e) => setPassDto((prev) => ({ ...prev, new: e.target.value }))}
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
                                    onChange={(e) => setPassDto((prev) => ({ ...prev, con: e.target.value }))}
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
                                <div key={check.label} className="flex items-center gap-3 rounded-xl bg-[var(--app-panel-bg)] px-4 py-3">
                                    {check.passed ? (
                                        <CheckLineIcon color="green" fontSize={18} />
                                    ) : (
                                        <CloseLineIcon color="red" fontSize={18} />
                                    )}
                                    <span className="text-sm text-gray-600 dark:text-gray-300">{check.label}</span>
                                </div>
                            ))}
                        </div>
                    </div>

                    <div className="mt-6 flex flex-wrap justify-end gap-3">
                        <Button onClickWithEvent={handleClick} name={type === FormType.CREATE ? "create" : "change"} size="sm">
                            {type === FormType.CREATE ? "Save Password" : "Update Password"}
                        </Button>
                        <Button variant="outline" onClickWithEvent={handleClick} name="cancel" size="sm">
                            Cancel
                        </Button>
                    </div>
                </div>
            ) : (
                <div className="grid gap-6 xl:grid-cols-[360px_minmax(0,1fr)]">
                    <aside className="rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs">
                        <div className="rounded-[24px] bg-[linear-gradient(180deg,rgba(59,130,246,0.10),rgba(255,255,255,0))] p-5 dark:bg-[linear-gradient(180deg,rgba(59,130,246,0.18),rgba(17,24,39,0))]">
                            <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">Profile Image</p>
                            <h2 className="mt-2 text-xl font-semibold text-gray-900 dark:text-white">Minimal identity block</h2>
                            <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                                Add an operator photo by dragging a file, browsing your device, or capturing one from the webcam.
                            </p>
                        </div>

                        <div className="mt-6 flex flex-col items-center gap-4">
                            <div className="flex h-44 w-44 items-center justify-center overflow-hidden rounded-[32px] border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] shadow-sm">
                                {imagePreview ? (
                                    <img src={imagePreview} alt="Operator preview" className="h-full w-full object-cover" />
                                ) : (
                                    <div className="flex flex-col items-center gap-3 text-gray-400">
                                        <UserCircleIcon className="size-16" />
                                        <span className="text-sm">No image selected</span>
                                    </div>
                                )}
                            </div>

                            <div
                                onDragEnter={(e) => {
                                    if (isReadOnly) return;
                                    e.preventDefault();
                                    setIsDragging(true);
                                }}
                                onDragOver={(e) => {
                                    if (isReadOnly) return;
                                    e.preventDefault();
                                }}
                                onDragLeave={(e) => {
                                    if (isReadOnly) return;
                                    e.preventDefault();
                                    setIsDragging(false);
                                }}
                                onDrop={(e) => {
                                    if (isReadOnly) return;
                                    e.preventDefault();
                                    setIsDragging(false);
                                    applyImage(e.dataTransfer.files?.[0]);
                                }}
                                className={`w-full rounded-[24px] border border-dashed p-5 text-center transition ${
                                    isDragging
                                        ? "border-brand-500 bg-brand-50/60 dark:bg-brand-500/10"
                                        : "border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/40"
                                } ${isReadOnly ? "opacity-60" : ""}`}
                            >
                                <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-2xl bg-[var(--app-panel-bg)] text-brand-500 shadow-sm">
                                    <UploadIcon className="size-6" />
                                </div>
                                <p className="mt-4 text-sm font-medium text-gray-800 dark:text-white/90">
                                    Drag and drop an image here
                                </p>
                                <p className="mt-1 text-xs text-gray-500 dark:text-gray-400">
                                    PNG, JPG, or WebP. This preview is local until an upload API is connected.
                                </p>
                            </div>

                            <input
                                ref={browseInputRef}
                                type="file"
                                accept={IMAGE_TYPES.join(",")}
                                className="hidden"
                                onChange={(e) => applyImage(e.target.files?.[0])}
                            />

                            <div className="grid w-full gap-3 sm:grid-cols-2">
                                <Button
                                    disabled={isReadOnly}
                                    variant="outline"
                                    onClick={() => browseInputRef.current?.click()}
                                    startIcon={<FileIcon />}
                                    className="justify-center"
                                >
                                    Browse
                                </Button>
                                <Button
                                    disabled={isReadOnly}
                                    variant="outline"
                                    onClick={cameraOpen ? () => {
                                        setCameraOpen(false);
                                        stopCamera();
                                    } : startCamera}
                                    startIcon={<CamIcon />}
                                    className="justify-center"
                                >
                                    {cameraOpen ? "Close Camera" : "Use Webcam"}
                                </Button>
                            </div>

                            {(imagePreview || cameraError) && (
                                <div className="w-full space-y-3">
                                    {cameraError && (
                                        <div className="rounded-2xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600 dark:border-red-900/60 dark:bg-red-950/30 dark:text-red-300">
                                            {cameraError}
                                        </div>
                                    )}
                                    {imagePreview && !isReadOnly && (
                                        <Button variant="danger" onClick={clearImage} startIcon={<ClearIcon />} className="w-full justify-center">
                                            Remove Image
                                        </Button>
                                    )}
                                </div>
                            )}

                            {cameraOpen && (
                                <div className="w-full rounded-[24px] border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/40 p-3">
                                    <div className="overflow-hidden rounded-[20px] bg-black">
                                        <video
                                            ref={videoRef}
                                            autoPlay
                                            playsInline
                                            muted
                                            className="h-64 w-full object-cover"
                                        />
                                    </div>
                                    <div className="mt-3 flex gap-3">
                                        <Button variant="green" onClick={capturePhoto} className="flex-1 justify-center">
                                            Capture Photo
                                        </Button>
                                        <Button variant="outline" onClick={() => {
                                            setCameraOpen(false);
                                            stopCamera();
                                        }} className="flex-1 justify-center">
                                            Cancel
                                        </Button>
                                    </div>
                                </div>
                            )}

                            <canvas ref={canvasRef} className="hidden" />
                        </div>
                    </aside>

                    <section className="space-y-6">
                        <div className="rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
                            <div className="mb-6 flex flex-col gap-3 lg:flex-row lg:items-end lg:justify-between">
                                <div>
                                    <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">Operator Details</p>
                                    <h2 className="mt-2 text-2xl font-semibold text-gray-900 dark:text-white">Lean and focused form</h2>
                                    <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                                        Clean inputs for account setup, contact details, role assignment, and location access.
                                    </p>
                                </div>
                                <div className="w-full max-w-xs">
                                    <Label>Password</Label>
                                    {type === FormType.UPDATE || type === FormType.CREATE ? (
                                        <Button
                                            onClick={handleChangePassword}
                                            disabled={isReadOnly}
                                            variant={dto.password.length > 0 ? "green" : "primary"}
                                            className="w-full justify-center"
                                        >
                                            {type === FormType.UPDATE ? "Change Password" : dto.password.length === 0 ? "Set Password" : "Password Ready"}
                                        </Button>
                                    ) : (
                                        <Input disabled name="password" type="password" value={dto.password} />
                                    )}
                                </div>
                            </div>

                            <div className="grid gap-5 md:grid-cols-2">
                                 <div>
                                    <Label htmlFor="operatorId">Operator Id</Label>
                                    <Input
                                        disabled={type === FormType.INFO || type === FormType.UPDATE}
                                        name="operatorId"
                                        id="operatorId"
                                        onChange={handleChange}
                                        value={dto.operatorId}
                                        placeholder="ABC123"
                                    />
                                </div>
                                <div>
                                    <Label htmlFor="username">Username</Label>
                                    <Input
                                        disabled={type === FormType.INFO || type === FormType.UPDATE}
                                        name="username"
                                        id="username"
                                        onChange={handleChange}
                                        value={dto.username}
                                        placeholder="operator.account"
                                    />
                                </div>

                                <div>
                                    <Label htmlFor="title">Title</Label>
                                    {/* <Input
                                        disabled={isReadOnly}
                                        name="title"
                                        id="title"
                                        onChange={handleChange}
                                        value={dto.title}
                                        placeholder="Mr / Ms / Dr"
                                        //Mr, Ms, Mrs, Dr, Prof, Other
                                    /> */}
                                    <Select 
                                    options={titleOptions}
                                    name="title"
                                    id="title"
                                    onChange={(e) => setDto(prev => ({...prev,title:Number(e)}))}
                                    defaultValue={dto.title}
                                    />
                                </div>

                                <div>
                                    <Label htmlFor="firstName">First Name</Label>
                                    <Input
                                        disabled={isReadOnly}
                                        name="firstName"
                                        id="firstName"
                                        onChange={handleChange}
                                        value={dto.firstName}
                                        placeholder="First name"
                                    />
                                </div>

                                <div>
                                    <Label htmlFor="middleName">Middle Name</Label>
                                    <Input
                                        disabled={isReadOnly}
                                        name="middleName"
                                        id="middleName"
                                        onChange={handleChange}
                                        value={dto.middleName}
                                        placeholder="Middle name"
                                    />
                                </div>

                                <div >
                                    <Label htmlFor="lastName">Last Name</Label>
                                    <Input
                                        disabled={isReadOnly}
                                        name="lastName"
                                        id="lastName"
                                        onChange={handleChange}
                                        value={dto.lastName}
                                        placeholder="Last name"
                                    />
                                </div>

                                <div>
                                    <Label htmlFor="email">Email</Label>
                                    <Input
                                        disabled={isReadOnly}
                                        name="email"
                                        type="email"
                                        id="email"
                                        onChange={handleChange}
                                        value={dto.email}
                                        placeholder="name@company.com"
                                    />
                                </div>

                                <div>
                                    <Label htmlFor="mobile">Mobile</Label>
                                    <Input
                                        disabled={isReadOnly}
                                        name="mobile"
                                        type="text"
                                        id="mobile"
                                        onChange={handleChange}
                                        value={dto.mobile}
                                        placeholder="+1 (555) 000-0000"
                                    />
                                </div>

                                <div className="md:col-span-2">
                                    <Label htmlFor="role">Role</Label>
                                    <Select
                                        disabled={isReadOnly}
                                        isString={false}
                                        options={roles}
                                        defaultValue={dto.roleId || -1}
                                        onChange={(value) => setDto((prev) => ({
                                            ...prev,
                                            roleId: Number(value),
                                            role: roles.find((option) => option.value === Number(value))?.label ?? ""
                                        }))}
                                        name="roleId"
                                        placeholder="Select role"
                                    />
                                </div>
                            </div>
                        </div>

                        <div className="rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
                            <div className="mb-6">
                                <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">Location Access</p>
                                <h3 className="mt-2 text-xl font-semibold text-gray-900 dark:text-white">Manage assigned locations</h3>
                                <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                                    Add locations one by one, then tap cards below to mark which ones should be removed.
                                </p>
                            </div>

                            <div className="flex flex-col gap-3 lg:flex-row">
                                <div className="flex-1">
                                    <Label htmlFor="location">Location</Label>
                                    <Select
                                        disabled={isReadOnly}
                                        isString={false}
                                        options={locations}
                                        defaultValue={locationsId}
                                        onChange={(value) => setLocationId(Number(value))}
                                        name="location"
                                        placeholder="Select location"
                                    />
                                </div>
                                <div className="flex gap-3 lg:items-end">
                                    <Button disabled={isReadOnly || locationsId === -1} onClick={addLocation} className="min-w-[120px] justify-center">
                                        Add
                                    </Button>
                                    <Button
                                        disabled={isReadOnly || selectedLocationIds.length === 0}
                                        variant="danger"
                                        onClick={removeSelectedLocations}
                                        className="min-w-[120px] justify-center"
                                    >
                                        Remove
                                    </Button>
                                </div>
                            </div>

                            <div className="mt-5 grid gap-3 sm:grid-cols-2 xl:grid-cols-3">
                                {dto.locationId.length > 0 ? dto.locationId.map((id) => (
                                    <button
                                        key={id}
                                        type="button"
                                        onClick={() => toggleLocationSelection(id)}
                                        className={`flex items-center gap-4 rounded-[22px] border px-4 py-4 text-left transition ${
                                            selectedLocationIds.includes(id)
                                                ? "border-brand-500 bg-brand-50 dark:bg-brand-500/10"
                                                : "border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/30 hover:border-brand-300"
                                        }`}
                                    >
                                        <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-[var(--app-panel-bg)] text-brand-500 shadow-sm">
                                            <LocationIcon />
                                        </div>
                                        <div>
                                            <p className="text-sm font-semibold text-gray-800 dark:text-white/90">
                                                {locations.find((location) => location.value === id)?.label ?? `Location ${id}`}
                                            </p>
                                            <p className="text-xs text-gray-500 dark:text-gray-400">
                                                {selectedLocationIds.includes(id) ? "Selected for removal" : "Assigned location"}
                                            </p>
                                        </div>
                                    </button>
                                )) : (
                                    <div className="col-span-full rounded-[22px] border border-dashed border-[var(--app-panel-border)] px-5 py-10 text-center text-sm text-gray-500 dark:text-gray-400">
                                        No locations assigned yet.
                                    </div>
                                )}
                            </div>
                        </div>

                        <div className="flex flex-wrap justify-end gap-3">
                            <Button
                                disabled={isReadOnly}
                                onClickWithEvent={handleClickWithEvent}
                                name={type === FormType.UPDATE ? "update" : "create"}
                                size="sm"
                            >
                                {type === FormType.UPDATE ? "Update Operator" : "Create Operator"}
                            </Button>
                            <Button variant="outline" onClickWithEvent={handleClickWithEvent} name="cancel" size="sm">
                                Cancel
                            </Button>
                        </div>
                    </section>
                </div>
            )}
        </>
    );
}
