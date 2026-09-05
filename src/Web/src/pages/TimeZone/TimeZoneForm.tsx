import React, { PropsWithChildren, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Helper from "../../utility/Helper";
import { IntervalDto } from "../../model/Interval/IntervalDto";
import { TimeZoneDto } from "../../model/TimeZone/TimeZoneDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { AddIcon, TimeIcon, TrashBinIcon } from "../../icons";
import { useToast } from "../../context/ToastContext";
import {
  FormActions,
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import { DaysInWeekDto } from "../../model/Interval/DaysInWeekDto";
import Modals from "../UiElements/Modals";
import Switch from "../../components/form/switch/Switch";
import Button from "../../components/ui/button/Button";

const daysInWeek = [
  "sunday",
  "monday",
  "tuesday",
  "wednesday",
  "thursday",
  "friday",
  "saturday",
];

const maxIntervals = 12;

const TimeZoneForm: React.FC<PropsWithChildren<FormProp<TimeZoneDto>>> = ({
  type,
  setDto,
  dto,
}) => {
  const readOnly = type === FormType.INFO;
  const { toggleToast } = useToast();
  const [intervalForm, setIntervalForm] = useState<boolean>(false);
  const [selectedInterval, setSelectedInterval] = useState<IntervalDto | null>(
    null,
  );

  const defaultDto: IntervalDto = {
    days: {
      sunday: false,
      monday: false,
      tuesday: false,
      wednesday: false,
      thursday: false,
      friday: false,
      saturday: false,
    },
    start: "",
    end: "",
    // guid: "00000000-0000-0000-0000-000000000000",
  };
  const [intervalDto, setIntervalDto] = useState<IntervalDto>(defaultDto);

  const dayDescBuilder = (days: DaysInWeekDto) => {
    const res: string[] = [];
    if (days.monday) res.push("Mon");
    if (days.tuesday) res.push("Tue");
    if (days.wednesday) res.push("Wed");
    if (days.thursday) res.push("Thu");
    if (days.friday) res.push("Fri");
    if (days.saturday) res.push("Sat");
    if (days.sunday) res.push("Sun");

    return res.join(" ");
  };

  const intervalCompare = (d1: IntervalDto, d2: IntervalDto) => {
    if (d1 == null || d2 == null) return false;

    if (
      d1.days.sunday == d2.days.sunday &&
      d1.days.monday == d2.days.monday &&
      d1.days.tuesday == d2.days.tuesday &&
      d1.days.wednesday == d2.days.wednesday &&
      d1.days.thursday == d2.days.thursday &&
      d1.days.friday == d2.days.friday &&
      d1.days.saturday == d2.days.saturday &&
      d1.start == d2.start &&
      d2.end == d2.end
    )
      return true;

    return false;
  };

  const addInterval = () => {
    if (
      dto.intervals.length >= maxIntervals ||
      dto.intervals.some((item) => intervalCompare(intervalDto, item))
    )
      return;

    setDto((previous) => ({
      ...previous,
      intervals: [...previous.intervals, intervalDto],
    }));
    setIntervalDto(defaultDto);
    setIntervalForm(false);
  };
  const removeSelected = () => {
    if (selectedInterval === null) return;
    setDto((previous) => ({
      ...previous,
      intervals: previous.intervals.filter(
        (item) => !intervalCompare(item, selectedInterval),
      ),
    }));
    setSelectedInterval(null);
  };

  return (
    <>
      {intervalForm && (
        <Modals
          header="Add Time Interval"
          handleClickWithEvent={(event) =>
            event.currentTarget.name === "close" && setIntervalForm(false)
          }
          body={
            <>
              <p className="mb-5 text-sm text-gray-500 dark:text-gray-400">
                Enter the interval details manually for the timezone.
              </p>
              <div className="grid gap-4 sm:grid-cols-2">
                <FormField className="grid grid-cols-3 col-span-2 gap-5">
                  {daysInWeek.map((d: string, i: number) => (
                    <div
                      key={i}
                      className="flex gap-10 justify-around items-center"
                    >
                      <div className="flex-1">
                        <Switch
                          label={Helper.toCapitalCase(d)}
                          defaultChecked={
                            intervalDto.days[
                              d as keyof DaysInWeekDto
                            ] as boolean
                          }
                          onChange={(e) => {
                            setIntervalDto((prev) => ({
                              ...prev,
                              days: {
                                ...prev.days,
                                [d]: e,
                              },
                            }));
                          }}
                        />
                      </div>
                    </div>
                  ))}
                </FormField>
                <FormField>
                  <Label>Start Time</Label>
                  <Input
                    type="time"
                    id="tm"
                    name="start"
                    onChange={(e) =>
                      setIntervalDto((prev) => ({
                        ...prev,
                        start: e.target.value,
                      }))
                    }
                    defaultValue={"00:00"}
                    value={intervalDto.start}
                    min="00:00"
                    placeholder={intervalDto.start}
                    disabled={type == FormType.INFO}
                  />
                </FormField>
                <FormField>
                  <Label>End Time</Label>
                  <Input
                    type="time"
                    id="tm"
                    name="end"
                    onChange={(e) =>
                      setIntervalDto((prev) => ({
                        ...prev,
                        end: e.target.value,
                      }))
                    }
                    defaultValue={"00:00"}
                    value={intervalDto.end}
                    min="00:00"
                    placeholder={intervalDto.end}
                    disabled={type == FormType.INFO}
                  />
                </FormField>
              </div>
              <FormActions
                disabled={
                  intervalDto.end == "" ||
                  intervalDto.start == "" ||
                  (intervalDto.days.sunday == false &&
                    intervalDto.days.monday == false &&
                    intervalDto.days.tuesday == false &&
                    intervalDto.days.wednesday == false &&
                    intervalDto.days.thursday == false &&
                    intervalDto.days.friday == false &&
                    intervalDto.days.saturday == false)
                }
                typeLabel="Create"
                submitName="add"
                cancelName="close"
                submitLabel="Add Interval "
                onSubmit={addInterval}
                onCancel={() => setIntervalForm(false)}
              />
            </>
          }
        />
      )}
      <FormSection
        title="Time Zone Details"
        description="Name the location, assign its country, and add a short description."
        className="pb-10 mb-5"
      >
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input
              placeholder="Time zone name"
              disabled={type == FormType.INFO}
              name="name"
              type="text"
              id="name"
              onChange={(e) =>
                setDto((prev: TimeZoneDto) => ({
                  ...prev,
                  name: e.target.value,
                }))
              }
              value={dto.name}
            />
          </FormField>
          <div className="flex flex-col justify-between rounded-2xl border border-brand-100 bg-brand-50/50 p-4 dark:border-brand-500/20 dark:bg-brand-500/5">
            <div className="flex items-start gap-3">
              <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-brand-500 text-white">
                <TimeIcon className="h-5 w-5" />
              </div>
              <div>
                <h4 className="font-semibold text-gray-900 dark:text-white">
                  Time intervals
                </h4>
                <p className="mt-0.5 text-xs leading-5 text-gray-500 dark:text-gray-400">
                  Time interval collection for this timezone.
                </p>
              </div>
            </div>
            <div className="mt-4 flex items-center justify-between">
              <span className="text-sm font-medium text-brand-700 dark:text-brand-300">
                {dto.intervals.length} of {maxIntervals} intervals added
              </span>
              <Button
                disabled={readOnly || dto.intervals.length >= maxIntervals}
                size="sm"
                startIcon={<AddIcon className="h-4 w-4" />}
                onClick={() => setIntervalForm(true)}
              >
                Add interval
              </Button>
            </div>
          </div>
          <div className="col-span-2 mt-5 rounded-2xl border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-4 sm:p-5">
            <div className="mb-4 flex flex-wrap items-center justify-between gap-3">
              <div>
                <h4 className="font-semibold text-gray-900 dark:text-white">
                  Card collection
                </h4>
                <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                  Select a card to manage it.
                </p>
              </div>
              <Button
                disabled={readOnly || selectedInterval === null}
                size="sm"
                variant="outline"
                startIcon={<TrashBinIcon className="h-4 w-4" />}
                onClick={removeSelected}
              >
                Remove selected
              </Button>
            </div>
            {dto.intervals.length === 0 ? (
              <div className="flex min-h-40 flex-col items-center justify-center rounded-xl border border-dashed border-gray-300 px-4 text-center dark:border-gray-700">
                <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-full bg-gray-100 text-gray-500 dark:bg-gray-800">
                  <TimeIcon className="h-5 w-5" />
                </div>
                <p className="font-medium text-gray-800 dark:text-white">
                  No intevals yet
                </p>
                <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                  Add a interval manually.
                </p>
              </div>
            ) : (
              <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-3">
                {dto.intervals.map((item, i) => {
                  const selected = intervalCompare(
                    selectedInterval as IntervalDto,
                    item,
                  );
                  return (
                    <button
                      type="button"
                      key={i}
                      onClick={() => setSelectedInterval(item)}
                      className={`rounded-xl border p-4 text-left transition ${selected ? "border-brand-500 bg-brand-50 ring-2 ring-brand-500/15 dark:bg-brand-500/10" : "border-[var(--app-panel-border)] hover:border-brand-200 dark:hover:border-brand-500/50"}`}
                    >
                      <div className="flex items-start justify-between">
                        <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-300">
                          <TimeIcon className="h-5 w-5" />
                        </div>
                        {selected && (
                          <span className="rounded-full bg-brand-500 px-2 py-0.5 text-xs font-semibold text-white">
                            Selected
                          </span>
                        )}
                      </div>
                      <p className="mt-4 text-lg font-semibold text-gray-900 dark:text-white">
                        {dayDescBuilder(item.days)}
                      </p>
                      <div className="mt-3 flex gap-2 text-xs text-gray-500 dark:text-gray-400">
                        <span className="rounded-md bg-gray-100 px-2 py-1 dark:bg-gray-800">
                          Start : {item.start}
                        </span>
                        <span className="rounded-md bg-gray-100 px-2 py-1 dark:bg-gray-800">
                          End : {item.end}
                        </span>
                      </div>
                    </button>
                  );
                })}
              </div>
            )}
          </div>
        </div>
      </FormSection>
    </>
  );
};

export default TimeZoneForm;
