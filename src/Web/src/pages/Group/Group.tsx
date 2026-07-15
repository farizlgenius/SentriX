import { GroupIcon } from '../../icons';
import {  useState } from 'react';
import GroupForm from './GroupForm';
import PageBreadcrumb from '../../components/common/PageBreadCrumb';
import Helper from '../../utility/Helper';
import { AccessLevelToast } from '../../model/ToastMessage';
import { useToast } from '../../context/ToastContext';
import { GroupDto } from '../../model/Group/GroupDto';
import { useLocation } from '../../context/LocationContext';
import { send } from '../../api/api';
import { GroupEndpoint } from '../../endpoint/GroupEndpoint';
import { BaseTable } from '../UiElements/BaseTable';
import { useAuth } from '../../context/AuthContext';
import { FeatureId } from '../../enum/FeatureId';
import { BaseForm } from '../UiElements/BaseForm';
import { FormContent } from '../../model/Form/FormContent';
import { FormType } from '../../model/Form/FormProp';
import { usePopup } from '../../context/PopupContext';
import { usePagination } from '../../context/PaginationContext';



// Access Group Page 
export const HEADER: string[] = [
    "Name", "Action"
]

export const KEY: string[] = [
    "name"
];

const Group = () => {
    const { toggleToast } = useToast();
    const { locationId } = useLocation();
    const { filterPermission } = useAuth();
    const { setPagination } = usePagination();
    const { setCreate, setUpdate, setRemove, setConfirmCreate, setConfirmRemove, setConfirmUpdate, setInfo, setMessage } = usePopup();
    const defaultDto: GroupDto = {
        id: 0,
        componentId: 0,
        name: '',
        doors: [],
        locationId: locationId,
        isActive: true,
        isDefault: false
    }
    const [dto, setDto] = useState<GroupDto>(defaultDto);
    const [groups, setGroups] = useState<GroupDto[]>([]);
    const [refresh, setRefresh] = useState(false);
    const toggleRefresh = () => setRefresh(!refresh);
    {/* Modal */ }
    const [form, setForm] = useState<boolean>(false);
    const [formType, setFormType] = useState<FormType>(FormType.CREATE);

    const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
        console.log(e.currentTarget.name);
        switch (e.currentTarget.name) {
            case "add":
                setForm(true);
                setFormType(FormType.CREATE);
                break;
            case "delete":
                if (selectedObjects.length == 0) {
                    setMessage("Please select object")
                    setInfo(true);
                }
                setConfirmRemove(() => async () => {
                    var data: number[] = [];
                    selectedObjects.map(async (a: GroupDto) => {
                        data.push(a.id)
                    })
                    var res = await send.post(GroupEndpoint.DELETE_RANGE, data)
                    if (Helper.handleToastByResCode(res, AccessLevelToast.DELETE_RANGE, toggleToast)) {
                        setSelectedObjects([])
                        toggleRefresh();
                    }
                })
                setRemove(true);
                break;
            case "create":
                setConfirmCreate(() => async () => {
                    const res = await send.post(GroupEndpoint.CREATE, dto);
                    if (Helper.handleToastByResCode(res, AccessLevelToast.CREATE, toggleToast)) {
                        setDto(defaultDto);
                        setForm(false);
                        toggleRefresh();
                    }
                })
                setCreate(true);
                break;
            case "update":
                setConfirmUpdate(() => async () => {
                    const res = await send.put(GroupEndpoint.UPDATE, dto);
                    if (Helper.handleToastByResCode(res, AccessLevelToast.UPDATE, toggleToast)) {
                        setDto(defaultDto)
                        setForm(false);
                        toggleRefresh();
                    }
                })
                setUpdate(true)
                break;
            case "close":
            case "cancle":
                setForm(false);
                setDto(defaultDto)
                break;
            default:
                break;
        }

    }

    {/* handle Table Action */ }
    const handleInfo = (data: GroupDto) => {
        setFormType(FormType.UPDATE)
        setDto(data)
        setForm(true);
    }
    const handleEdit = (data: GroupDto) => {
        setFormType(FormType.UPDATE)
        setDto(data)
        setForm(true);
    }

    const handleRemove = (data: GroupDto) => {
        setConfirmRemove(() => async () => {
            const res = await send.delete(GroupEndpoint.DELETE(data.id))
            if (Helper.handleToastByResCode(res, AccessLevelToast.DELETE, toggleToast))
                toggleRefresh();
        })
        setRemove(true);
    }


    {/* Group Data */ }
    const fetchData = async (pageNumber: number, pageSize: number,locationId?:number,search?: string, startDate?: string, endDate?: string) => {
                const res = await send.get(GroupEndpoint.PAGINATION(pageNumber,pageSize,locationId,search, startDate, endDate));
                if (res.data.success) {
                    setGroups(res.data.data.items);
                    setPagination(res.data.data);
                }
            }





    {/* checkBox */ }
    const [selectedObjects, setSelectedObjects] = useState<GroupDto[]>([]);

    const tabContent: FormContent[] = [
        {
            label: "Access Level",
            icon: <GroupIcon />,
            content: <GroupForm dto={dto} handleClick={handleClick} setDto={setDto} type={formType} />
        }
    ]

    return (
        <>
            <PageBreadcrumb pageTitle="Access Level" />
            {form ?
                <BaseForm tabContent={tabContent} />
                :
                <BaseTable<GroupDto> headers={HEADER} keys={KEY} data={groups} onEdit={handleEdit} onRemove={handleRemove} onClick={handleClick} select={selectedObjects} setSelect={setSelectedObjects} permission={filterPermission(FeatureId.group)} onInfo={handleInfo} fetchData={fetchData} locationId={locationId} refresh={refresh} />
            }

        </>
    )
}

export default Group