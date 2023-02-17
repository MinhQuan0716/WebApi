using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.ViewModels.TokenModels;
using Microsoft.IdentityModel.Tokens;
namespace WebAPI.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IClaimsService _claimsService;
        public UsersController(IUserService userService, IClaimsService claimsService)
        {
            _userService = userService;
            _claimsService = claimsService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDTO loginObject)
        {
            var checkAdd = await _userService.RegisterAsync(loginObject);
            if (checkAdd)
            {
                return Ok("Register Successfully!");
            }
            return BadRequest("Register Failed!");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO loginObject)
        {
            var token= await _userService.LoginAsync(loginObject);
            if(token== null)
            {
                return BadRequest();
            }
            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(string accessToken, string  refreshtoken)
        {
            var newToken = await _userService.RefreshToken(accessToken,refreshtoken);
            if(newToken == null)
            {
                return BadRequest();
            }
            return Ok(newToken);
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> TestAuthorize()
        //{
        //    var test = _claimsService.GetCurrentUserId;
        //    return Ok(test);
        //}

        [HttpPut] 
        public async Task<IActionResult> UpdateUser(UpdateDTO updateObject)
        {
            var result = await _userService.UpdateUserInformation(updateObject);
            if(result)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.SendResetPassword(email);
            if (!result.IsNullOrEmpty())
            {
                return Ok(result);
            }
            else return BadRequest("Can Not found User");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetObj)
        {
            var result = await _userService.ResetPassword(resetObj);
            if (result)
            {
                return Ok(result);
            }
            else return BadRequest();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var users = await _userService.GetAllAsync();
            if (users.IsNullOrEmpty())
            {
                return NoContent();
            }
            return Ok(users);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserPaginationAsync(int pageIndex = 0, int pageSize = 10)
        {
            var users = await _userService.GetUserPaginationAsync(pageIndex, pageSize);
            return Ok(users);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> ChangeUserRole(UpdateRoleDTO updateDTO)
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            if (currentUserId == updateDTO.UserID)
            {
                return BadRequest();
            }
            var result = await _userService.ChangeUserRole(updateDTO.UserID, updateDTO.RoleID);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NoContent();
            }
            return Ok(user);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> DisableUserById(string userId)
        {
            var result = await _userService.DisableUserById(userId);
            if (result is false)
            {
                return BadRequest("Disable failed");
            }
            return Ok("Disable successfully!");
        }
    }
}