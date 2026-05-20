namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,int Value,string Description,int? AdditionalInfo=0,bool IsTaken=false);