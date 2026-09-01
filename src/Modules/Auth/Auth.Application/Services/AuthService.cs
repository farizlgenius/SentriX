using System;
using Auth.Application.Interfaces;
using Auth.Contract.DTOs;
using Auth.Contract.Interfaces;
using Auth.Domain.Enums;
using Cache.Contract.Interfaces;
using Core.Contract.Queries;
using SharedKernel.Domain;
using SharedKernel.Exceptions;
using SharedKernel.Helpers;
using SharedKernel.Messaging;

namespace Auth.Application.Behaviors;

public sealed class AuthService(IJwt jwt, ICache redis, IRefreshTokenAuditRepository repo, IMessageBus bus) : IAuth
{
  public async Task<MeDto> GetMeByUsernameAndRoleGuidAsync(string username, Guid roleGuid)
  {
    var locations = await bus.QueryAsync(new LocationGuidByUsernameQuery(username));
    var permissions = await bus.QueryAsync(new PermissionByRoleGuidQuery(roleGuid));
    var user = await bus.QueryAsync(new UserByUsernameQuery(username));
    return new MeDto(user.Guid, user.Username, locations, permissions);
  }

  public async Task<AccessTokenDto> LoginAsync(LoginDto login)
  {
    //Check username is empty
    if (string.IsNullOrEmpty(login.Username))
      throw new BadRequestException(MessageHelper.Auth.UsernameCannotBeEmpty);

    //Check password is empty
    if (string.IsNullOrEmpty(login.Password))
      throw new BadRequestException(MessageHelper.Auth.PasswordCannotBeEmpty);

    // Check username existence
    var userExists = await bus.QueryAsync(new IsAnyUsernameQuery(login.Username));
    if (!userExists)
      throw new BadRequestException(MessageHelper.Auth.UserNotFound);

    // Validate Password
    var pass = await bus.QueryAsync(new HashedByUsernameQuery(login.Username));
    var isValidPassword = PasswordHasher.VerifyPassword(login.Password, pass);
    if (!isValidPassword)
      throw new BadRequestException(MessageHelper.Auth.InvalidCredentials);

    // Get User
    var user = await bus.QueryAsync(new UserByUsernameQuery(login.Username));

    // Generate token (for demonstration, using a simple string)
    var token = await jwt.GenerateTokenAsync(user);

    return new AccessTokenDto(
      token.AccessToken,
      token.RefreshToken,
      token.ExpiredAt,
      token.RefreshExpiredAt
      );
  }

  public async Task<string> LogoutAsync(string refreshToken)
  {
    var hashed = TokenHasher.Hash(refreshToken);
    var refresh = await jwt.GetRefreshTokenAsync(hashed);

    // Validate token
    if (string.IsNullOrWhiteSpace(refresh.HashedToken))
      throw new NotFoundException(MessageHelper.Auth.RefreshTokenNotFound);

    if (refresh.ExpiredAt < DateTime.UtcNow)
      throw new BadRequestException(MessageHelper.Auth.RefreshExpired);

    if (refresh.Action.Equals(TokenAction.REVOKE))
      throw new BadRequestException(MessageHelper.Auth.RefreshTokenInvalid);

    await jwt.RevokeTokenAsync(refreshToken);

    return "Logout success";

  }

  public async Task<AccessTokenDto> RefreshTokenAsync(string refreshToken)
  {
    var inCommingHashed = TokenHasher.Hash(refreshToken);

    // Get from redis first
    //...
    var refresh = await redis.GetAsync<RefreshTokenDto>(inCommingHashed);
    if (refresh == null)
      refresh = await repo.GetRefreshTokenAsync(inCommingHashed);

    // Validate token
    if (string.IsNullOrWhiteSpace(refresh.HashedToken))
      throw new NotFoundException(MessageHelper.Auth.RefreshTokenNotFound);

    if (refresh.ExpiredAt < DateTime.UtcNow)
      throw new BadRequestException(MessageHelper.Auth.RefreshExpired);

    if (refresh.Action.Equals(TokenAction.REVOKE))
      throw new BadRequestException(MessageHelper.Auth.RefreshTokenInvalid);

    // Generate token (for demonstration, using a simple string)
    var user = await bus.QueryAsync(new UserByUsernameQuery(refresh.Username));
    var token = await jwt.RefreshTokenAsync(user);



    return new AccessTokenDto(
      token.AccessToken,
      token.RefreshToken,
      token.ExpiredAt,
      token.RefreshExpiredAt
      );
  }
}
