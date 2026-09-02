using Core.Contract.DTOs.Operator;
using SharedKernel.Domain;

namespace Core.Contract.Interfaces;

public interface IOperator : IBase<OperatorDto, CreateOperatorDto, UpdateOperatorDto>
{
      Task<string> GetHashedPasswordByUsernameAsync(string username, CancellationToken ct = default);
      Task<OperatorDto> GetOperatorByUsernameAsync(string username, CancellationToken ct = default);
      Task<Stream> GetImageByGuidAsync(Guid guid, CancellationToken ct = default);
      Task<bool> UploadImageAsync(Guid guid, Stream stream, CancellationToken ct = default);
}