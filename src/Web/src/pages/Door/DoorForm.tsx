import { PropsWithChildren, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import AeroDoorForm from "./AeroDoorForm";
import AmicoDoorForm from "./AmicoDoorForm";
import { Vendor } from "../../enum/Vendor";
import { DoorType } from "../../enum/DoorType";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import Select from "../../components/form/Select";

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
        <text x="28" y="48" fill="#3f3f46" fontSize="30" fontWeight="700">
          ACS DOOR ACCESSORY
        </text>
        <rect
          x="130"
          y="150"
          width="150"
          height="250"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="2"
        />
        <rect
          x="142"
          y="162"
          width="126"
          height="226"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="1.5"
        />
        <circle
          cx="158"
          cy="270"
          r="8"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="1.5"
        />
        {device("readerIn", 102, 252, 16, 38)}
        <text
          x="100"
          y="320"
          textAnchor="middle"
          fill="#09090b"
          fontSize="15"
          fontWeight="600"
        >
          Reader
        </text>
        <text
          x="205"
          y="433"
          textAnchor="middle"
          fill="#09090b"
          fontSize="16"
          fontWeight="600"
        >
          Outside
        </text>

        <rect
          x="590"
          y="150"
          width="150"
          height="250"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="2"
        />
        <rect
          x="602"
          y="162"
          width="126"
          height="226"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="1.5"
        />
        <circle
          cx="712"
          cy="270"
          r="8"
          fill="#fff"
          stroke="#09090b"
          strokeWidth="1.5"
        />
        {device("magneticLock", 605, 144, 58, 22)}
        <text
          x="634"
          y="132"
          textAnchor="middle"
          fill="#09090b"
          fontSize="15"
          fontWeight="600"
        >
          Magnetic Lock
        </text>
        {device("sensor", 668, 145, 22, 10)}
        <text x="701" y="135" fill="#09090b" fontSize="15" fontWeight="600">
          Door Sensor
        </text>
        {hasReaderOut ? (
          <>
            {device("buzzer", 754, 144, 28, 28)}
            <rect
              x="759"
              y="152"
              width="18"
              height="10"
              rx="1"
              fill="none"
              stroke={selected === "buzzer" ? "#0284c7" : "#4f87a8"}
              strokeWidth="1.5"
            />
            <text x="790" y="190" fill="#09090b" fontSize="15" fontWeight="600">
              Emergency
            </text>
            <text x="790" y="212" fill="#09090b" fontSize="15" fontWeight="600">
              Break Glass
            </text>
          </>
        ) : (
          <>
            {device("buzzer", 540, 156, 28, 28)}
            <rect
              x="545"
              y="164"
              width="18"
              height="10"
              rx="1"
              fill="none"
              stroke={selected === "buzzer" ? "#0284c7" : "#4f87a8"}
              strokeWidth="1.5"
            />
            <text x="455" y="200" fill="#09090b" fontSize="15" fontWeight="600">
              Break Glass
            </text>
          </>
        )}
        {hasReaderOut
          ? device("readerOut", 754, 252, 16, 38)
          : device("rex", 754, 252, 16, 38)}
        <text x="785" y="280" fill="#09090b" fontSize="15" fontWeight="600">
          {hasReaderOut ? "Reader Out" : "Exit Button (REX)"}
        </text>
        <text
          x="665"
          y="433"
          textAnchor="middle"
          fill="#09090b"
          fontSize="16"
          fontWeight="600"
        >
          Inside
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
    <div className="grid grid-cols-3 gap-5">
      <FormSection
        title="Door setup"
        description="Provide a name for this door before configuring its access components."
        className="col-span-3"
      >
        <div className="grid gap-5 sm:grid-cols-2">
          <FormField>
            <Label htmlFor="name">Name</Label>
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
          <FormField>
            <Label htmlFor="type">Type</Label>
            <Select name={"type"} options={[]} />
          </FormField>
        </div>
      </FormSection>
      <FormSection
        title="Door component layout"
        description="Choose the access layout, then select a component in the door elevation to configure it."
        className="col-span-2"
      >
        <div className="mb-4 flex flex-wrap gap-2">
          <button
            type="button"
            onClick={() => chooseAccessLayout("inOut")}
            className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}
          >
            Dual Reader
          </button>
          <button
            type="button"
            onClick={() => chooseAccessLayout("inOnly")}
            className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${!hasReaderOut ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}
          >
            Single Reader
          </button>
        </div>
        <DoorLayout
          selected={selectedComponent}
          hasReaderOut={hasReaderOut}
          onSelect={setSelectedComponent}
        />
      </FormSection>
      {FormTypeSwitcher(selectedType)}
      <FormSection
        title="Door component layout"
        description="Choose the access layout, then select a component in the door elevation to configure it."
      >
        <FormField className="flex flex-col gap-1">
          <Label htmlFor="antiPassbackMode">Anti-Passback Mode</Label>
          <Select
            disabled={type == FormType.INFO}
            name="antiPassbackMode"
            options={[]}
            onChange={(value: string) => {}}
            className="dark:bg-dark-900"
            defaultValue={
              (dto.metadata as AeroDoorMetadata).antipassback
                ?.antipassbackMode ?? ""
            }
          />
          <Label htmlFor="antiPassBackIn">Area From</Label>
          <Select
            disabled={type == FormType.INFO}
            isString={false}
            name="antiPassBackIn"
            options={[]}
            onChange={(value: string) => {}}
            className="dark:bg-dark-900"
            defaultValue={
              (dto.metadata as AeroDoorMetadata).antipassback?.areaIn ?? ""
            }
          />
          <Label htmlFor="antiPassBackOut">Area To</Label>
          <Select
            disabled={type == FormType.INFO}
            isString={false}
            name="antiPassBackOut"
            options={[]}
            onChange={(value: string) => {}}
            className="dark:bg-dark-900"
            defaultValue={
              (dto.metadata as AeroDoorMetadata).antipassback?.areaOut ?? ""
            }
          />
        </FormField>
      </FormSection>

      <FormSection>
        <FormField className="flex flex-col gap-1">
          <Label htmlFor="offlineMode">Offline Mode</Label>
          <Select
            disabled={type == FormType.INFO}
            isString={false}
            name="offlineMode"
            options={[]}
            onChange={(value: string) => {
              setDto((prev) => ({
                ...prev,
                metadata: {
                  ...(prev.metadata as AeroDoorMetadata),
                  offlineMode: Number(value),
                },
              }));
            }}
            className="dark:bg-dark-900"
            defaultValue={(dto.metadata as AeroDoorMetadata).offlineMode ?? ""}
          />
          <Label htmlFor="defaultMode">Default Mode</Label>
          <Select
            disabled={type == FormType.INFO}
            isString={false}
            name="defaultMode"
            options={[]}
            onChange={(value: string) => {
              setDto((prev) => ({
                ...prev,
                metadata: {
                  ...(prev.metadata as AeroDoorMetadata),
                  defaultMode: Number(value),
                },
              }));
            }}
            className="dark:bg-dark-900"
            defaultValue={(dto.metadata as AeroDoorMetadata).defaultMode ?? ""}
          />
        </FormField>
      </FormSection>
      <FormSection>
        <div className="grid grid-cols-2 gap-4">
          {/* <div className="rounded-2xl border border-gray-200 bg-gray-50/80 p-5 dark:border-gray-800 dark:bg-white/[0.02]">
            <h4 className="text-base font-semibold text-gray-900 dark:text-white">
              Access Control Flags
            </h4>
            <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
              Enable only the behaviors this door should enforce during normal
              operation.
            </p>
            <div className="mt-4 grid grid-cols-1 gap-3">
              {accessFlag.map((d, i) => (
                <div
                  key={i}
                  className="rounded-xl border border-gray-200 bg-white px-4 py-3 dark:border-gray-700 dark:bg-gray-900"
                >
                  <Switch
                    disabled={type == FormType.INFO}
                    label={d.label}
                    defaultChecked={false}
                    onChange={(checked: boolean) => {
                      setDto((prev) => ({
                        ...prev,
                        metadata: {
                          ...(prev.metadata as AeroDoorMetadata),
                          accessControlFlag: checked
                            ? (prev.metadata as AeroDoorMetadata)
                                .accessControlFlag | Number(d.value)
                            : (prev.metadata as AeroDoorMetadata)
                                .accessControlFlag & ~Number(d.value),
                        },
                      }));
                    }}
                  />
                  {d.description && (
                    <p className="mt-1 whitespace-pre-line text-xs text-gray-500 dark:text-gray-400">
                      {formatFlagDescription(d.description)}
                    </p>
                  )}
                </div>
              ))}
            </div>
          </div>

          <div className="rounded-2xl border border-gray-200 bg-gray-50/80 p-5 dark:border-gray-800 dark:bg-white/[0.02]">
            <h4 className="text-base font-semibold text-gray-900 dark:text-white">
              Advanced Flags
            </h4>
            <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
              Additional low-level options. Keep these disabled unless your
              scenario requires them.
            </p>
            <div className="mt-4 grid grid-cols-1 gap-3">
              {spareFlag.map((d, i) => (
                <div
                  key={i}
                  className="rounded-xl border border-gray-200 bg-white px-4 py-3 dark:border-gray-700 dark:bg-gray-900"
                >
                  <Switch
                    disabled={type == FormType.INFO}
                    label={d.label}
                    defaultChecked={false}
                    onChange={(checked: boolean) => {
                      setDto((prev) => ({
                        ...prev,
                        metadata: {
                          ...(prev.metadata as AeroDoorMetadata),
                          spare: checked
                            ? (prev.metadata as AeroDoorMetadata).spare |
                              Number(d.value)
                            : (prev.metadata as AeroDoorMetadata).spare &
                              ~Number(d.value),
                        },
                      }));
                    }}
                  />
                  {d.description && (
                    <p className="mt-1 whitespace-pre-line text-xs text-gray-500 dark:text-gray-400">
                      {formatFlagDescription(d.description)}
                    </p>
                  )}
                </div>
              ))}
            </div>
          </div> */}
        </div>
      </FormSection>
    </div>
  );
};

export default DoorForm;
