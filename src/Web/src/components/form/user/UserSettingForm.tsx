import { PropsWithChildren, useEffect, useState } from "react"
import { FormProp, FormType } from "../../../model/Form/FormProp"
import { UserDto } from "../../../model/User/UserDto"
import Label from "../Label"
import Switch from "../switch/Switch"
import { send } from "../../../api/api"
import { CredentialEndpoint } from "../../../endpoint/CredentialEndpoint"
import { Options } from "../../../model/Options"
import Button from "../../ui/button/Button"

export const UserSettingForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({ dto, setDto, type }) => {
    const [userFlag, setUserFlag] = useState<Options[]>([])
    const fetchUserFlag = async () => {
        const res = await send.get(CredentialEndpoint.GET_FLAG);
        if (res && res.data) {
            setUserFlag(res.data)
        }
    }
    useEffect(() => {
        fetchUserFlag();
    }, [])

    const formatFlagDescription = (description?: string) => {
        if (!description) return '';
        return description.replace(/\s*🔹/g, '\n🔹').trim();
    }

    return (
        <>
            <Label>User Settings</Label>
            <div className="rounded-2xl border border-gray-200 bg-gray-50/80 p-5 dark:border-gray-800 dark:bg-white/[0.02]">
                <div className="mt-4 grid grid-cols-1 gap-3">
                    {

                        userFlag.map((d, i) =>

                            <div key={i} className="rounded-xl border border-gray-200 bg-white px-4 py-3 dark:border-gray-700 dark:bg-gray-900">
                                <Switch
                                    disabled={type == FormType.INFO}
                                    label={d.label}
                                    defaultChecked={false}
                                    onChange={(checked: boolean) => {
                                        setDto(prev => ({ ...prev, flag: checked ? prev.flag | Number(d.value) : prev.flag & (~Number(d.value)) }))
                                        console.log(checked)
                                    }}
                                />
                                {d.description && (
                                    <p className="mt-1 whitespace-pre-line text-xs text-gray-500 dark:text-gray-400">
                                        {formatFlagDescription(d.description)}
                                    </p>
                                )}
                            </div>
                        )

                    }

                </div>

            </div>
            <Button onClick={() => console.log('Current flag value:', dto.flag)}>Check Flag</Button>
        </>
    )
}
