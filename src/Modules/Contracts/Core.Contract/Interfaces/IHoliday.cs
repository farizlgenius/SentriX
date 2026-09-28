using Core.Contract.DTOs.Time;

namespace Core.Contract.Interfaces;

public interface IHoliday : IBase<HolidayDto, CreateHolidayDto, UpdateHolidayDto>
{
      Task UploadAsync(CancellationToken ct = default);
}