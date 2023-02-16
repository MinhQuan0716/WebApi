using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.ViewModels.TokenModels;

namespace WebAPI.Controllers
{
  public class UserController : BaseController
    {
        private readonly IUserService _userService;
        //private readonly IClaimsService _claimsService;
        public UserController(IUserService userService)
        {
            _userService = userService;
           //_claimsService = claimsService;
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
    }
}