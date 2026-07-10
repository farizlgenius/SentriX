namespace Adapter.Amico.Model.Request;

public sealed record CreateObjectRequest<TObject>(string Object,List<TObject> Values);