using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Infra.Services
{
    public class AuthService :IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IConfiguration configuration,
            IDistributedCache cache,
            ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
        }

         public async Task<string> AuthenticateAsync(string username, string password)
    {
        // Validation et génération du JWT
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            // Ajouter les claims des rôles et permissions
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Mise en cache du token
        await _cache.SetStringAsync(
            $"token:{username}",
            tokenString,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

        return tokenString;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
    {
        var cacheKey = $"permissions:{userId}";
        
        // Tenter de récupérer depuis le cache
        var permissions = await _cache.GetAsync(cacheKey);
        if (permissions != null)
        {
            return JsonSerializer.Deserialize<IEnumerable<string>>(permissions);
        }

        // Si non trouvé, récupérer de la base de données et mettre en cache
        var userPermissions = new List<string>(); // À implémenter avec votre logique de base de données
        
        await _cache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(userPermissions),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

        return userPermissions;
    }

    }
}