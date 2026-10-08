import { PropsWithChildren, useEffect, useState } from "react"
import { FormProp, FormType } from "../../../model/Form/FormProp"
import { UserDto } from "../../../model/User/UserDto"
import Label from "../Label"
import Switch from "../switch/Switch"
import { FormField } from "../template/FormTemplate"
import Input from "../input/InputField"
import { UserMetadata } from "../../../model/User/UserMetadata"

export const UserSettingForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({ dto, setDto, type }) => {

    return (
        <div className="grid grid-cols-3 gap-10">
            <FormField>
                <Label>Issue Code (Optional)</Label>
                <Input type="number"
                    onChange={(e) => setDto(
                        prev => ({
                            ...prev,
                            metadata: {
                                ...(prev.metadata as UserMetadata),
                                issueCode: Number(e.target.value)
                            }
                        })
                    )}
                    value={(dto.metadata as UserMetadata).issueCode ?? 0} />
            </FormField>
            <FormField>
                <Label>Use Count</Label>
                <Input type="number"
                    onChange={(e) => setDto(
                        prev => ({
                            ...prev,
                            metadata: {
                                ...(prev.metadata as UserMetadata),
                                useCount: Number(e.target.value)
                            }
                        })
                    )}
                    value={(dto.metadata as UserMetadata).useCount ?? 0} />
            </FormField>
            <FormField>
                <Label>Antipassback Location</Label>
                <Input type="number"
                    onChange={
                        (e) => setDto(
                            prev => ({
                                ...prev,
                                metadata: {
                                    ...(prev.metadata as UserMetadata),
                                    apbLoc: Number(e.target.value)
                                }
                            })
                        )}
                    value={(dto.metadata as UserMetadata).apbLoc ?? 0} />
            </FormField>
            <FormField className="col-span-3">
                <Label>User Settings</Label>
                <div className="grid grid-cols-3 mt-5">
                    <Switch
                        disabled={type == FormType.INFO}
                        label={"One Free Antipassback"}
                        defaultChecked={false}
                        onChange={(checked: boolean) => setDto(
                            prev => ({
                                ...prev,
                                metadata: {
                                    ...(prev.metadata as UserMetadata),
                                    oneFreeApb: checked
                                }
                            })
                        )}
                    />
                    <Switch
                        disabled={type == FormType.INFO}
                        label={"Antipassback Exempt"}
                        defaultChecked={false}
                        onChange={(checked: boolean) => setDto(
                            prev => ({
                                ...prev,
                                metadata: {
                                    ...(prev.metadata as UserMetadata),
                                    apbExempt: checked
                                }
                            })
                        )}
                    />
                    <Switch
                        disabled={type == FormType.INFO}
                        label={"Pin Exempt"}
                        defaultChecked={false}
                        onChange={(checked: boolean) => setDto(
                            prev => ({
                                ...prev,
                                metadata: {
                                    ...(prev.metadata as UserMetadata),
                                    pinExempt: checked
                                }
                            })
                        )}
                    />
                </div>
            </FormField>

        </div>
    )
}
