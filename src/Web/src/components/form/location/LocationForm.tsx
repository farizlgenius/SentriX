import { PropsWithChildren, useEffect, useState } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import Label from "../Label";
import Input from "../input/InputField";
import TextArea from "../input/TextArea";
import { Options } from "../../../model/Options";
import { LocationEndpoint } from "../../../endpoint/LocationEndpoint";
import { send } from "../../../api/api";
import { CountryDto } from "../../../model/Country/CountryDto";
import Select from "../Select";
import { LocationDto } from "../../../model/Location/LocationDto";
import { FormField, FormSection } from "../template/FormTemplate";
import { HttpStatusCode } from "axios";

export const LocationForm: React.FC<
  PropsWithChildren<FormProp<LocationDto>>
> = ({ type, setDto, dto }) => {
  const [countryOption, setCountryOption] = useState<Options[]>([]);
  const isReadOnly = type == FormType.INFO;
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };
  const fetchCountry = async () => {
    const res = await send.get(LocationEndpoint.COUNTRY);
    if (res.status == HttpStatusCode.Ok) {
      setCountryOption(res.data.data);
    }
  };
  useEffect(() => {
    fetchCountry();
  }, []);
  return (
    <>
      <div className="grid grid-cols-2 gap-5">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input
              disabled={isReadOnly}
              placeholder="Location Name"
              name="name"
              type="text"
              id="locationName"
              onChange={handleChange}
              value={dto.name}
            />
          </FormField>
          <FormField>
            <Label htmlFor="name">Country</Label>
            <Select
              disabled={isReadOnly}
              options={countryOption}
              name={"country"}
              defaultValue={dto.country.guid as string}
              onChange={e => {
                setDto((prev) => ({
                  ...prev,
                  country:{
                    ...prev.country,
                    guid:e.target.value
                  }
                }));
              }}
            />
          </FormField>
          <FormField className="col-span-2">
            <Label htmlFor="description">Description</Label>
            <TextArea
              disabled={isReadOnly}
              placeholder="Location Description"
              onChange={(e: string) =>
                setDto((prev) => ({ ...prev, description: e }))
              }
              value={dto.description}
            />
          </FormField>
        </div>
    </>
  );
};
