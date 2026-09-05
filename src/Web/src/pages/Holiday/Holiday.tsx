import React, { useState } from "react";
import { CalenderIcon, TimeIcon } from "../../icons";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import HolidayForm from "./HolidayForm";
import Helper from "../../utility/Helper";
import { HolidayDto } from "../../model/Holiday/HolidayDto";
import { useToast } from "../../context/ToastContext";
import { HolidayToast } from "../../model/ToastMessage";
import { HolidayEndpoint } from "../../endpoint/HolidayEndpoint";
import { send } from "../../api/api";
import { useLocation } from "../../context/LocationContext";
import { useAuth } from "../../context/AuthContext";
import { BaseTable } from "../UiElements/BaseTable";
import { FeatureId } from "../../enum/FeatureId";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { usePagination } from "../../context/PaginationContext";
import { TableCell } from "../../components/ui/table";

// Holiday Page
export const HEADER: string[] = ["Name", "Start", "End", "Action"];
export const KEY: string[] = ["name", "start", "end"];

const Holiday = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const { setPagination } = usePagination();
  const {
    setCreate,
    setUpdate,
    setRemove,
    setConfirmCreate,
    setConfirmRemove,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const defaultDto: HolidayDto = {
    name: "",
    start: new Date(),
    end: new Date(),
    locationGuid: locationGuid,
    isActive: true,
    isDefault: false,
    guid: "00000000-0000-0000-0000-000000000000",
  };
  const [holidatDto, setHolidayDto] = useState<HolidayDto>(defaultDto);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name);
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObjects.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          const data: string[] = [];
          selectedObjects.map(async (a: HolidayDto) => {
            data.push(a.guid);
          });
          const res = await send.post(HolidayEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              HolidayToast.DELETE_RANGE,
              toggleToast,
            )
          ) {
            setSelectedObjects([]);
            toggleRefresh();
          }
        });
        setRemove(true);
        break;
      case "create":
        setConfirmCreate(() => async () => {
          const res = await send.post(HolidayEndpoint.CREATE, holidatDto);
          if (
            Helper.handleToastByResCode(res, HolidayToast.CREATE, toggleToast)
          ) {
            setHolidayDto(defaultDto);
            setForm(false);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(HolidayEndpoint.UPDATE, holidatDto);
          if (
            Helper.handleToastByResCode(res, HolidayToast.UPDATE, toggleToast)
          ) {
            setHolidayDto(defaultDto);
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
        setForm(false);
        setHolidayDto(defaultDto);
        break;
      default:
        break;
    }
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: HolidayDto) => {
    setFormType(FormType.UPDATE);
    setHolidayDto(data);
    setForm(true);
  };

  const handleRemove = (data: HolidayDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(HolidayEndpoint.DELETE(data.guid));
      if (Helper.handleToastByResCode(res, HolidayToast.DELETE, toggleToast))
        toggleRefresh();
    });
    setRemove(true);
  };

  const handleInfo = (data: HolidayDto) => {
    setFormType(FormType.INFO);
    setHolidayDto(data);
    setForm(true);
  };

  {
    /* Group Data */
  }
  const [holidaysDto, setHolidaysDto] = useState<HolidayDto[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      HolidayEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setHolidaysDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<HolidayDto[]>([]);

  const content: FormContent[] = [
    {
      label: "Holiday",
      icon: <CalenderIcon />,
      content: (
        <HolidayForm
          type={formType}
          setDto={setHolidayDto}
          handleClick={handleClick}
          dto={holidatDto}
        />
      ),
    },
  ];
  return (
    <>
      <PageBreadcrumb pageTitle="Holiday" />
      {form ? (
        <BaseForm
          tabContent={content}
          header={""}
          desc={""}
          type={formType}
          handleClick={handleClick}
        />
      ) : (
        <BaseTable<HolidayDto>
          headers={HEADER}
          keys={KEY}
          data={holidaysDto}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onClick={handleClick}
          refresh={refresh}
          permission={filterPermission(FeatureId.time)}
          fetchData={fetchData}
          locationGuid={locationGuid}
          specialDisplay={[
            {
              key: "start",
              content: (data, i) => (
                <TableCell
                  key={i}
                  className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                >
                  <span className="flex gap-2 items-center">
                    {<TimeIcon className="w-6 h-6" />}
                    {new Date(data.start).toLocaleDateString("en-US", {
                      weekday: "long",
                      year: "numeric",
                      month: "long",
                      day: "numeric",
                    })}
                  </span>
                </TableCell>
              ),
            },
            {
              key: "end",
              content: (data, i) => (
                <TableCell
                  key={i}
                  className="px-4 py-3 text-gray-500 text-start text-theme-sm dark:text-gray-400"
                >
                  <span className="flex gap-2 items-center">
                    {<TimeIcon className="w-6 h-6" />}
                    {new Date(data.end).toLocaleDateString("en-US", {
                      weekday: "long",
                      year: "numeric",
                      month: "long",
                      day: "numeric",
                    })}
                  </span>
                </TableCell>
              ),
            },
          ]}
        />
      )}
    </>
  );
};

export default Holiday;
