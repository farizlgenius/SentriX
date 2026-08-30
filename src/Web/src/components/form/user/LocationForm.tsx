import React, { PropsWithChildren, useState } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import Label from "../Label";
import Button from "../../ui/button/Button";
import Select from "../Select";
import { Options } from "../../../model/Options";
import Helper from "../../../utility/Helper";
import { LocationIcon } from "../../../icons";
import { UserDto } from "../../../model/User/UserDto";
import { useLocation } from "../../../context/LocationContext";

export const LocationForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const { locationList } = useLocation();
  const [locationsGuid, setLocationGuid] = useState<string>("");
  const [selectedLocationGuids, setSelectedLocationGuids] = useState<string[]>(
    [],
  );
  const [locations, setLocations] = useState<Options[]>(
    locationList.map((l) => ({
      label: l.name,
      value: l.guid,
      isTaken: false,
    })),
  );

  const isReadOnly = type === FormType.INFO;

  const toggleLocationSelection = (data: string) => {
    setSelectedLocationGuids((prev) =>
      prev.includes(data) ? prev.filter((x) => x !== data) : [...prev, data],
    );
  };

  const addLocation = () => {
    console.log(locationsGuid);
    if (locationsGuid === "" || dto.locations.includes(locationsGuid)) return;

    setDto((prev) => ({
      ...prev,
      locations: [...prev.locations, locationsGuid],
    }));

    setLocations((prev) =>
      Helper.updateOptionByValue(prev, locationsGuid, true),
    );

    setLocationGuid("");
    console.log(dto.locations);
  };

  const removeSelectedLocations = () => {
    if (selectedLocationGuids.length === 0) return;

    const idsToRemove = [...selectedLocationGuids];
    setDto((prev) => ({
      ...prev,
      locations: prev.locations.filter((id) => !idsToRemove.includes(id)),
    }));
    setLocations((prev) =>
      prev.map((option) =>
        idsToRemove.includes(option.value.toString())
          ? { ...option, isTaken: false }
          : option,
      ),
    );
    setSelectedLocationGuids([]);
  };

  return (
    <>
      <div className="rounded-[28px] border border-[var(--app-panel-border)] bg-[var(--app-panel-bg)] p-6 shadow-theme-xs lg:p-8">
        <div className="mb-6">
          <p className="text-xs font-semibold uppercase tracking-[0.24em] text-brand-500">
            Location Access
          </p>
          <h3 className="mt-2 text-xl font-semibold text-gray-900 dark:text-white">
            Manage assigned locations
          </h3>
          <p className="mt-2 text-sm text-gray-500 dark:text-gray-400">
            Add locations one by one, then tap cards below to mark which ones
            should be removed.
          </p>
        </div>

        <div className="flex flex-col gap-3 lg:flex-row">
          <div className="flex-1">
            <Label htmlFor="location">Location</Label>
            <Select
              disabled={isReadOnly}
              isString={true}
              options={locations.filter((x) => x.isTaken == false)}
              defaultValue={locationsGuid}
              onChange={(value) => setLocationGuid(value)}
              name="location"
              placeholder="Select location"
            />
          </div>
          <div className="flex gap-3 lg:items-end">
            <Button
              disabled={isReadOnly || locationsGuid === ""}
              onClick={addLocation}
              className="min-w-[120px] justify-center"
            >
              Add
            </Button>
            <Button
              disabled={isReadOnly || selectedLocationGuids.length === 0}
              variant="danger"
              onClick={removeSelectedLocations}
              className="min-w-[120px] justify-center"
            >
              Remove
            </Button>
          </div>
        </div>

        <div className="mt-5 grid gap-3 sm:grid-cols-2 xl:grid-cols-3">
          {dto.locations.length > 0 ? (
            dto.locations.map((id, i) => (
              <button
                key={i}
                type="button"
                onClick={() => toggleLocationSelection(id)}
                className={`flex items-center gap-4 rounded-[22px] border px-4 py-4 text-left transition ${
                  selectedLocationGuids.includes(id)
                    ? "border-brand-500 bg-brand-50 dark:bg-brand-500/10"
                    : "border-[var(--app-panel-border)] bg-[var(--app-panel-muted)]/30 hover:border-brand-300"
                }`}
              >
                <div className="flex h-12 w-12 items-center justify-center rounded-2xl bg-[var(--app-panel-bg)] text-brand-500 shadow-sm">
                  <LocationIcon />
                </div>
                <div>
                  <p className="text-sm font-semibold text-gray-800 dark:text-white/90">
                    {locations.find((location) => location.value === id)
                      ?.label ?? `Location ${id}`}
                  </p>
                  <p className="text-xs text-gray-500 dark:text-gray-400">
                    {selectedLocationGuids.includes(id)
                      ? "Selected for removal"
                      : "Assigned location"}
                  </p>
                </div>
              </button>
            ))
          ) : (
            <div className="col-span-full rounded-[22px] border border-dashed border-[var(--app-panel-border)] px-5 py-10 text-center text-sm text-gray-500 dark:text-gray-400">
              No locations assigned yet.
            </div>
          )}
        </div>
      </div>
    </>
  );
};
