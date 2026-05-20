using Output.Contract.DTOs;
using SharedKernel.Domain;

public interface IOutput
{
      Task<Pagination<OutputDto>> GetPaginationAsync(PaginationParams param);
      Task<OutputDto> CreateAsync(CreateOutputDto dto);
      Task<IEnumerable<short>> GetAvailalbleOutputByModuleIdAsync(int ModuleId);
      Task<IEnumerable<OptionDto>> GetRelayModeAsync(string Type);
      Task<BaseResponse> TriggerOutputAsync(int id,short Command);

}