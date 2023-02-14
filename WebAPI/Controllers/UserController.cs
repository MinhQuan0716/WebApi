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
        public async Task RegisterAsync(RegisterDTO loginObject) => await _userService.RegisterAsync(loginObject);

        [HttpPost]
        public async Task<string> LoginAsync(LoginDTO loginObject) => await _userService.LoginAsync(loginObject);
    }
}