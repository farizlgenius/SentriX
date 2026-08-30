import { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../../model/Form/FormProp";
import { RoleDto } from "../../../model/Role/RoleDto";
import Label from "../Label";
import Input from "../input/InputField";
import { RoleEndpoint } from "../../../endpoint/RoleEndpoint";
import Switch from "../switch/Switch";
import {
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableRow,
} from "../../ui/table";
import Checkbox from "../input/Checkbox";
import api from "../../../api/api";
import { FeatureDto } from "../../../model/Feature/FeatureDto";
import {
  FormActions,
  FormField,
  FormSection,
  FormSectionChecked,
} from "../template/FormTemplate";
import { ModulePermissionDto } from "../../../model/Role/ModulePermissionDto";
import { FeaturePermissionDto } from "../../../model/Role/FeaturePermissionDto";

export const RoleForm: React.FC<PropsWithChildren<FormProp<RoleDto>>> = ({
  type,
  handleClick: handleClickWithEvent,
  dto,
  setDto,
}) => {
  const isReadOnly = type == FormType.INFO;
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setDto((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };
  const fetchFeatureList = async () => {
    const res = await api.get(RoleEndpoint.GET_FEATURE);
    console.log(res);
    if (res && res.data) {
      // setList(res.data.data)
      if (type == FormType.CREATE) {
        setDto((prev) => ({
          ...prev,
          permissions: res.data.map((feature: FeatureDto) => ({
            featureId: feature.id,
            featureName: feature.name,
            isCreated: false,
            isEnabled: false,
            isDeleted: false,
            isUpdated: false,
          })),
        }));
      }
    }
  };

  useEffect(() => {
    fetchFeatureList();
  }, []);
  return (
    <>
      <FormSection
        title="Role Details"
        description="Start with the role name, then fine-tune access below."
      >
        <FormField className="max-w-xl">
          <Label htmlFor="name">Name</Label>
          <Input
            disabled={isReadOnly}
            name="name"
            type="text"
            id="name"
            onChange={handleChange}
            value={dto.name}
          />
        </FormField>
      </FormSection>

      {dto.modules.map((m: ModulePermissionDto) => {
        return (
          <FormSectionChecked
            className="mt-10 mb-10"
            title={`${m.name} Permissions Matrix`}
            description="Enable a feature first, then grant the allowed actions."
            defaultChecked={m.isEnabled}
            onChange={(checked) => {
              setDto((prev) => ({
                ...prev,
                modules: prev.modules.map((a) =>
                  a.id == m.id
                    ? {
                        ...a,
                        isEnabled: checked,
                        features: a.features.map((f) =>
                          checked == false
                            ? {
                                ...f,
                                isCreated: false,
                                isDeleted: false,
                                isEnabled: false,
                                isUpdated: false,
                              }
                            : {
                                ...f,
                              },
                        ),
                      }
                    : {
                        ...a,
                      },
                ),
              }));
            }}
          >
            {m.isEnabled && (
              <div className="flex gap-2">
                <div className="flex-1 overflow-hidden rounded-[20px] border border-[var(--app-panel-border)]">
                  <div>
                    <Table>
                      {/* Table Header */}
                      <TableHeader className="border-b border-gray-100 dark:border-white/[0.05] bg-white dark:bg-gray-900 sticky top-0 z-10">
                        <TableRow>
                          <TableCell
                            isHeader
                            className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                          >
                            <div className="flex gap-2 justify-center item-center">
                              <Switch
                                disabled={isReadOnly}
                                label={""}
                                onChange={(checked) => {
                                  setDto((prev) => ({
                                    ...prev,
                                    modules: prev.modules.map((a) =>
                                      a.id == m.id
                                        ? {
                                            ...a,
                                            isEnabled: checked,
                                            features: a.features.map((f) => ({
                                              ...f,
                                              isEnabled: checked,
                                              isCreated: checked,
                                              isDeleted: checked,
                                              isUpdated: checked,
                                            })),
                                          }
                                        : {
                                            ...a,
                                          },
                                    ),
                                  }));
                                }}
                              />
                            </div>
                          </TableCell>
                          <TableCell
                            isHeader
                            className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400"
                          >
                            Features
                          </TableCell>
                          <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                            <div className="flex justify-center item-center gap-2">
                              <p>Create</p>
                            </div>
                          </TableCell>
                          <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                            <div className="flex justify-center item-center gap-2">
                              <p>Modify</p>
                            </div>
                          </TableCell>
                          <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                            <div className="flex justify-center item-center gap-2">
                              <p>Delete</p>
                            </div>
                          </TableCell>
                        </TableRow>
                      </TableHeader>
                      <TableBody className="divide-y divide-gray-100 dark:divide-white/[0.05]">
                        {m.features.map(
                          (data: FeaturePermissionDto, i: number) => (
                            <TableRow key={i}>
                              {/* <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                                <input name={String(data.value)} type="checkbox" />
                                            </TableCell > */}
                              <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                <div className="flex gap-2 justify-center item-center">
                                  <Switch
                                    disabled={isReadOnly}
                                    defaultChecked={data.isEnabled}
                                    label={""}
                                    onChange={(checked) => {
                                      setDto((prev) => ({
                                        ...prev,
                                        modules: prev.modules.map((a) =>
                                          a.id == m.id
                                            ? {
                                                ...a,
                                                features: a.features.map((f) =>
                                                  f.id == data.id
                                                    ? {
                                                        ...f,
                                                        isEnabled: checked,
                                                      }
                                                    : {
                                                        ...f,
                                                      },
                                                ),
                                              }
                                            : {
                                                ...a,
                                              },
                                        ),
                                      }));
                                    }}
                                  />
                                </div>
                              </TableCell>
                              <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                {data.name}
                              </TableCell>
                              {data.isEnabled && (
                                <>
                                  <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                    <div className="flex justify-center item-center gap-2">
                                      <Checkbox
                                        disabled={isReadOnly}
                                        name="isCreated"
                                        checked={data.isCreated}
                                        onChange={(e) => {
                                          setDto((prev) => ({
                                            ...prev,
                                            modules: prev.modules.map((a) =>
                                              a.id == m.id
                                                ? {
                                                    ...a,
                                                    features: a.features.map(
                                                      (f) =>
                                                        f.id == data.id
                                                          ? {
                                                              ...f,
                                                              isCreated:
                                                                e.target
                                                                  .checked,
                                                            }
                                                          : {
                                                              ...f,
                                                            },
                                                    ),
                                                  }
                                                : {
                                                    ...a,
                                                  },
                                            ),
                                          }));
                                        }}
                                      />
                                    </div>
                                  </TableCell>
                                  <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                    <div className="flex justify-center item-center gap-2">
                                      <Checkbox
                                        disabled={isReadOnly}
                                        name="isUpdated"
                                        checked={data.isUpdated}
                                        onChange={(e) => {
                                          setDto((prev) => ({
                                            ...prev,
                                            modules: prev.modules.map((a) =>
                                              a.id == m.id
                                                ? {
                                                    ...a,
                                                    features: a.features.map(
                                                      (f) =>
                                                        f.id == data.id
                                                          ? {
                                                              ...f,
                                                              isUpdated:
                                                                e.target
                                                                  .checked,
                                                            }
                                                          : {
                                                              ...f,
                                                            },
                                                    ),
                                                  }
                                                : {
                                                    ...a,
                                                  },
                                            ),
                                          }));
                                        }}
                                      />
                                    </div>
                                  </TableCell>
                                  <TableCell className="px-5 py-3 font-medium text-gray-500 text-start text-theme-xs dark:text-gray-400">
                                    <div className="flex justify-center item-center gap-2">
                                      <Checkbox
                                        disabled={isReadOnly}
                                        name="isDeleted"
                                        checked={data.isDeleted}
                                        onChange={(e) => {
                                          setDto((prev) => ({
                                            ...prev,
                                            modules: prev.modules.map((a) =>
                                              a.id == m.id
                                                ? {
                                                    ...a,
                                                    features: a.features.map(
                                                      (f) =>
                                                        f.id == data.id
                                                          ? {
                                                              ...f,
                                                              isDeleted:
                                                                e.target
                                                                  .checked,
                                                            }
                                                          : {
                                                              ...f,
                                                            },
                                                    ),
                                                  }
                                                : {
                                                    ...a,
                                                  },
                                            ),
                                          }));
                                        }}
                                      />
                                    </div>
                                  </TableCell>
                                </>
                              )}
                            </TableRow>
                          ),
                        )}
                      </TableBody>
                    </Table>
                  </div>
                </div>
              </div>
            )}
          </FormSectionChecked>
        );
      })}

      <FormActions
        disabled={isReadOnly}
        onSubmit={handleClickWithEvent}
        onCancel={handleClickWithEvent}
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      />
    </>
  );
};
