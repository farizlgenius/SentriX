import { PropsWithChildren, useState } from "react";
import Label from "../../components/form/Label";
import { FormField } from "../../components/form/template/FormTemplate";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";
import { ReaderType } from "../../enum/ReaderType";
import Switch from "../../components/form/switch/Switch";
import { Options } from "../../model/Options";
import { send } from "../../api/api";
import { ModuleEndpoint } from "../../endpoint/ModuleEndpoint";
import { AeroReaderMetadata } from "../../model/Door/AeroReaderMetadata";
import { readerAddress, readerBaudrate } from "../../model/Door/ReaderOption";

type ExtraProps = {
  setModuleOption: React.Dispatch<React.SetStateAction<Options[]>>;
  moduleOption:Options[]
};

const DoorOutForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  type,
  setDto,
  dto,
  setModuleOption,
  moduleOption
}) => {
  const [readerType, setReaderType] = useState<ReaderType>(
    ReaderType.odsp,
  );
  const [readerOption,setReaderOption] = useState<Options[]>([]);
    const fetchReader = async (guid:string) => {
      var res = await send.get(ModuleEndpoint.GET_READER_SLOT(guid))
      setReaderOption(res.data.data);
    }

  return (
    <>
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
            setDto((prev) => ({
              ...prev,
              readers:prev.readers.map((reader,index) => 
              index == 1 ? {
                ...reader,
                mode:Number(value)
              } : reader
              )
            }))
          }}
          className="dark:bg-dark-900"
          defaultValue={readerType}
        />
      </FormField>
      <FormField>
        <Label htmlFor="module">Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="module"
          options={moduleOption}
          placeholder="Select Option"
          onChangeWithEvent={(value: string) => {
             setDto((prev) => ({
              ...prev,
              readers:prev.readers.map((reader,index) => 
              index == 1 ? {
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
            dto.readers[1].deviceModuelGuid
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
              index == 1 ? {...reader,slotNo:Number(value)} : reader
              )
            }))
          }}
          className="dark:bg-dark-900"
         defaultValue={dto.readers[1].slotNo}
        />
      </FormField>

      {readerType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="readerOut.osdpAddress">Address</Label>
            <Select
              disabled={type == FormType.INFO}
              name="readerOut.osdpAddress"
              options={readerAddress}
              placeholder="Select Option"
              onChange={(value: string) => {
                setDto(prev => ({
                                  ...prev,
                                  readers: prev.readers.map((reader, index) =>
                                    index == 1 ? { ...reader, metadata: {
                                      ...(reader.metadata as AeroReaderMetadata),
                                      address:Number(value)
                                    } } : reader
                                  )
                                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers[1].metadata as AeroReaderMetadata).address
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
                    index == 1 ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      baudrate:Number(value)
                    } } : reader
                  )
                }))
              }}
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers[1].metadata as AeroReaderMetadata).baudrate
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
                    index == 1 ? { ...reader, metadata: {
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
                    index == 1 ? { ...reader, metadata: {
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
                    index == 1 ? { ...reader, metadata: {
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
      
    </>
  );
};

export default DoorOutForm;
