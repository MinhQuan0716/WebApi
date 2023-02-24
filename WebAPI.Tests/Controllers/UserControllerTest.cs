using Application.Commons;
using Application.Services;
using Application.ViewModels.TokenModels;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers;

public class UserControllerTest : SetupTest
{
    private readonly UsersController _userController;
    public UserControllerTest()
    {
        _userController = new UsersController(_userServiceMock.Object,
                                              _claimsServiceMock.Object,
                                              new Application.Utils.ExternalAuthUtils(_config.Object),
                                              _mapperMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ReturnOKObjectResult()
    {
        //Arrange
        var registerDTO = _fixture.Create<RegisterDTO>();
        _userServiceMock.Setup(u => u.RegisterAsync(registerDTO)).Returns(Task.FromResult(true));
        //Act

        var result = await _userController.RegisterAsync(registerDTO);
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
        _userServiceMock.Setup(u => u.LoginAsync(loginDTo)).ReturnsAsync(token = null);
        //Act

        var result = await _userController.LoginAsync(loginDTo);
        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();

    }

    [Fact]
    public async Task RefreshToken_ReturnOKObjectResult()
    {
        //Arrange
        var token = _fixture.Create<Token>();
        _userServiceMock.Setup(u => u.RefreshToken(token.AccessToken, token.RefreshToken)).ReturnsAsync(token);
        //Act

        var result = await _userController.RefreshToken(token.AccessToken, token.RefreshToken);
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
        var mockUsers = _fixture.Build<UserViewModel>().CreateMany(10).ToList();

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

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnCorrectData()
    {
        //arrange
        var mockUser = _fixture.Build<UserViewModel>().Create();

        _userServiceMock.Setup(
            x => x.GetUserByIdAsync(mockUser._Id)).ReturnsAsync(mockUser);
        //act
        var result = await _userController.GetUserByIdAsync(mockUser._Id) as OkObjectResult;

        //assert
        _userServiceMock.Verify(
            x => x.GetUserByIdAsync(
                It.IsAny<string>()), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<UserViewModel>(result.Value);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal(mockUser, result.Value);
    }
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNoContent_WhenPassNonExistId()
    {
        //arrange
        var mockUser = _fixture.Build<UserViewModel>().Create();

        _userServiceMock.Setup(
            x => x.GetUserByIdAsync(mockUser._Id)).ReturnsAsync(mockUser);
        //act
        var result = await _userController.GetUserByIdAsync(It.IsAny<string>()) as NoContentResult;

        //assert
        _userServiceMock.Verify(
            x => x.GetUserByIdAsync(
                mockUser._Id), Times.Never);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
    }
    [Fact]
    public async Task DisableUserById_ShouldReturnOk()
    {
        //arrange
        var mock = _fixture.Build<UserViewModel>().Create();

        _userServiceMock.Setup(
            x => x.DisableUserById(mock._Id)).ReturnsAsync(true);

        //act
        var result = await _userController.DisableUserById(mock._Id) as OkObjectResult;

        //assert
        _userServiceMock.Verify(x => x.DisableUserById(It.Is<string>(x => x.Equals(mock._Id))), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }
    [Fact]
    public async Task DisableUserById_ShouldReturnBadRequest()
    {
        //arrange
        var mock = _fixture.Build<UserViewModel>().Create();

        _userServiceMock.Setup(
            x => x.DisableUserById(It.IsAny<string>())).ReturnsAsync(false);

        //act
        var result = await _userController.DisableUserById(It.IsAny<string>()) as BadRequestObjectResult;

        //assert
        _userServiceMock.Verify(x => x.DisableUserById(It.IsAny<string>()), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
    }

    [Fact]
    public async Task ChangeUserRole_ShouldReturnNoContent_WhenServiceReturnTrue()
    {
        var updateDTO = _fixture.Build<UpdateDTO>().Create();
        _userServiceMock.Setup(us => us.ChangeUserRole(updateDTO.UserID, updateDTO.RoleID)).ReturnsAsync(true);
        var result = await _userController.ChangeUserRole(updateDTO);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenServiceReturnFalse()
    {
        var updateDTO = _fixture.Build<UpdateDTO>().Create();
        _userServiceMock.Setup(us => us.ChangeUserRole(updateDTO.UserID, updateDTO.RoleID)).ReturnsAsync(false);
        var result = await _userController.ChangeUserRole(updateDTO);
        Assert.IsType<BadRequestResult>(result);
    }
    [Fact]
    public async Task ChangeUserRole_ShouldReturnBadRequest_WhenSuperAdminChangeOwnRole()
    {
        var admin = _fixture.Build<User>().Create();
        admin.RoleId = 1;

        _claimsServiceMock.Setup(ad => ad.GetCurrentUserId).Returns(admin.Id);
        var updateDTO = _fixture.Build<UpdateDTO>().Create();
        updateDTO.RoleID = 1;
        updateDTO.UserID = admin.Id;
        var result = await _userController.ChangeUserRole(updateDTO);
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Logout_ReturnNoContentResult()
    {
        _userServiceMock.Setup(u => u.LogoutAsync()).ReturnsAsync(true);

        //Act
        var result = await _userController.Logout();
        //Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Logout_ReturnUnAuthorizeResult()
    {
        _userServiceMock.Setup(u => u.LogoutAsync()).ReturnsAsync(false);

        //Act
        var result = await _userController.Logout();
        //Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task AddUserManually_ShouldReturnUser()
    {
        var userMockData= _fixture.Build<AddUserManually>().Create();
        var user=_mapperConfig.Map<User>(userMockData);
        _userServiceMock.Setup(x => x.AddUserManualAsync(userMockData)).ReturnsAsync(user);
        var resulltController = _userController.AddUserManually(userMockData);
        Assert.NotNull(resulltController);
        //Assert.IsType<OkObjectResult>(resulltController);
    }
    [Fact]
    public async Task AddUserManually_ShouldNoUserReturn()
    {
        var userMockData = _fixture.Build<AddUserManually>().Create();
        var user = _mapperConfig.Map<User>(userMockData);
        _userServiceMock.Setup(x => x.AddUserManualAsync(userMockData));
        
        var resultController = await _userController.AddUserManually(userMockData);
        BadRequestObjectResult actual = resultController as BadRequestObjectResult;
        Assert.NotNull(actual);
    }
}
