using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(IdentityUser user);
    }
}