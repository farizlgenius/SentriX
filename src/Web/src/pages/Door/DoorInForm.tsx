import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { FormField } from "../../components/form/template/FormTemplate";
import { ReaderType } from "../../enum/ReaderType";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import Switch from "../../components/form/switch/Switch";
import { Options } from "../../model/Options";
import { readerAddress, readerBaudrate } from "../../model/Door/ReaderOption";
import { AeroReaderMetadata } from "../../model/Door/AeroReaderMetadata";

type ExtraProps = {
  moduleOption:Options[];
  readerOption:Options[];
};

const DoorInForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  readerOption
}) => {
  const [readerType, setReaderType] = useState<ReaderType>(ReaderType.odsp);
  
 

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
        <Label htmlFor="readerIn.module">Module</Label>
        <Select
          isString={true}
          disabled={type == FormType.INFO}
          name="readerIn.module"
          options={moduleOption}
          placeholder="Select Option"
          onChange={(value: string) => {
      
            setDto((prev) => ({
              ...prev,
              readers:prev.readers.map((reader,index) => 
              index == 0 ? {
                ...reader,
                deviceModuelGuid:value
              } : reader
              )
            }))
            // Fetch Reader Slot
            fetchReader(value)

          }}
          className="dark:bg-dark-900"
          defaultValue={
            dto.readers[0].deviceModuelGuid
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="readers">Slot No</Label>
        <Select
          disabled={type == FormType.INFO}
          name="readers"
          options={readerOption}
          placeholder="Select Option"
          onChange={(value: string) => {
            setDto(prev => ({
              ...prev,
              readers:prev.readers.map((reader,index) => 
              index == 0 ? {...reader,slotNo:Number(value)} : reader
              )
            }))
          }}
          className="dark:bg-dark-900"
          defaultValue={dto.readers[0].slotNo}
        />
      </FormField>
      {readerType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="address">Address</Label>
            <Select
              disabled={type == FormType.INFO}
              name="address"
              options={readerAddress}
              placeholder="Select Option"
              onChange={(value: string) => {
                setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader, index) =>
                    index == 0 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      address:Number(value)
                    } } : reader
                  )
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers[0].metadata as AeroReaderMetadata).address
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="baudrate">Baudrate</Label>
            <Select
              disabled={type == FormType.INFO}
              name="baudrate"
              options={readerBaudrate}
              placeholder="Select Option"
              onChange={(value: string) => {
               setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader, index) =>
                    index == 0 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      baudrate:Number(value)
                    } } : reader
                  )
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers[0].metadata as AeroReaderMetadata).baudrate
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
                   setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader, index) =>
                    index == 0 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      discover:checked ? 0x00 : 0x08
                    } } : reader
                  )
                }))
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO}
                label="Tracing"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                  setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader, index) =>
                    index == 0 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      discover:checked ? 0x10 : 0x00
                    } } : reader
                  )
                }))
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO}
                label="Secure Channel"
                defaultChecked={false}
                onChange={(checked: boolean) => {
            
                   setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader, index) =>
                    index == 0 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      discover:checked ? 0x80 : 0x00,
                    } } : reader
                  )
                }))
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
