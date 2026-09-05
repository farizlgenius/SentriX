import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { TimezoneEndPoint } from "../../endpoint/TimeZoneEndpoint";
import { DoorEndpoint } from "../../endpoint/DoorEndpoint";
import { GroupDto } from "../../model/Group/GroupDto";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorIcon, TimeIcon } from "../../icons";
import React from "react";
import { GroupDoorDto } from "../../model/Group/GroupDoorDto";

const GroupForm: React.FC<PropsWithChildren<FormProp<GroupDto>>> = ({
  dto,
  setDto,
  handleClick,
  type,
}) => {
  const defaulComponent: GroupDoorDto = {
    mac: "",
    doorComponentId: -1,
    timezoneComponentId: -1,
    type: "",
    doorId: -1,
    timezoneId: -1,
  };

  const [selectedId, setSelectedId] = useState<number | null>(null);

  const handleDelete = (id: number) => {
    setDto((prev) => ({
      ...prev,
      doors: prev.doors.filter((x) => x.doorId != id),
    }));
  };
  const { locationGuid: locationId } = useLocation();
  const [doorOption, setDoorOption] = useState<Options[]>([]);
  const [timeZoneOption, setTimeZoneOption] = useState<Options[]>([]);
  const [selectComponent, setSelectComponent] =
    useState<GroupDoorDto>(defaulComponent);

  const handleSelect = (
    value: string,
    e: React.ChangeEvent<HTMLSelectElement>,
  ) => {
    console.log(e.target.name);
    switch (e.target.name) {
      case "door":
        // setDoorTimezone(prev => ({ ...prev, doorId: Number(value), doorName: doorOption.find(a => a.value === Number(value))?.label ?? "", doorMacAddress: doorOption.find(a => a.value === Number(value))?.description ?? "" }))
        setSelectComponent((prev) => ({
          ...prev,
          type:
            doorOption
              .find((a) => a.value === Number(value))
              ?.description?.split(",")[1] ?? "",
          mac:
            doorOption
              .find((a) => a.value === Number(value))
              ?.description?.split(",")[0] ?? "",
          doorId: Number(value),
          doorComponentId: doorOption.find((a) => a.value === Number(value))
            ?.additionalInfo,
        }));
        break;
      case "timezone":
        // setDoorTimezone(prev => ({ ...prev, timeZoneId: Number(value), timeZoneName: timeZoneOption.find(a => a.value === Number(value))?.label ?? "" }))
        setSelectComponent((prev) => ({
          ...prev,
          timezoneId: Number(value),
          timezoneComponentId: timeZoneOption.find(
            (x) => x.value == Number(value),
          )?.additionalInfo,
        }));
        break;
      default:
        break;
    }
  };

  // Fetch Data
  const fetchDoor = async () => {
    let res = await send.get(DoorEndpoint.GET_OPTION(locationId));
    if (res.data) {
      res.data.map((a: Options) => {
        setDoorOption((prev) => [
          ...prev,
          {
            value: a.value,
            label: a.label,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchTimeZone = async () => {
    let res = await send.get(
      TimezoneEndPoint.GET_OPTION_BY_LOCATION(locationId),
    );
    if (res.data) {
      res.data.map((a: Options) => {
        setTimeZoneOption((prev) => [
          ...prev,
          {
            value: a.value,
            label: a.label,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const Info = ({ label, value }: { label: string; value: any }) => (
    <div className="flex flex-col">
      <span className="text-xs text-gray-500 dark:text-gray-400">{label}</span>
      <span className="font-medium text-gray-800 dark:text-white/90">
        {value}
      </span>
    </div>
  );

  useEffect(() => {
    fetchDoor();
    fetchTimeZone();
  }, []);

  return (
    <div className="flex flex-col gap-5 justify-center items-center p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
      {/*sm:flex-row sm:gap-8 */}
      <div className="flex flex-col justify-center items-center gap-6 ">
        <>
          <div className="flex flex-col gap-6 w-full">
            <div className="flex flex-col gap-1">
              <Label htmlFor="name">Name</Label>
              <Input
                placeholder="Group Name"
                disabled={type == FormType.INFO}
                name="name"
                type="text"
                id="name"
                value={dto.name}
                onChange={(e) =>
                  setDto((prev) => ({ ...prev, name: e.target.value }))
                }
              />
            </div>
            {/* List Transfer */}
            <div className="flex gap-2 items-end">
              <div className="flex-2">
                <Label>Door</Label>
                <Select
                  disabled={type == FormType.INFO}
                  name="door"
                  options={doorOption.filter((x) => x.isTaken == false)}
                  placeholder="Select Option"
                  onChangeWithEvent={handleSelect}
                  className="dark:bg-dark-900"
                  defaultValue={selectComponent.doorId}
                />
              </div>
              <div className="flex-2">
                <Label>Time Zone</Label>
                <Select
                  disabled={type == FormType.INFO}
                  isString={false}
                  name="timezone"
                  options={timeZoneOption.filter((x) => x.isTaken == false)}
                  placeholder="Select Option"
                  onChangeWithEvent={handleSelect}
                  className="dark:bg-dark-900"
                  defaultValue={selectComponent.timezoneId}
                />
              </div>
              <div>
                <Button
                  onClickWithEvent={() => {
                    if (
                      selectComponent.mac == "" ||
                      selectComponent.timezoneId == -1
                    )
                      return;

                    setDto((prev) => ({
                      ...prev,
                      doors:
                        prev.doors.length != 0
                          ? [...prev.doors, selectComponent]
                          : [
                              {
                                mac: selectComponent.mac,
                                doorId: selectComponent.doorId,
                                doorComponentId:
                                  selectComponent.doorComponentId,
                                timezoneComponentId:
                                  selectComponent.timezoneComponentId,
                                timezoneId: selectComponent.timezoneId,
                                type: selectComponent.type,
                              },
                            ],
                    }));

                    setSelectComponent(defaulComponent);
                  }}
                  name="addDoor"
                  size="sm"
                >
                  Add
                </Button>
              </div>
            </div>

            <div className="flex justify-stretch w-full">
              <div className="items-center w-full">
                <div>
                  <Label>Doors / Timezone</Label>

                  <div className="flex flex-col gap-2 overflow-auto scrollbar-thin scrollbar-transparent h-64 w-full rounded-lg border px-4 py-3 text-sm shadow-theme-xs bg-transparent">
                    {dto.doors.map((item, i) => {
                      const isSelected = selectedId === item.doorId;
                      return (
                        <div
                          key={i}
                          onClick={() => setSelectedId(item.doorId)}
                          onDoubleClick={() => handleDelete(item.doorId)}
                          className={`flex gap-4 rounded-lg border p-4 cursor-pointer transition select-none ${
                            isSelected
                              ? "border-blue-500 bg-blue-50 dark:bg-blue-500/10"
                              : "border-gray-200 dark:border-gray-800 bg-white dark:bg-gray-900"
                          } hover:shadow-md`}
                        >
                          {/* Icon */}
                          <div className="pt-1">
                            <DoorIcon className="w-6 h-6 text-gray-500 dark:text-gray-400" />
                          </div>
                          <div className="flex-1 grid grid-cols-2 gap-y-1 gap-x-4">
                            <Info
                              label="Door"
                              value={
                                doorOption.find((x) => x.value == item.doorId)
                                  ?.label || "Unknown"
                              }
                            />
                          </div>
                          <div className="pt-1">
                            <TimeIcon className="w-6 h-6 text-gray-500 dark:text-gray-400" />
                          </div>
                          <div className="flex-1 grid grid-cols-2 gap-y-1 gap-x-4">
                            <Info
                              label="Timezone"
                              value={
                                timeZoneOption.find(
                                  (x) => x.value == item.timezoneId,
                                )?.label || "Unknown"
                              }
                            />
                          </div>
                        </div>
                      );
                    })}
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="flex gap-5">
            <Button
              name="create"
              onClickWithEvent={handleClick}
              className="w-50"
            >
              Create
            </Button>
            <Button
              name="cancle"
              onClickWithEvent={handleClick}
              className="w-50"
              variant="danger"
            >
              Cancel
            </Button>
          </div>
        </>
      </div>
    </div>
  );
};

export default GroupForm;
