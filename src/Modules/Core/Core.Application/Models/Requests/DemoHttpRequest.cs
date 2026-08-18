namespace Core.Application.Models.Requests;

public sealed record DemoHttpRequest(
      string Customer,
      string EndUser,
      string MachineId,
      string Product
);