using Application.Interfaces;
using Application.Services;
using Application.ViewModels.Location;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    public class LocationServiceTest : SetupTest
    {
        private readonly ILocationService _locationService;

        public LocationServiceTest()
        {
            _locationService = new LocationService(_unitOfWorkMock.Object, _mapperConfig);
        }
        [Fact]
        public async Task AddNewLocation_ShouldReturnCorrectData_WhenSavedSucceed()
        {
            //arrange
            var mock = _fixture.Build<CreateLocationDTO>().Create();
            _unitOfWorkMock.Setup(x => x.LocationRepository.AddAsync(It.IsAny<Location>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<LocationDTO>(
                _mapperConfig.Map<Location>(mock)
                );
            //act
            var result = await _locationService.AddNewLocation(mock);

            //assert
            _unitOfWorkMock.Verify(x => x.LocationRepository.AddAsync(It.IsAny<Location>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task AddNewLocation_ShouldReturnCorrectData_WhenSavedFail()
        {
            //arrange
            var mock = _fixture.Build<CreateLocationDTO>().Create();
            _unitOfWorkMock.Setup(x => x.LocationRepository.AddAsync(It.IsAny<Location>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);
            //act
            var result = await _locationService.AddNewLocation(mock);

            //assert
            _unitOfWorkMock.Verify(x => x.LocationRepository.AddAsync(It.IsAny<Location>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
            result.Should().BeNull();
        }
        [Fact]
        public async Task GetAllLocation_ShouldReturnCorrectData()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().CreateMany().ToList();
            var mockLocationDTO = _mapperConfig.Map<List<LocationDTO>>(mockLocation);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetAllAsync()).ReturnsAsync(mockLocation);

            //act
            var result = await _locationService.GetAllLocation();

            //assert
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetAllAsync(), Times.Once);
            result.Should().BeEquivalentTo(mockLocationDTO);
        }
    }
}
