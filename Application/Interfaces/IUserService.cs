using Application.Commons;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<bool> RegisterAsync(RegisterDTO userDto);
    public Task<Token> RefreshToken(string accessToken, string refreshToken);
    public Task<Token> LoginAsync(LoginDTO userDto);
    public Task<bool> UpdateUserInformation(UpdateDTO updateUser);
    Task<string> SendResetPassword(string email);
    Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO);

    Task<Pagination<UserViewModel>> GetUserPaginationAsync(int pageIndex = 0, int pageSize = 10);
    Task<List<UserViewModel>> GetAllAsync();
    public Task<bool> ChangeUserRole(Guid userId, int roleId);
}
