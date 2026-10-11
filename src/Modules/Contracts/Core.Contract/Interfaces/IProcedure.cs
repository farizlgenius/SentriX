using Core.Contract.DTOs.Position;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IProcedure : IBase<PositionDto,CreatePositionDto,UpdatePositionDto>
{
      
}