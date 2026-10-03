using Core.Contract.DTOs.CardFormat;
using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface ICardFormatRepository : IBaseRepository<CardFormatDto,CardFormat>
{
      
}