import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { FormField } from "../../components/form/template/FormTemplate";
import { ReaderType } from "../../enum/ReaderType";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import Helper from "../../utility/Helper";
import Switch from "../../components/form/switch/Switch";
import { Vendor } from "../../enum/Vendor";
import { send } from "../../api/api";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { Options } from "../../model/Options";
import { DeviceModuleDto } from "../../model/Device/DeviceModuleDto";
import { useLocation } from "../../context/LocationContext";
import { ReaderMode } from "../../enum/ReaderMode";
import { ReaderDirection } from "../../enum/DoorDirection";

const DoorInForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  dto,
  setDto,
  type,
}) => {
  const { locationGuid } = useLocation();
  const [readerType, setReaderType] = useState<ReaderType>(ReaderType.odsp);
  const [moduleOption, setModuleOption] = useState<Options[]>([]);

  const fetchDeviceModule = async (vendor: Vendor) => {
    const res = await send.get(
      ModuleEndpoint.GET_BY_VENDOR(locationGuid, vendor),
    );
    if (res) {
      res.data.data.map((a: DeviceModuleDto) => {
        setModuleOption((prev) =>
          prev.some((x) => x.value == a.guid)
            ? prev
            : [
                ...prev,
                {
                  label: a.name,
                  value: a.guid,
                  isTaken: false,
                },
              ],
        );
      });
    }
  };

  useEffect(() => {
    fetchDeviceModule(dto.vendor);
  }, []);

  return (
    <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="ReaderType">Type</Label>
        <Select
          disabled={type == FormType.INFO}
          name="ReaderType"
          options={[
            {
              label: "Wiegand",
              value: ReaderType.wiegand,
              description: "",
              isTaken: false,
            },
            {
              label: "OSDP",
              value: ReaderType.odsp,
              description: "",
              isTaken: false,
            },
          ]}
          placeholder="Select Option"
          onChange={(value) => {
            setReaderType(Number(value));
          }}
          className="dark:bg-dark-900"
          defaultValue={readerType}
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerIn.readerModuleComponentId">Module</Label>
        <Select
          isString={true}
          disabled={type == FormType.INFO}
          name="readerIn.readerModuleComponentId"
          options={moduleOption}
          placeholder="Select Option"
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              readers:
                prev.readers.length == 0
                  ? [
                      {
                        guid: "",
                        slotNo: 0,
                        mode: ReaderMode.wiegand,
                        metadata: "",
                        vendor: dto.vendor,
                        readerDirection: ReaderDirection.In,
                        deviceModuelGuid: value,
                      },
                    ]
                  : [
                      ...prev.readers.map((d, i) =>
                        i == 0
                          ? {
                              guid: "",
                              slotNo: 0,
                              mode: ReaderMode.wiegand,
                              metadata: "",
                              vendor: dto.vendor,
                              readerDirection: ReaderDirection.In,
                              deviceModuelGuid: value,
                            }
                          : d,
                      ),
                    ],
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            dto.readers.length == 0 ? "" : dto.readers[0].deviceModuelGuid
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerIn.readerNo">Slot No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="readerIn.readerNo"
          options={[]}
          placeholder="Select Option"
          // onChange={(value: string) => {
          //   setDto((prev) => ({
          //     ...prev,
          //     metadata: {
          //       ...(prev.metadata as AeroDoorMetadata),
          //       readerIn: {
          //         ...(prev.metadata as AeroDoorMetadata).readerIn,
          //         readerNumber: Number(value),
          //       },
          //     },
          //   }));
          //   setReaderInOption((prev) =>
          //     Helper.updateOptionByValue(prev, Number(value), true),
          //   );
          // }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).readerIn?.readerNumber
          }
        />
      </FormField>
      {readerType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="readerIn.osdpAddress">Address</Label>
            <Select
              disabled={type == FormType.INFO}
              name="readerIn.osdpAddress"
              options={[]}
              placeholder="Select Option"
              onChange={(value: string) => {
                setDto((prev) => ({
                  ...prev,
                  metadata: {
                    ...(prev.metadata as AeroDoorMetadata),
                    readerIn: {
                      ...(prev.metadata as AeroDoorMetadata).readerIn,
                      osdpAddress: Number(value),
                    },
                  },
                }));
                Helper.updateOptionByValue(osdpAddress, Number(value), true);
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.metadata as AeroDoorMetadata).readerIn?.osdpAddress
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="readerOut.osdpBaudrate">Reader Baud Rate</Label>
            <Select
              disabled={type == FormType.INFO}
              name="readerOut.osdpBaudrate"
              options={[]}
              placeholder="Select Option"
              // onChange={(value: string) => {
              //   setDto((prev) => ({
              //     ...prev,
              //     metadata: {
              //       ...(prev.metadata as AeroDoorMetadata),
              //       readerIn: {
              //         ...(prev.metadata as AeroDoorMetadata).readerIn,
              //         osdpBaudrate: Number(value),
              //       },
              //     },
              //   }));
              // }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.metadata as AeroDoorMetadata).readerIn?.osdpBaudrate
              }
            />
          </FormField>
          <FormField>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO}
                label="Auto Discover"
                defaultChecked={true}
                onChange={(checked: boolean) => {
                  setDto((prev) => ({
                    ...prev,
                    metadata: {
                      ...(prev.metadata as AeroDoorMetadata),
                      readerIn: {
                        ...(prev.metadata as AeroDoorMetadata).readerIn,
                        osdpDiscover: checked ? 0x00 : 0x08,
                      },
                    },
                  }));
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO}
                label="Tracing"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                  setDto((prev) => ({
                    ...prev,
                    metadata: {
                      ...(prev.metadata as AeroDoorMetadata),
                      readerIn: {
                        ...(prev.metadata as AeroDoorMetadata).readerIn,
                        osdpTracing: checked ? 0x10 : 0x00,
                      },
                    },
                  }));
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO}
                label="Secure Channel"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                  setDto((prev) => ({
                    ...prev,
                    metadata: {
                      ...(prev.metadata as AeroDoorMetadata),
                      readerIn: {
                        ...(prev.metadata as AeroDoorMetadata).readerIn,
                        osdpTracing: checked ? 0x80 : 0x00,
                      },
                    },
                  }));
                }}
              />
            </div>
          </FormField>
        </>
      )}
    </div>
  );
};

export default DoorInForm;
