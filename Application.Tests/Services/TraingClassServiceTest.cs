using Application.Interfaces;
using Application.Services;
using Application.ViewModels.TrainingClassModels;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.TrainingClassRelated;
using Domain.Enums;
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
        public async Task SoftRemoveTrainingClass_ShouldReturnTrue_WhenSaveSucceed()
        {
            //arrange
            var mockTrainingClass = _fixture.Build<TrainingClass>().Without(x => x.Applications).Without(x => x.Attendances).Without(x => x.Feedbacks).Without(x => x.TrainingProgram).Without(x => x.TrainingClassParticipates).Without(x => x.Location).Create();
            var mockId = mockTrainingClass.Id;
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.SoftRemove(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(mockId)).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingClassService.SoftRemoveTrainingClassAsync(mockId.ToString());

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .SoftRemove(It.Is<TrainingClass>(
                    x => x.Equals(_mapperConfig.Map<TrainingClass>(mockTrainingClass)))
                ), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeTrue();
        }
        [Fact]
        public async Task SoftRemoveTrainingClass_ShouldReturnFalse_WhenSaveFail()
        {
            //arrange
            var mockTrainingClass = _fixture.Build<TrainingClass>().
                Without(x => x.Applications).
                Without(x => x.Attendances).Without(x => x.Feedbacks).
                Without(x => x.TrainingProgram).
                Without(x => x.TrainingClassParticipates).
                Without(x => x.Location)
                .With(x => x.StatusClassDetail)
                .With(x => x.Branch)
                .With(x => x.Attendee)
                .Create();
            var mockId = mockTrainingClass.Id;
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.SoftRemove(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(mockId)).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            //act
            var result = await _trainingClassService.SoftRemoveTrainingClassAsync(mockId.ToString());

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository
                .SoftRemove(It.Is<TrainingClass>(
                    x => x.Equals(_mapperConfig.Map<TrainingClass>(mockTrainingClass)))
                ), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once());
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnCorrectData_WhenSavedSucceed()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();

            var mockAdmin = _fixture.Build<User>()
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Role)
                .With(x => x.RoleId, (int)RoleEnums.Admin).Create();

            var mockTrainingClassAdmins = _fixture.Build<AdminsDTO>()
                .With(x => x.AdminId, mockAdmin.Id)
                .CreateMany(1).ToList();

            var mockTrainer = _fixture.Build<User>()
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Role)
                .With(x => x.RoleId, (int)RoleEnums.Trainer).Create();

            var mockTrainingClassTrainers = _fixture.Build<TrainerDTO>()
                .With(x => x.TrainerId, mockTrainer.Id)
                .CreateMany(1).ToList();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            DateTime mockStartTime = new();
            DateTime mockEndTime = mockStartTime.AddHours(2);
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).
                With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName, mockLocation.LocationName)
                .With(x => x.Admins, mockTrainingClassAdmins)
                .With(x => x.Trainers, mockTrainingClassTrainers)
                .With(x => x.StartTime, mockStartTime)
                .With(x => x.EndTime, mockEndTime)
                .Create();

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mockAdmin.Id)).ReturnsAsync(mockAdmin);
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(mockTrainer.Id)).ReturnsAsync(mockTrainer);
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x == mockAdmin.Id)), Times.Once);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x == mockTrainer.Id)), Times.Once);
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldThroWException_WhenInCorrectAdminId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).
                With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName, mockLocation.LocationName)
                .With(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()));
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = async () => await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result!.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async Task CreateTrainingClass_ShouldThroWException_WhenInCorrectTrainerId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).
                With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName, mockLocation.LocationName)
                .Without(x => x.Admins)
                .With(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()));
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = async () => await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result!.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task CreateTrainingClass_SaveChangeShouldBeAtLeastOnce_WhenLocationNameDoesNotExist()
        {
            //arrange
            Location mockLocation = null;
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>()
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnNull_WhenSavedFail()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>()
                .With(x => x.LocationID, mockLocation.Id)
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .Without(x => x.Attendees)
                .Without(x => x.TimeFrame)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            //act
            var result = await _trainingClassService.CreateTrainingClassAsync(mockCreate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository
            .AddAsync(It.IsAny<TrainingClass>()), Times.Once);

            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);

            result.Should().BeNull();
        }
        [Fact]
        public async Task CreateTrainingClass_ShouldThrowException_WhenWrongLocationId()
        {
            //arrange
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>()
                .Without(x => x.Attendees)
                .Without(x => x.TimeFrame).Create();
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
            var mockCreate = _fixture.Build<CreateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).Without(x => x.Attendees).Without(x => x.TimeFrame).Create();

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
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>()
                .With(x => x.LocationID, mockLocation.Id)
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Applications)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.TrainingClassAdmins)
                .Without(x => x.TrainingClassTrainers)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<UpdateTrainingClassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = await _trainingClassService.UpdateTrainingClassAsync(mockTrainingClass.Id.ToString(), mockUpdate);

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
        public async Task UpdateTrainingClass_ShouldThroWException_WhenInCorrectAdminId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).
                With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName, mockLocation.LocationName)
                .With(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()));
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = async () => await _trainingClassService.UpdateTrainingClassAsync(It.IsAny<Guid>().ToString(), mockUpdate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result!.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async Task UpdateTrainingClass_ShouldThroWException_WhenInCorrectTrainerId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().
                Without(x => x.TrainingClasses).
                Without(x => x.DetailTrainingClassesParticipate)
                .Create();

            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).
                With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName, mockLocation.LocationName)
                .Without(x => x.Admins)
                .With(x => x.Trainers)
                .Create();

            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>()));
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.AddAsync(It.IsAny<TrainingClass>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = async () => await _trainingClassService.UpdateTrainingClassAsync(It.IsAny<Guid>().ToString(), mockUpdate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result!.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async Task UpdateTrainingClass_SaveChangeShouldBeAtleastOnce_WhenLocationNameIsNew()
        {
            //arrange
            //var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>()
                .With(x => x.LocationName)
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Applications)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.TrainingClassAdmins)
                .Without(x => x.TrainingClassTrainers)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>()));
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingClassService.UpdateTrainingClassAsync(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.AtLeastOnce);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnFalse_WhenSaveFail()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>()
                .With(x => x.LocationID, mockLocation.Id)
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Applications)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.TrainingClassAdmins)
                .Without(x => x.TrainingClassTrainers)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var expected = _mapperConfig.Map<UpdateTrainingClassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = await _trainingClassService.UpdateTrainingClassAsync(mockTrainingClass.Id.ToString(), mockUpdate);

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
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>().With(x => x.TrainingProgramId, mockTrainingProgram.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Applications)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.TrainingClassAdmins)
                .Without(x => x.TrainingClassTrainers)
                .Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockLocation.Id)))).ReturnsAsync(mockLocation);
            //act
            var result = async () => await _trainingClassService.UpdateTrainingClassAsync(mockTrainingClass.Id.ToString(), mockUpdate);

            //assert
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Never);
            await result.Should().ThrowAsync<NullReferenceException>();
        }
        [Fact]
        public async Task UpdateTrainingClass_ShouldThrowException_WhenWrongTrainingProgramId()
        {
            //arrange
            var mockLocation = _fixture.Build<Location>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingClassesParticipate).Create();
            var mockTrainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>().With(x => x.LocationID, mockLocation.Id).Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>().Without(x => x.TrainingClassParticipates).Without(x => x.TrainingProgram).Without(x => x.Applications).Without(x => x.Attendances).Without(x => x.Feedbacks).Without(x => x.Location).Create();

            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingProgram.Id)))).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var expected = _mapperConfig.Map<UpdateTrainingClassDTO, TrainingClass>(mockUpdate, mockTrainingClass);
            expected.Location = mockLocation;
            expected.TrainingProgram = mockTrainingProgram;
            //act
            var result = async () => await _trainingClassService.UpdateTrainingClassAsync(mockTrainingClass.Id.ToString(), mockUpdate);

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
        public async Task UpdateTrainingClass_SaveChangeShouldBeAtLeastOnce_WhenLocationNameDoesNotExist()
        {
            //arrange
            Location mockLocation = null;
            var mockTrainingProgram = _fixture.Build<TrainingProgram>()
                .Without(x => x.TrainingClasses)
                .Without(x => x.DetailTrainingProgramSyllabus)
                .Create();
            var mockUpdate = _fixture.Build<UpdateTrainingClassDTO>()
                .With(x => x.TrainingProgramId, mockTrainingProgram.Id)
                .With(x => x.Attendee)
                .With(x => x.Branch)
                .With(x => x.StatusClassDetail)
                .With(x => x.Attendees)
                .With(x => x.TimeFrame)
                .With(x => x.LocationName)
                .Without(x => x.Admins)
                .Without(x => x.Trainers)
                .Create();
            var mockTrainingClass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Applications)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.TrainingClassAdmins)
                .Without(x => x.TrainingClassTrainers)
                .Without(x => x.Location).Create();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>())).Verifiable();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);
            _unitOfWorkMock.Setup(x => x.LocationRepository.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockLocation);
            _unitOfWorkMock.Setup(x => x.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingProgram);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            //act
            var result = await _trainingClassService.UpdateTrainingClassAsync(Guid.NewGuid().ToString(), mockUpdate);

            //Assert
            _unitOfWorkMock.Verify(x => x.TrainingClassRepository.Update(It.IsAny<TrainingClass>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.AtLeastOnce);
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
            TrainingClass mockTrainingClass = new();
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
            TrainingClass mockTrainingClass = new();
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockTrainingClass);

            //act
            var result = async () => await _trainingClassService.GetTrainingClassByIdAsync("abc123");

            //assert
            _unitOfWorkMock.Verify(
                x => x.TrainingClassRepository.GetByIdAsync(
                    It.Is<Guid>(e => e.Equals(mockId))), Times.Never);
            await result.Should().ThrowAsync<AutoMapperMappingException>("Id is not a guid");
        }

        [Fact]
        public async Task CheckTrainingClassAdminsIdAsync_ShouldReturnTrue()
        {
            //arrange
            var mockTrainingClassAdmin = _fixture.Build<TrainingClassAdmin>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)RoleEnums.Admin)
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassAdminsIdAsync(mockTrainingClassAdmin);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassAdmin.AdminId))), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CheckTrainingClassAdminsIdAsync_ShouldReturnFalse_WhenAdminIdIsWrong()
        {
            //arrange
            var mockTrainingClassAdmin = _fixture.Build<TrainingClassAdmin>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)RoleEnums.Admin)
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockUser.Id)))).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassAdminsIdAsync(mockTrainingClassAdmin);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassAdmin.AdminId))), Times.Once);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CheckTrainingClassAdminsIdAsync_ShouldReturnFalse_WhenRoleIdIsWrong()
        {
            //arrange
            var mockTrainingClassAdmin = _fixture.Build<TrainingClassAdmin>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)It.Is<RoleEnums>(x => !x.Equals(RoleEnums.Admin)))
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassAdminsIdAsync(mockTrainingClassAdmin);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassAdmin.AdminId))), Times.Once);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CheckTrainingClassTrainersIdAsync_ShouldReturnTrue()
        {
            //arrange
            var mockTrainingClassTrainer = _fixture.Build<TrainingClassTrainer>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)RoleEnums.Trainer)
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassTrainersIdAsync(mockTrainingClassTrainer);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassTrainer.TrainerId))), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task CheckTrainingClassTrainersIdAsync_ShouldReturnFalse_WhenAdminIdIsWrong()
        {
            //arrange
            var mockTrainingClassTrainer = _fixture.Build<TrainingClassTrainer>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)RoleEnums.Trainer)
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockUser.Id)))).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassTrainersIdAsync(mockTrainingClassTrainer);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassTrainer.TrainerId))), Times.Once);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task CheckTrainingClassTrainersIdAsync_ShouldReturnFalse_WhenRoleIdIsWrong()
        {
            //arrange
            var mockTrainingClassTrainer = _fixture.Build<TrainingClassTrainer>()
                .Without(x => x.TrainingClass)
                .Create();
            var mockUser = _fixture.Build<User>()
                .Without(x => x.SubmitQuizzes)
                .Without(x => x.Applications)
                .Without(x => x.Syllabuses)
                .Without(x => x.Feedbacks)
                .Without(x => x.Attendances)
                .Without(x => x.DetailTrainingClassParticipate)
                .With(x => x.RoleId, (int)It.Is<RoleEnums>(x => !x.Equals(RoleEnums.Trainer)))
                .Create();
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(mockUser);

            //act
            var result = await _trainingClassService.CheckTrainingClassTrainersIdAsync(mockTrainingClassTrainer);

            //assert
            _unitOfWorkMock.Verify(x => x.UserRepository.GetByIdAsync(It.Is<Guid>(x => x.Equals(mockTrainingClassTrainer.TrainerId))), Times.Once);
            result.Should().BeFalse();
        }
    }
}
