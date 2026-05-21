import React, { PropsWithChildren, useEffect, useState } from 'react'
import Label from '../../components/form/Label';
import Input from '../../components/form/input/InputField';
import DatePicker from '../../components/form/date-picker';
import Select from '../../components/form/Select';
import Helper from '../../utility/Helper';
import ActionElement from '../UiElements/ActionElement';
import { IntervalDto } from '../../model/Interval/IntervalDto';
import { TimeZoneDto } from '../../model/TimeZone/TimeZoneDto';
import { Options } from '../../model/Options';
import { ModeDto } from '../../model/ModeDto';
import { TimeZoneEndPoint } from '../../endpoint/TimezoneEndpoint';
import { IntervalEndpoint } from '../../endpoint/IntervalEndpoint';
import { useLocation } from '../../context/LocationContext';
import { FormProp, FormType } from '../../model/Form/FormProp';
import { send } from '../../api/api';
import { CalenderIcon, TimeIcon } from '../../icons';
import TextArea from '../../components/form/input/TextArea';
import { useToast } from '../../context/ToastContext';
import { FormActions, FormField, FormSection } from '../../components/form/template/FormTemplate';
import { IntervalForm } from '../../components/form/interval/IntervalForm';
import { DaysInWeekDto } from '../../model/Interval/DaysInWeekDto';
import { DeviceType } from '../../enum/DeviceType';



const TimeZoneForm: React.FC<PropsWithChildren<FormProp<TimeZoneDto>>> = ({ type, setDto, dto, handleClick }) => {
  const { toggleToast } = useToast();
  const { locationId } = useLocation();
  const [modeDetail, setModeDetail] = useState<string>("");
  const [modeOption, setModeOption] = useState<Options[]>([])
  const [intervalForm, setIntervalForm] = useState<boolean>(false);

  const defaultDto: IntervalDto = {
    locationId: locationId,
    isActive: true,
    id: 0,
    days: {
      sunday: false,
      monday: false,
      tuesday: false,
      wednesday: false,
      thursday: false,
      friday: false,
      saturday: false,
      id: 0,
      componentId: 0,
      locationId: 0
    },
    daysDetail: "",
    start: "",
    end: "",
    componentId: 0,
    type: ''
  }
  const [intervalDto, setIntervalDto] = useState<IntervalDto>(defaultDto);


  const dayDescBuilder = (days: DaysInWeekDto) => {
    let res: string[] = [];
    if (days.monday) res.push('Mon');
    if (days.tuesday) res.push('Tue');
    if (days.wednesday) res.push('Wed');
    if (days.thursday) res.push('Thu');
    if (days.friday) res.push('Fri');
    if (days.saturday) res.push('Sat');

    return res.join(' ');
  }



  const toLocalISOWithOffset = (date: Date) => {
    const pad = (n: number) => String(n).padStart(2, "0");
    const tzOffset = -date.getTimezoneOffset();
    const sign = tzOffset >= 0 ? "+" : "-";
    const offsetHours = pad(Math.floor(Math.abs(tzOffset) / 60));
    const offsetMinutes = pad(Math.abs(tzOffset) % 60);

    return (
      date.getFullYear() + "-" +
      pad(date.getMonth() + 1) + "-" +
      pad(date.getDate()) + "T" +
      pad(date.getHours()) + ":" +
      pad(date.getMinutes()) + ":" +
      pad(date.getSeconds()) +
      sign + offsetHours + ":" + offsetMinutes
    );
  }


  const fetchMode = async () => {
    const res = await send.get(TimeZoneEndPoint.GET_MODE(DeviceType.AERO));
    if (res) {
      res.data.map((a:Options) => {
          setModeOption((prev) => [...prev, {
            value: a.value,
            label: a.label,
            description: a.description
          }])
        })
    }
  }




  const handleClickWithData = (data: IntervalDto) => {
    setDto((prev: TimeZoneDto) => ({ ...prev, intervals: [...prev.intervals.filter(a => a.id !== data.id)] }));
  };
  const handleClickWithEvent = (e: React.MouseEvent<HTMLButtonElement>) => {
    console.log(e.currentTarget.name)
    switch (e.currentTarget.name) {
      case "close-interval":
        setIntervalForm(false);
        setIntervalDto(defaultDto);
        break;
      case "add-interval":
        if (!Helper.isDayEmpty(intervalDto.days)) {
          toggleToast("error", "Day can't be empty")
        } else if (!Helper.isValidTimeRange(intervalDto.start, intervalDto.end)) {
          toggleToast("error", "Start time must lower than end time")
        }
        else {
          setIntervalDto(prev => ({ ...prev, daysDetail: dayDescBuilder(prev.days) }))
          console.log();
          setDto(prev => ({
            ...prev, intervals: [...prev.intervals,
              intervalDto
            ]
          }))
          setIntervalForm(false);
          setIntervalDto(defaultDto);
        }

        break;
      case "detail":
        setModeDetail(modeOption.filter(a => a.value == dto.mode)[0].description ?? "")
        // setModeDetailPopup(true);
        break;
      default:
        break;
    }

  }

  useEffect(() => {
    fetchMode()
  }, [])

  return (
    <>
      <FormSection title="Time Zone Details" description="Name the location, assign its country, and add a short description." className="pb-10 mb-5">
        <div className="grid gap-5 grid-cols-2 md:grid-cols-2 gap-x-10 gap-y-6 mb-8 p-5">
          <FormField>
            <Label htmlFor="name">Name</Label>
            <Input placeholder='Time zone name' disabled={type == FormType.INFO} name="name" type="text" id="name" onChange={(e) => setDto((prev: TimeZoneDto) => ({ ...prev, name: e.target.value }))} value={dto.name} />
          </FormField>
          <FormField>
            <Label>TimeZone Mode</Label>
            <Select
              name="mode"
              options={modeOption}
              placeholder="Select Option"
              onChangeWithEvent={(e) => {
                setDto((prev: TimeZoneDto) => ({ ...prev, mode: Number(e) }));
                setModeDetail(modeOption.filter(a => a.value == dto.mode)[0].description ?? "")
              }}
              className="dark:bg-dark-900"
              defaultValue={dto.mode == -1 ? -1 : dto.mode}
            />
          </FormField>
          <FormField>
            <DatePicker
              id="activeTime"
              label="Activate Date"
              placeholder="Select a date"
              value={dto.active}
              onChange={(dates, currentDateString) => {
                // Handle your logic
                console.log({ dates, currentDateString });
                setDto((prev: TimeZoneDto) => ({ ...prev, active: toLocalISOWithOffset(dates[0]) }))
              }}
            />
          </FormField>
          <FormField>
            <DatePicker
              id="deactiveTime"
              label="Deactive Date"
              placeholder="Select a date"
              value={dto.deactive}
              onChange={(dates, currentDateString) => {
                // Handle your logic
                console.log({ dates, currentDateString });
                setDto((prev: TimeZoneDto) => ({ ...prev, deactive: toLocalISOWithOffset(dates[0]) }))
              }}
            />
          </FormField>

          <FormField className='col-span-2'>
            <TextArea disabled={true} value={modeDetail} placeholder='Detail info will show here' />
          </FormField>
          <FormField className='col-span-2'>
            {/* Interval */}
            <div className="flex-1 flex flex-col gap-5 items-center p-5 border border-gray-200 rounded-2xl dark:border-gray-800 lg:p-6">
              <div className='w-full'>
                {intervalForm ?
                  <>
                    <IntervalForm type={FormType.CREATE} handleClick={handleClickWithEvent} setDto={setIntervalDto} dto={intervalDto} />
                  </>
                  :
                  <>
                    <div className="flex flex-col gap-4 swim-lane">
                      <div className="flex items-center justify-between mb-2">
                        <h3 className="flex items-center gap-3 text-base font-medium text-gray-800 dark:text-white/90">
                          Intervals {dto.intervals.length}/12
                          <span className="inline-flex rounded-full bg-gray-100 px-2 py-0.5 text-theme-xs font-medium text-gray-700 dark:bg-white/[0.03] dark:text-white/80">
                            {/* {createIntervalDtoList.length}/12 */}
                          </span>
                        </h3>
                        <a onClick={() => setIntervalForm(true)} className="cursor-pointer font-medium text-blue-600 dark:text-blue-500 hover:underline">Add</a>
                      </div>
                    </div>

                    <div className='flex flex-col gap-1'>
                      {dto.intervals.map((a: IntervalDto, i: number) => (
                        <div key={i} className="p-3 bg-white border border-gray-200 task rounded-xl shadow-theme-sm dark:border-gray-800 dark:bg-white/5">
                          <div className="flex flex-col gap-5 xl:flex-row xl:items-center xl:justify-between">
                            <div className="flex items-start w-full gap-4">
                              <label className="w-full cursor-pointer">
                                <div className="relative flex flex-col items-start gap-3">
                                  <div className='flex justify-center items-center gap-5'>
                                    <TimeIcon fontSize={25} />
                                    <p className="-mt-0.5 text-base text-gray-800 dark:text-white/90">
                                      {a.start} - {a.end}
                                    </p>
                                  </div>
                                  <div className='flex justify-center items-center gap-5'>
                                    <CalenderIcon fontSize={25} />
                                    <p className="-mt-0.5 text-base text-gray-800 dark:text-white/90">
                                      {dayDescBuilder(a.days)}
                                    </p>
                                  </div>

                                </div>
                              </label>
                            </div>
                            <div className="flex flex-col-reverse items-start justify-end w-full gap-3 xl:flex-row xl:items-center xl:gap-5">
                              <ActionElement<IntervalDto> onEditClick={(data) => handleClickWithData(data)} onRemoveClick={(data) => handleClickWithData(data)} data={a} RemoveOnly={true} />
                            </div>
                          </div>
                        </div>
                      ))}
                    </div>
                  </>
                }


              </div>
            </div>
          </FormField>
        </div>
      </FormSection>
      <FormActions
        // disabled={isReadOnly}
        onSubmit={handleClick}
        onCancel={handleClick}
        cancelName="close"
        submitName={type == FormType.UPDATE ? "update" : "create"}
        typeLabel={type == FormType.UPDATE ? "Update" : "Create"}
      />



    </>

  )
}

export default TimeZoneForm