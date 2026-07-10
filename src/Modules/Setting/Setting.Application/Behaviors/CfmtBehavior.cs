using Adapter.Abstraction.Interfaces;
using Device.Contract.Queries;
using Setting.Application.Interfaces;
using Setting.Contract.DTOs;
using Setting.Contract.Interfaces;
using SharedKernel.Domain;
using SharedKernel.Enums;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Setting.Application.Behaviors;

public sealed class CfmtBehavior(
      ICfmtRepository repo,
      IAdapterFactory adapter,
      IMessageBus bus 
      ) : ICardFormat
{
      public async Task<CardFormatDto> CreateAsync(CreateCardFormatDto dto, CancellationToken ct = default)
      {
            var componentId = await GetLowestComponentIdAsync(dto.LocationId,ct);
            if(componentId == -1)
                  throw new Exception();

            var domain = new Domain.Entities.CardFormat(
                  0,
                  0,
                  dto.Name,
                  dto.Fac,
                  dto.Offset,
                  dto.FunctionId,
                  dto.Flag,
                  dto.Bits,
                  dto.PeLn,
                  dto.PeLoc,
                  dto.PoLn,
                  dto.PoLoc,
                  dto.FcLn,
                  dto.FcLoc,
                  dto.ChLn,
                  dto.ChLoc,
                  dto.IcLn,
                  dto.IcLoc,
                  dto.LocationId,
                  dto.IsActive
            );

            var devices = await bus.QueryAsync(new DeviceByLocationIdQuery(dto.LocationId), ct);

            foreach(var device in devices)
            {
                  await adapter.GetAdapter(DeviceType.aero.ToString()).Setting.CardFormatConfiguration(
                        device.Mac,
                        device.ComponentId,
                        componentId,
                        domain.Fac,
                        domain.Offset,
                        domain.FunctionId,
                        domain.Flag,
                        domain.Bits,
                        domain.PeLn,
                        domain.PeLoc,
                        domain.PoLn,
                        domain.PoLoc,
                        domain.FcLn,
                        domain.FcLoc,
                        domain.ChLn,
                        domain.ChLoc,
                        domain.IcLn,
                        domain.IcLoc
                  );

                  // await adapter.GetAdapter(DeviceType.AMICO.ToString()).Setting.CardFormatConfiguration(
                  //       device.Mac,
                  //       device.ComponentId,
                  //       componentId,
                  //       domain.Offset,
                  //       domain.FunctionId,
                  //       domain.Flag,
                  //       domain.Bits,
                  //       domain.PeLn,
                  //       domain.PeLoc,
                  //       domain.PoLn,
                  //       domain.PoLoc,
                  //       domain.FcLn,
                  //       domain.FcLoc,
                  //       domain.ChLn,
                  //       domain.ChLoc,
                  //       domain.IcLn,
                  //       domain.IcLoc
                  // );
            }

            return await repo.CreateCardFormatAsync(domain, ct);
      }

      public async Task<CardFormatDto> DeleteByIdAsync(int id, CancellationToken ct = default)
      {
            var domain = await repo.GetByIdAsync(id,ct);
            var devices = await bus.QueryAsync(new DeviceByLocationIdQuery(domain.LocationId), ct);

            foreach(var device in devices)
            {
                  await adapter.GetAdapter(DeviceType.aero.ToString()).Setting.CardFormatConfiguration(
                        device.Mac,
                        device.ComponentId,
                        domain.ComponentId,
                        domain.Fac,
                        domain.Offset,
                        0,
                        domain.Flag,
                        domain.Bits,
                        domain.PeLn,
                        domain.PeLoc,
                        domain.PoLn,
                        domain.PoLoc,
                        domain.FcLn,
                        domain.FcLoc,
                        domain.ChLn,
                        domain.ChLoc,
                        domain.IcLn,
                        domain.IcLoc
                  );


            }

            return await repo.DeleteByIdAsync(id,ct);
      }

      public async Task<CardFormatDto> GetByIdAsync(int id, CancellationToken ct = default)
      {
            return await repo.GetByIdAsync(id,ct);
      }

      public async Task<Pagination<CardFormatDto>> GetCardFormatPaginationAsync(PaginationParams param, CancellationToken ct = default)
      {
            return await repo.GetCardFormatPaginationAsync(param, ct);
      }

      public async Task<CardFormatDto> UpdateAsync(CardFormatDto dto, CancellationToken ct = default)
      {
            var domain = new Domain.Entities.CardFormat(
                  dto.Id,
                  dto.ComponentId,
                  dto.Name,
                  dto.Fac,
                  dto.Offset,
                  dto.FunctionId,
                  dto.Flag,
                  dto.Bits,
                  dto.PeLn,
                  dto.PeLoc,
                  dto.PoLn,
                  dto.PoLoc,
                  dto.FcLn,
                  dto.FcLoc,
                  dto.ChLn,
                  dto.ChLoc,
                  dto.IcLn,
                  dto.IcLoc,
                  dto.LocationId,
                  dto.IsActive
            );

            var devices = await bus.QueryAsync(new DeviceByLocationIdQuery(dto.LocationId), ct);

            foreach(var device in devices)
            {
                  await adapter.GetAdapter(DeviceType.aero.ToString()).Setting.CardFormatConfiguration(
                        device.Mac,
                        device.ComponentId,
                        domain.ComponentId,
                        domain.Fac,
                        domain.Offset,
                        domain.FunctionId,
                        domain.Flag,
                        domain.Bits,
                        domain.PeLn,
                        domain.PeLoc,
                        domain.PoLn,
                        domain.PoLoc,
                        domain.FcLn,
                        domain.FcLoc,
                        domain.ChLn,
                        domain.ChLoc,
                        domain.IcLn,
                        domain.IcLoc
                  );
            }

            return await repo.UpdateAsync(domain,ct);
      }

      private async Task<short> GetLowestComponentIdAsync(int LocationId,CancellationToken ct = default)
      {
            // Implementation for getting the lowest unassigned component ID
            return await repo.GetLowestComponentIdAsync(LocationId,ct);
      }
}