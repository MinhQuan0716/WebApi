﻿using Application.Commons;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
        private readonly UsersController _userController;
        public UserControllerTest()
        {
                _userController=new UsersController(_userServiceMock.Object,_claimsServiceMock.Object);
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

        [Fact]
        public async Task GetAllUserAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockUsers = _fixture.Build<UserViewModel>().CreateMany(50).ToList();

            _userServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(mockUsers);
            //act
            var result = await _userController.GetAllUserAsync() as OkObjectResult;
            //assert
            _userServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<List<UserViewModel>>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(mockUsers, result.Value);
        }
        [Fact]
        public async Task GetAllUserAsync_ShouldReturnNoContent_WhenIsNullOrEmpty()
        {
            //act
            var result = await _userController.GetAllUserAsync() as NoContentResult;
            //assert
            _userServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
        }
        [Fact]
        public async Task GetUserPaginationAsync_ShouldReturnCorrectDataWithDefaultParameter()
        {
            //arrange
            var mock = _fixture.Build<Pagination<UserViewModel>>().Create();

            _userServiceMock.Setup(
                x => x.GetUserPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mock);

            //act
            var result = await _userController.GetUserPaginationAsync() as OkObjectResult;

            //assert
            _userServiceMock.Verify(x => x.GetUserPaginationAsync(
                It.Is<int>(x => x.Equals(0)),
                It.Is<int>(x => x.Equals(10))
                ), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<Pagination<UserViewModel>>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(mock, result.Value);
        }
        [Fact]
        public async Task GetUserPaginationAsync_ShouldReturnCorrectDataWithParameter()
        {
            //arrange
            var mock = _fixture.Build<Pagination<UserViewModel>>().Create();

            _userServiceMock.Setup(
                x => x.GetUserPaginationAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(mock);

            //act
            var result = await _userController.GetUserPaginationAsync(1, 100) as OkObjectResult;

            //assert
            _userServiceMock.Verify(x => x.GetUserPaginationAsync(
                It.Is<int>(x => x.Equals(1)),
                It.Is<int>(x => x.Equals(100))
                ), Times.Once);
            Assert.NotNull(result);
            Assert.IsType<Pagination<UserViewModel>>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(mock, result.Value);
        }
    }
}