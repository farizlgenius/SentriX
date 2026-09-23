using Setting.Application.Interfaces;
using Setting.Contract.DTOs.Setting;
using Setting.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Exceptions;

namespace Setting.Infrastructure.Repositories;

public sealed class SettingRepository(SettingDbContext context) : ISettingRepository
{
  public async Task<AeroDriverSettingDto> GetAeroDriverSettingAsync(CancellationToken ct = default)
  {
    return await context.AeroDriverSettings
      .AsNoTracking()
      .Select(x => new AeroDriverSettingDto(
        x.guid,
        x.n_port,
        x.n_scps,
        x.c_type,
        x.c_port,
        x.n_msp1_port,
        x.n_trasaction,
        x.n_sio,
        x.n_mp,
        x.n_cp,
        x.n_acr,
        x.n_alvl,
        x.n_trgr,
        x.n_proc,
        x.gmt_offset,
        x.is_daylight_saving,
        x.n_tz,
        x.n_hol,
        x.n_mpg,
        x.n_tran_limit,
        x.n_cards,
        x.n_alvl_per_card,
        x.pin_duress_mode,
        x.duress_const_digit,
        x.card_id_size,
        x.pin_digit,
        x.issue_code_bit,
        x.apb_location,
        x.store_act_date,
        x.store_deact_date,
        x.used_limit,
        x.apb_time,
        x.host_timeout,
        x.escort_timeout,
        x.multi_card_timeout,
        x.max_elalvl,
        x.max_floor_per_acr
      ))
      .FirstOrDefaultAsync(ct) ?? throw new NotFoundException($"Aero Driver Setting.");
  }
}