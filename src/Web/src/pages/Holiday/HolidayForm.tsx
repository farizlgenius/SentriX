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
    <>
      <FormSection title="Holiday Details" description="Name the holiday and select the date it applies to.">
        <div className='grid gap-5'>
          <FormField>
            <Label>Name</Label>
            <Input disabled={isReadOnly} defaultValue={dto.name} value={dto.name} onChange={(e) => setDto(prev => ({ ...prev, name: e.target.value }))} />
          </FormField>
          <FormField>
            <DatePicker
              isTime={true}
              id="date-picker1"
              label="Start Date"
              placeholder="Select a date"
              value={dto.start}
              onChange={(date) => {
                setDto((prev) => ({ ...prev, start: new Date(date[0]).toISOString() }));
                console.log(date[0])
              }}
            />
          </FormField>
          <FormField>
            <DatePicker
              isTime={true}
              id="date-picker2"
              label="End Date"
              placeholder="Select a date"
              value={dto.end}
              onChange={(date) => {
                setDto((prev) => ({ ...prev, end:new Date(date[0]).toISOString() }));
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
