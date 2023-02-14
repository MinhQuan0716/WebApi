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
        //var user = await _unitOfWork.UserRepository.GetUserByUserNameANdPaswordHashAsync(userDto.UserName, userDto.Password);
        var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(userDto.Email);
        if (userDto.Password.CheckPassword(user.PasswordHash)==false)
        {
            throw new Exception("Password is not incorrect!!");
        }
        return user.GenerateJsonWebToken(_configuration.JWTSecretKey, _currentTime.GetCurrentTime());
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

        var user = await _unitOfWork.UserRepository.GetByIdAsync(updateUser.UserID);
        
        if (user != null)
        {
            user.Email = updateUser.Email;
            user.UserName = updateUser.Email;
            user.Gender = updateUser.Gender;
            user.FullName = updateUser.FullName;
            user.RoleId = updateUser.RoleID;
            user.DateOfBirth = updateUser.DateOfBirth;
            
            _unitOfWork.UserRepository.Update(user);
            if (await _unitOfWork.SaveChangeAsync() > 0) return true;
            else return false;
        }
        else throw new Exception();

    }
}
