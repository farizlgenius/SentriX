using System;

namespace Device.Contract.DTOs;

public sealed record IdReportDto(int ScpId, string SerialNumber, string Mac, string Ip, int Port, string Fw);