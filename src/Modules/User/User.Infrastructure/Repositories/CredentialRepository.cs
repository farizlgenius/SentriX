using Microsoft.EntityFrameworkCore;
using SharedKernel.Helpers;
using User.Application.Interfaces;
using User.Infrastructure.Persistences;

namespace User.Infrastructure.Repositories;

public sealed class CredentialRepository(UserDbContext context) : ICredentialRepository
{

      public async Task<bool> IsAnyCardNumberAsync(int card, CancellationToken ct = default)
      {
            return await context.Cards.AsNoTracking().AnyAsync(x => x.card_number == card,ct);
      }

      public async Task<bool> IsAnyLicensePlateAsync(string license, CancellationToken ct = default)
      {
           return await context.LicensePlates.AsNoTracking().AnyAsync(x => x.license_plate.Equals(license),ct);
      }

      public async Task<bool> IsAnyPinAsync(string pin, CancellationToken ct = default)
      {
             return await context.Pins.AsNoTracking().AnyAsync(x => x.pin.Equals(pin),ct);
      }

      public async Task<bool> IsAnyQrCodeAsync(string qr, CancellationToken ct = default)
      {
             return await context.QrCodes.AsNoTracking().AnyAsync(x => x.qr_code.Equals(qr),ct);
      }
}