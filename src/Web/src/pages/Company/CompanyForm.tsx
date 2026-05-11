import { PropsWithChildren } from "react"
import { FormProp, FormType } from "../../model/Form/FormProp"
import { CompanyDto } from "../../model/Company/CompanyDto"
import Label from "../../components/form/Label"
import Input from "../../components/form/input/InputField"
import TextArea from "../../components/form/input/TextArea"
import { FormActions, FormField, FormSection } from "../../components/form/template/FormTemplate"

export const CompanyForm: React.FC<PropsWithChildren<FormProp<CompanyDto>>> = ({ type, handleClick: handleClickWithEvent, setDto, dto }) => {
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDto(prev => ({ ...prev, [e.target.name]: e.target.value }))
    }
    const isReadOnly = type == FormType.INFO;
    return (
        // <FormShell
        //     eyebrow="Company"
        //     title="Clean company form"
        //     description="A lighter, more modern layout for company details without changing the underlying create and update flow."
        // >
        //     <FormSection title="Company Details" description="Basic profile information used throughout the workspace.">
        //         <div className="grid gap-5">
        //             <FormField>
        //                 <Label htmlFor="name">Name</Label>
        //                 <Input disabled={isReadOnly} placeholder="Company Name" name="name" type="text" id="locationName" onChange={handleChange} value={dto.name} />
        //             </FormField>
        //             <FormField>
        //                 <Label htmlFor="description">Address</Label>
        //                 <TextArea disabled={isReadOnly} placeholder="Company Address" onChange={(e: string) => setDto(prev => ({ ...prev, address: e }))} value={dto.address} />
        //             </FormField>
        //             <FormField>
        //                 <Label htmlFor="description">Description</Label>
        //                 <TextArea disabled={isReadOnly} placeholder="Company Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
        //             </FormField>
        //         </div>
        //     </FormSection>
        //     <FormActions
        //         disabled={isReadOnly}
        //         onSubmit={handleClickWithEvent}
        //         onCancel={handleClickWithEvent}
        //         submitName={type == FormType.UPDATE ? "update" : "create"}
        //         typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
        //     />
        // </FormShell>
        <>
         <FormSection title="Company Details" description="Basic profile information used throughout the workspace.">
                <div className="grid gap-5">
                    <FormField>
                        <Label htmlFor="name">Name</Label>
                        <Input disabled={isReadOnly} placeholder="Company Name" name="name" type="text" id="locationName" onChange={handleChange} value={dto.name} />
                    </FormField>
                    <FormField>
                        <Label htmlFor="description">Address</Label>
                        <TextArea disabled={isReadOnly} placeholder="Company Address" onChange={(e: string) => setDto(prev => ({ ...prev, address: e }))} value={dto.address} />
                    </FormField>
                    <FormField>
                        <Label htmlFor="description">Description</Label>
                        <TextArea disabled={isReadOnly} placeholder="Company Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
                    </FormField>
                </div>
            </FormSection>
            <FormActions
                disabled={isReadOnly}
                onSubmit={handleClickWithEvent}
                onCancel={handleClickWithEvent}
                submitName={type == FormType.UPDATE ? "update" : "create"}
                typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
            />
        </>
    )
}
