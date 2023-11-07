using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project.Web.Models;
using Project.Web.Service.IService;
using Project.Web.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsycn(obj);

            if (responseDto != null && responseDto.IsSucess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingrole;
            if (result != null && result.IsSucess)
            {
                obj.Role = SD.RoleCustomer;
                assingrole = await _authService.AssignRoleAsync(obj);
                if (assingrole != null && assingrole.IsSucess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            } 
            else
            {
                TempData["error"] = result.Message;
            }

            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ManageUser()
        {
            List<UserDto> list = new();

            ResponseDto? response = await _authService.GetAllUserAsync();
            if (response != null && response.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string Id)
        {
            UserDto userInfo = new UserDto();
            ResponseDto? response = await _authService.GetUserByIdAsync(Id);
            if (response != null && response.IsSucess)
            {
                userInfo = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(response.Result));

                var roleList = new List<SelectListItem>()
                {
                    new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
                    new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
                };
                ResponseDto? response2 = await _authService.GetUserRole(Id);
                if (response2 != null && response2.IsSucess && response2.Result != null) {
                    userInfo.Role = response2.Result.ToString();
                }

                ViewBag.RoleList = roleList;
            } else
            {
                TempData["error"] = response?.Message;
            }
            return View(userInfo);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserDto obj)
        {
            ResponseDto result = await _authService.UpdateUser(obj);
            if (result != null && result.IsSucess)
            {
                TempData["success"] = "Update Successful";
                return RedirectToAction(nameof(ManageUser));
            } 
            else
            {
                TempData["error"] = result.Message;
            }

            return View(obj);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin}
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assingRole;

            if(result!=null && result.IsSucess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }
                assingRole = await _authService.AssignRoleAsync(obj);
                if (assingRole!=null && assingRole.IsSucess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(ManageUser));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.RoleCustomer,Value=SD.RoleCustomer},
                new SelectListItem{Text=SD.RoleAdmin,Value=SD.RoleAdmin},
            };

            ViewBag.RoleList = roleList;
            return View(obj);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var result = await _authService.DeleteUser(Id);
            if (result !=null && result.IsSucess)
            {
                TempData["success"] = "Delete Successful";
                return RedirectToAction(nameof(ManageUser));
            } 
            else
            {
                TempData["error"] = "Delete fail";
            }
            return View(nameof(ManageUser));
        }

    }
}
