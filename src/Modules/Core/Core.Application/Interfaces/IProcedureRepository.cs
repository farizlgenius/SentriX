using Core.Contract.DTOs.Procedure;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IProcedureRepository : IRepository<ProcedureDto,Procedure>
{
      
}