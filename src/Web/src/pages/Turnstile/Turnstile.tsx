import { SetStateAction, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseForm } from "../UiElements/BaseForm";
import { BaseTable } from "../UiElements/BaseTable";
import { FormType } from "../../model/Form/FormProp";
import { FormContent } from "../../model/Form/FormContent";
import TurnstileForm from "./TurnstileForm";
import { DoorIcon } from "../../icons";

const TABLE_HEADER: string[] = ["Name", "Door Type", "Status", "", "Action"];
const KEY: string[] = ["name", "doorType"];

const content: FormContent[] = [
  {
    label: "Door",
    content: (
      <TurnstileForm
        type={FormType.CREATE}
        setDto={function (value: SetStateAction<object>): void {
          throw new Error("Function not implemented.");
        }}
        dto={{}} // handleClick={handleClick}
        // dto={doorDto}
        // setDto={setDoorDto}
        // type={formType}
      />
    ),
    icon: <DoorIcon />,
  },
];

const Turnstile = () => {
  const [form, setForm] = useState(true);
  const [formType, setFormType] = useState<FormType>(FormType.CREATE);
  return (
    <>
      <PageBreadcrumb pageTitle="Turnstile" />
      {form ? (
        <BaseForm type={formType} tabContent={content} />
      ) : (
        <BaseTable
          headers={TABLE_HEADER}
          keys={KEY}
          data={[]}
          onInfo={function (data: {
            guid: string;
            isDefault: boolean;
            isActive: boolean;
          }): void {
            throw new Error("Function not implemented.");
          }}
          onEdit={function (data: {
            guid: string;
            isDefault: boolean;
            isActive: boolean;
          }): void {
            throw new Error("Function not implemented.");
          }}
          onRemove={function (data: {
            guid: string;
            isDefault: boolean;
            isActive: boolean;
          }): void {
            throw new Error("Function not implemented.");
          }}
          onClick={function (
            e: React.MouseEvent<HTMLButtonElement, MouseEvent>,
          ): void {
            throw new Error("Function not implemented.");
          }}
          select={[]}
          setSelect={function (
            value: SetStateAction<
              { guid: string; isDefault: boolean; isActive: boolean }[]
            >,
          ): void {
            throw new Error("Function not implemented.");
          }}
          fetchData={function (
            pageNumber: number,
            pageSize: number,
            locationGuid?: string | undefined,
            search?: string | undefined,
            startDate?: string | undefined,
            endDate?: string | undefined,
          ): Promise<void> {
            throw new Error("Function not implemented.");
          }}
          locationGuid={""}
        />
      )}
    </>
  );
};

export default Turnstile;
