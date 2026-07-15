namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,int Value,string Description,Guid? AdditionalInfo=default,bool IsTaken=false);