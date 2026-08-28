import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import { UserDto } from "../../../model/User/UserDto";
import { useLocation } from "../../../context/LocationContext";
import { send } from "../../../api/api";
import { GroupEndpoint } from "../../../endpoint/GroupEndpoint";
import ListTransfer from "../list-transfer/ListTransfer";
import { GroupDto } from "../../../model/Group/GroupDto";
import { FormSection } from "../template/FormTemplate";

export const GroupForm: React.FC<PropsWithChildren<FormProp<UserDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const { locationGuid: locationId } = useLocation();

  const [groups, setGroups] = useState<GroupDto[]>([]);
  const [loading, setLoading] = useState(true);

  const handleListChange = (data: GroupDto[]) => {
    setDto((prev) => ({
      ...prev,
      groups: data.map((x) => x.id),
    }));
  };

  const fetchGroup = async () => {
    try {
      const res = await send.get(GroupEndpoint.GET_BY_LOCATION(locationId));

      if (res?.data) {
        setGroups(res.data);
      }
    } catch (err) {
      console.error("Failed to load groups", err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchGroup();
  }, [locationId]);

  const selectedItems = groups.filter((x) => dto.groups.includes(x.id));

  const availableItems = groups.filter((x) => !dto.groups.includes(x.id));

  return (
    <FormSection className="flex flex-col">
      {loading ? (
        <p>Loading groups...</p>
      ) : (
        <ListTransfer<GroupDto>
          availableItems={availableItems}
          selectedItems={selectedItems}
          onChange={handleListChange}
          disabled={type == FormType.INFO}
        />
      )}
    </FormSection>
  );
};
