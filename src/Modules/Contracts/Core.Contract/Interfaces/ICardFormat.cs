
using Core.Contract.DTOs.CardFormat;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface ICardFormat : IBase<CardFormatDto,CreateCardFormatDto,UpdateCardFormatDto>
{
      
}