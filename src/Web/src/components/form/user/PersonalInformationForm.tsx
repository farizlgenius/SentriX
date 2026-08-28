import { useEffect, useState } from "react";
import { CamIcon, EnvelopeIcon, FileIcon } from "../../../icons";
import Button from "../../ui/button/Button";
import DatePicker from "../date-picker";
import DropzoneComponent from "../form-elements/DropZone";
import Input from "../input/InputField";
import Radio from "../input/Radio";
import Label from "../Label";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import { UserDto } from "../../../model/User/UserDto";
import TextArea from "../input/TextArea";
import { NativeWebcam } from "../../../pages/UiElements/NativeWebcam";
import Modals from "../../../pages/UiElements/Modals";
import { Avatar } from "../../../pages/UiElements/Avatar";
import { Gender } from "../../../enum/Gender";
import { send } from "../../../api/api";
import { CompanyEndpoint } from "../../../endpoint/CompanyEndpoint";
import { useLocation } from "../../../context/LocationContext";
import { Options } from "../../../model/Options";
import { DepartmentEndpoint } from "../../../endpoint/DepartmentEndpoint";
import { PositionEndpoint } from "../../../endpoint/PositionEndpoint";
import Select from "../Select";
import { FormField, FormSection } from "../template/FormTemplate";

interface PersonalInformationFormProp extends FormProp<UserDto> {
  image: File | undefined;
  setImage: React.Dispatch<React.SetStateAction<File | undefined>>;
}

export const PersonalInformationForm: React.FC<PersonalInformationFormProp> = ({
  dto,
  setDto,
  type,
  handleClick,
  image,
  setImage,
}) => {
  const { locationGuid: locationId } = useLocation();
  const isReadOnly = type == FormType.INFO;
  const [newImage, setNewImage] = useState<File | undefined>();
  const [file, setFile] = useState<boolean>(false);
  const [cam, setCam] = useState<boolean>(false);
  const [selectedValue, setSelectedValue] = useState<Gender | string>(
    Gender.Male.toString(),
  );
  const [com, setCom] = useState<Options[]>([]);
  const [dep, setDep] = useState<Options[]>([]);
  const [pos, setPos] = useState<Options[]>([]);

  function generateEmployeeId(): string {
    return `${crypto.randomUUID().slice(0, 8).toUpperCase()}`;
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log(e.target.name);
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleRadioChange = (value: string) => {
    setSelectedValue(value);
    setDto((prev) => ({ ...prev, gender: value }));
  };

  const handleClickInternal = (e: React.MouseEvent<HTMLButtonElement>) => {
    switch (e.currentTarget.name) {
      case "file":
        setFile(true);
        break;
      case "cam":
        setCam(true);
        break;
      case "close":
        setCam(false);
        setFile(false);
        break;
      case "cancle":
        setCam(false);
        setFile(false);
        break;
    }
  };

  const fetchCompany = async () => {
    const res = await send.get(
      CompanyEndpoint.GET_OPTION_BY_LOCATION(locationId),
    );
    if (res.data) {
      res.data.map((a: Options) => {
        setCom((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchDepartment = async (companyId: number) => {
    const res = await send.get(
      DepartmentEndpoint.GET_OPTION_BY_COMPANY(companyId),
    );
    console.log(res);
    if (res.data) {
      res.data.map((a: Options) => {
        setDep((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            description: a.description,
            additionalInfo: a.additionalInfo,
            isTaken: false,
          },
        ]);
      });
    }
  };

  const fetchPosition = async (departmentId: number) => {
    const res = await send.get(
      PositionEndpoint.GET_OPTION_BY_DEPARTMENT(departmentId),
    );
    if (res.data) {
      res.data.map((a: Options) => {
        setPos((prev) => [
          ...prev,
          {
            label: a.label,
            value: a.value,
            additionalInfo: a.additionalInfo,
            description: a.description,
            isTaken: false,
          },
        ]);
      });
    }
  };

  useEffect(() => {
    fetchCompany();
  }, []);

  return (
    <div className="flex justify-center items-center flex-col gap-4">
      <div className="flex justify-center gap-10">
        <div className="flex flex-col gap-5">
          <div className="flex gap-5">
            <FormSection
              title="Image"
              description="Upload from file or take a live picture with webcam."
            >
              <FormField className="flex flex-col justify-center items-center gap-10">
                {file || cam ? (
                  file ? (
                    <Modals
                      handleClickWithEvent={handleClickInternal}
                      body={
                        <DropzoneComponent
                          newImage={newImage}
                          setNewImage={setNewImage}
                          image={image}
                          setImage={setImage}
                          setFile={setFile}
                        />
                      }
                    />
                  ) : (
                    <Modals
                      isWide={true}
                      handleClickWithEvent={handleClickInternal}
                      body={
                        <NativeWebcam
                          setNewImage={setNewImage}
                          image={image}
                          setImage={setImage}
                          modelStatus={cam}
                          handleClick={handleClickInternal}
                        />
                      }
                    />
                  )
                ) : (
                  <>
                    <div className="h-56 w-56 overflow-hidden rounded-full border-4 border-white bg-white shadow-lg ring-1 ring-gray-200 dark:border-gray-900 dark:bg-gray-900 dark:ring-gray-700">
                      <Avatar
                        userId={dto.userId}
                        newImage={newImage}
                        image={image}
                      />
                    </div>
                    <div className="flex flex-wrap justify-center gap-3">
                      <Button
                        disabled={isReadOnly}
                        name="file"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<FileIcon />}
                      >
                        Browse
                      </Button>
                      <Button
                        disabled={isReadOnly}
                        name="cam"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<CamIcon />}
                      >
                        Take Picture
                      </Button>
                    </div>
                  </>
                )}
              </FormField>
            </FormSection>
            <FormSection
              title="Personal Information"
              description="efwwefwefwef"
            >
              <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
                <div className="flex gap-3 mb-3 w-full col-span-2">
                  <FormField className="flex-1">
                    <Label htmlFor="userId">Cardholder ID / Employee ID</Label>
                    <div className="flex gap-2">
                      <Input
                        disabled={isReadOnly}
                        name="userId"
                        type="text"
                        id="cardHolderId"
                        onChange={handleChange}
                        value={dto.userId}
                      />
                      <Button
                        disabled={isReadOnly}
                        onClick={() =>
                          setDto((prev) => ({
                            ...prev,
                            userId: generateEmployeeId(),
                          }))
                        }
                      >
                        Auto
                      </Button>
                    </div>
                  </FormField>
                </div>

                <div className="flex gap-3 mb-3 w-full col-span-2">
                  <FormField className="flex-1">
                    <Label htmlFor="title">Title</Label>
                    <Input
                      disabled={isReadOnly}
                      name="title"
                      type="text"
                      id="title"
                      onChange={handleChange}
                      value={dto.title}
                      placeholder="Mr."
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="firstName">First Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="firstName"
                      type="text"
                      id="firstName"
                      onChange={handleChange}
                      value={dto.firstName}
                      placeholder="John"
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="middleName">Middle Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="middleName"
                      type="text"
                      id="middleName"
                      onChange={handleChange}
                      value={dto.middleName}
                      placeholder="Jr"
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="lastName">Last Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="lastName"
                      type="text"
                      id="lastName"
                      onChange={handleChange}
                      value={dto.lastName}
                      placeholder="Doh"
                    />
                  </FormField>
                </div>
                <FormField>
                  <Label htmlFor="gender">Gender</Label>
                  <div className="flex justify-around gap-3 pb-3">
                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender1"
                        name="gender"
                        value={Gender.Male.toString()}
                        checked={selectedValue === Gender.Male.toString()}
                        onChange={handleRadioChange}
                        label="Male"
                      />
                    </div>

                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender2"
                        name="gender"
                        value={Gender.Female.toString()}
                        checked={selectedValue === Gender.Female.toString()}
                        onChange={handleRadioChange}
                        label="Female"
                      />
                    </div>
                    <div className="flex flex-col flex-wrap gap-8">
                      <Radio
                        id="gender3"
                        name="gender"
                        value={Gender.Other.toString()}
                        checked={selectedValue === Gender.Other.toString()}
                        onChange={handleRadioChange}
                        label="Other"
                      />
                    </div>
                  </div>
                </FormField>
                <FormField>
                  <DatePicker
                    isTime={false}
                    id="Date"
                    label="Date of birth"
                    placeholder="Select a date"
                    onChange={(date) =>
                      setDto((prev) => ({
                        ...prev,
                        dateOfBirth: date[0].toISOString(),
                      }))
                    }
                    value={dto.dateOfBirth}
                  />
                </FormField>
                <FormField>
                  <Label>Email</Label>
                  <div className="relative">
                    <Input
                      disabled={isReadOnly}
                      name="email"
                      placeholder="info@gmail.com"
                      type="text"
                      className="pl-[62px]"
                      onChange={handleChange}
                      value={dto.email}
                    />
                    <span className="absolute left-0 top-1/2 -translate-y-1/2 border-r border-gray-200 px-3.5 py-3 text-gray-500 dark:border-gray-800 dark:text-gray-400">
                      <EnvelopeIcon className="size-6" />
                    </span>
                  </div>
                </FormField>
                <FormField>
                  <Label>Phone</Label>
                  <Input
                    disabled={isReadOnly}
                    onChange={handleChange}
                    value={dto.phone}
                    name="phone"
                    placeholder="+1 (555) 000-0000"
                  />
                </FormField>
                <FormField className="col-span-2">
                  <Label>Address</Label>
                  <TextArea
                    disabled={isReadOnly}
                    value={dto.address}
                    onChange={(e: string) =>
                      setDto((prev) => ({ ...prev, address: e }))
                    }
                  />
                </FormField>
                <div className="flex col-span-2 gap-5">
                  <FormField className="flex-1">
                    <Label>Company</Label>
                    <Select
                      name={"Company"}
                      disabled={type == FormType.INFO}
                      onChange={(e) => {
                        setDep([]);
                        setPos([]);
                        setDto((prev) => ({
                          ...prev,
                          companyId: Number(e),
                          company:
                            com.find((x) => x.value == Number(e))?.label ?? "",
                        }));
                        fetchDepartment(Number(e));
                      }}
                      defaultValue={dto.companyId}
                      options={com}
                    />
                  </FormField>
                  <FormField className="flex-1">
                    <Label>Department</Label>
                    <Select
                      name={"Department"}
                      defaultValue={dto.departmentId}
                      disabled={type == FormType.INFO}
                      onChange={(e) => {
                        setPos([]);
                        setDto((prev) => ({
                          ...prev,
                          departmentId: Number(e),
                          department:
                            dep.find((x) => x.value == Number(e))?.label ?? "",
                        }));
                        fetchPosition(Number(e));
                      }}
                      options={dep}
                    />
                  </FormField>
                  <FormField className="flex-1">
                    <Label>Position</Label>
                    <Select
                      name={"Position"}
                      disabled={type == FormType.INFO}
                      onChange={(e) =>
                        setDto((prev) => ({
                          ...prev,
                          positionId: Number(e),
                          position:
                            pos.find((x) => x.value == Number(e))?.label ?? "",
                        }))
                      }
                      defaultValue={dto.positionId}
                      options={pos}
                    />
                  </FormField>
                </div>

                <FormField className="col-span-2">
                  <div className="flex justify-between mt-5">
                    <Label>Additionals Field</Label>
                    <a
                      onClick={() =>
                        setDto((prev) => ({
                          ...prev,
                          additionals: [...prev.additionals, ""],
                        }))
                      }
                      className="cursor-pointer font-medium text-blue-600 dark:text-blue-500 hover:underline"
                    >
                      Add
                    </a>
                  </div>
                  <div className="p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
                    {dto.additionals.map((a: string, i: number) => {
                      return (
                        <div className="mb-3" key={i}>
                          <Label>Additionals {i + 1}</Label>
                          <Input
                            disabled={isReadOnly}
                            key={i}
                            onChange={(e) => {
                              const newAdditional = [...dto.additionals];
                              newAdditional[i] = e.target.value;
                              setDto({ ...dto, additionals: newAdditional });
                            }}
                            value={a}
                            name={String(i)}
                            placeholder="Additional Information"
                          />
                        </div>
                      );
                    })}
                  </div>
                </FormField>
              </div>
            </FormSection>
          </div>
        </div>
      </div>
    </div>
  );
};
