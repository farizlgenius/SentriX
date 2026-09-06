import { PropsWithChildren, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";
import { AddIcon, BoxCubeIcon, DoorIcon, TrashBinIcon } from "../../icons";
import {
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import { FormProp, FormType } from "../../model/Form/FormProp";

type LaneDirection = "bidirectional" | "entry" | "exit";

interface TurnstileLane {
  id: string;
  name: string;
  direction: LaneDirection;
  entryReader: string;
  exitReader: string;
  unlockRelay: string;
  alarmRelay: string;
}

interface TurnstileConfiguration {
  name: string;
  model: "swing" | "tripod" | "flap";
  lanes: TurnstileLane[];
}

const createLane = (number: number): TurnstileLane => ({
  id: `lane-${Date.now()}-${number}`,
  name: `Lane ${number}`,
  direction: "bidirectional",
  entryReader: "",
  exitReader: "",
  unlockRelay: "",
  alarmRelay: "",
});

const initialConfiguration = (dto: object): TurnstileConfiguration => {
  const config = dto as Partial<TurnstileConfiguration>;
  return {
    name: config.name ?? "",
    model: config.model ?? "swing",
    lanes: config.lanes?.length ? config.lanes : [createLane(1)],
  };
};

const TurnstilePreview = ({
  lanes,
  selectedLaneId,
  onSelect,
}: {
  lanes: TurnstileLane[];
  selectedLaneId: string;
  onSelect: (laneId: string) => void;
}) => (
  <div className="overflow-x-auto rounded-2xl border border-[var(--app-panel-border)] bg-white p-5 dark:bg-gray-100">
    <svg
      aria-label="Top-down turnstile floor plan"
      className="mx-auto min-w-[520px]"
      viewBox={`0 0 ${Math.max(lanes.length * 130 + 100, 520)} 250`}
      role="img"
    >
      {(() => {
        const width = Math.max(lanes.length * 130 + 100, 520);
        const laneWidth = (width - 100) / lanes.length;
        const railStart = 50;
        const postY = 67;
        const postHeight = 105;

        return (
          <>
            <text x="22" y="26" fill="#111827" fontSize="12" fontWeight="600">
              TOP VIEW · PUBLIC SIDE
            </text>
            <text x="22" y="232" fill="#111827" fontSize="12" fontWeight="600">
              SECURE SIDE
            </text>
            {lanes.map((lane, index) => {
              const x = railStart + index * laneWidth;
              const selected = selectedLaneId === lane.id;
              const centerX = x + laneWidth / 2;
              const entryReaderX = x + 20;
              const readerX = x + laneWidth - 36;
              const directionArrow =
                lane.direction === "bidirectional"
                  ? "↕"
                  : lane.direction === "entry"
                    ? "↓"
                    : "↑";
              return (
                <g key={lane.id} className="cursor-pointer" onClick={() => onSelect(lane.id)}>
                  <rect x={x + 8} y="51" width={laneWidth - 16} height="139" rx="7" fill={selected ? "#e0f2fe" : "transparent"} stroke={selected ? "#0284c7" : "transparent"} strokeWidth="2" />
                  <path d={`M ${x + 22} 122 L ${centerX - 11} 122`} fill="none" stroke="#111827" strokeWidth="4" strokeLinecap="round" />
                  <path d={`M ${x + laneWidth - 22} 122 L ${centerX + 11} 122`} fill="none" stroke="#111827" strokeWidth="4" strokeLinecap="round" />
                  <text x={centerX} y="104" textAnchor="middle" fill="#111827" fontSize="32" fontWeight="700">{directionArrow}</text>
                  <rect x={entryReaderX} y="75" width="16" height="16" rx="3" fill="#ffffff" stroke="#111827" strokeWidth="1.5" />
                  <circle cx={entryReaderX + 8} cy="83" r="2.5" fill="#0284c7" />
                  <text x={entryReaderX + 8} y="70" textAnchor="middle" fill="#374151" fontSize="8" fontWeight="700">IN</text>
                  <rect x={readerX} y="155" width="16" height="16" rx="3" fill="#ffffff" stroke="#111827" strokeWidth="1.5" />
                  <circle cx={readerX + 8} cy="163" r="2.5" fill="#f97316" />
                  <text x={readerX + 8} y="183" textAnchor="middle" fill="#374151" fontSize="8" fontWeight="700">OUT</text>
                  <text x={centerX} y="207" textAnchor="middle" fill={selected ? "#0369a1" : "#374151"} fontSize="12" fontWeight={selected ? "700" : "500"}>{lane.name}</text>
                </g>
              );
            })}
            {Array.from({ length: lanes.length + 1 }).map((_, index) => {
              const x = railStart + index * laneWidth - 12;
              return (
                <g key={index}>
                  <rect x={x} y={postY} width="24" height={postHeight} rx="11" fill="#ffffff" stroke="#111827" strokeWidth="2" />
                  <line x1={x + 4} y1="84" x2={x + 20} y2="84" stroke="#111827" strokeWidth="2" />
                  <line x1={x + 4} y1="155" x2={x + 20} y2="155" stroke="#111827" strokeWidth="2" />
                </g>
              );
            })}
          </>
        );
      })()}
    </svg>
  </div>
);

const TurnstileForm: React.FC<PropsWithChildren<FormProp<object>>> = ({
  dto,
  setDto,
  type,
}) => {
  const readOnly = type === FormType.INFO;
  const [configuration, setConfiguration] = useState<TurnstileConfiguration>(
    () => initialConfiguration(dto),
  );
  const [selectedLaneId, setSelectedLaneId] = useState(
    configuration.lanes[0].id,
  );

  const updateConfiguration = (next: TurnstileConfiguration) => {
    setConfiguration(next);
    setDto(next);
  };
  const updateLane = (laneId: string, update: Partial<TurnstileLane>) =>
    updateConfiguration({
      ...configuration,
      lanes: configuration.lanes.map((lane) =>
        lane.id === laneId ? { ...lane, ...update } : lane,
      ),
    });
  const addLane = () => {
    const lane = createLane(configuration.lanes.length + 1);
    updateConfiguration({
      ...configuration,
      lanes: [...configuration.lanes, lane],
    });
    setSelectedLaneId(lane.id);
  };
  const removeLane = (laneId: string) => {
    if (configuration.lanes.length === 1) return;
    const lanes = configuration.lanes.filter((lane) => lane.id !== laneId);
    updateConfiguration({ ...configuration, lanes });
    setSelectedLaneId(lanes[0].id);
  };
  const selectedLane =
    configuration.lanes.find((lane) => lane.id === selectedLaneId) ??
    configuration.lanes[0];

  return (
    <div className="space-y-5">
      <FormSection
        title="Turnstile setup"
        description="Choose a model, lay out its lanes, and assign reader and relay components to each lane."
      >
        <div className="grid gap-5 lg:grid-cols-[1fr_auto]">
          <div className="grid gap-5 sm:grid-cols-2">
            <FormField>
              <Label htmlFor="name">Turnstile name</Label>
              <Input
                id="name"
                name="name"
                placeholder="Main lobby turnstiles"
                value={configuration.name}
                disabled={readOnly}
                onChange={(event) =>
                  updateConfiguration({
                    ...configuration,
                    name: event.target.value,
                  })
                }
              />
            </FormField>
            <FormField>
              <Label htmlFor="model">Physical model</Label>
              <select
                id="model"
                value={configuration.model}
                disabled={readOnly}
                onChange={(event) =>
                  updateConfiguration({
                    ...configuration,
                    model: event.target
                      .value as TurnstileConfiguration["model"],
                  })
                }
                className="h-12 w-full rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] px-4 text-sm text-gray-800 shadow-theme-xs dark:text-white"
              >
                <option value="swing">Swing gate</option>
                <option value="tripod">Tripod</option>
                <option value="flap">Flap barrier</option>
              </select>
            </FormField>
          </div>
          <div className="flex items-center gap-2 rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-muted)] px-4 py-3 text-sm font-medium text-gray-600 dark:text-gray-300">
            <BoxCubeIcon className="h-4 w-4" />
            Top-down floor plan
          </div>
        </div>
      </FormSection>

      <div className="grid gap-5 xl:grid-cols-[minmax(0,1.25fr)_minmax(340px,0.75fr)]">
        <FormSection
          title="Lane layout"
          description="Click a lane in the model or lane list to configure it."
        >
          <TurnstilePreview
            lanes={configuration.lanes}
            selectedLaneId={selectedLane.id}
            onSelect={setSelectedLaneId}
          />
          <div className="mt-4 flex flex-wrap gap-2">
            {configuration.lanes.map((lane) => (
              <button
                key={lane.id}
                type="button"
                onClick={() => setSelectedLaneId(lane.id)}
                className={`rounded-xl border px-3 py-2 text-sm font-medium transition ${selectedLane.id === lane.id ? "border-brand-500 bg-brand-50 text-brand-700 dark:bg-brand-500/10 dark:text-brand-300" : "border-[var(--app-panel-border)] text-gray-600 dark:text-gray-300"}`}
              >
                {lane.name}
              </button>
            ))}
            <Button
              size="sm"
              variant="outline"
              disabled={readOnly}
              startIcon={<AddIcon className="h-4 w-4" />}
              onClick={addLane}
            >
              Add lane
            </Button>
          </div>
        </FormSection>

        <FormSection
          title={`${selectedLane.name} configuration`}
          description="Map the components used by this lane."
        >
          <div className="space-y-4">
            <FormField>
              <Label htmlFor="lane-name">Lane name</Label>
              <Input
                id="lane-name"
                value={selectedLane.name}
                disabled={readOnly}
                onChange={(event) =>
                  updateLane(selectedLane.id, { name: event.target.value })
                }
              />
            </FormField>
            <FormField>
              <Label htmlFor="direction">Direction</Label>
              <select
                id="direction"
                value={selectedLane.direction}
                disabled={readOnly}
                onChange={(event) =>
                  updateLane(selectedLane.id, {
                    direction: event.target.value as LaneDirection,
                  })
                }
                className="h-12 w-full rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] px-4 text-sm text-gray-800 shadow-theme-xs dark:text-white"
              >
                <option value="bidirectional">Bidirectional</option>
                <option value="entry">Entry only</option>
                <option value="exit">Exit only</option>
              </select>
            </FormField>
            <div className="grid gap-4 sm:grid-cols-2">
              <FormField>
                <Label htmlFor="entry-reader">Entry reader component</Label>
                <Input
                  id="entry-reader"
                  placeholder="Reader module 1 / port 1"
                  value={selectedLane.entryReader}
                  disabled={readOnly}
                  onChange={(event) =>
                    updateLane(selectedLane.id, {
                      entryReader: event.target.value,
                    })
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="exit-reader">Exit reader component</Label>
                <Input
                  id="exit-reader"
                  placeholder="Reader module 1 / port 2"
                  value={selectedLane.exitReader}
                  disabled={readOnly || selectedLane.direction === "entry"}
                  onChange={(event) =>
                    updateLane(selectedLane.id, {
                      exitReader: event.target.value,
                    })
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="unlock-relay">Unlock relay component</Label>
                <Input
                  id="unlock-relay"
                  placeholder="Relay module 2 / output 1"
                  value={selectedLane.unlockRelay}
                  disabled={readOnly}
                  onChange={(event) =>
                    updateLane(selectedLane.id, {
                      unlockRelay: event.target.value,
                    })
                  }
                />
              </FormField>
              <FormField>
                <Label htmlFor="alarm-relay">Alarm relay component</Label>
                <Input
                  id="alarm-relay"
                  placeholder="Optional"
                  value={selectedLane.alarmRelay}
                  disabled={readOnly}
                  onChange={(event) =>
                    updateLane(selectedLane.id, {
                      alarmRelay: event.target.value,
                    })
                  }
                />
              </FormField>
            </div>
            <div className="flex items-center justify-between rounded-xl bg-[var(--app-panel-muted)] p-3">
              <span className="flex items-center gap-2 text-sm text-gray-600 dark:text-gray-300">
                <BoxCubeIcon className="h-4 w-4" />
                {selectedLane.direction === "bidirectional"
                  ? "Entry and exit enabled"
                  : `${selectedLane.direction === "entry" ? "Entry" : "Exit"} only`}
              </span>
              <Button
                size="sm"
                variant="outline"
                disabled={readOnly || configuration.lanes.length === 1}
                startIcon={<TrashBinIcon className="h-4 w-4" />}
                onClick={() => removeLane(selectedLane.id)}
              >
                Remove
              </Button>
            </div>
          </div>
        </FormSection>
      </div>
      <div className="flex items-center gap-3 rounded-2xl border border-brand-100 bg-brand-50/60 p-4 text-sm text-brand-800 dark:border-brand-500/20 dark:bg-brand-500/10 dark:text-brand-200">
        <DoorIcon className="h-5 w-5 shrink-0" />
        {configuration.lanes.length} lane
        {configuration.lanes.length === 1 ? "" : "s"} configured. The selected
        lane is highlighted in the floor plan.
      </div>
    </div>
  );
};

export default TurnstileForm;
