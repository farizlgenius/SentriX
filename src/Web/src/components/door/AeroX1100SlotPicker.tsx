import { useState } from "react";
import { Modal } from "../ui/modal";
import type { Options } from "../../model/Options";

export type AeroSlotType = "reader" | "input" | "output";

interface AeroX1100SlotPickerProps {
  slotType: AeroSlotType;
  slotOptions: Options[];
  selectedSlot?: number;
  onSelect: (slotNo: number) => void;
  moduleSelected: boolean;
  disabled?: boolean;
}

const slotDefinitions: Record<
  AeroSlotType,
  { label: string; x: number; y: number; width: number; height: number }[]
> = {
  reader: [
    { label: "Reader 1", x: 12, y: 100, width: 44, height: 243 },
    { label: "Reader 2", x: 760, y: 100, width: 44, height: 243 },
  ],
  input: [
    { label: "Input 1", x: 12, y: 350, width: 44, height: 47 },
    { label: "Input 2", x: 12, y: 398, width: 44, height: 49 },
    { label: "Input 3", x: 760, y: 350, width: 44, height: 47 },
    { label: "Input 4", x: 760, y: 398, width: 44, height: 49 },
  ],
  output: [
    { label: "Output 1", x: 114, y: 444, width: 107, height: 62 },
    { label: "Output 2", x: 114, y: 508, width: 107, height: 57 },
    { label: "Output 3", x: 592, y: 444, width: 107, height: 62 },
    { label: "Output 4", x: 592, y: 508, width: 107, height: 57 },
  ],
};

const slotTypeLabels: Record<AeroSlotType, string> = {
  reader: "Reader",
  input: "Input",
  output: "Output",
};

const AeroX1100SlotPicker = ({
  slotType,
  slotOptions,
  selectedSlot,
  onSelect,
  moduleSelected,
  disabled = false,
}: AeroX1100SlotPickerProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const slots = slotDefinitions[slotType];
  const availableSlots = new Set(
    slotOptions
      .map((option) => Number(option.value))
      .filter((slotNo) => Number.isInteger(slotNo)),
  );
  const selectedLabel =
    selectedSlot !== undefined && selectedSlot >= 0
      ? `${slotTypeLabels[slotType]} ${selectedSlot + 1}`
      : undefined;

  const handleSelect = (slotNo: number) => {
    onSelect(slotNo);
    setIsOpen(false);
  };

  return (
    <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-900">
            <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
              <p className="text-sm text-gray-500 dark:text-gray-400">
                Green slots are available, red slots are occupied, and blue marks the current selection.
              </p>
              <div className="flex flex-wrap gap-3 text-xs text-gray-600 dark:text-gray-300">
                <span className="inline-flex items-center gap-1.5">
                <span className="h-2.5 w-2.5 rounded bg-green-500" />
                Available
              </span>
              <span className="inline-flex items-center gap-1.5">
                <span className="h-2.5 w-2.5 rounded bg-red-500" />
                Unavailable
              </span>
              <span className="inline-flex items-center gap-1.5">
                <span className="h-2.5 w-2.5 rounded bg-blue-600" />
                Selected
              </span>
              </div>
              <div className="max-h-[70vh] overflow-auto">
              <div className="relative mx-auto w-full max-w-[816px]">
                <img
                  src="/images/aero-x1100.png"
                  alt="Aero X1100 access controller with reader, input, and relay terminals"
                  className="block h-auto w-full rounded-lg"
                />
                <svg
                  aria-label={`Interactive X1100 ${slotTypeLabels[slotType]} slots`}
                  className="absolute inset-0 h-full w-full"
                  viewBox="0 0 816 699"
                  preserveAspectRatio="none"
                  role="group"
                >
                  {slots.map((slot, index) => {
                    const isSelected = selectedSlot === index;
                    const isAvailable = availableSlots.has(index) || isSelected;
                    const slotStatus = isSelected
                      ? "Selected"
                      : isAvailable
                        ? "Available"
                        : "Unavailable";
    
                    return (
                      <g
                        key={slot.label}
                        className={
                          isAvailable && !disabled
                            ? "cursor-pointer"
                            : "cursor-not-allowed"
                        }
                        role="button"
                        tabIndex={isAvailable && !disabled ? 0 : -1}
                        aria-disabled={!isAvailable || disabled}
                        aria-pressed={isSelected}
                        aria-label={`${slot.label}, ${slotStatus}`}
                        onClick={() => {
                          if (isAvailable && !disabled) handleSelect(index);
                        }}
                        onKeyDown={(event) => {
                          if (
                            isAvailable &&
                            !disabled &&
                            (event.key === "Enter" || event.key === " ")
                          ) {
                            event.preventDefault();
                            handleSelect(index);
                          }
                        }}
                      >
                        <title>{`${slot.label} - ${slotStatus}`}</title>
                        <rect
                          x={slot.x}
                          y={slot.y}
                          width={slot.width}
                          height={slot.height}
                          rx="5"
                          fill={
                            isSelected
                              ? "#2563eb"
                              : isAvailable
                                ? "#22c55e"
                                : "#ef4444"
                          }
                          fillOpacity={isSelected ? 0.4 : 0.28}
                          stroke={
                            isSelected
                              ? "#1d4ed8"
                              : isAvailable
                                ? "#16a34a"
                                : "#dc2626"
                          }
                          strokeWidth={isSelected ? "4" : "2"}
                        />
                      </g>
                    );
                  })}
                </svg>
              </div>
            </div>
            </div>
            </div>
  );
};

export default AeroX1100SlotPicker;
