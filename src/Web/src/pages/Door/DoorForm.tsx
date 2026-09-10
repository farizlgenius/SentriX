import { PropsWithChildren, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import AeroDoorForm from "./AeroDoorForm";
import AmicoDoorForm from "./AmicoDoorForm";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import { LockIcon, SettingIcon } from "../../icons";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";

type DoorComponent =
  | "readerIn"
  | "readerOut"
  | "rex"
  | "magneticLock"
  | "buzzer"
  | "sensor";

type DoorAccessLayout = "inOut" | "inOnly";

const componentDetails: Record<
  DoorComponent,
  { title: string; detail: string }
> = {
  readerIn: {
    title: "Reader In",
    detail: "Public-side reader used to request entry.",
  },
  readerOut: {
    title: "Reader Out",
    detail: "Secure-side reader used to request exit.",
  },
  rex: {
    title: "REX",
    detail: "Request-to-exit input used when no Reader Out is installed.",
  },
  magneticLock: {
    title: "Magnetic Lock",
    detail: "Relay-controlled locking component on the door frame.",
  },
  buzzer: {
    title: "Buzzer",
    detail: "Alarm or door-status sounder output.",
  },
  sensor: {
    title: "Door Sensor",
    detail: "Monitors the door position and held-open state.",
  },
};

const DoorLayout = ({
  selected,
  hasReaderOut,
  onSelect,
}: {
  selected: DoorComponent;
  hasReaderOut: boolean;
  onSelect: (component: DoorComponent) => void;
}) => {
  const device = (
    id: DoorComponent,
    x: number,
    y: number,
    width: number,
    height: number,
  ) => {
    const active = selected === id;
    return (
      <g className="cursor-pointer" onClick={() => onSelect(id)}>
        <rect
          x={x}
          y={y}
          width={width}
          height={height}
          rx="4"
          fill={active ? "#e0f2fe" : "#72b6dc"}
          stroke={active ? "#0284c7" : "#4f87a8"}
          strokeWidth={active ? "2.5" : "1.5"}
        />
      </g>
    );
  };

  return (
    <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-100">
      <svg
        aria-label="Single ACS door accessory layout"
        className="mx-auto min-w-[760px]"
        viewBox="0 0 900 500"
        role="img"
      >
        <text x="28" y="48" fill="#3f3f46" fontSize="30" fontWeight="700">SINGLE ACS DOOR ACCESSORY</text>
        <rect x="130" y="150" width="150" height="250" fill="#fff" stroke="#09090b" strokeWidth="2" />
        <rect x="142" y="162" width="126" height="226" fill="#fff" stroke="#09090b" strokeWidth="1.5" />
        <circle cx="158" cy="270" r="8" fill="#fff" stroke="#09090b" strokeWidth="1.5" />
        {device("readerIn", 102, 252, 16, 38)}
        <text x="110" y="320" textAnchor="middle" fill="#09090b" fontSize="15" fontWeight="600">Reader</text>
        <text x="205" y="433" textAnchor="middle" fill="#09090b" fontSize="16" fontWeight="600">Outside</text>

        <rect x="590" y="150" width="150" height="250" fill="#fff" stroke="#09090b" strokeWidth="2" />
        <rect x="602" y="162" width="126" height="226" fill="#fff" stroke="#09090b" strokeWidth="1.5" />
        <circle cx="712" cy="270" r="8" fill="#fff" stroke="#09090b" strokeWidth="1.5" />
        {device("magneticLock", 605, 144, 58, 22)}
        <text x="634" y="132" textAnchor="middle" fill="#09090b" fontSize="15" fontWeight="600">Magnetic Lock</text>
        {device("sensor", 668, 145, 22, 10)}
        <text x="701" y="135" fill="#09090b" fontSize="15" fontWeight="600">Door Sensor</text>
        {hasReaderOut ? (
          <>
            {device("buzzer", 754, 144, 28, 28)}
            <rect x="759" y="152" width="18" height="10" rx="1" fill="none" stroke={selected === "buzzer" ? "#0284c7" : "#4f87a8"} strokeWidth="1.5" />
            <text x="790" y="190" fill="#09090b" fontSize="15" fontWeight="600">Emergency</text>
            <text x="790" y="212" fill="#09090b" fontSize="15" fontWeight="600">Break Glass</text>
          </>
        ) : (
          <>
            {device("buzzer", 540, 156, 28, 28)}
            <rect x="545" y="164" width="18" height="10" rx="1" fill="none" stroke={selected === "buzzer" ? "#0284c7" : "#4f87a8"} strokeWidth="1.5" />
            <text x="455" y="200" fill="#09090b" fontSize="15" fontWeight="600">Emergency</text>
            <text x="455" y="222" fill="#09090b" fontSize="15" fontWeight="600">Break Glass</text>
          </>
        )}
        {hasReaderOut ? device("readerOut", 754, 252, 16, 38) : device("rex", 754, 252, 16, 38)}
        <text x="785" y="280" fill="#09090b" fontSize="15" fontWeight="600">{hasReaderOut ? "Reader Out" : "Exit Button (REX)"}</text>
        <text x="665" y="433" textAnchor="middle" fill="#09090b" fontSize="16" fontWeight="600">Inside</text>
      </svg>
    </div>
  );
};

const DoorForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  handleClick,
  dto,
  setDto,
  type,
}) => {
  const selectedType = Vendor.aero;
  const [selectedComponent, setSelectedComponent] =
    useState<DoorComponent>("readerIn");
  const [accessLayout, setAccessLayout] = useState<DoorAccessLayout>(
    dto.type === DoorType.Dual ? "inOut" : "inOnly",
  );
  const hasReaderOut = accessLayout === "inOut";

  const chooseAccessLayout = (nextLayout: DoorAccessLayout) => {
    const readerOutEnabled = nextLayout === "inOut";
    setAccessLayout(nextLayout);
    setSelectedComponent(readerOutEnabled ? "readerOut" : "rex");
    setDto((previous) => ({
      ...previous,
      type: readerOutEnabled ? DoorType.Dual : DoorType.Single,
    }));
  };
  const FormTypeSwitcher = (value: Vendor) => {
    switch (value) {
      case Vendor.aero:
        return (
          <AeroDoorForm
            handleClick={handleClick}
            dto={dto}
            setDto={setDto}
            type={type}
            focusComponent={selectedComponent}
            hasReaderOut={hasReaderOut}
          />
        );
      case Vendor.amico:
        return (
          <AmicoDoorForm
            handleClick={handleClick}
            dto={dto}
            setDto={setDto}
            type={type}
          />
        );
      default:
        return <></>;
    }
  };
  return (
    <div className="space-y-5">
      <FormSection
        title="Door setup"
        description="Provide a name for this door before configuring its access components."
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <FormField>
            <Label htmlFor="name">Door name</Label>
            <Input
              id="name"
              name="name"
              placeholder="Main lobby door"
              value={dto.name}
              disabled={type === FormType.INFO}
              onChange={(event) =>
                setDto((previous) => ({
                  ...previous,
                  name: event.target.value,
                }))
              }
            />
          </FormField>
        </div>
      </FormSection>
      <div className="grid gap-5 xl:grid-cols-[minmax(0,1.25fr)_minmax(340px,0.75fr)]">
        <FormSection title="Door component layout" description="Choose the access layout, then select a component in the door elevation to configure it.">
          <div className="mb-4 flex flex-wrap gap-2">
            <button type="button" onClick={() => chooseAccessLayout("inOut")} className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}>
              In / Out readers
            </button>
            <button type="button" onClick={() => chooseAccessLayout("inOnly")} className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${!hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}>
              In only + REX
            </button>
          </div>
          <DoorLayout selected={selectedComponent} hasReaderOut={hasReaderOut} onSelect={setSelectedComponent} />
          <div className="mt-4 flex items-start gap-3 rounded-xl border border-brand-100 bg-brand-50/60 p-4 dark:border-brand-500/20 dark:bg-brand-500/10">
            {selectedComponent === "magneticLock" ? <LockIcon className="mt-0.5 h-5 w-5 shrink-0 text-brand-600 dark:text-brand-300" /> : <SettingIcon className="mt-0.5 h-5 w-5 shrink-0 text-brand-600 dark:text-brand-300" />}
            <div>
              <p className="font-semibold text-brand-900 dark:text-brand-100">{componentDetails[selectedComponent].title} selected</p>
              <p className="mt-1 text-sm text-brand-800 dark:text-brand-200">{componentDetails[selectedComponent].detail}</p>
            </div>
          </div>
        </FormSection>
        <div className="min-w-0">{FormTypeSwitcher(selectedType)}</div>
      </div>
    </div>
  );
};

export default DoorForm;
