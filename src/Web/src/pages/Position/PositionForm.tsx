import { PropsWithChildren } from "react"
import { FormProp, FormType } from "../../model/Form/FormProp"
import Label from "../../components/form/Label"
import Input from "../../components/form/input/InputField"
import TextArea from "../../components/form/input/TextArea"
import { PositionDto } from "../../model/Position/PositionDto"
import { FormActions, FormField, FormSection } from "../../components/form/template/FormTemplate"

export const PositionForm: React.FC<PropsWithChildren<FormProp<PositionDto>>> = ({ type, handleClick: handleClickWithEvent, setDto, dto }) => {
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setDto(prev => ({ ...prev, [e.target.name]: e.target.value }))
    }
    const isReadOnly = type == FormType.INFO;

    return (
        // <FormShell
        //     eyebrow="Position"
        //     title="Minimal position form"
        //     description="The same create and update behavior, refreshed with the operator-style visual system."
        // >
        //     <FormSection title="Position Details" description="Define the position name and optional description.">
        //         <div className="grid gap-5">
        //             <FormField>
        //                 <Label htmlFor="name">Name</Label>
        //                 <Input disabled={isReadOnly} placeholder="Position Name" name="name" type="text" id="positionName" onChange={handleChange} value={dto.name} />
        //             </FormField>
        //             <FormField>
        //                 <Label htmlFor="description">Description</Label>
        //                 <TextArea disabled={isReadOnly} placeholder="Position Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
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
        <FormSection title="Position Details" description="Define the position name and optional description.">
                <div className="grid gap-5">
                    <FormField>
                        <Label htmlFor="name">Name</Label>
                        <Input disabled={isReadOnly} placeholder="Position Name" name="name" type="text" id="positionName" onChange={handleChange} value={dto.name} />
                    </FormField>
                    <FormField>
                        <Label htmlFor="description">Description</Label>
                        <TextArea disabled={isReadOnly} placeholder="Position Description" onChange={(e: string) => setDto(prev => ({ ...prev, description: e }))} value={dto.description} />
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
