namespace User.Application.Interfaces;

public interface ICredentialRepository
{
      Task<bool> IsAnyCardNumberAsync(int card,CancellationToken ct = default);
      Task<bool> IsAnyLicensePlateAsync(string license,CancellationToken ct = default);
      Task<bool> IsAnyPinAsync(string pin,CancellationToken ct = default);
      Task<bool> IsAnyQrCodeAsync(string qr,CancellationToken ct = default);
}