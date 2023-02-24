using Application.ViewModels.TrainingClassModels;
using AutoFixture;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;

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
            var mock = _fixture.Build<CreateTrainingClassDTO>().Create();

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
            var mock = _fixture.Build<CreateTrainingClassDTO>().Create();

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
            var mock = _fixture.Build<CreateTrainingClassDTO>().Create();

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
    }
}
