import React, { useState } from "react";
import { TimezonIcon } from "../../icons";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import TimeZoneForm from "./TimeZoneForm";
import Helper from "../../utility/Helper";
import { TimeZoneDto } from "../../model/TimeZone/TimeZoneDto";
import { useToast } from "../../context/ToastContext";
import { TimeZoneToast } from "../../model/ToastMessage";
import { useLocation } from "../../context/LocationContext";
import { useAuth } from "../../context/AuthContext";
import { send } from "../../api/api";
import { BaseTable } from "../UiElements/BaseTable";
import { FeatureId } from "../../enum/FeatureId";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { usePopup } from "../../context/PopupContext";
import { FormType } from "../../model/Form/FormProp";
import { usePagination } from "../../context/PaginationContext";
import { TimezoneEndPoint } from "../../endpoint/TimezoneEndpoint";

const TIMEZONE_TABLE_HEAD: string[] = ["Name", "Action"];
const TIMEZONE_KEY: string[] = ["name"];

const TimeZone = () => {
  const { locationGuid } = useLocation();

  const defaultDto: TimeZoneDto = {
    isActive: true,
    name: "",
    intervalGuids: [],
    guid: "",
    isDefault: false,
    locationGuid,
  };

  const { filterPermission } = useAuth();
  const { setPagination } = usePagination();
  const { toggleToast } = useToast();
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const {
    setConfirmRemove,
    setConfirmCreate,
    setConfirmUpdate,
    setUpdate,
    setRemove,
    setCreate,
    setMessage,
    setInfo,
  } = usePopup();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);
  {
    /* Data */
  }

  const [timeZoneDto, setTimeZoneDto] = useState<TimeZoneDto>(defaultDto);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
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
          selectedObjects.map(async (a: TimeZoneDto) => {
            if (a.guid != null) data.push(a.guid);
          });
          const res = await send.post(TimezoneEndPoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              TimeZoneToast.DELETE_RANGE,
              toggleToast,
            )
          ) {
            setRemove(false);
            toggleRefresh();
          }
        });
        setRemove(true);
        break;
      case "create":
        timeZoneDto.locationGuid = locationGuid;
        setConfirmCreate(() => async () => {
          const res = await send.post(TimezoneEndPoint.CREATE, timeZoneDto);
          if (
            Helper.handleToastByResCode(res, TimeZoneToast.CREATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
            setTimeZoneDto(defaultDto);
          }
        });
        setCreate(true);
        break;
      case "close":
        setForm(false);
        setTimeZoneDto(defaultDto);
        break;
      case "update":
        timeZoneDto.locationGuid = locationGuid;
        setConfirmUpdate(() => async () => {
          const res = await send.put(TimezoneEndPoint.UPDATE, timeZoneDto);
          if (
            Helper.handleToastByResCode(res, TimeZoneToast.UPDATE, toggleToast)
          ) {
            setForm(false);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      default:
        break;
    }
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: TimeZoneDto) => {
    setFormType(FormType.UPDATE);
    setTimeZoneDto(data);
    setForm(true);
  };

  const handleRemove = async (data: TimeZoneDto) => {
    console.log(data);
    setConfirmRemove(() => async () => {
      const res = await send.delete(TimezoneEndPoint.DELETE(data.guid));
      console.log(res);
      if (Helper.handleToastByResCode(res, TimeZoneToast.DELETE, toggleToast)) {
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  const handleInfo = (data: TimeZoneDto) => {
    setFormType(FormType.INFO);
    setTimeZoneDto(data);
    setForm(true);
  };

  {
    /* Group Data */
  }
  const [timeZonesDto, setTimeZonesDto] = useState<TimeZoneDto[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      TimezoneEndPoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res.data.success) {
      setTimeZonesDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<TimeZoneDto[]>([]);

  const tabContent: FormContent[] = [
    {
      label: "Time Zone",
      icon: <TimezonIcon />,
      content: (
        <TimeZoneForm
          handleClick={handleClick}
          dto={timeZoneDto}
          setDto={setTimeZoneDto}
          type={formType}
        />
      ),
    },
  ];
  return (
    <>
      <PageBreadcrumb pageTitle="Time Zone" />
      {form ? (
        <BaseForm
          handleClick={handleClick}
          type={formType}
          tabContent={tabContent}
          header={""}
          desc={""}
        />
      ) : (
        <BaseTable<TimeZoneDto>
          keys={TIMEZONE_KEY}
          headers={TIMEZONE_TABLE_HEAD}
          data={timeZonesDto}
          onRemove={handleRemove}
          onEdit={handleEdit}
          onInfo={handleInfo}
          onClick={handleClick}
          select={selectedObjects}
          setSelect={setSelectedObjects}
          permission={filterPermission(FeatureId.time)}
          fetchData={fetchData}
          locationGuid={locationGuid}
          refresh={refresh}
        />
      )}
    </>
  );
};

export default TimeZone;
