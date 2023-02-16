using Application.Commons;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<bool> RegisterAsync(RegisterDTO userDto);
    public Task<Token> RefreshToken(string accessToken, string refreshToken);
    public Task<Token> LoginAsync(LoginDTO userDto);
    public Task<bool> UpdateUserInformation(UpdateDTO updateUser);
    Task<Pagination<UserViewModel>> GetUserPaginationAsync(int pageIndex = 0, int pageSize = 10);
    Task<List<UserViewModel>> GetAllAsync();
}
