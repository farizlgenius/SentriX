import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Input from "../../components/form/input/InputField";
import { send } from "../../api/api";
import { EventEndpoint } from "../../endpoint/TransactionEndpoint";
import { AlertDto } from "../../model/Alert/AlertDto";
import { Options } from "../../model/Options";
import Select from "../../components/form/Select";




const AlertForm: React.FC<PropsWithChildren<FormProp<AlertDto>>> = ({
      dto,
      type,
      setDto,
}) => {
      const isReadOnly = type == FormType.INFO;
      const [events,setEvents] = useState<Options[]>([]);
      const [modules,setModules] = useState<Options[]>([]);

      const fetchData = async () => {
            var res = await send.get(EventEndpoint.GET_BASE);
            setModules(res.data.data);
      }     

      useEffect(() => {
            fetchData();
      },[])

      return (
            <div className="grid grid-cols-2 gap-5">
                  <FormField>
                        <Label>Name</Label>
                        <Input type="text" disabled={isReadOnly}
                              placeholder="Action name"
                              onChange={(e) => {
                                    setDto(prev => ({
                                          ...prev,
                                          name: e.target.value
                                    }))
                              }}
                              value={dto.name}
                        />
                  </FormField>
                  <FormField>
                        <Label>Module</Label>
                       <Select name={""} options={modules} onChange={} defaultValue={} />
                  </FormField>
                  <FormField>
                        <Label>Events</Label>
                       <Select name={""} options={events} onChange={} defaultValue={} />
                  </FormField>
            
            </div>
      );
};

export default AlertForm;
