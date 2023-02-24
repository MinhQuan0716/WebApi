using Application.Interfaces;
using Application.Services;
using Application.ViewModels.TrainingClassModels;
using AutoFixture;
using AutoMapper;
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
    public class TraingClassServiceTest : SetupTest
    {
        private readonly ITrainingClassService _trainingClassService;

        public TraingClassServiceTest()
        {
            _trainingClassService = new TrainingClassService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnCorrectData_WhenSavedSucceed()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            TrainingClass mockClass = _mapperConfig.Map<TrainingClass>(mockCreate);
            mockClass.Location = mockLocation;
            var expected = _mapperConfig.Map<TrainingClassViewModel>(mockClass);

            //act
            var result = await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnNull_WhenSavedFail()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            //act
            var result = await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository
            .AddAsync(It.IsAny<TrainingClass>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

            result.Should().BeNull();
        }
        [Fact]
        public async Task CreateTrainingClass_ShouldThrowException_WhenWrongLocationId()
        {
            //arrange
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().Create();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            //act
            var result = async () => await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);

            await result.Should().ThrowAsync<Exception>("Invalid location Id");
        }
        [Fact]
        public async Task CreateTrainingClass_ShouldThrowException_WhenWrongProgramId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            //act
            var result = async () => await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Never);

            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);

            await result.Should().ThrowAsync<Exception>("Invalid location Id");
        }
    }
}
