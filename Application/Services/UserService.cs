using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentTime _currentTime;
    private readonly AppConfiguration _configuration;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, AppConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
    }

    public async Task<Token> LoginAsync(LoginDTO userDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email);
        if (userDto.Password.CheckPassword(user.PasswordHash)==false)
        {
            throw new Exception("Password is not incorrect!!");
        }
        var refreshToken = RefreshTokenString.GetRefreshToken();
        var accessToken = user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());
        var expireRefreshTokenTime = DateTime.Now.AddHours(24);

        user.RefreshToken = refreshToken;
        user.ExpireTokenTime = expireRefreshTokenTime;
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveChangeAsync();
        return new Token 
        {
            UserName=user.UserName,
            AccessToken=accessToken,
            RefreshToken=refreshToken
        };

    }

    public async Task<Token> RefreshToken(string accessToken, string refreshToken)
    {
        if (accessToken.IsNullOrEmpty() || refreshToken.IsNullOrEmpty())
        {
            return null;
        }
        var principal = accessToken.GetPrincipalFromExpiredToken(_configuration.JWTSecretKey);

        var id = principal.FindFirstValue("userID");
        _ = Guid.TryParse(id, out Guid userID);
        var userLogin = await _unitOfWork.UserRepository.GetByIdAsync(userID, x => x.Role);
        if (userLogin == null || userLogin.RefreshToken != refreshToken || userLogin.ExpireTokenTime <= DateTime.Now)
        {
            return null;
        }

        var newAccessToken = userLogin.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());
        var newRefreshToken = RefreshTokenString.GetRefreshToken();

        userLogin.RefreshToken = newRefreshToken;
        _unitOfWork.UserRepository.Update(userLogin);
        await _unitOfWork.SaveChangeAsync();

        return new Token
        {
            UserName= userLogin.UserName,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<bool> RegisterAsync(RegisterDTO userDto)
    {
        var isExisted = await _unitOfWork.UserRepository.CheckEmailExistedAsync(userDto.Email);
        
        if (isExisted)
        {
            throw new Exception("Username exited please try again");
        }

        var newUser = new User
        {
            UserName = userDto.Email, // lay email lam username luon
            PasswordHash = userDto.Password.Hash(),
            Email = userDto.Email
        };

        await _unitOfWork.UserRepository.AddAsync(newUser);
        return await _unitOfWork.SaveChangeAsync()>0;
    }

    public async Task<bool> UpdateUserInformation(UpdateDTO updateUser)
    {   
        if(updateUser != null)
        {
            User user = await _unitOfWork.UserRepository.GetByIdAsync(updateUser.UserID);
            _ = _mapper.Map(updateUser, user, typeof(UpdateDTO), typeof(User));

            _unitOfWork.UserRepository.Update(user);
            if (await _unitOfWork.SaveChangeAsync() > 0) return true;
            else return false;
        } throw new Exception("User can not be null");
        
        
      
    }
    /// <summary>
    /// Return collection of item with parameter
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public async Task<Pagination<UserViewModel>> GetUserPaginationAsync(int pageIndex = 0, int pageSize = 10)
    {
        var users = await _unitOfWork.UserRepository.ToPagination(pageIndex, pageSize);
        var result = _mapper.Map<Pagination<UserViewModel>>(users);
        return result;
    }
    public async Task<List<UserViewModel>> GetAllAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        var result = _mapper.Map<List<UserViewModel>>(users);
        return result;
    }
}
