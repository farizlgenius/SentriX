using Adapter.Abstraction.Constants;
using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Messaging;
using Time.Application.Interfaces;
using Time.Contract.DTOs;
using Time.Contract.Interfaces;
using Time.Domain.Entities;

namespace Time.Application.Behaviors;

public sealed class TimeBehavior(IHolidayRepository holRepo,IMessageBus bus,IAdapterFactory factory) : ITime
{
      public async Task<HolidayDto> CreateHolidayAsync(CreateHolidayDto dto)
      {
            // Generate ComponentId
            var datas = await bus.QueryAsync(new MacAndComponentIdListByLocationIdQuery(dto.LocationId));

            // Send Command
            if(dto.Type == Venders.AERO)
            {
                  foreach(var data in datas)
                  {
                        await factory.GetAdapter(Venders.AERO).Time.CreateHolidayAsync(
                        data.Mac,
                        data.ComponentId,
                        dto.Year,
                        dto.Month,
                        dto.Day,
                        dto.Metadata
                        );
                  }
                  
            }
            else
            {
                  // factory.GetAdapter(Venders.AMICO).Time.CreateHolidayAsync();
            }
            

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

            var res = await holRepo.CreateHolidayAsync(domain);

            return res;
      }

      public async Task<Pagination<HolidayDto>> HolidayPaginationAsync(PaginationParams param)
      {
            var res = await holRepo.HolidayPaginationAsync(param);
            return res;
      }
}