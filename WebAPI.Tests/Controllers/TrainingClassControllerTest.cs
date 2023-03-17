using Application.ViewModels.TrainingClassModels;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    public class TrainingClassControllerTest : SetupTest
    {
        private readonly TrainingClassController _trainingClassController;

        public TrainingClassControllerTest()
        {
            _trainingClassController = new TrainingClassController(_trainingClassServiceMock.Object, _claimsServiceMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnOK_WhenSavedSucceed()
        {
            //arrange
            var mock = _fixture.Build<CreateTrainingClassDTO>().Without(x => x.Attendee).Without(x => x.TimeFrame).Create();

            var expected = _fixture.Build<TrainingClassViewModel>().Create();
            _trainingClassServiceMock.Setup(
                x => x.CreateTrainingClassAsync(It.IsAny<CreateTrainingClassDTO>()))
                .ReturnsAsync(expected);

            //act
            var result = await _trainingClassController.CreateTrainingClass(mock);

            //assert
            _trainingClassServiceMock.Verify(x => x.CreateTrainingClassAsync(It.Is<CreateTrainingClassDTO>(x => x.Equals(mock))), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            (result as OkObjectResult)!.Value.Should().Be(expected);
        }

        [Fact]
        public async Task CreateTrainingClass_ShouldReturnBadRequest_WhenSavedFail()
        {
            //arrange
            var mock = _fixture.Build<CreateTrainingClassDTO>().Without(x => x.Attendees).Without(x => x.TimeFrame).Create();

            TrainingClassViewModel expected = null!;
            _trainingClassServiceMock.Setup(
                x => x.CreateTrainingClassAsync(It.IsAny<CreateTrainingClassDTO>()))
                .ReturnsAsync(expected);

            //act
            var result = await _trainingClassController.CreateTrainingClass(mock);

            //assert
            _trainingClassServiceMock.Verify(x => x.CreateTrainingClassAsync(It.Is<CreateTrainingClassDTO>(x => x.Equals(mock))), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().BeEquivalentTo("Create training class fail: Saving fail");
        }
        [Fact]
        public async Task CreateTrainingClass_ShouldReturnBadRequest_WhenGetException()
        {
            //arrange
            var mock = _fixture.Build<CreateTrainingClassDTO>().Without(x => x.Attendees).Without(x => x.TimeFrame).Create();

            var exceptionMessage = "test message";

            _trainingClassServiceMock.Setup(
                x => x.CreateTrainingClassAsync(It.IsAny<CreateTrainingClassDTO>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            //act
            var result = await _trainingClassController.CreateTrainingClass(mock);

            //assert
            _trainingClassServiceMock.Verify(x => x.CreateTrainingClassAsync(It.Is<CreateTrainingClassDTO>(x => x.Equals(mock))), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().Be("Create training class fail: " + exceptionMessage);
        }

        [Fact]
        public async Task SoftRemoveTrainingClass_ShouldReturnOK_WhenSoftRemoveResultIsTrue()
        {
            //arrange
            var mockId = Guid.NewGuid();
            _trainingClassServiceMock
                .Setup(x => x.SoftRemoveTrainingClassAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            //act
            var result = await _trainingClassController.SoftRemoveTrainingClass(mockId.ToString());

            //assert
            _trainingClassServiceMock.Verify(x => x.SoftRemoveTrainingClassAsync(
                It.Is<string>(x => x.Equals(mockId.ToString()))
                ), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            (result as OkObjectResult)!.Value.Should().Be("SoftRemove class successfully");
        }

        [Fact]
        public async Task SoftRemoveTrainingClass_ShouldReturnBadRequest_WhenSoftRemoveResultIsFalse()
        {
            //arrange
            var mockId = Guid.NewGuid();
            _trainingClassServiceMock
                .Setup(x => x.SoftRemoveTrainingClassAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            //act
            var result = await _trainingClassController.SoftRemoveTrainingClass(mockId.ToString());

            //assert
            _trainingClassServiceMock.Verify(x => x.SoftRemoveTrainingClassAsync(
                It.Is<string>(x => x.Equals(mockId.ToString()))
                ), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().Be("SoftRemove class fail: Saving fail");
        }
        [Fact]
        public async Task SoftRemoveTrainingClass_ShouldReturnBadRequest_WhenGetException()
        {
            //arrange
            var mockId = Guid.NewGuid();
            var exceptionMessage = "test message";
            _trainingClassServiceMock
                .Setup(x => x.SoftRemoveTrainingClassAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception(exceptionMessage));
            //act
            var result = await _trainingClassController.SoftRemoveTrainingClass(mockId.ToString());

            //assert
            _trainingClassServiceMock.Verify(x => x.SoftRemoveTrainingClassAsync(
                It.IsAny<string>()
                ), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().Be("SoftRemove class fail: " + exceptionMessage);
        }

        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnOK_WhenUpdateResultIsTrue()
        {
            //arrange
            var mockId = Guid.NewGuid();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().Create();
            _trainingClassServiceMock
                .Setup(x => x.UpdateTrainingClassAsync(It.IsAny<string>(), It.IsAny<UpdateTrainingCLassDTO>()))
                .ReturnsAsync(true);

            //act
            var result = await _trainingClassController.UpdateTrainingClass(mockId.ToString(), mockUpdate);

            //assert
            _trainingClassServiceMock.Verify(x => x.UpdateTrainingClassAsync(
                It.Is<string>(x => x.Equals(mockId.ToString())),
                It.Is<UpdateTrainingCLassDTO>(x => x.Equals(mockUpdate))
                ), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            (result as OkObjectResult)!.Value.Should().Be("Update class successfully");
        }

        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnBadRequest_WhenUpdateResultIsFalse()
        {
            //arrange
            var mockId = Guid.NewGuid();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().Create();
            _trainingClassServiceMock
                .Setup(x => x.UpdateTrainingClassAsync(It.IsAny<string>(), It.IsAny<UpdateTrainingCLassDTO>()))
                .ReturnsAsync(false);

            //act
            var result = await _trainingClassController.UpdateTrainingClass(mockId.ToString(), mockUpdate);

            //assert
            _trainingClassServiceMock.Verify(x => x.UpdateTrainingClassAsync(
                It.Is<string>(x => x.Equals(mockId.ToString())),
                It.Is<UpdateTrainingCLassDTO>(x => x.Equals(mockUpdate))
                ), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().Be("Update class fail: Saving fail");
        }

        [Fact]
        public async Task UpdateTrainingClass_ShouldReturnBadRequest_WhenGetAutoMappingException()
        {
            //arrange
            var mockId = Guid.NewGuid();
            var mockUpdate = _fixture.Build<UpdateTrainingCLassDTO>().Create();
            var exceptionMessage = "test message";
            _trainingClassServiceMock
                .Setup(x => x.UpdateTrainingClassAsync(It.IsAny<string>(), It.IsAny<UpdateTrainingCLassDTO>()))
                .ThrowsAsync(new Exception(exceptionMessage));
            //act
            var result = await _trainingClassController.UpdateTrainingClass(mockId.ToString(), mockUpdate);

            //assert
            _trainingClassServiceMock.Verify(x => x.UpdateTrainingClassAsync(
                It.IsAny<string>(),
                It.IsAny<UpdateTrainingCLassDTO>()
                ), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
            (result as BadRequestObjectResult)!.Value.Should().Be("Update class fail: " + exceptionMessage);
        }
        [Fact]
        public async  Task ViewDetailTrainingClass_ShouldOk_WhenFindSuccess()
        {
            //Arrange
            var mockData=_fixture.Create<FinalTrainingClassDTO>();
            Guid mockId = new Guid();
            _trainingClassServiceMock.Setup(s => s.GetFinalTrainingClassesAsync(mockId)).ReturnsAsync(mockData);
            //Act 
            var result =await  _trainingClassController.GetTrainingClassDetail(mockId);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
