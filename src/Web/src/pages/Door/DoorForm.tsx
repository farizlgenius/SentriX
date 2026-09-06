import { PropsWithChildren, useState } from "react";
import { FormProp } from "../../model/Form/FormProp";
import { AeroDoorMetadata, DoorDto } from "../../model/Door/DoorDto";
import AeroDoorForm from "./AeroDoorForm";
import AmicoDoorForm from "./AmicoDoorForm";
import { Vendor } from "../../enum/Vendor";
import { LockIcon, SettingIcon } from "../../icons";
import { FormSection } from "../../components/form/template/FormTemplate";

type DoorComponent =
  | "readerIn"
  | "readerOut"
  | "rex"
  | "magneticLock"
  | "buzzer";

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
  const component = (
    id: DoorComponent,
    x: number,
    y: number,
    label: string,
    fill = "#ffffff",
  ) => {
    const active = selected === id;
    return (
      <g className="cursor-pointer" onClick={() => onSelect(id)}>
        <rect
          x={x}
          y={y}
          width="116"
          height="38"
          rx="7"
          fill={active ? "#e0f2fe" : fill}
          stroke={active ? "#0284c7" : "#374151"}
          strokeWidth={active ? "2.5" : "1.5"}
        />
        <text
          x={x + 58}
          y={y + 24}
          textAnchor="middle"
          fill={active ? "#0369a1" : "#111827"}
          fontSize="12"
          fontWeight="600"
        >
          {label}
        </text>
      </g>
    );
  };

  return (
    <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-100">
      <svg
        aria-label="Door component elevation layout"
        className="mx-auto min-w-[640px]"
        viewBox="0 0 760 380"
        role="img"
      >
        <text x="28" y="28" fill="#111827" fontSize="12" fontWeight="700">
          PUBLIC SIDE
        </text>
        <text x="628" y="28" fill="#111827" fontSize="12" fontWeight="700">
          SECURE SIDE
        </text>
        <line
          x1="28"
          y1="328"
          x2="732"
          y2="328"
          stroke="#111827"
          strokeWidth="2"
        />
        <rect
          x="230"
          y="70"
          width="300"
          height="258"
          fill="#ffffff"
          stroke="#111827"
          strokeWidth="2"
        />
        <rect
          x="248"
          y="92"
          width="120"
          height="218"
          fill="#f8fafc"
          stroke="#111827"
          strokeWidth="1.5"
        />
        <rect
          x="392"
          y="92"
          width="120"
          height="218"
          fill="#f8fafc"
          stroke="#111827"
          strokeWidth="1.5"
        />
        <line
          x1="380"
          y1="70"
          x2="380"
          y2="328"
          stroke="#111827"
          strokeWidth="2"
        />
        <line
          x1="230"
          y1="82"
          x2="530"
          y2="82"
          stroke="#111827"
          strokeWidth="4"
        />
        <path
          d="M 310 292 A 62 62 0 0 0 372 230"
          fill="none"
          stroke="#9ca3af"
          strokeWidth="1.5"
          strokeDasharray="5 4"
        />
        <path
          d="M 450 292 A 62 62 0 0 1 388 230"
          fill="none"
          stroke="#9ca3af"
          strokeWidth="1.5"
          strokeDasharray="5 4"
        />
        <text x="308" y="210" textAnchor="middle" fill="#6b7280" fontSize="11">
          DOOR LEAF
        </text>
        <text x="452" y="210" textAnchor="middle" fill="#6b7280" fontSize="11">
          DOOR LEAF
        </text>
        <line
          x1="226"
          y1="198"
          x2="160"
          y2="198"
          stroke="#6b7280"
          strokeWidth="1"
        />
        <line
          x1="534"
          y1="198"
          x2="600"
          y2="198"
          stroke="#6b7280"
          strokeWidth="1"
        />
        <line
          x1="380"
          y1="66"
          x2="380"
          y2="42"
          stroke="#6b7280"
          strokeWidth="1"
        />
        <line
          x1="530"
          y1="110"
          x2="604"
          y2="76"
          stroke="#6b7280"
          strokeWidth="1"
        />
        {component("readerIn", 104, 180, "Reader In")}
        {hasReaderOut
          ? component("readerOut", 608, 180, "Reader Out")
          : component("rex", 608, 180, "REX")}
        {component("magneticLock", 322, 20, "Magnetic Lock", "#fefce8")}
        {component("buzzer", 608, 54, "Buzzer", "#fff7ed")}
        <text x="104" y="238" fill="#6b7280" fontSize="10">
          ACCESS CONTROL
        </text>
        <text x="608" y="238" fill="#6b7280" fontSize="10">
          ACCESS CONTROL
        </text>
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
  const aeroMetadata =
    typeof dto.metadata === "string"
      ? null
      : (dto.metadata as AeroDoorMetadata);
  const hasReaderOut =
    (aeroMetadata?.readerOut?.readerModuleComponentId ?? -1) > -1;
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
        title="Door component layout"
        description="Select a component in the top-down plan, then use the detailed configuration below to set its module and port."
      >
        <DoorLayout
          selected={selectedComponent}
          hasReaderOut={hasReaderOut}
          onSelect={setSelectedComponent}
        />
        <div className="mt-4 flex items-start gap-3 rounded-xl border border-brand-100 bg-brand-50/60 p-4 dark:border-brand-500/20 dark:bg-brand-500/10">
          {selectedComponent === "magneticLock" ? (
            <LockIcon className="mt-0.5 h-5 w-5 shrink-0 text-brand-600 dark:text-brand-300" />
          ) : (
            <SettingIcon className="mt-0.5 h-5 w-5 shrink-0 text-brand-600 dark:text-brand-300" />
          )}
          <div>
            <p className="font-semibold text-brand-900 dark:text-brand-100">
              {componentDetails[selectedComponent].title} selected
            </p>
            <p className="mt-1 text-sm text-brand-800 dark:text-brand-200">
              {componentDetails[selectedComponent].detail}
            </p>
          </div>
        </div>
      </FormSection>
      {FormTypeSwitcher(selectedType)}
    </div>
  );
};

export default DoorForm;
