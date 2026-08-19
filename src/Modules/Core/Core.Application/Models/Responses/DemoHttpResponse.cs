namespace Core.Application.Models.Responses;

public sealed class DemoHttpResponse
{
  public string CipherText { get; set; } = string.Empty;
  public string EcdhPublicKey { get; set; } = string.Empty;
  public string EcdsaPublicKey { get; set; } = string.Empty;
  public string Signature { get; set; } = string.Empty;
}