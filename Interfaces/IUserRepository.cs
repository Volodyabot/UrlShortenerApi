using UrlShortener.DTOs;

namespace UrlShortener.Interfaces
{
    public interface IUserRepository
    {
        Task<LoginUserResponseDto> RegisterAsync(RegisterUserDto registerUserDto);

        Task<LoginUserResponseDto> LoginAsync(LoginUserDto loginUserDto);

        Task<bool> AssignRole(string userName, string roleName);
    }
}