namespace Core.Contract.DTOs.License;

public sealed record CreateDemoLicenseDto(string Company,string CustomerSite,string MachineId,string Session);

