using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IBase<T, CR, UP> where T : class where CR : class where UP : class
{
  Task<Pagination<T>> GetPaginationAsync(PaginationParams param);
  Task<T> GetByGuidAsync(Guid guid);
  Task<T> CreateAsync(CR dto);
  Task<T> DeleteByGuidAsync(Guid guid);
  Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<Guid> guids);
  Task<T> UpdateAsync(UP dto);
}
