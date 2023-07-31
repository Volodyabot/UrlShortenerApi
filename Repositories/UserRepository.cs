using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.DTOs;
using UrlShortener.Interfaces;

namespace UrlShortener.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ITokenService _tokenService;

        public UserRepository(DataContext dataContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<LoginUserResponseDto> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var user = new IdentityUser
            {
                UserName = registerUserDto.Username,
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                var roleResult = await _userManager.AddToRoleAsync(user, "User");

                if (!roleResult.Succeeded) throw new Exception("Role not added");

                return new LoginUserResponseDto { UserName = user.UserName, Token = await _tokenService.CreateToken(user) };
            }
            else
            {
                var list = result.Errors.ToList();
                string longErrorsDescription = "";
                foreach (var identityError in list)
                {
                    longErrorsDescription += $"{identityError.Description} ";
                }
                throw new Exception(longErrorsDescription);
            }
        }

        public async Task<LoginUserResponseDto> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginUserDto.Username.ToLower());

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
            {
                return new LoginUserResponseDto { UserName = user.UserName!, Token = await _tokenService.CreateToken(user) };
            }

            throw new Exception("Incorrect password");
        }

        public async Task<bool> AssignRole(string userName, string roleName)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());

            if (user == null)
            {
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
    }
}