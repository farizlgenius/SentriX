namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,long Value,string Description,Guid? AdditionalInfo=default,bool IsTaken=false);