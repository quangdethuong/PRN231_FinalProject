using Project.Web.Models;

namespace Project.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsycn(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> GetAllUserAsync();
        Task<ResponseDto?> GetUserByIdAsync(string Id);
        Task<ResponseDto?> GetUserRole(string Id);
        Task<ResponseDto?> DeleteUser(string Id);
        Task<ResponseDto?> UpdateUser(UserDto updateUser);
    }
}
