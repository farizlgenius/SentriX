import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp } from "../../../model/Form/FormProp";
import { UserDto } from "../../../model/User/UserDto";
import { useLocation } from "../../../context/LocationContext";
import { send } from "../../../api/api";
import { GroupEndpoint } from "../../../endpoint/GroupEndpoint";
import ListTransfer from "../list-transfer/ListTransfer";
import { GroupDto } from "../../../model/Group/GroupDto";
import { FormSection } from "../template/FormTemplate";


export const GroupForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({ dto, setDto, type, handleClick }) => {
    const { locationId } = useLocation();
    const [groups, setGroups] = useState<GroupDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);


    const handleListChange = (data: GroupDto[]) => {
        setDto(prev => ({ ...prev, groups: data.map(x => x.id) }))
    }



    const fetchGroup = async () => {
        const res = await send.get(GroupEndpoint.GET_BY_LOCATION(locationId))
        console.log(res);
        if (res && res.data) {
            setGroups(res.data.filter((al: GroupDto) => !dto.groups.some(selected => selected === al.id)));
            setLoading(false);
        }
    }

    useEffect(() => {
        fetchGroup();
    }, [])

    return (
        <FormSection className='flex flex-col'>

            {loading ? (
                <p>Loading access levels...</p>
            ) : (
                <ListTransfer<GroupDto>
                    availableItems={groups}
                    selectedItems={groups.filter(x => dto.groups.find(s => x.id == s))}
                    onChange={handleListChange}
                />
            )}

        </FormSection>

    )

}