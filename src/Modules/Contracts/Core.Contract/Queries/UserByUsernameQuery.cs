using Core.Contract.DTOs.User;
using SharedKernel.Messaging;

namespace Core.Contract.Queries;

public sealed record UserByUsernameQuery(string username) : IQuery<UserDto>;