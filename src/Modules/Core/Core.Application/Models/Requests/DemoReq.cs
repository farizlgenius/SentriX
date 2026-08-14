namespace Core.Application.Models.Requests;

public sealed record DemoReq(
      string Company,
      string CustomerSite,
      string MachineId,
      string SessionId
);