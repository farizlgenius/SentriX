import { useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseTable } from "../UiElements/BaseTable";
import { BaseForm } from "../UiElements/BaseForm";
import { FormContent } from "../../model/Form/FormContent";
import { send } from "../../api/api";
import { AlertEndpoint } from "../../endpoint/AlertEndpoint";
import Helper from "../../utility/Helper";
import { useToast } from "../../context/ToastContext";
import { useLocation } from "../../context/LocationContext";
import { TriggerIcon } from "../../icons";
import { useAuth } from "../../context/AuthContext";
import { FeatureId } from "../../enum/FeatureId";
import { FormType } from "../../model/Form/FormProp";
import { AlertToast, ProcedureToast } from "../../model/ToastMessage";
import { usePopup } from "../../context/PopupContext";
import { usePagination } from "../../context/PaginationContext";
import { ActionEndpoint } from "../../endpoint/ActionEndpoint";
import { AlertLevelDto } from "../../model/Alert/AlertLevelDto";
import AlertLevelForm from "./AlertLevelForm";
import { TableCell } from "../../components/ui/table";

const HEADERS: string[] = ["Name", "Enable", "Action"];
const KEYS: string[] = ["name"];

export const AlertLevel = () => {
  const { toggleToast } = useToast();
  const { locationGuid } = useLocation();
  const { filterPermission } = useAuth();
  const [form, setForm] = useState<boolean>(false);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  const [selectedObject, setSelectedObjects] = useState<AlertLevelDto[]>([]);
  const [levelDtos, setLevelDtos] = useState<AlertLevelDto[]>([]);
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
  const { setPagination } = usePagination();
  const [refresh, setRefresh] = useState<boolean>(false);
  const defaultDto: AlertLevelDto = {
    guid: "",
    name: "",
    colorCode: "",
    isActive: true,
    isDefault: false
  };
  const [dto, setDto] = useState<AlertLevelDto>(defaultDto);

  const toggleRefresh = () => setRefresh((prev) => !prev);

  const handleRemove = (data: AlertLevelDto) => {
    setConfirmRemove(() => async () => {
      const res = await send.delete(AlertEndpoint.DELETE_LEVEL(data.guid));
      if (
        Helper.handleToastByResCode(res, ProcedureToast.DELETE, toggleToast)
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
  const handleEdit = (data: AlertLevelDto) => {
    setDto(data);
    setFormType(FormType.UPDATE);
    setForm(true);
  };

  const handleInfo = (data: AlertLevelDto) => {
    setDto(data);
    setFormType(FormType.INFO);
    setForm(true);
  };

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "add":
        setFormType(FormType.CREATE);
        setForm(true);
        break;
      case "delete":
        if (selectedObject.length == 0) {
          setMessage("Please select object");
          setInfo(true);
        }
        setConfirmRemove(() => async () => {
          var data: string[] = [];
          selectedObject.map((a: AlertLevelDto) => {
            data.push(a.guid);
          });
          var res = await send.post(AlertEndpoint.DLETE_RANGE, data);
          if (
            Helper.handleToastByResCode(
              res,
              AlertToast.DELETE_RANGE,
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
          const res = await send.post(AlertEndpoint.CREATE_LEVEL, dto);
          if (
            Helper.handleToastByResCode(res, AlertToast.CREATE_LEVEL, toggleToast)
          ) {
            setForm(false);
            setDto(defaultDto);
            toggleRefresh();
          }
        });
        setCreate(true);
        break;
      case "update":
        setConfirmUpdate(() => async () => {
          const res = await send.put(AlertEndpoint.UPDATE_LEVEL, dto);
          if (
            Helper.handleToastByResCode(res, AlertToast.UPDATE_LEVEL, toggleToast)
          ) {
            setForm(false);
            setDto(defaultDto);
            toggleRefresh();
          }
        });
        setUpdate(true);
        break;
      case "close":
      case "cancel":
        setDto(defaultDto);
        setForm(false);
        break;
      default:
        break;
    }
  };

  const fetchData = async (
    pageNumber: number,
    pageSize: number,
    locationGuid?: string,
    search?: string,
    startDate?: string,
    endDate?: string,
  ) => {
    const res = await send.get(
      AlertEndpoint.PAGINATION_LEVEL(
        pageNumber,
        pageSize,
        locationGuid,
        search,
        startDate,
        endDate,
      ),
    );
    if (res && res.data.data) {
      setLevelDtos(res.data.data.items);
      setPagination(res.data.data);
    }
  };

  const tabContent: FormContent[] = [
    {
      label: "Alerts",
      icon: <TriggerIcon />,
      content: (
        <AlertLevelForm
          handleClick={handleClick}
          dto={dto}
          setDto={setDto}
          type={formType}
          handleChange={() => { }}
        />
      ),
    },
  ];

  return (
    <>
      <PageBreadcrumb pageTitle="Alert Level" />

      {form ? (
        <BaseForm handleClick={handleClick} tabContent={tabContent} type={formType} desc="" />
      ) : (
        <BaseTable<AlertLevelDto>
          refresh={refresh}
          keys={KEYS}
          headers={HEADERS}
          data={levelDtos}
          onInfo={handleInfo}
          onEdit={handleEdit}
          onRemove={handleRemove}
          onClick={handleClick}
          select={selectedObject}
          setSelect={setSelectedObjects}
          permission={filterPermission(FeatureId.trigger)}
          fetchData={fetchData}
          locationGuid={locationGuid}
          specialDisplay={[
            {
              key: "name",
              content: (item, index) => (
                <TableCell key={index} className="px-4 py-3 text-start">
                  <div className="flex items-center gap-2.5">
                    {/* Color Swatch */}
                    <span
                      className="h-5 w-5 shrink-0 rounded-md border border-black/10 shadow-inner dark:border-white/10"
                      style={{ backgroundColor: item.colorCode }}
                      aria-hidden="true"
                    />

                    {/* Label & Hex Code */}
                    <div className="flex items-center gap-2">
                      <span className="text-theme-sm font-medium text-gray-800 dark:text-gray-200">
                        {item.name}
                      </span>
                      <span className="font-mono text-xs text-gray-400 uppercase">
                        {item.colorCode}
                      </span>
                    </div>
                  </div>
                </TableCell>
                // <TableCell key={index} className="px-4 py-3 text-start">
                //   <div
                //     className="inline-flex items-center gap-1.5 rounded-full px-2.5 py-1 text-theme-xs font-medium border border-black/5 dark:border-white/10"
                //     style={{
                //       backgroundColor: `${item.colorCode}15`, // 15 = ~8% opacity
                //       color: item.colorCode,
                //     }}
                //   >
                //     <span
                //       className="h-2 w-2 rounded-full"
                //       style={{ backgroundColor: item.colorCode }}
                //     />
                //     <span>{item.name}</span>
                //   </div>
                // </TableCell>
              ),
            }
          ]}
        />
      )}
    </>
  );
};
