import React, { PropsWithChildren, useEffect, useState } from "react";
import { AreaDto } from "../../../model/Area/AreaDto";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import Label from "../Label";
import Input from "../input/InputField";
import Select from "../Select";
import { Options } from "../../../model/Options";
import { FormField } from "../template/FormTemplate";
import { AreaMultiOccMode } from "../../../enum/AreaMultiOccMode";
import { AreaMode } from "../../../enum/AreaMode";
import { DoorAreaDto } from "../../../model/Area/DoorAreaDto";
import { DoorIcon, DoorInIcon, DoorOutIcon, TrashBinIcon } from "../../../icons";
import { useLocation } from "../../../context/LocationContext";
import { send } from "../../../api/api";
import { DoorEndpoint } from "../../../endpoint/DoorEndpoint";
import { HttpCode } from "../../../enum/Httpcode";
import { AntipassBackMode } from "../../../enum/AntipassbackMode";
import { DoorType } from "../../../enum/DoorType";
import { DoorDto } from "../../../model/Door/DoorDto";
import { AreaEndpoint } from "../../../endpoint/AreaEndpoint";
import Switch from "../switch/Switch";
import { AeroAreaMetadata } from "../../../model/Area/AeroAreaMetadata";


export const AreaForm: React.FC<PropsWithChildren<FormProp<AreaDto>>> = ({ setDto, dto, type }) => {
  const { locationGuid } = useLocation();
  const isReadOnly = type == FormType.INFO;
  const [doors, setDoors] = useState<DoorDto[]>([]);
  const [areaOptions, setAreaOptions] = useState<Options[]>([]);
  const [selectedDoor, setSelectedDoor] = useState("");
  const [loadError, setLoadError] = useState("");

  const antipassbackOptions: Options[] = [
    { label: "None", value: AntipassBackMode.NONE },
    { label: "Soft", value: AntipassBackMode.SOFT },
    { label: "Hard", value: AntipassBackMode.HARD },
    { label: "Reader based (last used, seconds)", value: AntipassBackMode.READER_BASED_LAST_USED_SECOND },
    { label: "Reader based (seconds)", value: AntipassBackMode.READER_BASED_SECOND },
    { label: "Time based (seconds)", value: AntipassBackMode.TIMED_BASED_SECOND },
    { label: "Reader based (last used, minutes)", value: AntipassBackMode.READER_BASED_LAST_USED_MINUTE },
    { label: "Reader based (minutes)", value: AntipassBackMode.READER_BASED_MINUTE },
    { label: "Time based (minutes)", value: AntipassBackMode.TIMED_BASED_MINUTE },
  ];

  useEffect(() => {
    const fetchFormOptions = async () => {
      const [doorResponse, areaResponse] = await Promise.all([
        send.get(DoorEndpoint.GET_BY_LOCATION(locationGuid)),
        send.get(AreaEndpoint.PAGINATION(1, 100, locationGuid)),
      ]);

      if (doorResponse.status !== HttpCode.OK) {
        throw new Error("Unable to load doors for this location.");
      }
      if (areaResponse.status !== HttpCode.OK) {
        throw new Error("Unable to load areas for this location.");
      }

      setDoors(doorResponse.data.data);
      setAreaOptions(areaResponse.data.data.items.map((area: AreaDto) => ({
        label: area.name,
        value: area.guid,
      })));
    };

    setLoadError("");
    fetchFormOptions().catch((error: unknown) => {
      setLoadError(error instanceof Error ? error.message : "Unable to load door settings.");
    });
  }, [locationGuid]);

  const availableDoorOptions: Options[] = doors
    .filter(door => !dto.doors.some(selected => selected.doorGuid === door.guid))
    .map(door => ({ label: door.name, value: door.guid }));

  const updateDoor = (index: number, values: Partial<DoorAreaDto>) => {
    setDto(prev => ({
      ...prev,
      doors: prev.doors.map((door, doorIndex) =>
        doorIndex === index ? { ...door, ...values } : door
      ),
    }));
  };

  const areaOptionsFor = (guid: string) => (
    guid && !areaOptions.some(option => option.value === guid)
      ? [...areaOptions, { label: guid, value: guid }]
      : areaOptions
  );


  return (
    <div className="grid gap-5 grid-cols-3">
      <FormField>
        <Label>Name</Label>
        <Input disabled={isReadOnly} type="text" placeholder="Area name" value={dto.name} onChange={e => setDto(prev => ({
          ...prev,
          name: e.target.value
        }))} />
      </FormField>
      <FormField>
        <Label>Multi Occupancy</Label>
        <Select disabled={isReadOnly} isString={false} options={[
          {
            label: "Not require 2 or more occupancy.",
            value: AreaMultiOccMode.TWO_MORE_NOT_REQUIRE
          }, {
            label: "Require 2 or more occupancy.",
            value: AreaMultiOccMode.TWO_MORE_REQUIRE
          }
        ]} defaultValue={dto.multiMode} onChange={e => setDto(prev => ({
          ...prev,
          multiMode: Number(e.target.value)
        }))} name={"multiMode"} />
      </FormField>
      <FormField>
        <Label>Area Mode</Label>
        <Select
          disabled={isReadOnly}
          isString={false}
          options={[
            {
              label: "No Operation",
              value: AreaMode.NOP
            }, {
              label: "Disabled",
              value: AreaMode.DISABLED
            },
            {
              label: "Enabled",
              value: AreaMode.ENABLED
            }
          ]}
          defaultValue={dto.mode}
          onChange={e => setDto(prev => ({
            ...prev,
            mode: Number(e.target.value)
          }))} name={"multiMode"} />
      </FormField>
      <FormField>
        <Label>Max Occupancy</Label>
        <Input disabled={isReadOnly} type="number" min="0" placeholder="Max" value={dto.occMax} onChange={e => setDto(prev => ({
          ...prev,
          occMax: Number(e.target.value)
        }))} />
      </FormField>
      <FormField>
        <Label>Upper Limit Occupany</Label>
        <Input disabled={isReadOnly} type="number" min="0" placeholder="Up" value={dto.occUp} onChange={e => setDto(prev => ({
          ...prev,
          occUp: Number(e.target.value)
        }))} />
      </FormField>
      <FormField>
        <Label>Down Limit Occupany</Label>
        <Input disabled={isReadOnly} type="number" min="0" placeholder="Down" value={dto.occDown} onChange={e => setDto(prev => ({
          ...prev,
          occDown: Number(e.target.value)
        }))} />
      </FormField>
      <FormField>
        <Switch defaultChecked={(dto.metadata as AeroAreaMetadata).airLock} disabled={isReadOnly} label={"Interlock"} onChange={e => setDto(prev => ({
          ...prev,
          metadata:{
            ...(prev.metadata as AeroAreaMetadata),
            airLock:e
          }
        }))}/>
      </FormField>
      <FormField>
        <Switch disabled={isReadOnly} defaultChecked={(dto.metadata as AeroAreaMetadata).onlyOneDoor} label={"Only One Door"} onChange={e => setDto(prev => ({
          ...prev,
          metadata:{
            airLock:e,
            onlyOneDoor:e
          }
        }))}/>
      </FormField>
      <FormField className="col-span-3">
        <Label>
          Doors
        </Label>
        <Select
          disabled={isReadOnly}
          icon={<DoorIcon />}
          isString={true}
          options={availableDoorOptions}
          defaultValue={selectedDoor}
          onChange={e => {
            const selectedValue = e.target.value;
            const selected = doors.find(door => door.guid === selectedValue);
            if (!selected) return;

            setDto(prev => prev.doors.some(door => door.doorGuid === selectedValue)
              ? prev
              : {
                ...prev,
                doors: [...prev.doors, {
                  doorGuid: selected.guid,
                  type: selected.type,
                  antipassbackFrom: AntipassBackMode.NONE,
                  areaFromGuid: "",
                  antipassbackTo: AntipassBackMode.NONE,
                  areaToGuid: null,
                }],
              });
            setSelectedDoor("");
          }}
          name={"door"} />
      </FormField>
      <FormField className="col-span-3">
        {loadError && <p role="alert" className="mb-4 text-sm text-error-500">{loadError}</p>}
        <div className="grid gap-4 xl:grid-cols-2">
          {dto.doors.map((door: DoorAreaDto, index: number) => {
            if (!door.doorGuid) return null;

            const doorInfo = doors.find(item => item.guid === door.doorGuid);
            const doorType = door.type ?? doorInfo?.type ?? DoorType.Single;
            const isDual = doorType === DoorType.Dual;
            const title = doorInfo?.name ?? door.doorGuid;

            return (
              <section
                key={door.doorGuid}
                className="flex overflow-hidden rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] shadow-theme-xs transition-shadow hover:shadow-theme-md"
              >
                <aside className="flex w-24 shrink-0 flex-col items-center justify-center gap-3 border-r border-brand-100 bg-gradient-to-b from-brand-50 to-white px-2 py-6 text-brand-600 dark:border-brand-500/20 dark:from-brand-500/10 dark:to-gray-900 dark:text-brand-300 sm:w-32">
                  <span className="flex size-16 items-center justify-center rounded-2xl bg-white text-brand-500 shadow-theme-sm ring-1 ring-brand-100 dark:bg-gray-800 dark:text-brand-300 dark:ring-brand-500/20 sm:size-20">
                    <DoorIcon className="size-10 sm:size-12" />
                  </span>
                  <span className="rounded-full bg-white/80 px-2.5 py-1 text-[11px] font-semibold uppercase tracking-wide text-brand-700 dark:bg-gray-800/80 dark:text-brand-300">
                    {DoorType[doorType] ?? "Door"}
                  </span>
                </aside>
                <div className="min-w-0 flex-1">
                  <header className="flex flex-wrap items-center justify-between gap-3 border-b border-[var(--app-panel-border)] px-4 py-4 sm:px-5">
                    <div className="min-w-0">
                      <h3 className="truncate text-base font-semibold text-gray-900 dark:text-white">{title}</h3>
                      <span className="text-xs text-gray-500 dark:text-gray-400">
                        {isDual ? "Two-way access" : "Single direction access"}
                      </span>
                    </div>
                    <div className="flex shrink-0 items-center gap-3">
                      <div className="flex items-center gap-2" aria-label={`${isDual ? "In and out" : "In"} direction`}>
                        <span className="inline-flex items-center gap-1 rounded-full bg-success-50 px-2.5 py-1 text-xs font-medium text-success-700 dark:bg-success-500/10 dark:text-success-400">
                          <DoorInIcon className="size-4" /> In
                        </span>
                        {isDual && (
                          <span className="inline-flex items-center gap-1 rounded-full bg-blue-light-50 px-2.5 py-1 text-xs font-medium text-blue-light-700 dark:bg-blue-light-500/10 dark:text-blue-light-400">
                            <DoorOutIcon className="size-4" /> Out
                          </span>
                        )}
                      </div>
                      {!isReadOnly && (
                        <button
                          type="button"
                          aria-label={`Remove ${title}`}
                          onClick={() => setDto(prev => ({
                            ...prev,
                            doors: prev.doors.filter((_, doorIndex) => doorIndex !== index),
                          }))}
                          className="rounded-lg p-2 text-gray-400 transition-colors hover:bg-error-50 hover:text-error-500 dark:hover:bg-error-500/10"
                        >
                          <TrashBinIcon className="size-4" />
                        </button>
                      )}
                    </div>
                  </header>
                  <div className={`grid gap-4 p-4 sm:p-5 ${isDual ? "md:grid-cols-2" : "md:grid-cols-1"}`}>
                    <div className="space-y-3">
                      <div className="flex items-center gap-2 text-sm font-semibold text-gray-800 dark:text-white/90">
                        <DoorInIcon className="size-4 text-success-600" /> In direction
                      </div>
                      <div>
                        <Label>Anti-passback</Label>
                        <Select
                          disabled={isReadOnly}
                          isString={false}
                          options={antipassbackOptions}
                          defaultValue={door.antipassbackFrom}
                          onChange={e => updateDoor(index, { antipassbackFrom: Number(e.target.value) })}
                          name={`antipassback-from-${door.doorGuid}`}
                        />
                      </div>
                      <div>
                        <Label>Area from</Label>
                        <Select
                          disabled={isReadOnly}
                          isString
                          options={areaOptionsFor(door.areaFromGuid)}
                          defaultValue={door.areaFromGuid}
                          onChange={e => updateDoor(index, { areaFromGuid: e.target.value })}
                          name={`area-from-${door.doorGuid}`}
                        />
                      </div>
                    </div>
                    {isDual && (
                      <div className="space-y-3">
                        <div className="flex items-center gap-2 text-sm font-semibold text-gray-800 dark:text-white/90">
                          <DoorOutIcon className="size-4 text-blue-light-600" /> Out direction
                        </div>
                        <div>
                          <Label>Anti-passback</Label>
                          <Select
                            disabled={isReadOnly}
                            isString={false}
                            options={antipassbackOptions}
                            defaultValue={door.antipassbackTo}
                            onChange={e => updateDoor(index, { antipassbackTo: Number(e.target.value) })}
                            name={`antipassback-to-${door.doorGuid}`}
                          />
                        </div>
                        <div>
                          <Label>Area to</Label>
                          <Input
                            disabled={true}
                            type="text"
                            value={dto.name}
                          />
                        </div>
                      </div>
                    )}
                  </div>
                </div>
              </section>
            );
          })}
        </div>
      </FormField>
    </div>

  )
}
