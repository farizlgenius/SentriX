using Core.Contract.DTOs.Operator;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record OperatorByUsernameQuery(string username) : IQuery<OperatorDto>;