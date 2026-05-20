

namespace Adapter.Abstraction.Interfaces;

public interface ITimeAdapter
{
     Task CreateHolidayAsync(
       string Mac,
            short ScpId,
            short Year,
            short Month,
            short Day,
            string Metadata
     );  
}