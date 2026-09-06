import React, { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import { IntervalDto } from "../../model/Interval/IntervalDto";
import { TimeZoneDto } from "../../model/TimeZone/TimeZoneDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { AddIcon, TimeIcon, TrashBinIcon } from "../../icons";
import {
  FormActions,
  FormField,
  FormSection,
} from "../../components/form/template/FormTemplate";
import { DaysInWeekDto } from "../../model/Interval/DaysInWeekDto";
import Modals from "../UiElements/Modals";
import Button from "../../components/ui/button/Button";
import { send } from "../../api/api";
import { IntervalEndpoint } from "../../endpoint/IntervalEndpoint";
import { useLocation } from "../../context/LocationContext";

const maxIntervals = 12;

const TimeZoneForm: React.FC<PropsWithChildren<FormProp<TimeZoneDto>>> = ({
  type,
  setDto,
  dto,
}) => {
  const readOnly = type === FormType.INFO;
  const { locationGuid } = useLocation();
  const [intervalForm, setIntervalForm] = useState<boolean>(false);
  const [selectedIntervalGuid, setSelectedIntervalGuid] = useState<string | null>(
    null,
  );
  const [intervalGuidToAdd, setIntervalGuidToAdd] = useState<string | null>(
    null,
  );
  const [availableIntervals, setAvailableIntervals] = useState<IntervalDto[]>(
    [],
  );
  const [isLoadingIntervals, setIsLoadingIntervals] = useState(false);

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

  const addInterval = () => {
    if (
      intervalGuidToAdd === null ||
      dto.intervalGuids.length >= maxIntervals ||
      dto.intervalGuids.includes(intervalGuidToAdd)
    )
      return;

    setDto((previous) => ({
      ...previous,
      intervalGuids: [...previous.intervalGuids, intervalGuidToAdd],
    }));
    setIntervalGuidToAdd(null);
    setIntervalForm(false);
  };

  const removeSelected = () => {
    if (selectedIntervalGuid === null) return;
    setDto((previous) => ({
      ...previous,
      intervalGuids: previous.intervalGuids.filter(
        (guid) => guid !== selectedIntervalGuid,
      ),
    }));
    setSelectedIntervalGuid(null);
  };

  useEffect(() => {
    const fetchIntervals = async () => {
      setIsLoadingIntervals(true);
      const res = await send.get(
        IntervalEndpoint.PAGINATION(1, 1000, locationGuid || dto.locationGuid),
      );

      setAvailableIntervals(res?.data?.success ? res.data.data.items ?? [] : []);
      setIsLoadingIntervals(false);
    };

    fetchIntervals();
  }, [locationGuid, dto.locationGuid]);

  const selectedIntervals = availableIntervals.filter((interval) =>
    dto.intervalGuids.includes(interval.guid),
  );

  return (
    <>
      {intervalForm && (
        <Modals
          header="Select Time Interval"
          handleClickWithEvent={(event) =>
            event.currentTarget.name === "close" && setIntervalForm(false)
          }
          body={
            <>
              <p className="mb-5 text-sm text-gray-500 dark:text-gray-400">
                Choose a saved interval for this timezone.
              </p>
              {isLoadingIntervals ? (
                <p className="py-8 text-center text-sm text-gray-500 dark:text-gray-400">
                  Loading intervals...
                </p>
              ) : availableIntervals.length === 0 ? (
                <p className="rounded-xl border border-dashed border-gray-300 p-6 text-center text-sm text-gray-500 dark:border-gray-700 dark:text-gray-400">
                  No saved intervals are available for this location.
                </p>
              ) : (
                <div className="grid max-h-80 gap-3 overflow-y-auto pr-1 sm:grid-cols-2">
                  {availableIntervals.map((item) => {
                    const alreadyAdded = dto.intervalGuids.includes(item.guid);
                    const selected = intervalGuidToAdd === item.guid;
                    return (
                      <button
                        type="button"
                        key={item.guid}
                        disabled={alreadyAdded}
                        onClick={() => setIntervalGuidToAdd(item.guid)}
                        className={`rounded-xl border p-4 text-left transition ${selected ? "border-brand-500 bg-brand-50 ring-2 ring-brand-500/15 dark:bg-brand-500/10" : "border-[var(--app-panel-border)] hover:border-brand-200 dark:hover:border-brand-500/50"} ${alreadyAdded ? "cursor-not-allowed opacity-50" : ""}`}
                      >
                        <p className="font-semibold text-gray-900 dark:text-white">
                          {dayDescBuilder(item.days)}
                        </p>
                        <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
                          {item.start} - {item.end}
                        </p>
                        {alreadyAdded && (
                          <span className="mt-3 inline-block text-xs font-medium text-brand-600 dark:text-brand-300">
                            Already added
                          </span>
                        )}
                      </button>
                    );
                  })}
                </div>
              )}
              <FormActions
                disabled={intervalGuidToAdd === null}
                typeLabel="Create"
                submitName="add"
                cancelName="close"
                submitLabel="Add selected interval"
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
                {dto.intervalGuids.length} of {maxIntervals} intervals added
              </span>
              <Button
                disabled={readOnly || dto.intervalGuids.length >= maxIntervals}
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
                  Interval collection
                </h4>
                <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                  Select an interval to manage it.
                </p>
              </div>
              <Button
                disabled={readOnly || selectedIntervalGuid === null}
                size="sm"
                variant="outline"
                startIcon={<TrashBinIcon className="h-4 w-4" />}
                onClick={removeSelected}
              >
                Remove selected
              </Button>
            </div>
            {isLoadingIntervals ? (
              <p className="py-8 text-center text-sm text-gray-500 dark:text-gray-400">
                Loading selected intervals...
              </p>
            ) : selectedIntervals.length === 0 ? (
              <div className="flex min-h-40 flex-col items-center justify-center rounded-xl border border-dashed border-gray-300 px-4 text-center dark:border-gray-700">
                <div className="mb-3 flex h-11 w-11 items-center justify-center rounded-full bg-gray-100 text-gray-500 dark:bg-gray-800">
                  <TimeIcon className="h-5 w-5" />
                </div>
                <p className="font-medium text-gray-800 dark:text-white">
                  No intervals yet
                </p>
                <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                  Select an interval from the saved intervals list.
                </p>
              </div>
            ) : (
              <div className="grid gap-3 md:grid-cols-2 xl:grid-cols-3">
                {selectedIntervals.map((item) => {
                  const selected = selectedIntervalGuid === item.guid;
                  return (
                    <button
                      type="button"
                      key={item.guid}
                      onClick={() => setSelectedIntervalGuid(item.guid)}
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
