import { PropsWithChildren, useState } from "react";
import Label from "../../components/form/Label";
import { FormField } from "../../components/form/template/FormTemplate";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";
import { ReaderType } from "../../enum/ReaderType";
import Switch from "../../components/form/switch/Switch";

const DoorOutForm: React.FC<PropsWithChildren<FormProp<DoorDto>>> = ({
  type,
  setDto,
  dto,
}) => {
  const [readerOutType, setReaderOutType] = useState<ReaderType>(
    ReaderType.odsp,
  );
  return (
    <>
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
          // onChange={(value: string) => {
          //   if (value == ReaderType.Wiegand) {
          //     setDto((prev) => ({
          //       ...prev,
          //       metadata: {
          //         ...(prev.metadata as AeroDoorMetadata),
          //         ledMode: 1,
          //         readerOut: {
          //           ...(prev.metadata as AeroDoorMetadata).readerOut,
          //           osdpFlag: false,
          //           osdpAddress: 0x00,
          //           osdpBaudRate: 0x00,
          //           osdpDiscover: 0x00,
          //           osdpSecureChannel: 0x00,
          //           osdpTracing: 0x00,
          //         },
          //       },
          //     }));
          //   } else {
          //     setDto((prev) => ({
          //       ...prev,
          //       metadata: {
          //         ...(prev.metadata as AeroDoorMetadata),
          //         ledMode: 7,
          //         readerOut: {
          //           ...(prev.metadata as AeroDoorMetadata).readerOut,
          //           osdpFlag: true,
          //         },
          //       },
          //     }));
          //   }
          //   setReaderOutType(value);
          // }}
          className="dark:bg-dark-900"
          defaultValue={readerOutType}
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerOut.readerModuleComponentId">Module</Label>
        <Select
          disabled={type == FormType.INFO}
          name="readerOut.readerModuleComponentId"
          options={[]}
          placeholder="Select Option"
          onChangeWithEvent={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                readerOut: {
                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                  readerModuleComponentId: Number(value),
                  readerModuleId: moduleOption.find(
                    (x) => x.value == Number(value),
                  )?.additionalInfo,
                },
              },
            }));
            fetchReaderOut(Number(value));
            if (readerOutType == ReaderType.odsp)
              fetchOsdpAddress(Number(value));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).readerOut
              ?.readerModuleComponentId
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerOut.readerNumber">Slot No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="readerOut.readerNo"
          options={[]}
          placeholder="Select Option"
          onChange={(value: string) => {
            setDto((prev) => ({
              ...prev,
              metadata: {
                ...(prev.metadata as AeroDoorMetadata),
                readerOut: {
                  ...(prev.metadata as AeroDoorMetadata).readerOut,
                  readerNumber: Number(value),
                },
              },
            }));
          }}
          className="dark:bg-dark-900"
          defaultValue={
            (dto.metadata as AeroDoorMetadata).readerOut?.readerNumber
          }
        />
      </FormField>

      {readerOutType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="readerOut.osdpAddress">Address</Label>
            <Select
              disabled={type == FormType.INFO}
              name="readerOut.osdpAddress"
              options={[]}
              placeholder="Select Option"
              onChange={(value: string) => {
                setDto((prev) => ({
                  ...prev,
                  metadata: {
                    ...(prev.metadata as AeroDoorMetadata),
                    readerOut: {
                      ...(prev.metadata as AeroDoorMetadata).readerOut,
                      osdpAddress: Number(value),
                    },
                  },
                }));
                Helper.updateOptionByValue(osdpAddress, Number(value), true);
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.metadata as AeroDoorMetadata).readerOut?.osdpAddress
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="readerOut.osdpBaudrate">Baudrate</Label>
            <Select
              disabled={type == FormType.INFO}
              name="readerOut.osdpBaudrate"
              options={[]}
              placeholder="Select Option"
              onChange={(value: string) => {
                setDto((prev) => ({
                  ...prev,
                  metadata: {
                    ...(prev.metadata as AeroDoorMetadata),
                    readerOut: {
                      ...(prev.metadata as AeroDoorMetadata).readerOut,
                      osdpBaudrate: Number(value),
                    },
                  },
                }));
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.metadata as AeroDoorMetadata).readerOut?.osdpBaudrate
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
                      readerOut: {
                        ...(prev.metadata as AeroDoorMetadata).readerOut,
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
                      readerOut: {
                        ...(prev.metadata as AeroDoorMetadata).readerOut,
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
                      readerOut: {
                        ...(prev.metadata as AeroDoorMetadata).readerOut,
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
    </>
  );
};

export default DoorOutForm;
