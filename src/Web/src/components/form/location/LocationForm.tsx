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

export const LocationForm: React.FC<
  PropsWithChildren<FormProp<LocationDto>>
> = ({ type, setDto, dto }) => {
  const [country, setCountry] = useState<Options[]>([]);
  const isReadOnly = type == FormType.INFO;
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };
  const fetchCountry = async () => {
    const res = await send.get(LocationEndpoint.COUNTRY);
    if (res.data.data) {
      res.data.data.map((item: CountryDto) => {
        setCountry((prev) => [...prev, { label: item.name, value: item.id }]);
      });
    }
  };
  useEffect(() => {
    fetchCountry();
  }, []);
  return (
    <>
      <FormSection
        overall="Location Details"
        title="Locations"
        description="Name the location, assign its country, and add a short description."
        className="pb-10"
      >
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
              options={country}
              name={"countryId"}
              defaultValue={dto.countryId}
              onChange={(value: string) => {
                setDto((prev) => ({
                  ...prev,
                  countryId: parseInt(value),
                  country:
                    country.find((x) => x.value == Number(value))?.label ?? "",
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
      </FormSection>
      {/* <FormActions
        disabled={isReadOnly}
        onSubmit={handleClickWithEvent}
        onCancel={handleClickWithEvent}
        cancelName="close"
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      /> */}
    </>
  );
};
