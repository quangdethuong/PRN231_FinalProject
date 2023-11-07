using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.MessageBus;
using Project.Services.AuthAPI.Model;
using Project.Services.AuthAPI.Model.Dto;
using Project.Services.AuthAPI.Service.IService;

namespace Project.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _response;
        // private IMapper _mapper;
        public AuthAPIController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _messageBus = messageBus;
            _configuration = configuration;
            _response = new();
            _userManager = userManager;
            // _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue"));
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        [HttpGet("getUser")]
        public IActionResult GetAllUser()
        {
            try
            {
                IEnumerable<ApplicationUser> objList = _userManager.Users;
                _response.Result = objList;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }
        [HttpGet("getUser/{Id}")]
        public IActionResult GetUserByIdAsync(string Id)
        {
            try
            {
                IEnumerable<ApplicationUser> objList = _userManager.Users;
                _response.Result = objList.FirstOrDefault(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }
        [HttpGet("getRole/{Id}")]
        public async Task<IActionResult> GetUserRole(string Id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(Id);
                var roles = await _userManager.GetRolesAsync(user);
                _response.Result = roles[0];
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return Ok(_response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto model)
        {
            var errorMessage = await _authService.UpdateUser(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpDelete("delete/{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded) {
                return Ok(_response);
            } else {
                _response.IsSuccess = false;
                _response.Message = "Delete fail";
                return BadRequest(_response);
            }
        }
    }
}
