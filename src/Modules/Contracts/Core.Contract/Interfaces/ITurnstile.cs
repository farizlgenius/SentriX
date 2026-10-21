using Core.Contract.DTOs.Turnstile;

namespace Core.Contract.Interfaces;

public interface ITurnstile : IService<TurnstileDto, CreateTurnstileDto, UpdateTurnstileDto>
{
      Task UploadAsync(CancellationToken ct = default);
}