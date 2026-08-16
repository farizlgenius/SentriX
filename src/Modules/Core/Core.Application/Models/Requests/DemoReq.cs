namespace Core.Application.Models.Requests;

public sealed record DemoReq(
      string Customer,
      string EndUser,
      string MachineId
);