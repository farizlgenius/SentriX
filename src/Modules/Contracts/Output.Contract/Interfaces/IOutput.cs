using Output.Contract.DTOs;
using SharedKernel.Domain;

public interface IOutput
{
      Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param);
      Task<OutputDto> CreateAsync(CreateOutputDto dto);
      Task<IEnumerable<short>> GetAvailalbleOutputByModuleIdAsync(int ModuleId);
      Task<IEnumerable<OptionDto>> GetRelayModeAsync(string Type);
      Task TriggerOutputAsync(int id,short Command);
      Task<OutputDto> DeleteByIdAsync(int id);
      Task<OutputDto> UpdateAsync(OutputDto dto);
      Task<IEnumerable<OptionDto>> GetRelayDriveModeAsync();
      Task<IEnumerable<OptionDto>> GetRelayOfflineModeAsync();
      Task CommandOutputDto(OutputCommandDto dto,CancellationToken ct = default);


}