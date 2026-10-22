using System.Linq.Expressions;
using Core.Contract.DTOs;
using Core.Contract.DTOs.Output;
using Core.Contract.DTOs.User;
using SharedKernel.Enums;

namespace Core.Infrastructure.Projections;

public static class UserProjections
{
    // Reusable projection expression for EF Core queries
    public static readonly Expression<Func<Persistences.Entities.User, UserDto>> ToDto = 
        x => new UserDto(
        x.guid,
        x.user_code,
        x.username ?? string.Empty,
        x.identification,
        x.title,
        x.firstname,
        x.middlename,
        x.lastname,
        x.gender,
        x.date_of_birth,
        x.email,
        x.phone,
        x.role == null ? string.Empty : x.role.name,
        x.company == null ? string.Empty : x.company.name,
        x.department == null ? string.Empty : x.department.name,
        x.position == null ? string.Empty : x.position.name,
        x.address,
        x.active_time,
        x.expire_time,
        x.additionals.Select(x => x.additional).ToList(),
        x.user_groups.Select(x => x.group.name).ToList(),
        x.cards.Select(c => new CardDto(
          c.bits,
          c.fac,
          c.card_number
        )).ToList(),
        new LicensePlateDto(x.license_plate == null ? string.Empty : x.license_plate.license_plate),
        new PinDto(x.pin == null ? string.Empty : x.pin.pin),
        new QrCodeDto(x.qr_code == null ? string.Empty : x.qr_code.qr_code),
        x.user_locations.Select(x => x.location.name).ToList(),
        x.metadata
      );

    public static readonly Expression<Func<Persistences.Entities.User,CommandDetailDto>> ToCommand = 
    x => new CommandDetailDto(
        -1,
        -1,
        "",
        "",
        -1,
        -1,
        Vendor.unknown,
        x.location_id,
        x.id,
        x.external_id
    );
}