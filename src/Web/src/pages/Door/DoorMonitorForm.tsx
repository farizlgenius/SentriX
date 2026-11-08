import React, { PropsWithChildren, useEffect } from "react";
import { FormProp, FormType } from "../../model/Form/FormProp";
import { DoorDto } from "../../model/Door/DoorDto";
import { FormField } from "../../components/form/template/FormTemplate";
import Label from "../../components/form/Label";
import Select from "../../components/form/Select";
import { Options } from "../../model/Options";
import { InputMode } from "../../enum/InputMode";
import { Vendor } from "../../enum/Vendor";
import { AeroSensorMetadata } from "../../model/Door/AeroSensorMetadata";
import Input from "../../components/form/input/InputField";
import Button from "../../components/ui/button/Button";

type ExtraProps = {
  moduleOption:Options[]
  inputOption:Options[]
};

const DoorMonitorForm: React.FC<PropsWithChildren<FormProp<DoorDto> & ExtraProps>> = ({
  dto,
  setDto,
  type,
  handleChange,
  moduleOption,
  inputOption,
  handleClick,
  setIsNext
}) => {

  const isFinished = dto.sensor?.slotNo == undefined ;
      const isReadOnly = type == FormType.INFO;
      const isModuleSelected = dto.sensor?.deviceModuleGuid  == "" || dto.sensor?.deviceModuleGuid  == undefined;

   useEffect(() => {
      if (setIsNext) {
        let isValid = false;
        // 1. Safely find the reader (might be undefined)
        const sensor = dto.sensor;
  
        // 2. Extract values safely, providing fallbacks if undefined
        const moduleGuid = sensor?.deviceModuleGuid || "";
        const slotNo = sensor?.slotNo ?? -1;

        if(moduleGuid !== ""){
           if(moduleGuid !== ""){
          handleChange({
            target: {
              name: "sensor.module",
              value: moduleGuid
            }
          } as React.ChangeEvent<HTMLInputElement>);
        }
        }
  
        // 3. Perform the validation check safely
        isValid = moduleGuid.trim() !== "" && slotNo !== -1;
  
  
        console.log("SensorForm isValid:", isValid); 
        setIsNext(isValid); // This will now successfully run!
      }
    }, [dto.sensor, setIsNext]); // Simplified dependency array
    
  return (
     <div className="grid grid-cols-2 gap-5">
      <FormField>
        <Label htmlFor="sensor.module">Module</Label>
        <Select
        isString={true}
          disabled={type == FormType.INFO}
          name="sensor.module"
          options={moduleOption}
          onChange={(e) => {
             setDto((prev) => ({
              ...prev,
              sensor:{
                slotNo:-1,
                vendor:Vendor.aero,
                deviceModuleGuid:e.target.value,
                metadata:{
                  mode:InputMode.NC,
                  debounce:4,
                  holdTime:0,
                  dcHeld:1,
                  dcHeldADA:0
                }
              }
            }))
            handleChange(e)
          }
          }
          className="dark:bg-dark-900"
          defaultValue={dto.sensor?.deviceModuleGuid ?? ""}
        />
      </FormField>
      <FormField>
        <Label htmlFor="sensor.slot">Slot No</Label>
        {/* <Select
          disabled={isReadOnly}
          name="sensor.slot"
          options={inputOption}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              sensor:prev.sensor != null 
              ?
              {
                ...prev.sensor,
                slotNo:Number(e.target.value)
              }
               :
               prev.sensor
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={dto.sensor?.slotNo ?? -1}
        /> */}
        <Button disabled={isReadOnly || isModuleSelected} name="select.slot" onClick={handleClick} variant={isFinished ? "primary" : "green" }>
          {isFinished ? "Select Slot" : "Selected"}
        </Button>
      </FormField>
      <FormField>
        <Label htmlFor="sensor.mode">Input Mode</Label>
        <Select
          disabled={type == FormType.INFO}
          name="sensor.mode"
         options={[
            {
              label:"Normally Close",
              value:InputMode.NC
            },
            {
              label:"Normally Open",
              value:InputMode.NO
            },
             {
              label:"Standard Noraml 1K Active 2K",
              value:InputMode.StandardN1A2
            },
            {
              label:"Standard Noraml 2K Active 1K",
              value:InputMode.StandardN2A1
            }
          ]}
          onChange={(e) => 
            setDto((prev) => ({
              ...prev,
              sensor:prev.sensor != null ? 
              {
                ...prev.sensor,
                 metadata:{
                  ...(prev.sensor.metadata as AeroSensorMetadata),
                  mode:Number(e.target.value)
                }
              }
              :
              prev.sensor
            }))
          }
          className="dark:bg-dark-900"
          defaultValue={
           (dto.sensor?.metadata as AeroSensorMetadata)?.mode ?? InputMode.NC
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.debounce">Debounce</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="0" max="15" 
        value={ (dto.sensor?.metadata as AeroSensorMetadata)?.debounce ?? 0}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              sensor: prev.sensor != null ? 
              {
                ...prev.sensor,
                metadata:{
                  ...(prev.sensor.metadata as AeroSensorMetadata),
                  debounce:Number(e.target.value)
                }
              }
              :
              prev.sensor
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.holdTime">Hold Time</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="2" max="15" 
        defaultValue={2}
        value={ (dto.sensor?.metadata as AeroSensorMetadata)?.holdTime ?? 2}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              sensor: prev.sensor != null ? 
              {
                ...prev.sensor,
                metadata:{
                  ...(prev.sensor.metadata as AeroSensorMetadata),
                  holdTime:Number(e.target.value)
                }
              }
              :
              prev.sensor
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.holdTime">Dc Held</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="1" max="32767" 
        value={ (dto.sensor?.metadata as AeroSensorMetadata)?.dcHeld ?? 1}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              sensor: prev.sensor != null ? 
              {
                ...prev.sensor,
                metadata:{
                  ...(prev.sensor.metadata as AeroSensorMetadata),
                  dcHeld:Number(e.target.value)
                }
              }
              :
              prev.sensor
            }))
          }
        />
      </FormField>
       <FormField>
        <Label htmlFor="sensor.holdTime">Dc Held (ADA)</Label>
        <Input 
        disabled={isReadOnly}
        type="number" min="0" max="32767" 
        value={ (dto.sensor?.metadata as AeroSensorMetadata)?.dcHeldADA ?? 0}
        onChange={(e) => 
            setDto(prev => ({
              ...prev,
              sensor: prev.sensor != null ? 
              {
                ...prev.sensor,
                metadata:{
                  ...(prev.sensor.metadata as AeroSensorMetadata),
                  dcHeldADA:Number(e.target.value)
                }
              }
              :
              prev.sensor
            }))
          }
        />
      </FormField>
    </div>
  );
};

export default DoorMonitorForm;
