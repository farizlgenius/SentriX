using Setting.Contract.DTOs.Setting;
using SharedKernel.Messaging;

namespace Setting.Contract.Queries;

public sealed record AeroDriverSettingQuery : IQuery<AeroDriverSettingDto>;