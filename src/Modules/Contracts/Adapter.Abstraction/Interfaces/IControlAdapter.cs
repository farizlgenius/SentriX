using System;
using Output.Contract.DTOs;

namespace Adapter.Abstraction.Interfaces;

public interface IControlAdapter
{
      Task CreateAsync(CreateOutputDto dto);
}
