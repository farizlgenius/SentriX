namespace SharedKernel.Domain;

public record BaseDtoEntity(Guid Guid,short ComponentId,int LocationId,string Type,bool IsActive,bool IsDefault);