using Application.Interfaces;
using Application.Services;
using Application.ViewModels.UserViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Moq;

namespace Application.Tests
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
        public async Task UpdateUser_ShouldThrowException()
        {
            UpdateDTO mockData = null;
            var result =  await _userService.UpdateUserInformation(mockData);
            result.Should().As<Exception>();
           

        }
    }
}