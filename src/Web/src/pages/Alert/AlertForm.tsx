import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import { send } from "../../api/api";
import { EventEndpoint } from "../../endpoint/EventEndpoint";
import { AlertDto } from "../../model/Alert/AlertDto";
import { Options } from "../../model/Options";
import Select from "../../components/form/Select";
import { AlertEndpoint } from "../../endpoint/AlertEndpoint";

const AlertForm: React.FC<PropsWithChildren<FormProp<AlertDto>>> = ({
  dto,
  type,
  setDto,
}) => {
  const isReadOnly = type === FormType.INFO;
  const [entity,setEntity] = useState<Options[]>([]);
  const [event,setEvent] = useState<Options[]>([]);
  const [levels,setLevels] = useState<Options[]>([]);

  const fetchEntity = async () => {
    const res = await send.get(EventEndpoint.GET_ENTITY);
    if(res.data.data){
      setEntity(res.data.data);
    }
  };

  const fetchEvent = async (guid:string) => {
    const res = await send.get(EventEndpoint.GET_EVENT(guid));
    if(res.data.data){
      setEvent(res.data.data);
    }
  };

  const fetchLevel = async () => {
    const res = await send.get(AlertEndpoint.GET_LEVEL);
    setLevels(res.data.data);
  };

  useEffect(() => {
    fetchEntity();
    fetchLevel();
    if(type == FormType.INFO || type == FormType.UPDATE){
      fetchEvent(dto.coreEntityGuid);
    }
  }, []);

  // Find selected level color description (e.g. "#ff0000" or "red")
  const selectedLevelColor = levels.find(
    (x) => x.value === dto.alertLevelGuid
  )?.description;

  // Reusable square color badge element
  const renderColorBadge = (color?: string) => {
    if (!color) return null;

    return (
      <span
        aria-hidden="true"
        className="inline-block w-3.5 h-3.5 rounded-[3px] border border-black/10 shadow-sm shrink-0 transition-colors duration-200"
        style={{ backgroundColor: color }}
      />
    );
  };

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label>Name</Label>
        <Input
          type="text"
          disabled={isReadOnly}
          placeholder="Action name"
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              name: e.target.value,
            }));
            
          }}
          value={dto.name}
        />
      </FormField>

      <FormField>
        <Label>Module</Label>
        <Select
          name=""
          options={entity}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              coreEntityGuid: e.target.value
            }));
            fetchEvent(e.target.value);
          }
            
          }
          defaultValue={dto.coreEntityGuid}
        />
      </FormField>

      <FormField>
        <Label>Events</Label>
        <Select
          disabled={isReadOnly || dto.coreEntityGuid === ""}
          isString={true}
          name=""
          options={event}
          defaultValue={dto.coreEventGuid}
          onChange={(e) => {
            setDto((prev) => ({
              ...prev,
              coreEventGuid: e.target.value
            }))
          }
            
          }
        />
      </FormField>

      <FormField>
        <Label>Level</Label>
        <Select
          disabled={isReadOnly}
          icon={renderColorBadge(selectedLevelColor)}
          isString={true}
          name=""
          options={levels}
          defaultValue={dto.alertLevelGuid}
          onChange={(e) =>
            setDto((prev) => ({
              ...prev,
              alertLevelGuid: e.target.value,
              alertLevelColor:selectedLevelColor ?? "",
              alertLevelName:levels.find(x => x.value == e.target.value)?.label ?? ""
            }))
          }
        />
      </FormField>
    </div>
  );
};

export default AlertForm;