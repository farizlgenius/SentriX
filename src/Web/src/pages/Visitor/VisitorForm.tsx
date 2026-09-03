import React, { PropsWithChildren, useEffect, useState } from "react";
import Button from "../../components/ui/button/Button";
import { VisitorDto } from "../../model/Visitor/VisitorDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { Gender } from "../../enum/Gender";
import { Options } from "../../model/Options";
import { FormField, FormSection } from "../../components/form/template/FormTemplate";
import Modals from "../UiElements/Modals";
import DropzoneComponent from "../../components/form/form-elements/DropZone";
import { NativeWebcam } from "../UiElements/NativeWebcam";
import Label from "../../components/form/Label";
import DatePicker from "../../components/form/date-picker";
import { Avatar } from "../UiElements/Avatar";
import { CamIcon, EnvelopeIcon, FileIcon } from "../../icons";
import Input from "../../components/form/input/InputField";
import { Title } from "../../enum/Title";
import Select from "../../components/form/Select";
import Radio from "../../components/form/input/Radio";
import PhoneInput from "../../components/form/group-input/PhoneInput";
import TextArea from "../../components/form/input/TextArea";
import { countries } from "../../constants/phone-code";
import { CompanyDto } from "../../model/Company/CompanyDto";
import { send } from "../../api/api";
import { CompanyEndpoint } from "../../endpoint/CompanyEndpoint";
import { DepartmentEndpoint } from "../../endpoint/DepartmentEndpoint";
import { DepartmentDto } from "../../model/Department/DepartmentDto";

interface VisitorFormProps extends FormProp<VisitorDto> {
  image: File | undefined;
  setImage: React.Dispatch<React.SetStateAction<File | undefined>>;
}


const defaultOptions: Options[] = [
  {
    label: "Not set",
    value: "00000000-0000-0000-0000-000000000000",
  },
];


const VisitorForm: React.FC<VisitorFormProps> = ({
  dto,
  setDto,
  type,
  handleClick,
  image,
  setImage,
}) => {
  const isReadOnly = type == FormType.INFO;
  const [newImage, setNewImage] = useState<File | undefined>();
  const [file, setFile] = useState<boolean>(false);
  const [cam, setCam] = useState<boolean>(false);
  const [selectedValue, setSelectedValue] = useState<Gender | string>(
    Gender.Male.toString(),
  );
  const [com, setCom] = useState<Options[]>(defaultOptions);
  const [dep, setDep] = useState<Options[]>(defaultOptions);
  const [pos, setPos] = useState<Options[]>(defaultOptions);

  function generateEmployeeId(): string {
    return `${crypto.randomUUID().slice(0, 8).toUpperCase()}`;
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    console.log(e.target.name);
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleRadioChange = (value: string) => {
    setSelectedValue(value);
    // setDto((prev) => ({ ...prev, gender: value }));
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
      const res = await send.get(CompanyEndpoint.GET);
      console.log(res);
      if (res) {
        res.data.data.map((a: CompanyDto) => {
          setCom((prev) => [
            ...prev,
            {
              label: a.name,
              value: a.guid,
              additionalInfo: a.address,
              description: a.description,
              isTaken: false,
            },
          ]);
        });
      }
    };
  
    const fetchDepartment = async (guid: string) => {
      const res = await send.get(DepartmentEndpoint.GET_BY_COMPANY(guid));
      console.log(res);
      if (res) {
        res.data.data.map((a: DepartmentDto) => {
          setDep((prev) => [
            ...prev,
            {
              label: a.name,
              value: a.guid,
              description: a.description,
              additionalInfo: "",
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
              overall="Profile Image"
              title="Photo"
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
                        userId={dto.identification}
                        newImage={newImage}
                        image={image}
                      />
                    </div>

                    <div className="flex flex-wrap justify-center gap-3">
                      {/* <Button
                        disabled={isReadOnly}
                        name="file"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<FileIcon />}
                      >
                        Browse
                      </Button> */}
                      <Button
                        disabled={isReadOnly}
                        variant="outline"
                        onClickWithEvent={handleClickInternal}
                        name="file"
                        startIcon={<FileIcon />}
                        className="justify-center"
                      >
                        Browse
                      </Button>
                      {/* <Button
                        disabled={isReadOnly}
                        name="cam"
                        onClickWithEvent={handleClickInternal}
                        startIcon={<CamIcon />}
                      >
                        Take Picture
                      </Button> */}
                      <Button
                        disabled={isReadOnly}
                        variant="outline"
                        onClickWithEvent={handleClickInternal}
                        name="cam"
                        startIcon={<CamIcon />}
                        className="justify-center"
                      >
                        Take Picture
                      </Button>
                    </div>
                  </>
                )}
              </FormField>
            </FormSection>
            <FormSection
              overall="User detail"
              title="Personal Information"
              description="Fill the user detail and information for used in system."
            >
              <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
                <div className="flex gap-3 mb-3 w-full col-span-2">
                  <FormField className="flex-1">
                    <Label htmlFor="userCode">User Code / Employee ID</Label>
                    <div className="flex gap-2">
                      <div className="flex-2">
                        <Input
                          className=""
                          placeholder="User Code"
                          disabled={isReadOnly}
                          name="userCode"
                          type="text"
                          id="userCode"
                          onChange={handleChange}
                          value={dto.userCode}
                        />
                      </div>
                      <Button
                        className="flex-1"
                        disabled={isReadOnly}
                        variant="outline"
                        onClick={() =>
                          setDto((prev) => ({
                            ...prev,
                            userCode: generateEmployeeId(),
                            identification: generateEmployeeId(),
                          }))
                        }
                      >
                        Generate
                      </Button>
                    </div>
                  </FormField>
                </div>
                <div className="flex gap-3 mb-3 w-full col-span-2">
                  <FormField className="flex-1">
                    <Label htmlFor="userId">
                      Identification / Document ID No.
                    </Label>
                    <div className="flex gap-2">
                      <div className="flex-2">
                        <Input
                          className=""
                          placeholder="Identification"
                          disabled={isReadOnly}
                          name="identification"
                          type="text"
                          id="identification"
                          onChange={handleChange}
                          value={dto.identification}
                        />
                      </div>
                    </div>
                  </FormField>
                </div>

                <div className="flex gap-3 mb-3 w-full col-span-2">
                  <FormField className="flex-1">
                    <Label htmlFor="title">Title</Label>
                    <Select
                      options={[
                        {
                          label: Title[Title.Mr],
                          value: Title.Mr,
                        },
                        {
                          label: Title[Title.Miss],
                          value: Title.Miss,
                        },
                        {
                          label: Title[Title.Ms],
                          value: Title.Ms,
                        },
                        {
                          label: Title[Title.Other],
                          value: Title.Other,
                        },
                      ]}
                      onChange={(e) => {
                        setDto((prev) => ({
                          ...prev,
                          title: Number(e),
                        }));
                      }}
                      name={"title"}
                      defaultValue={dto.title}
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="firstName">First Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="firstname"
                      type="text"
                      id="firstName"
                      onChange={handleChange}
                      value={dto.firstname}
                      placeholder="John"
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="middleName">Middle Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="middlename"
                      type="text"
                      id="middleName"
                      onChange={handleChange}
                      value={dto.middlename}
                      placeholder="Jr"
                    />
                  </FormField>
                  <FormField className="flex-2">
                    <Label htmlFor="lastName">Last Name</Label>
                    <Input
                      disabled={isReadOnly}
                      name="lastname"
                      type="text"
                      id="lastName"
                      onChange={handleChange}
                      value={dto.lastname}
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
                        dateOfBirth: date[0],
                      }))
                    }
                    value={dto.dateOfBirth.toISOString()}
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
                  <PhoneInput countries={countries} />
                </FormField>
                <FormField className="col-span-2">
                  <Label>Address</Label>
                  <TextArea
                    placeholder="Please enter address here"
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
                      isString={true}
                      disabled={type == FormType.INFO}
                      onChange={(e) => {
                        setDep(defaultOptions);
                        setPos(defaultOptions);
                        setDto((prev) => ({
                          ...prev,
                          companyGuid: e,
                          company: com.find((x) => x.value == e)?.label ?? "",
                        }));
                        fetchDepartment(e);
                      }}
                      defaultValue={dto.companyGuid}
                      options={com}
                    />
                  </FormField>
                  <FormField className="flex-1">
                    <Label>Department</Label>
                    <Select
                      isString={true}
                      name={"Department"}
                      defaultValue={dto.departmentGuid}
                      disabled={type == FormType.INFO}
                      onChange={(e) => {
                        setPos(defaultOptions);
                        setDto((prev) => ({
                          ...prev,
                          departmentGuid: e,
                          department:
                            dep.find((x) => x.value == e)?.label ?? "",
                        }));
                        fetchPosition(e);
                      }}
                      options={dep}
                    />
                  </FormField>
                  <FormField className="flex-1">
                    <Label>Position</Label>
                    <Select
                      isString={true}
                      name={"position"}
                      disabled={type == FormType.INFO}
                      onChange={(e) =>
                        setDto((prev) => ({
                          ...prev,
                          positionId: e,
                          position: pos.find((x) => x.value == e)?.label ?? "",
                        }))
                      }
                      defaultValue={dto.positionGuid}
                      options={pos}
                    />
                  </FormField>
                </div>
                <FormField>
                  <DatePicker
                    isTime={false}
                    id="Date"
                    label="Joined Date"
                    placeholder="Select a date"
                    onChange={(date) =>
                      setDto((prev) => ({
                        ...prev,
                        joinedDate: date[0],
                      }))
                    }
                    value={dto.joinedDate.toISOString()}
                  />
                </FormField>
                <FormField>
                  <DatePicker
                    isTime={false}
                    id="Date"
                    label="Expired Date"
                    placeholder="Select a date"
                    onChange={(date) =>
                      setDto((prev) => ({
                        ...prev,
                        expiredDate: date[0],
                      }))
                    }
                    value={dto.expiredDate.toISOString()}
                  />
                </FormField>

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


export default VisitorForm;