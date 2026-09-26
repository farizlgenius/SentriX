namespace SharedKernel.Domain;

public sealed class PropertyDiff
{
    public object? Old { get; set; }
    public object? New { get; set; }

    public PropertyDiff() { }

    public PropertyDiff(object? oldVal, object? newVal)
    {
        Old = oldVal;
        New = newVal;
    }
}

public sealed class EntityDiffResponse
{
    public string EntityName { get; set; } = string.Empty;
    public string EntityState { get; set; } = string.Empty;
    public Dictionary<string, PropertyDiff> Changes { get; set; } = new();
}