using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;

namespace ProgramServer.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly string androidClientId;
    private readonly string iosClientId;
    
    public AuthService(IRepository<User> userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        androidClientId = configuration.GetSection("AppSettings")?.GetValue<string>("AndroidClientId") ?? string.Empty;
        iosClientId = configuration.GetSection("AppSettings")?.GetValue<string>("IosClientId") ?? string.Empty;
    }
    public async Task<string> GenerateToken(string idToken)
    {
        
        var googleUser = await GoogleJsonWebSignature.ValidateAsync(idToken,
            new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new string[]{androidClientId,iosClientId}
            });

        if (googleUser is null)
            throw new UnauthorizedAccessException();
        
        var user = await _userRepository.Where(o => o.Email == googleUser.Email)
            .Include(o=>o.Role)
            .FirstOrDefaultAsync();

        if (user == null)
            throw new UnauthorizedAccessException();

        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Role, user.Role.RoleName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        var token = string.Empty;
        return token;
    }
}