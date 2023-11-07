﻿using Microsoft.AspNetCore.Identity;
using Project.Services.AuthAPI.Data;
using Project.Services.AuthAPI.Model;
using Project.Services.AuthAPI.Model.Dto;
using Project.Services.AuthAPI.Service.IService;

namespace Project.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            
            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;

        }
        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email,
                Name = registrationRequestDto.Name.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            } catch (Exception ex)
            {

            }
            return "Error Encounter";
        }
        public async Task<string> UpdateUser(UserDto updateUser)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(updateUser.ID);
            user.UserName = updateUser.Email;
            user.Email = updateUser.Email;
            user.NormalizedEmail = updateUser.Email;
            user.Name = updateUser.Name.ToUpper();
            user.PhoneNumber = updateUser.PhoneNumber;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(updateUser.Role).GetAwaiter().GetResult())
                    {
                        _roleManager.CreateAsync(new IdentityRole(updateUser.Role)).GetAwaiter().GetResult();
                    }
                    var roles = await _userManager.GetRolesAsync(user);

                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    await _userManager.AddToRoleAsync(user, updateUser.Role);
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            } catch (Exception ex)
            {

            }
            return "Error Encounter";
        }
    }
}
