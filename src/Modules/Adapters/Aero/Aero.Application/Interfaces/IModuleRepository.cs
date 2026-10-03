using SharedKernel.Model;

namespace Aero.Application.Interfaces;

public interface IModuleRepository
{
      CommandResponse EnCcSioSrq(string mac,
            short scpId,
            short sioNo,
            CancellationToken ct = default);
}