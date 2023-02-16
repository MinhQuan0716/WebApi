using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class UserControllerTest:SetupTest
    {
        private readonly UserController _userController;
        public UserControllerTest()
        {
                _userController=new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ReturnOKObjectResult()
        {
            //Arrange
            var registerDTO = _fixture.Create<RegisterDTO>();
            _userServiceMock.Setup(u=>u.RegisterAsync(registerDTO)).Returns(Task.FromResult(true));
            //Act

            var result= await _userController.RegisterAsync(registerDTO);
            //Assert
            result.Should().BeOfType<OkObjectResult>();

        }
        [Fact]
        public async Task RegisterAsync_ReturnBadRequestObjectResult()
        {
            //Arrange
            var registerDTO = _fixture.Create<RegisterDTO>();
            _userServiceMock.Setup(u => u.RegisterAsync(registerDTO)).Returns(Task.FromResult(false));
            //Act

            var result = await _userController.RegisterAsync(registerDTO);
            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();

        }

        [Fact]
        public async Task LoginAsync_ReturnOKObjectResult()
        {
            //Arrange
            var loginDTo = _fixture.Create<LoginDTO>();
            var token = _fixture.Create<Token>();
            _userServiceMock.Setup(u => u.LoginAsync(loginDTo)).ReturnsAsync(token);
            //Act

            var result = await _userController.LoginAsync(loginDTo);
            //Assert
            result.Should().BeOfType<OkObjectResult>();

        }

        [Fact]
        public async Task LoginAsync_ReturnBadRequestResult()
        {
            //Arrange
            var loginDTo = _fixture.Create<LoginDTO>();
            var token = _fixture.Create<Token>();
            _userServiceMock.Setup(u => u.LoginAsync(loginDTo)).ReturnsAsync(token=null);
            //Act

            var result = await _userController.LoginAsync(loginDTo);
            //Assert
            result.Should().BeOfType<BadRequestResult>();

        }

        [Fact]
        public async Task RefreshToken_ReturnOKObjectResult()
        {
            //Arrange
            var token = _fixture.Create<Token>();
            _userServiceMock.Setup(u => u.RefreshToken(token.AccessToken,token.RefreshToken)).ReturnsAsync(token);
            //Act

            var result = await _userController.RefreshToken(token.AccessToken,token.RefreshToken);
            //Assert
            result.Should().BeOfType<OkObjectResult>();

        }
        [Fact]
        public async Task RefreshToken_ReturnBadRequestResult()
        {
            //Arrange
            var token = _fixture.Create<Token>();
            Token newToken = null;
            _userServiceMock.Setup(u => u.RefreshToken(token.AccessToken, token.RefreshToken)).ReturnsAsync(newToken);
            //Act

            var result = await _userController.RefreshToken(token.AccessToken, token.RefreshToken);
            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task UpdateUser_ReturnNoContentResult()
        {
            //Arrange
            var updateDTO = _fixture.Create<UpdateDTO>();
            _userServiceMock.Setup(u => u.UpdateUserInformation(updateDTO)).ReturnsAsync(true);
            //Act

            var result = await _userController.UpdateUser(updateDTO);
            //Assert
            result.Should().BeOfType<NoContentResult>();

        }

        [Fact]
        public async Task UpdateUser_ReturnBadRequestResult()
        {
            //Arrange
            var updateDTO = _fixture.Create<UpdateDTO>();
            _userServiceMock.Setup(u => u.UpdateUserInformation(updateDTO)).ReturnsAsync(false);
            //Act

            var result = await _userController.UpdateUser(updateDTO);
            //Assert
            result.Should().BeOfType<BadRequestResult>();

        }
    }
}
