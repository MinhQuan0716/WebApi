﻿using Application.Commons;
using Application.ViewModels.TokenModels;
using Application.Utils;
using Application.ViewModels.UserViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<bool> RegisterAsync(RegisterDTO userDto);
    public Task<Token> RefreshToken(string accessToken, string refreshToken);
    public Task<Token> LoginAsync(LoginDTO userDto);
    public Task<bool> UpdateUserInformation(UpdateDTO updateUser);
    public Task<UserViewModel> GetUserByIdAsync(string id);
    Task<string> SendResetPassword(string email);
    Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO);

    Task<Pagination<UserViewModel>> GetUserPaginationAsync(int pageIndex = 0, int pageSize = 10);
    Task<List<UserViewModel>> GetAllAsync();
    public Task<bool> ChangeUserRole(Guid userId, int roleId);
    /// <summary>
    /// Change current User password.
    /// Authorized as anyone
    /// </summary>
    /// <param name="oldPassword">send the old password</param>
    /// <param name="newPassword">send the new password</param>
    /// <returns></returns>
    public Task<string> ChangePasswordAsync(string oldPassword, string newPassword);

    public Task<bool> DisableUserById(string userId);
    Task<User> AddUserManualAsync(AddUserManually addUserManually);

    Task<bool> LogoutAsync();
}
