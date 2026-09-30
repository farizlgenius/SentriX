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
import { ReaderMode } from "../../enum/ReaderMode";
import { Vendor } from "../../enum/Vendor";
import { ReaderDirection } from "../../enum/DoorDirection";

type ExtraProps = {
  moduleOption:Options[];
  readerOption:Options[];
};

const DoorInForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  moduleOption,
  readerOption,
  handleChange,
  setIsNext
}) => {
  const [readerType, setReaderType] = useState<ReaderType>(ReaderType.wiegand);
  

  // NEW: Automatically validate whenever name or deviceGuid changes
 useEffect(() => {
    if (setIsNext) {
      let isValid = false;
      // 1. Safely find the reader (might be undefined)
      const readerIn = dto.readers.find(x => x.readerDirection === ReaderDirection.In);

      // 2. Extract values safely, providing fallbacks if undefined
      const moduleGuid = readerIn?.deviceModuleGuid || "";
      const slotNo = readerIn?.slotNo ?? -1;
      const address = (readerIn?.metadata as AeroReaderMetadata)?.address ?? -1;
      const baudrate = (readerIn?.metadata as AeroReaderMetadata)?.baudrate ?? -1;

      // 3. Perform the validation check safely
      isValid = moduleGuid.trim() !== "" && slotNo !== -1;

      if(readerIn?.mode == ReaderMode.osdp && isValid){
        isValid = address !== -1 && baudrate !== -1;
      }

      console.log("DoorInForm isValid:", isValid); 
      setIsNext(isValid); // This will now successfully run!
    }
  }, [dto.readers, setIsNext]); // Simplified dependency array
 

  return (
    <div className="grid grid-cols-2 gap-5">
       <FormField>
        <Label htmlFor="readerIn.module">Module</Label>
        <Select
          isString={true}
          disabled={type == FormType.INFO}
          name="readerIn.module"
          options={moduleOption}
          placeholder="Select Option"
          onChange={(e) => {
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
                discover:-1,
                tracing:-1,
                secureChannel:-1
              },
              vendor: Vendor.aero,
              readerDirection: ReaderDirection.In,
              deviceModuleGuid: e.target.value
            }
          ]
        }))
          }
            
            
          }
          className="dark:bg-dark-900"
          defaultValue={
            dto.readers.find(x => x.readerDirection == ReaderDirection.In)?.deviceModuleGuid ?? ""
          }
        />
      </FormField>
      <FormField>
        <Label htmlFor="readerIn.type">Type</Label>
        <Select
          disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
          name="readerIn.type"
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
          onChange={(e) => {
            setReaderType(Number(e.target.value));
            setDto((prev) => ({
          ...prev,
          readers: prev.readers.map((reader) =>
            reader.readerDirection == ReaderDirection.In ? {
              ...reader,
              mode: Number(e.target.value)
            } : reader
          )
        }))
          }}
          className="dark:bg-dark-900"
          defaultValue={readerType}
        />
      </FormField>
     
      <FormField>
        <Label htmlFor="readerIn.slot">Slot No</Label>
        <Select
           disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
          name="readerIn.slot"
          options={readerOption}
          placeholder="Select Option"
          onChange={(e) => 
            setDto(prev => ({
          ...prev,
          readers: prev.readers.map((reader) =>
            reader.readerDirection == ReaderDirection.In ? { ...reader, slotNo: Number(e.target.value) } : reader
          )
        }))
          }
          className="dark:bg-dark-900"
          defaultValue={ dto.readers.find(x => x.readerDirection == ReaderDirection.In)?.slotNo ?? -1}
        />
      </FormField>
      {readerType == ReaderType.odsp && (
        <>
          <FormField>
            <Label htmlFor="readerIn.address">Address</Label>
            <Select
               disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
              name="readerIn.address"
              options={readerAddress}
              placeholder="Select Option"
              onChange={(e) => 
                 setDto(prev => ({
                          ...prev,
                          readers: prev.readers.map((reader) =>
                            reader.readerDirection == ReaderDirection.In ? {
                              ...reader, metadata: {
                                ...(reader.metadata as AeroReaderMetadata),
                                address: Number(e.target.value)
                              }
                            } : reader
                          )
                        }))
              }
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers.find(x => x.readerDirection == ReaderDirection.In)?.metadata as AeroReaderMetadata)?.address ?? -1
              }
            />
          </FormField>
          <FormField>
            <Label htmlFor="readerIn.baudrate">Baudrate</Label>
            <Select
               disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
              name="readerIn.baudrate"
              options={readerBaudrate}
              placeholder="Select Option"
              onChange={(e) => 
                 setDto(prev => ({
                          ...prev,
                          readers: prev.readers.map((reader) =>
                           reader.readerDirection == ReaderDirection.In ? {
                              ...reader, metadata: {
                                ...(reader.metadata as AeroReaderMetadata),
                                baudrate: Number(e.target.value)
                              }
                            } : reader
                          )
                        }))
              }
              className="dark:bg-dark-900"
              defaultValue={
                (dto.readers.find(x => x.readerDirection == ReaderDirection.In)?.metadata as AeroReaderMetadata)?.baudrate ?? -1
              }
            />
          </FormField>
          <FormField>
            <div className="mt-3">
              <Switch
                 disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
                label="Auto Discover"
                defaultChecked={true}
                onChange={(checked: boolean) => {

                  setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                   reader.readerDirection == ReaderDirection.In ? { ...reader, metadata: {
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
                 disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
                label="Tracing"
                defaultChecked={false}
                onChange={(checked: boolean) => {

                  setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                    reader.readerDirection == ReaderDirection.In ? { ...reader, metadata: {
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
                 disabled={type == FormType.INFO || dto.readers.length == 0 || dto.readers.find(x => x.readerDirection == ReaderDirection.In) == undefined}
                label="Secure Channel"
                defaultChecked={false}
                onChange={(checked: boolean) => {
                  
                   setDto(prev => ({
                  ...prev,
                  readers: prev.readers.map((reader) =>
                   reader.readerDirection == ReaderDirection.In ? { ...reader, metadata: {
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
