using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using SharedKernel.Model;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Contract.Interfaces;
using Time.Domain.Entities;

namespace Time.Application.Behaviors;

public sealed class TimeBehavior(
      IHolidayRepository holRepo,
      ITimeZoneRepository repo,
      IMessageBus bus,
      IAdapterFactory factory) : ITime
{
      public async Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto)
      {
            // Generate ComponentId
            var componentId = await holRepo.GetLowestHolidayComponentIdAsync();
            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

             var domain = new Holiday(
                  Guid.NewGuid(),
                  (short)componentId,
                  dto.Name,
                  dto.Start,
                  dto.End,
                  dto.LocationId,
                  dto.IsActive
            );


            // Send Command
            foreach (var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.CreateHolidayAsync(
                              domain.Guid,
                              data.ComponentId,
                              domain.ComponentId,
                              domain.Name,
                              data.Mac,
                              domain.Start,
                              domain.End
                              );

            }     
            

            await holRepo.AddAsync(domain);

           return new HolidayDto(
                  domain.Guid,
                  domain.ComponentId,
                  domain.Name,
                  domain.Start,
                  domain.End,
                  domain.LocationId,
                  domain.IsActive,
                  domain.IsDefault
            );
      }

      public async Task<TimeZoneDto> CreateTimezoneAsync(CreateTimezoneDto dto)
      {
            if(await repo.IsAnyNameAsync(dto.Name))
                  throw new BadRequestException(MessageHelper.Common.Duplicate(nameof(dto.Name)));

            var componentId = await repo.GetLowestTimeZoneComponentIdAsync(dto.LocationId);
            var tzGuid = Guid.NewGuid();
            var interComs = new List<int>();
            foreach (var interval in dto.Intervals)
            {
                  interComs.Add(await repo.GetLowestIntervalComponentIdExceptStartFromOneAsync(interComs,tzGuid));
            }

            var intervals = dto.Intervals.Select((x,index) => new Interval(
                        Guid.NewGuid(),
                        (short)interComs.ElementAt(index),
                        new DayInWeek(
                              Guid.NewGuid(),
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday
                        ),
                        x.DaysDetail,
                        x.Start,
                        x.End
                  ));
            
            var d = new Domain.Entities.TimeZone(
                  tzGuid,
                  componentId,
                  dto.Name,
                  dto.Mode,
                  dto.Type,
                  dto.Active,
                  dto.Deactive,
                  intervals.ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );

            
            
            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.CreateTimeZoneAsync(
                              d.Guid,
                              data.ComponentId,
                              d.ComponentId,
                              d.Name,
                              data.Mac,
                              d.Mode,
                              d.Active,
                              d.Deactive,
                              d.Intervals.Select(x => new IntervalObject(
                                    x.ComponentId,
                                    DateTimeHelper.ConvertTimeToEndMinute(x.Start),
                                    DateTimeHelper.ConvertTimeToEndMinute(x.End),
                                    x.Days.Sunday,
                                    x.Days.Monday,
                                    x.Days.Tuesday,
                                    x.Days.Wednesday,
                                    x.Days.Thursday,
                                    x.Days.Friday,
                                    x.Days.Friday
                              )).ToList()
                              );

            } 

            await repo.AddAsync(d);

            return new TimeZoneDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.Mode,
                  d.Active,
                  d.Deactive,
                  d.Intervals.Select(
                        i => new IntervalDto(
                              i.Guid,
                              i.ComponentId,
                              new DaysInWeekDto(
                                    i.Days.Guid,
                                    i.Days.Sunday,
                                    i.Days.Monday,
                                    i.Days.Tuesday,
                                    i.Days.Wednesday,
                                    i.Days.Thursday,
                                    i.Days.Friday,
                                    i.Days.Saturday
                              ),
                              i.DaysDetail,
                              i.Start,
                              i.End
                        )
                  ).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );

      }

      public async Task<HolidayDto> DeleteHolidayByGuidAsync(Guid guid)
      {
            var e = await holRepo.GetByGuidAsync(guid);
            if(e.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("Holiday", guid.ToString()));

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(e.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteHolidayAsync(
                              data.ComponentId,
                              e.ComponentId,
                              data.Mac,
                              e.Start ?? default,
                              e.End ?? default
                        );
            }

            await holRepo.DeleteByGuidAsync(guid);

            return new HolidayDto(
                  e.Guid,
                  e.ComponentId,
                  e.Name,
                  e.Start,
                  e.End,
                  e.LocationId,
                  e.IsActive,
                  e.IsDefault
            );
           
      }

      public async Task<TimeZoneDto> DeleteTimeZoneByGuidAsync(Guid guid)
      {
            var d = await repo.GetByGuidAsync(guid);
            if(d.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound("TimeZone", guid.ToString()));

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(d.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteTimeZoneAsync(
                        data.Mac,
                        data.ComponentId,
                        d.ComponentId
                        );
            }

           await repo.DeleteByGuidAsync(guid);

          return new TimeZoneDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.Mode,
                  d.Active,
                  d.Deactive,
                  d.Intervals.Select(
                        i => new IntervalDto(
                              i.Guid,
                              i.ComponentId,
                              new DaysInWeekDto(
                                    i.Days.Guid,
                                    i.Days.Sunday,
                                    i.Days.Monday,
                                    i.Days.Tuesday,
                                    i.Days.Wednesday,
                                    i.Days.Thursday,
                                    i.Days.Friday,
                                    i.Days.Saturday
                              ),
                              i.DaysDetail,
                              i.Start,
                              i.End
                        )
                  ).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );

            
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(string Type)
      {
            return await factory.GetAdapter(Type).Time.GetTimezoneMode();
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneOptionByLocationIdAsync(int locationId)
      {
            return await repo.GetTimezoneOptionByLocationIdAsync(locationId);
      }

      public async Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param)
      {
            var res = await holRepo.GetPaginationAsync(param);
            return res;
      }

      public async Task<Pagination<TimeZoneDto>> TimezonePaginationAsync(PaginationParams param)
      {
            var res = await repo.GetPaginationAsync(param);
            return res;
      }

      public async Task<HolidayDto> UpdateHolidayAsync(HolidayDto dto)
      {

            var e = await holRepo.GetByGuidAsync(dto.Guid);

            if(e.Guid == Guid.Empty)
                  throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));


            var d = new Holiday(
                  dto.Guid,
                  dto.ComponentId,
                  dto.Name,
                  dto.Start ?? default,
                  dto.End ?? default,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            var d1 = new Holiday(
                  e.Guid,
                  e.ComponentId,
                  e.Name,
                  e.Start ?? default,
                  e.End ?? default,
                  e.LocationId,
                  e.IsActive,
                  e.IsDefault
            );

             var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(d.LocationId));

            var aeroData = datas.Where(x => x.Type.Equals(DeviceType.aero.ToString()));

            foreach(var data in aeroData)
            {
                  await factory.GetAdapter(data.Type).Time.DeleteHolidayAsync(
                        data.ComponentId,
                        d1.ComponentId,
                        data.Mac,
                        d1.Start,
                        d1.End
                  );
            }

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.UpdateHolidayAsync(
                        d.Guid,
                        d.Name,
                        d.ComponentId,
                        d.ComponentId,
                        data.Mac,
                        d.Start,
                        d.End
                  );
            }

            await holRepo.UpdateAsync(d);

            return new HolidayDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.Start,
                  d.End,
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );
      }

      public async Task<TimeZoneDto> UpdateTimezoneAsync(TimeZoneDto dto)
      {
            var tz = await repo.GetByGuidAsync(dto.Guid);

            if(tz.Guid == Guid.Empty)
                   throw new BadRequestException(MessageHelper.Common.NotFound(nameof(dto.Guid),dto.Guid.ToString()));

            
            var d = new Domain.Entities.TimeZone(
                  Guid.NewGuid(),
                  dto.ComponentId,
                  dto.Name,
                  dto.Mode,
                  dto.Type,
                  dto.Active,
                  dto.Deactive,
                  dto.Intervals.Select(x => new Interval(
                        Guid.NewGuid(),
                        (short)x.ComponentId,
                        new DayInWeek(
                              Guid.NewGuid(),
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday
                        ),
                        x.DaysDetail,
                        x.Start,
                        x.End
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.UpdateTimeZoneAsync(
                              d.Guid,
                              data.ComponentId,
                              d.ComponentId,
                              d.Name,
                              data.Mac,
                              d.Mode,
                              d.Active,
                              d.Deactive,
                              d.Intervals.Select(x => new IntervalObject(
                                    x.ComponentId,
                                    DateTimeHelper.ConvertTimeToEndMinute(x.Start),
                                    DateTimeHelper.ConvertTimeToEndMinute(x.End),
                                    x.Days.Sunday,
                                    x.Days.Monday,
                                    x.Days.Tuesday,
                                    x.Days.Wednesday,
                                    x.Days.Thursday,
                                    x.Days.Friday,
                                    x.Days.Friday
                              )).ToList()
                              );

            } 


            await repo.UpdateAsync(d);


            return new TimeZoneDto(
                  d.Guid,
                  d.ComponentId,
                  d.Name,
                  d.Mode,
                  d.Active,
                  d.Deactive,
                  d.Intervals.Select(
                        i => new IntervalDto(
                              i.Guid,
                              i.ComponentId,
                              new DaysInWeekDto(
                                    i.Days.Guid,
                                    i.Days.Sunday,
                                    i.Days.Monday,
                                    i.Days.Tuesday,
                                    i.Days.Wednesday,
                                    i.Days.Thursday,
                                    i.Days.Friday,
                                    i.Days.Saturday
                              ),
                              i.DaysDetail,
                              i.Start,
                              i.End
                        )
                  ).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );




      }
}