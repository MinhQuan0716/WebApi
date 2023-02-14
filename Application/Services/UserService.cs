using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<string> LoginAsync(LoginDTO userDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUserNameANdPaswordHashAsync(userDto.UserName, userDto.Password);
        return user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());
    }

    public async Task RegisterAsync(RegisterDTO userDto)
    {
        var isExisted = await _unitOfWork.UserRepository.CheckUserNameExistedAsync(userDto.UserName)
                                && await _unitOfWork.UserRepository.CheckEmailExistedAsync(userDto.Email);
        
        if (isExisted)
        {
            throw new Exception("Username exited please try again");
        }

        var newUser = new User
        {
            UserName = userDto.UserName,
            PasswordHash = userDto.Password.Hash(),
            Email = userDto.Email
        };

        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveChangeAsync();
    }
}
