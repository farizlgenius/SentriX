using Adapter.Abstraction.Interfaces;
using Adapter.Aero.Interfaces;
using AeroAdapter.Application.Interfaces;
using Output.Contract.DTOs;

namespace Adapter.Aero.Services;

public sealed class AeroMonitorService(IMpRepository repo) : IMonitorAdapter
{

}