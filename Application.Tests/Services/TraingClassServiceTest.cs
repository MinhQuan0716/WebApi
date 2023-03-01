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
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
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
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
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
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
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
        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnTrue_WhenSaveSucceed()
        {
            //arrange
            var mockLocation = new Location();
            var mockTrainingProgram =new TrainingProgram();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().With(x => x.LocationID, mockLocation.Id).With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x=>x.TrainingClassParticipates)
                .Without(x=>x.Applications)
                .Without(x=>x.Attendances)
                .Without(x=>x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<UpdateTrainingCLassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = await _trainingClassService.UpdateTrainingClass(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .Update(It.Is<TrainingClass>(
                    x => x.Equals(expected))
                ), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeTrue();
        }
        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnFalse_WhenSaveFail()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().With(x => x.LocationID, mockLocation.Id).With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x=>x.TrainingClassParticipates)
                .Without(x=>x.Applications)
                .Without(x=>x.Attendances)
                .Without(x=>x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var expected = _mapperConfig.Map<UpdateTrainingCLassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = await _trainingClassService.UpdateTrainingClass(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .Update(It.Is<TrainingClass>(
                    x => x.Equals(expected))
                ), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeFalse();
        }
        [Fact]
        public async Task UpdateTrainingClass_ShouldThrowException_WhenWrongLocationId()
        {
            //arrange
            var mockLocation = new Location();
            var mockTrainingProgram = new TrainingProgram();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x=>x.TrainingClassParticipates)
                .Without(x=>x.Applications)
                .Without(x=>x.Attendances)
                .Without(x=>x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockLocation.Id)))).ReturnsAsync(mockLocation);
            //_unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<UpdateTrainingCLassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = async () => await _trainingClassService.UpdateTrainingClass(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .Update(It.Is<TrainingClass>(
                    x => x.Equals(expected))
                ), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result.Should().ThrowAsync<NullReferenceException>();
        }
        [Fact]
        public async Task UpdateTrainingClass_ShouldThrowException_WhenWrongTrainingProgramId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().With(x => x.LocationID, mockLocation.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>().Without(x=>x.TrainingClassParticipates).Without(x => x.TrainingProgram).Without(x=>x.Applications).Without(x=>x.Attendances).Without(x=>x.Feedbacks).Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingProgram.Id)))).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<UpdateTrainingCLassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = async () => await _trainingClassService.UpdateTrainingClass(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .Update(It.Is<TrainingClass>(
                    x => x.Equals(expected))
                ), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result.Should().ThrowAsync<NullReferenceException>();
        }
        [Fact]
        public async void GetTrainingClassByIdAsync_ShouldReturnCorrectData()
        {
            //arrange
            var mockId = Guid.NewGuid();
            TrainingClass mockTrainingClass = new();
            _unitOfWorkMock.Setup(
                x => x.TrainingClassRepository.GetByIdAsync(
                    mockId)).ReturnsAsync(mockTrainingClass);

            //act
            var result = await _trainingClassService.GetTrainingClassByIdAsync(mockId.ToString());

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository.GetByIdAsync(
                    It.Is<Guid>(e => e.Equals(mockId))), Times.Once);
            result.Should().Be(mockTrainingClass);
        }
        [Fact]
        public async Task GetTrainingClassByIdAsync_ShouldThrowException_WhenIdIsIncorrect()
        {
            //arrange
            var mockId = Guid.NewGuid();
            TrainingClass mockTrainingClass =  new();
            _unitOfWorkMock.Setup(
                x => x.TrainingClassRepository.GetByIdAsync(
                    It.Is<Guid>(e => e.Equals(mockId)))).ReturnsAsync(mockTrainingClass);

            //act
            var result = async () => await _trainingClassService.GetTrainingClassByIdAsync(Guid.NewGuid().ToString());

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository.GetByIdAsync(
                    It.Is<Guid>(e => e.Equals(mockId))), Times.Never);
            await result.Should().ThrowAsync<NullReferenceException>("Incorrect Id");
        }
        [Fact]
        public async Task GetTrainingClassByIdAsync_ShouldThrowException_WhenGetMappingError()
        {
            //arrange
            var mockId = Guid.NewGuid();
            TrainingClass mockTrainingClass =  new();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);

            //act
            var result = async () => await _trainingClassService.GetTrainingClassByIdAsync("abc123");

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository.GetByIdAsync(
                    It.Is<Guid>(e => e.Equals(mockId))), Times.Never);
            await result.Should().ThrowAsync<AutoMapperMappingException>("Id is not a guid");
        }
    }
}
