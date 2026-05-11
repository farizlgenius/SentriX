import { PropsWithChildren } from "react"
import { FormProp, FormType } from "../../model/Form/FormProp"
import Label from "../../components/form/Label"
import Input from "../../components/form/input/InputField"
import TextArea from "../../components/form/input/TextArea"
import { DepartmentDto } from "../../model/Department/DepartmentDto"
import { FormActions, FormField, FormSection } from "../../components/form/template/FormTemplate"

export const DepartmentForm: React.FC<PropsWithChildren<FormProp<DepartmentDto>>> = ({ type, handleClick: handleClickWithEvent, setDto, dto }) => {
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDto(prev => ({ ...prev, [e.target.name]: e.target.value }))
    }
    const isReadOnly = type == FormType.INFO;

    return (
        // <FormShell
        //     eyebrow="Department"
        //     title="Minimal department form"
        //     description="Keeps the same DTO updates and button actions, with a cleaner visual hierarchy."
        // >
        //     <FormSection title="Department Details" description="Capture the department name and a short description.">
        //         <div className="grid gap-5">
        //             <FormField>
        //                 <Label htmlFor="name">Name</Label>
        //                 <Input disabled={isReadOnly} placeholder="Department Name" name="name" type="text" id="departmentName" onChange={handleChange} value={dto.name} />
        //             </FormField>
        //             <FormField>
        //                 <Label htmlFor="description">Description</Label>
        //                 <TextArea disabled={isReadOnly} placeholder="Department Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
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
          <FormSection title="Department Details" description="Capture the department name and a short description.">
                <div className="grid gap-5">
                    <FormField>
                        <Label htmlFor="name">Name</Label>
                        <Input disabled={isReadOnly} placeholder="Department Name" name="name" type="text" id="departmentName" onChange={handleChange} value={dto.name} />
                    </FormField>
                    <FormField>
                        <Label htmlFor="description">Description</Label>
                        <TextArea disabled={isReadOnly} placeholder="Department Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
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
