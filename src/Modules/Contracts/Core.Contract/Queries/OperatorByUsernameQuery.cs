using Core.Contract.DTOs.Operator;
using Core.Contract.DTOs.User;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record OperatorByUsernameQuery(string username) : IQuery<OperatorDto>;