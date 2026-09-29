namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,object Value,string Description,Guid? AdditionalInfo=default,bool IsTaken=false);