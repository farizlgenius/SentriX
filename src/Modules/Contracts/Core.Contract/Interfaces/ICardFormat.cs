
using Core.Contract.DTOs.CardFormat;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ICardFormat : IService<CardFormatDto,CreateCardFormatDto,UpdateCardFormatDto>
{
      
}