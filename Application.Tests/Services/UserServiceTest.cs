using Application.Interfaces;
using Application.Services;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Moq;

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
    }
}