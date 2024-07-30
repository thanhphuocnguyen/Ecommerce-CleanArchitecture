using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IPermissionService _permissionService;

    public JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
    {
        _jwtOptions = options.Value;
        _permissionService = permissionService;
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        HashSet<string> permissions = await _permissionService.GetPermissionsForUser(user.Id);

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permissions", permission));
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.Now.AddHours(36),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}