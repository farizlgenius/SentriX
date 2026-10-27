import { useEffect, useState } from "react";
import { useToast } from "../../context/ToastContext";
import { AreaDto } from "../../model/Area/AreaDto";
import Helper from "../../utility/Helper";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { AddIcon, AreaIcon } from "../../icons";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { BaseTable } from "../UiElements/BaseTable";
import { send } from "../../api/api";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { useLocation } from "../../context/LocationContext";
import { usePagination } from "../../context/PaginationContext";
import { FormType } from "../../model/Form/FormProp";
import { usePopup } from "../../context/PopupContext";
import { AccessAreaToast } from "../../model/ToastMessage";
import { AreaEndpoint } from "../../endpoint/AreaEndpoint";
import { AreaMode } from "../../enum/AreaMode";
import { AreaMultiOccMode } from "../../enum/AreaMultiOccMode";
import { AreaForm } from "../../components/form/Area/AreaForm";

const AREA_HEADERS = ["Name", "Action"];
const AREA_KEY = ["name"];

export const Area = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { setPagination } = usePagination();
  const {
    setRemove,
    setConfirmRemove,
    setConfirmCreate,
    setCreate,
    setUpdate,
    setConfirmUpdate,
    setInfo,
    setMessage,
  } = usePopup();
  const { filterPermission } = useAuth();
  const [refresh, setRefresh] = useState(false);
  const toggleRefresh = () => setRefresh(!refresh);
  var defaultDto: AreaDto = {
    guid: "",
    name: "",
    multiMode: AreaMultiOccMode.TWO_MORE_NOT_REQUIRE,
    mode: AreaMode.ENABLED,
    occMax: 0,
    occDown: 0,
    occUp: 0,
    doors: [],
    deviceGuid: "",
    isActive: true,
    isDefault: false
  };
  const [areaDto, setAreaDto] = useState<AreaDto>(defaultDto);
  {
    /* Modal */
  }
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);

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
          var data: string[] = [];
          selectedObjects.map(async (a: AreaDto) => {
            data.push(a.guid);
          });
          var res = await send.post(AreaEndpoint.DELETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              AccessAreaToast.DELETE_RANGE,
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
        setConfirmCreate(() => async () => {
          const res = await send.post(AreaEndpoint.CREATE, areaDto);
          if (
            Helper.handleToastByResCode(
              res,
              AccessAreaToast.CREATE,
              toggleToast,
            )
          ) {
            setForm(false);
            setAreaDto(defaultDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(AreaEndpoint.UPDATE, areaDto);
          if (
            Helper.handleToastByResCode(
              res,
              AccessAreaToast.UPDATE,
              toggleToast,
            )
          ) {
            setForm(false);
            setAreaDto(defaultDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setAreaDto(defaultDto);
        setForm(false);
        break;
      default:
        break;
    }
  };

  const handleRemove = (data: AreaDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(AreaEndpoint.DELETE(data.guid));
      if (
        Helper.handleToastByResCode(res, AccessAreaToast.DELETE, toggleToast)
      ) {
        setRemove(false);
        toggleRefresh();
      }
    });
    setRemove(true);
  };

  {
    /* handle Table Action */
  }
  const handleEdit = (data: AreaDto) => {
    setAreaDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: AreaDto) => {
    setAreaDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  {
    /* Group Data */
  }
  const [areasDto, setAreasDto] = useState<AreaDto[]>([]);
  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      AreaEndpoint.PAGINATION(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    console.log(res?.data.data);
    if (res && res.data.data) {
      console.log(res.data.data);
      setAreasDto(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  {
    /* checkBox */
  }
  const [selectedObjects, setSelectedObjects] = useState<AreaDto[]>([]);

  {
    /* Form */
  }
  const createContent: FormContent[] = [
    {
      icon: <AreaIcon />,
      label: "Area",
      content: (
        <AreaForm
          dto={areaDto}
          setDto={setAreaDto}
          handleClick={handleClick}
          type={formType}
          handleChange={() => { }}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Access Area" />
      {form ? (
        <BaseForm handleClick={handleClick} tabContent={createContent} type={formType} />
      ) : (
        <div className="space-y-6">
          <BaseTable<AreaDto>
            headers={AREA_HEADERS}
            keys={AREA_KEY}
            data={areasDto}
            select={selectedObjects}
            setSelect={setSelectedObjects}
            onInfo={handleInfo}
            onEdit={handleEdit}
            onRemove={handleRemove}
            onClick={handleClick}
            permission={filterPermission(FeatureId.area)}
            fetchData={fetchData}
            locationGuid={locationGuid}
          />
        </div>
      )}
    </>
  );
};
