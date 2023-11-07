using Project.Web.Models;
using Project.Web.Service.IService;
using Project.Web.Utility;

namespace Project.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/assignRole"
            }, withBearer:false);
        }

        public async Task<ResponseDto?> LoginAsycn(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
        public async Task<ResponseDto?> GetAllUserAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/getUser"
            }, withBearer: false);
        }
        public async Task<ResponseDto?> GetUserByIdAsync(string Id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/getUser/" + Id
            }, withBearer: false);
        }
        public async Task<ResponseDto?> UpdateUser(UserDto updateUser)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = updateUser,
                Url = SD.AuthAPIBase + "/api/auth/update"
            }, withBearer: false);
        }
        public async Task<ResponseDto?> GetUserRole(string Id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.AuthAPIBase + "/api/auth/getRole/" + Id
            }, withBearer: false);
        }
        public async Task<ResponseDto?> DeleteUser(string Id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.AuthAPIBase + "/api/auth/delete/" + Id
            }, withBearer: false);
        }
    }
}
