import { PropsWithChildren, useEffect, useState } from "react";
import Label from "../../components/form/Label";
import { FormField } from "../../components/form/template/FormTemplate";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormProp, FormType } from "../../model/Form/FormProp";
import Select from "../../components/form/Select";
import { ReaderType } from "../../enum/ReaderType";
import Switch from "../../components/form/switch/Switch";
import { Options } from "../../model/Options";
import { AeroReaderMetadata } from "../../model/Door/AeroReaderMetadata";
import { readerAddress, readerBaudrate } from "../../model/Door/ReaderOption";
import { ReaderMode } from "../../enum/ReaderMode";
import { Vendor } from "../../enum/Vendor";
import { ReaderDirection } from "../../enum/DoorDirection";

type ExtraProps = {
  moduleOption:Options[]
  readerOption:Options[]
};

const DoorOutForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  type,
  setDto,
  dto,
  handleChange,
  moduleOption,
  readerOption,
  setIsNext
  
}) => {
  const [readerType, setReaderType] = useState<ReaderType>(
    ReaderType.wiegand,
  );

  // NEW: Automatically validate whenever name or deviceGuid changes
  useEffect(() => {
    if (setIsNext) {


      // 1. Safely find the reader (might be undefined)
      const readerOut = dto.readers.find(x => x.readerDirection === ReaderDirection.Out);

      // 2. Extract values safely, providing fallbacks if undefined
      const moduleGuid = readerOut?.deviceModuleGuid || "";
      const slotNo = readerOut?.slotNo ?? -1;

      if (moduleGuid !== "") {
        handleChange({
          target: {
            name: "readerOut.module",
            value: moduleGuid
          }
        } as React.ChangeEvent<HTMLInputElement>);
      }



      // 3. Perform the validation check safely
      const isValid = moduleGuid.trim() !== "" && slotNo !== -1;

      console.log("DoorOutForm isValid:", isValid);
      setIsNext(isValid); // This will now successfully run!
    }
  }, [dto.readers, setIsNext]); // Simplified dependency array

  return (
    <>
    <div className="grid grid-cols-2 gap-5">
        <FormField>
        <Label htmlFor="readerOut.module">Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="readerOut.module"
          options={moduleOption}
          placeholder="Select Option"
          onChange={(e) => 
          {
            handleChange(e)
           setDto((prev) => ({
                     ...prev,
                     readers: [...prev.readers,
                       {
                         slotNo: -1,
                         mode: ReaderMode.wiegand,
                         metadata: {
                           osdpFlag:false,
                           address:-1,
                           baudrate:-1,
                           autoDiscover: 0x00,
                  tracing: 0x00,
                  secureChannel: 0x00
                         },
                         vendor: Vendor.aero,
                         readerDirection: ReaderDirection.Out,
                         deviceModuleGuid: e.target.value
                       }
                     ]
                   }))
          }  
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.readers.find(x => x.readerDirection == ReaderDirection.Out)?.deviceModuleGuid ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerOut.type">Type</Label>
        <Select
          disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
          name="readerOut.type"
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
              readers:prev.readers.map((reader) => 
              reader.readerDirection == ReaderDirection.Out ? {
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
        <Label htmlFor="readerOut.slot">Slot No</Label>
        <Select
          disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
          name="readerOut.slot"
          options={readerOption}
          placeholder="Select Option"
          onChange={(e) => 
            setDto(prev => ({
              ...prev,
              readers:prev.readers.map((reader) => 
              reader.readerDirection == ReaderDirection.Out ? {...reader,slotNo:Number(e.target.value)} : reader
              )
            }))
          }
          className="dark:bg-dark-900"
         defaultValue={ dto.readers.find(x => x.readerDirection == ReaderDirection.Out)?.slotNo ?? -1}
        />
      </FormField>

      {readerType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="readerOut.osdpAddress">Address</Label>
            <Select
              disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
              name="readerOut.osdpAddress"
              options={readerAddress}
              placeholder="Select Option"
              onChange={(e) => 
                   setDto(prev => ({
                                  ...prev,
                                  readers: prev.readers.map((reader) =>
                                    reader.readerDirection == ReaderDirection.Out ? { ...reader, metadata: {
                                      ...(reader.metadata as AeroReaderMetadata),
                                      address:Number(e.target.value)
                                    } } : reader
                                  )
                                }))
              }
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers.find(x => x.readerDirection == ReaderDirection.Out)?.metadata as AeroReaderMetadata)?.address ?? -1
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="readerOut.baudrate">Baudrate</Label>
            <Select
              disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
              name="readerOut.baudrate"
              options={readerBaudrate}
              placeholder="Select Option"
              onChange={(e) => 
                 setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                    reader.readerDirection == ReaderDirection.Out ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      baudrate:Number(e.target.value)
                    } } : reader
                  )
                }))
              }
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers.find(x => x.readerDirection == ReaderDirection.Out)?.metadata as AeroReaderMetadata)?.baudrate ?? -1
              }
            />
          </FormField>
          <FormField>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
                label="Auto Discover"
                defaultChecked={true}
                onChange={(checked: boolean) => {
                  setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                   reader.readerDirection == ReaderDirection.Out ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      autoDiscover:checked ? 0x00 : 0x08
                    } } : reader
                  )
                }))
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
                disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
                label="Tracing"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                   setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                    reader.readerDirection == ReaderDirection.Out ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      tracing:checked ? 0x10 : 0x00
                    } } : reader
                  )
                }))
                }}
              />
            </div>
            <div className="mt-3">
              <Switch
               disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.Out) == undefined}
                label="Secure Channel"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                  setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                    reader.readerDirection == ReaderDirection.Out ? { ...reader, metadata: {
                      ...(reader.metadata as AeroReaderMetadata),
                      secureChannel:checked ? 0x80 : 0x00,
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
