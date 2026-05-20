namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,int Value,string Description,bool IsTaken=false);