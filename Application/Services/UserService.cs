using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
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
            Status="Login Successfully!",
            AccessToken=accessToken,
            RefreshToken=refreshToken
        };

    }

    public async Task<Token> RefreshToken(string accessToken, string refreshToken)
    {
        if (accessToken == null || refreshToken==null)
        {
            return null;
        }
        var principal = accessToken.GetPrincipalFromExpiredToken(_configuration.JWTSecretKey);
        if (principal == null)
        {
            return null;
        }
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
            Status = "Login Successfully!",
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
}
