using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Contract.Interfaces;
using Time.Domain.Entities;

namespace Time.Application.Behaviors;

public sealed class TimeBehavior(
      IHolidayRepository holRepo,
      ITimezoneRepository timezoneRepo,
      IMessageBus bus,
      IAdapterFactory factory) : ITime
{
      public async Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto)
      {
            // Generate ComponentId
            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

             var domain = new Holiday(
                  0,
                  0,
                  dto.Name,
                  dto.Year,
                  dto.Month,
                  dto.Day,
                  dto.LocationId,
                  dto.IsActive
                  );


            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.CreateHolidayAsync(
                              data.Mac,
                              data.ComponentId,
                              domain.Year,
                              domain.Month,
                              domain.Day,
                              domain.Metadata
                              );

            }      
            


            var res = await holRepo.CreateHolidayAsync(domain);

            return res;
      }

      public async Task<TimezoneDto> CreateTimezoneAsync(CreateTimezoneDto dto)
      {
            var componentId = await timezoneRepo.GetLowestTimezoneComponentIdAsync();
            var domain = new Timezone(
                  0,
                  componentId,
                  dto.Name,
                  dto.Mode,
                  dto.Type,
                  dto.Active,
                  dto.Deactive,
                  dto.Intervals.Select(x => new Interval(
                        x.Id,
                        0,
                        x.LocationId,
                        x.IsActive,
                        new DayInWeek(
                              0,
                              x.Days.Sunday,
                              x.Days.Monday,
                              x.Days.Tuesday,
                              x.Days.Wednesday,
                              x.Days.Thursday,
                              x.Days.Friday,
                              x.Days.Saturday,
                              0,
                              x.LocationId,
                              x.IsActive
                              ),
                        x.DaysDetail,
                        x.Start,
                        x.End,
                        x.Type
                        )).ToList(),
                  dto.LocationId,
                  dto.IsActive
                  );
            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

            // Send Command
            foreach(var data in datas)
            {
                  await factory.GetAdapter(data.Type).Time.CreateTimezoneAsync(
                              data.Mac,
                              data.ComponentId,
                              domain.ComponentId,
                              domain.Mode,
                              domain.Active,
                              domain.Deactive,
                              dto.Intervals
                              );

            } 

            return await timezoneRepo.CreateAsync(domain);

      }

      public async Task<HolidayDto> DeleteHolidayAsync(int id)
      {
            var entity = await holRepo.GetByIdAsync(id);
            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Time.HolidayNotFound(id));

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(entity.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteHoliday(
                        data.Mac,
                        data.ComponentId,
                        entity.Year,
                        entity.Month,
                        entity.Day,
                        entity.Metadata
                        );
            }

            return await holRepo.DeleteByIdAsync(id);
           
      }

      public async Task<TimezoneDto> DeleteTimezoneAsync(int id)
      {
            var entity = await timezoneRepo.GetByIdAsync(id);
            if(entity.Id == 0)
                  throw new BadRequestException(MessageHelper.Time.TimezoneNotFound(id));

            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(entity.LocationId));

            foreach(var data in datas)
            {
                   await factory.GetAdapter(data.Type).Time.DeleteTimezone(
                        data.Mac,
                        data.ComponentId,
                        entity.ComponentId
                        );
            }

            return await timezoneRepo.DeleteByIdAsync(id);

            
      }

      public async Task<IEnumerable<OptionDto>> GetTimezoneModeAsync(string Type)
      {
            return await factory.GetAdapter(Type).Time.GetTimezoneMode();
      }

      public async Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param)
      {
            var res = await holRepo.GetPaginationAsync(param);
            return res;
      }

      public async Task<Pagination<TimezoneDto>> TimezonePaginationAsync(PaginationParams param)
      {
            var res = await timezoneRepo.GetPaginationAsync(param);
            return res;
      }

      public Task<HolidayDto> UpdateHolidayAsync(HolidayDto dto)
      {
            throw new NotImplementedException();
      }

      public Task<TimezoneDto> UpdateTimezoneAsync(TimezoneDto dto)
      {
            throw new NotImplementedException();
      }
}