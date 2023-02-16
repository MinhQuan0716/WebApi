using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Moq;
using System;

namespace Application.Tests.Services
{
    public class UserServiceTest : SetupTest
    {
        private readonly IUserService _userService;
        public UserServiceTest()
        {
            _userService = new UserService(_unitOfWorkMock.Object, _mapperConfig, _currentTimeMock.Object, _appConfigurationMock.Object);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnTrue()
        {
            var mockData = _fixture.Create<UpdateDTO>();
            _unitOfWorkMock.Setup(um => um.UserRepository.Update(It.IsAny<User>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            var result = await _userService.UpdateUserInformation(mockData);
            Assert.True(result);

        }


        [Fact]
        public async Task UpdateUser_ShouldReturnFalse()
        {
            var mockData = _fixture.Create<UpdateDTO>();
            _unitOfWorkMock.Setup(um => um.UserRepository.Update(It.IsAny<User>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(-1);
            var result = await _userService.UpdateUserInformation(mockData);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateUser_ShouldThrowException()
        {
            UpdateDTO mockData = null;
            //Get Return Exception
            Func<Task> act = async () => await _userService.UpdateUserInformation(mockData);
            act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task Register_ShouldReturnUser()
        {
            var mockData = _fixture.Build<RegisterDTO>().Create();
            _unitOfWorkMock.Setup(um => um.UserRepository.CheckEmailExistedAsync(mockData.Email)).ReturnsAsync(false);
            var addMockData = _mapperConfig.Map<User>(mockData);
            _unitOfWorkMock.Setup(um => um.UserRepository.AddAsync(addMockData)).Verifiable();
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            var result = await _userService.RegisterAsync(mockData);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Register_ShouldThrowException()
        {

            var mockData = _fixture.Build<RegisterDTO>().Create();
            _unitOfWorkMock.Setup(u => u.UserRepository.CheckEmailExistedAsync(mockData.Email)).ReturnsAsync(true);
            Func<Task> act = async () => await _userService.RegisterAsync(mockData);
            act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task LoginAsync_ReturnCorrectData()
        {
            //Arange
            var mockUser = _fixture.Build<User>().Create();
            var loginDTO = new LoginDTO { Email = mockUser.Email, Password = mockUser.PasswordHash };
            mockUser.PasswordHash = mockUser.PasswordHash.Hash();
            mockUser.ExpireTokenTime = DateTime.UtcNow.AddDays(1);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmailAsync(mockUser.Email)).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(u => u.UserRepository.Update(mockUser)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(1);
            _appConfigurationMock.SetupAllProperties();
            _appConfigurationMock.Object.JWTSecretKey = "ChanquaChanquaChanquaChanqua";
            //Act
            var result = await _userService.LoginAsync(loginDTO);
            //Assert

            result.Should().NotBeNull();
        }
        [Fact]
        public async Task LoginAsync_ThrowException_WhenPasswordIncorrect()
        {
            //Arange
            var mockUser = _fixture.Build<User>().Create();
            var loginDTO = new LoginDTO { Email = mockUser.Email, Password = _fixture.Create<string>() };
            mockUser.PasswordHash = mockUser.PasswordHash.Hash();
            mockUser.ExpireTokenTime = DateTime.UtcNow.AddDays(1);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmailAsync(mockUser.Email)).ReturnsAsync(mockUser);

            //Act
            Func<Task> act = async () => await _userService.LoginAsync(loginDTO);
            //Assert

            act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RefreshToken_ReturnCorrectData()
        {
            //Arange
            var mockUser = _fixture.Build<User>().Create();
            var loginDTO = new LoginDTO { Email = mockUser.Email, Password = mockUser.PasswordHash };
            mockUser.PasswordHash = mockUser.PasswordHash.Hash();
            mockUser.ExpireTokenTime = DateTime.UtcNow.AddDays(1);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmailAsync(mockUser.Email)).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(u => u.UserRepository.Update(mockUser)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(1);
            _appConfigurationMock.SetupAllProperties();
            _appConfigurationMock.Object.JWTSecretKey = "ChanquaChanquaChanquaChanqua";
            var token = await _userService.LoginAsync(loginDTO);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetByIdAsync(mockUser.Id, x => x.Role)).ReturnsAsync(mockUser);
            //Assert

            var result = await _userService.RefreshToken(token.AccessToken, token.RefreshToken);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task RefreshToken_ReturnNull_WhenAccessTokenOrRefreshTokenIsNull()
        {

            //Assert

            var result = await _userService.RefreshToken(string.Empty,string.Empty);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RefreshToken_ReturnNull_WhenGetPrincipalThowException()
        {
            //Arrange
            var token = _fixture.Create<Token>();
            //Assert

            Func<Task> act = async () => await _userService.RefreshToken(token.AccessToken,token.RefreshToken);

            //Assert
            act.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async Task RefreshToken_ReturnNull_WhenRefreshTokenInCorrect()
        {
            // Arange
            var tokenRefresh = _fixture.Create<string>();
            var mockUser = _fixture.Build<User>().Create();
            var loginDTO = new LoginDTO { Email = mockUser.Email, Password = mockUser.PasswordHash };
            mockUser.PasswordHash = mockUser.PasswordHash.Hash();
            mockUser.ExpireTokenTime = DateTime.UtcNow.AddDays(1);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmailAsync(mockUser.Email)).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(u => u.UserRepository.Update(mockUser)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(1);
            _appConfigurationMock.SetupAllProperties();
            _appConfigurationMock.Object.JWTSecretKey = "ChanquaChanquaChanquaChanqua";
            var token = await _userService.LoginAsync(loginDTO);
            //Assert

            var result = await _userService.RefreshToken(token.AccessToken, tokenRefresh);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RefreshToken_ReturnNull_WhenExpireTimeInCorrect()
        {
            // Arange
            var tokenRefresh = _fixture.Create<string>();
            var mockUser = _fixture.Build<User>().Create();
            var loginDTO = new LoginDTO { Email = mockUser.Email, Password = mockUser.PasswordHash };
            mockUser.PasswordHash = mockUser.PasswordHash.Hash();
            mockUser.ExpireTokenTime = DateTime.Now;
            _unitOfWorkMock.Setup(u => u.UserRepository.GetUserByEmailAsync(mockUser.Email)).ReturnsAsync(mockUser);
            _unitOfWorkMock.Setup(u => u.UserRepository.Update(mockUser)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangeAsync()).ReturnsAsync(1);
            _appConfigurationMock.SetupAllProperties();
            _appConfigurationMock.Object.JWTSecretKey = "ChanquaChanquaChanquaChanqua";
            var token = await _userService.LoginAsync(loginDTO);
            //Assert

            var result = await _userService.RefreshToken(token.AccessToken, token.RefreshToken);

            //Assert
            result.Should().BeNull();
        }

    }
}