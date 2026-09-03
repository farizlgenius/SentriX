import { SetStateAction, useEffect, useState } from "react";
import PageBreadcrumb from "../../components/common/PageBreadCrumb";
import { BaseForm } from "../UiElements/BaseForm";
import { BaseTable } from "../UiElements/BaseTable";
import { FormType } from "../../model/Form/FormProp";
import { FormContent } from "../../model/Form/FormContent";
import { AddIcon, VisitorIcon } from "../../icons";
import { ActionButton } from "../../model/ActionButton";
import VisitorForm from "./VisitorForm";
import { VisitorDto } from "../../model/Visitor/VisitorDto";
import { Title } from "../../enum/Title";
import { Gender } from "../../enum/Gender";
import "@cardid/webcard";


export const Visitor = () => {
       const defaultDto: VisitorDto = {
             guid: "",
             identification: "",
             title: Title.Mr,
             firstname: "",
             middlename: "",
             lastname: "",
             gender: Gender.Male,
             email: "",
             phone: "",
             address: "",
             joinedDate: new Date(),
             expiredDate: new Date(new Date().setHours(new Date().getHours() + 6)),
             additionals: [],
             groups: "",
             locations: "",
             isActive: false,
       };

      const [form, setForm] = useState<boolean>(false);
      const [formType, setFormType] = useState<FormType>(FormType.CREATE);
       const [visitorDto, setVisitorDto] = useState<VisitorDto>(defaultDto);
        const [image, setImage] = useState<File | undefined>();

      const content: FormContent[] = [
            {
                  label: "Personal Information",
                  content: (
                        <VisitorForm
                              type={formType}
                              dto={visitorDto}
                              setDto={setVisitorDto}
                              // handleClick={handleClick}
                              image={image}
                              setImage={setImage}
                        />
                  ),
                  icon: <VisitorIcon />,
            },
          
      ];

      const action: ActionButton[] = [
            {
                  lable: "deactivate",
                  buttonName: "Deactivate",
                  icon: <AddIcon />,
            },
            {
                  lable: "activate",
                  buttonName: "Activate",
                  icon: <AddIcon />,
            },
            {
                  lable: "reset",
                  buttonName: "Reset Anti-Passback",
                  icon: <AddIcon />,
            },
      ];

      const fetchReader = async () => {
            const readers = await navigator.webcard.readers();

            console.log(readers);
      }

      useEffect(() => {
            fetchReader();
      }, []);

      return (
            <>
                  <PageBreadcrumb pageTitle="Visitors" />
                  {
                        form ? (
                              <BaseForm tabContent={content} type={formType} />
                        ) : (
                              <BaseTable headers={[]} keys={[]} data={[]} onInfo={function (data: { guid: string; isDefault: boolean; isActive: boolean; }): void {
                                    throw new Error("Function not implemented.");
                              }} onEdit={function (data: { guid: string; isDefault: boolean; isActive: boolean; }): void {
                                    throw new Error("Function not implemented.");
                              }} onRemove={function (data: { guid: string; isDefault: boolean; isActive: boolean; }): void {
                                    throw new Error("Function not implemented.");
                              }} onClick={function (e: React.MouseEvent<HTMLButtonElement, MouseEvent>): void {
                                    throw new Error("Function not implemented.");
                              }} select={[]} setSelect={function (value: SetStateAction<{ guid: string; isDefault: boolean; isActive: boolean; }[]>): void {
                                    throw new Error("Function not implemented.");
                              }} fetchData={function (pageNumber: number, pageSize: number, locationGuid?: string | undefined, search?: string | undefined, startDate?: string | undefined, endDate?: string | undefined): Promise<void> {
                                    throw new Error("Function not implemented.");
                              }} locationGuid={""} />
                        )
                  }

            </>
      );
}