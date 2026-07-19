namespace SharedKernel.Domain;

public record BaseDtoEntity(Guid Guid,int LocationId,string Type,bool IsActive,bool IsDefault);