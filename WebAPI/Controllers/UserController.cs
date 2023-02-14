using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.UserViewModels;

namespace WebAPI.Controllers
{
  public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            var accessToken= await _userService.LoginAsync(loginObject);
            return Ok(accessToken);
        }

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