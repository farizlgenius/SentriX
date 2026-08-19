namespace Core.Application.Models.Requests;

public sealed record DemoHttpRequest(
      string BackendId,
      string Customer,
      string EndUser,
      string MachineId,
      string Product,
      string EcdsaPublicKey,
      string EcdhPulickey
);