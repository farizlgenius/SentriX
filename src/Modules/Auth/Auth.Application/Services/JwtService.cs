using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Application.Interfaces;
using Auth.Contract.DTOs;
using Auth.Domain.Enums;
using Cache.Contract.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Operator.Contract.DTOs;
using Operator.Contract.Queries;
using SharedKernel.Helpers;
using SharedKernel.Messaging;


namespace Auth.Application.Services;

public sealed class JwtService(IRefreshTokenAuditRepository repo, IJwtSetting settings, ICache redis, IMessageBus bus) : IJwt
{
      private readonly string _secretKey = settings.Secret;
      private readonly string _issuer = settings.Issuer;
      private readonly string _audience = settings.Audience;
      private readonly short _accessTokenMinutes = settings.AccessTokenMinutes;
      private readonly short _refreshTokenDays = settings.RefreshTokenDays;
      private readonly TimeSpan _ttl = TimeSpan.FromDays(settings.RefreshTokenDays);
      public async Task<AccessTokenDto> GenerateTokenAsync(OperatorDto user)
      {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var locations = await bus.QueryAsync(new LocationListByUsernameQuery(user.Username));

            var claims = new[]
            {
                  // Adding data to token claims (for demonstration, only username is added)
                  new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                  // Authorized
                  new Claim("role_id",user.RoleId.ToString()),
                  new Claim("tenants",string.Join(",", locations.Select(l => l))),

            };

            var token = new JwtSecurityToken(
              issuer: _issuer,
              audience: _audience,
              claims: claims,
              notBefore: now,
              expires: now.AddMinutes(_accessTokenMinutes),
              signingCredentials: creds
            );

            // create random refresh token and store hashed in redis + audit in DB
            var rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Store Token in Cache
            var hashedRefresh = TokenHasher.Hash(rawRefresh);
            Console.WriteLine(hashedRefresh);
            var json = new RefreshTokenDto
            (hashedRefresh, user.Username, TokenAction.CREATE.ToString(), now.AddDays(_refreshTokenDays));

            await redis.SetAsync(
              hashedRefresh,
              json,
              _ttl
            );



            // Save Refresh Token in Redis with expiration (not implemented here, just a placeholder)
            await repo.AddAsync(user.Username, hashedRefresh, TokenAction.CREATE.ToString(), now.AddDays(_refreshTokenDays));

            return new AccessTokenDto(
              AccessToken: new JwtSecurityTokenHandler().WriteToken(token),
              RefreshToken: rawRefresh,
              ExpiredAt: _accessTokenMinutes,
              RefreshExpiredAt: now.AddDays(_refreshTokenDays)
            );
      }

      public async Task<AccessTokenDto> RefreshTokenAsync(OperatorDto user)
      {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var locations = await bus.QueryAsync(new LocationListByUsernameQuery(user.Username));

            var claims = new[]
            {

      // Adding data to token claims (for demonstration, only username is added)
      new Claim(JwtRegisteredClaimNames.Sub, user.Username),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

      // Authorized
      new Claim("role_id",user.RoleId.ToString()),
      new Claim("tenants",string.Join(",", locations.Select(l => l))),

    };

            var res = new JwtSecurityToken(
              issuer: _issuer,
              audience: _audience,
              claims: claims,
              notBefore: now,
              expires: now.AddMinutes(_accessTokenMinutes),
              signingCredentials: creds
            );

            // create random refresh token and store hashed in redis + audit in DB
            var rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // Save Refresh Token in Redis with expiration (not implemented here, just a placeholder)
            await repo.AddAsync(user.Username, Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(rawRefresh))), TokenAction.ROTATE.ToString(), now.AddDays(_refreshTokenDays));

            return new AccessTokenDto(
              AccessToken: new JwtSecurityTokenHandler().WriteToken(res),
              RefreshToken: rawRefresh,
              ExpiredAt: _accessTokenMinutes,
              RefreshExpiredAt: now.AddDays(_refreshTokenDays)
            );

      }

      public async Task<RefreshTokenDto> GetRefreshTokenAsync(string hashed)
      {
            var res = await repo.GetRefreshTokenAsync(hashed);
            return new RefreshTokenDto(res.HashedToken, res.Username, res.Action, res.ExpiredAt);
      }

      public async Task<bool> RevokeTokenAsync(string refreshToken)
      {
            var hash = TokenHasher.Hash(refreshToken);
            await repo.AddAsync(string.Empty, hash, TokenAction.REVOKE.ToString(), DateTime.UtcNow);

            // Remove from redis

            return true;
      }
}
