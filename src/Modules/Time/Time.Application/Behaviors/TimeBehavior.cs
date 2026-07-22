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
            var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(dto.LocationId));

             var domain = new Holiday(
                  Guid.NewGuid(),
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
                              data.Guid,
                              domain.Name,
                              domain.Start,
                              domain.End
                              );

            }     
            

            await holRepo.AddAsync(domain);

           return new HolidayDto(
                  domain.Guid,
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

            var tzGuid = Guid.NewGuid();

            var intervals = dto.Intervals.Select(x => new Interval(
                        Guid.NewGuid(),
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
                        x.Start,
                        x.End
                  ));
            
            var d = new Domain.Entities.TimeZone(
                  tzGuid,
                  dto.Name,
                  intervals.ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
                  );

            
            
            var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(dto.LocationId));

            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.CreateTimeZoneAsync(
                              data.Guid,
                              d.Guid,
                              d.Name,
                              d.Intervals.Select(x => new IntervalObject(
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
                  d.Name,
                  d.Intervals.Select(
                        i => new IntervalDto(
                              i.Guid,
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

            var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(e.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteHolidayAsync(
                              data.Guid,
                              e.Start ?? default,
                              e.End ?? default
                        );
            }

            await holRepo.DeleteByGuidAsync(guid);

            return new HolidayDto(
                  e.Guid,
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

            var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(d.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteTimeZoneAsync(
                        data.Guid,
                        guid,
                        d.Intervals.Select(x => (short)x.ComponentId).ToList()
                        );
            }

           await repo.DeleteByGuidAsync(guid);

          return new TimeZoneDto(
                  d.Guid,
                  d.Name,
                  d.Intervals.Select(
                        i => new IntervalDto(
                              i.Guid,
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
                              i.Start,
                              i.End
                        )
                  ).ToList(),
                  d.LocationId,
                  d.IsActive,
                  d.IsDefault
            );

            
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
                  dto.Name,
                  dto.Start ?? default,
                  dto.End ?? default,
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            var d1 = new Holiday(
                  e.Guid,
                  e.Name,
                  e.Start ?? default,
                  e.End ?? default,
                  e.LocationId,
                  e.IsActive,
                  e.IsDefault
            );

             var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(d.LocationId));

            var aeroData = datas.Where(x => x.Type.Equals(DeviceType.aero.ToString()));

            foreach(var data in aeroData)
            {
                  await factory.GetAdapter(data.Type).Time.DeleteHolidayAsync(
                        data.Guid,
                        d1.Start,
                        d1.End
                  );
            }

            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.UpdateHolidayAsync(
                        data.Guid,
                        d.Name,
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
                        x.Start,
                        x.End
                  )).ToList(),
                  dto.LocationId,
                  dto.IsActive,
                  dto.IsDefault
            );

            var datas = await bus.QueryAsync(new GuidAndTypeByLocationIdQuery(dto.LocationId));

            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.UpdateTimeZoneAsync(
                              d.Guid,
                              data.ComponentId,
                              d.ComponentId,
                              d.Name,
                              data.Mac,
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