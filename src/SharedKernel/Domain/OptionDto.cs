namespace SharedKernel.Domain;

public sealed record OptionDto(string Label,object Value,object Description,Guid? AdditionalInfo=default,bool IsTaken=false);