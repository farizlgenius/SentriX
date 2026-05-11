import React, { PropsWithChildren } from 'react'

import DatePicker from '../../components/form/date-picker';
import { HolidayDto } from '../../model/Holiday/HolidayDto';
import { FormProp, FormType } from '../../model/Form/FormProp';
import Input from '../../components/form/input/InputField';
import Label from '../../components/form/Label';
import { FormActions, FormField, FormSection } from '../../components/form/template/FormTemplate';


const HolidayForm: React.FC<PropsWithChildren<FormProp<HolidayDto>>> = ({ type, setDto, handleClick, dto }) => {
  const isReadOnly = type == FormType.INFO;
  // Alert Modal 
  return (
    // <FormShell
    //   eyebrow="Holiday"
    //   title="Minimal holiday form"
    //   description="Use the same holiday create and update actions inside a cleaner scheduling layout."
    // >
    //   <FormSection title="Holiday Details" description="Name the holiday and select the date it applies to.">
    //     <div className='grid gap-5'>
    //         <FormField>
    //           <Label>Name</Label>
    //           <Input disabled={isReadOnly} defaultValue={dto.name} value={dto.name} onChange={(e) => setDto(prev => ({...prev,name:e.target.value}))} />
    //         </FormField>
    //         <FormField>
    //           <DatePicker
    //             isTime={false}
    //             id="Date"
    //             label="Selected Date"
    //             placeholder="Select a date"
    //             defaultDate={new Date(dto.year, dto.month - 1, dto.day)}
    //             value={`${dto.year}-${dto.month}-${dto.day} ${"00"}:${"00"}`}
    //             //value={createTimeZoneDto.activeTime}
    //             // onChange={(dates, currentDateString) => {
    //             //   // Handle your logic
    //             //   console.log({ dates, currentDateString });
    //             //   //handleChange((prev) => ({ ...prev, day: dates[0].getDate(), month: dates[0].getMonth() + 1, year: dates[0].getFullYear() }))
    //             //   //handleChange
    //             // }}
    //             onChange={(date) => {
    //               setDto((prev) => ({ ...prev, day: date[0].getDate(), month: date[0].getMonth() + 1, year: date[0].getFullYear() }));
    //               console.log(date[0])
    //             }}
    //           />
    //         </FormField>
    //     </div>
    //   </FormSection>
    //   <FormActions
    //     disabled={isReadOnly}
    //     onSubmit={handleClick}
    //     onCancel={handleClick}
    //     submitName={type == FormType.UPDATE ? "update" : "create"}
    //     cancelName='close'
    //     typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
    //   />
    // </FormShell>
    <>
    <FormSection title="Holiday Details" description="Name the holiday and select the date it applies to.">
        <div className='grid gap-5'>
            <FormField>
              <Label>Name</Label>
              <Input disabled={isReadOnly} defaultValue={dto.name} value={dto.name} onChange={(e) => setDto(prev => ({...prev,name:e.target.value}))} />
            </FormField>
            <FormField>
              <DatePicker
                isTime={false}
                id="Date"
                label="Selected Date"
                placeholder="Select a date"
                defaultDate={new Date(dto.year, dto.month - 1, dto.day)}
                value={`${dto.year}-${dto.month}-${dto.day} ${"00"}:${"00"}`}
                //value={createTimeZoneDto.activeTime}
                // onChange={(dates, currentDateString) => {
                //   // Handle your logic
                //   console.log({ dates, currentDateString });
                //   //handleChange((prev) => ({ ...prev, day: dates[0].getDate(), month: dates[0].getMonth() + 1, year: dates[0].getFullYear() }))
                //   //handleChange
                // }}
                onChange={(date) => {
                  setDto((prev) => ({ ...prev, day: date[0].getDate(), month: date[0].getMonth() + 1, year: date[0].getFullYear() }));
                  console.log(date[0])
                }}
              />
            </FormField>
        </div>
      </FormSection>
      <FormActions
        disabled={isReadOnly}
        onSubmit={handleClick}
        onCancel={handleClick}
        submitName={type == FormType.UPDATE ? "update" : "create"}
        cancelName='close'
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      />
    </>
  )
}

export default HolidayForm
