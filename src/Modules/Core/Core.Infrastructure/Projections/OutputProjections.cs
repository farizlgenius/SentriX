using System.Linq.Expressions;
using Core.Contract.DTOs.Output;

namespace Core.Infrastructure.Projections;

public static class OutputProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.Output, OutputDto>> ToDto = 
        x => new OutputDto(
            x.guid,
            x.name,
            x.slot_no,
            x.metadata,
            x.vendor,
            x.device_module.device.guid,
            x.device_module.device.name,
            x.device_module.guid,
            x.device_module.name,
            x.location.guid,
            x.location.name,
            x.is_active,
            x.is_default
        );
}