using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
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
    public class AttendanceControllerTest : SetupTest
    {
        private readonly AttendanceController _attendancecontroller;

        public AttendanceControllerTest()
        {
            _attendancecontroller = new AttendanceController(_attendanceServiceMock.Object);
        }
        [Fact]
        public async Task GetAttendanceByClassId_Should_ReturnData()
        {
            // Arange

            var mockclass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.Location)
                .Without(x => x.Applications).Create();
            var mockAttendence = _fixture.Build<Attendance>().Without(x => x.Application).Without(x => x.TrainingClass).Without(x => x.User).With(x => x.TrainingClass, mockclass).CreateMany(1).ToList();
            _attendanceServiceMock.Setup(x => x.GetAttendancesByTraineeClassID(It.IsAny<Guid>())).ReturnsAsync(mockAttendence);
            // Act
            var result = await _attendancecontroller.GetAttendanceByClassId(mockclass.Id);
            // Assert
            Assert.NotNull(result);
            _attendanceServiceMock.Verify(x => x.GetAttendancesByTraineeClassID(It.Is<Guid>(x => x.Equals(mockclass.Id))), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            ((OkObjectResult)result)!.Value.Should().Be(mockAttendence);
        }

        [Fact]
        public async Task GetAttendanceByTraineeId_Should_ReturnData()
        {
            var mockclass = _fixture.Build<TrainingClass>()
                .Without(x => x.TrainingClassParticipates)
                .Without(x => x.Attendances)
                .Without(x => x.Feedbacks)
                .Without(x => x.TrainingProgram)
                .Without(x => x.Location)
                .Without(x => x.Applications).Create();
            var mockAttendance = _fixture.Build<Attendance>().With(x => x.TrainingClass, mockclass).Without(x => x.Application).Without(x => x.TrainingClass).Without(x => x.User).CreateMany(2).ToList();
            _attendanceServiceMock.Setup(x => x.GetAttendanceByTraineeID(It.IsAny<Guid>())).ReturnsAsync(mockAttendance);

            //act
            var result = await _attendancecontroller.GetAttendanceByTraineeId(mockclass.Id);

            //assert
            Assert.NotNull(result);
            _attendanceServiceMock.Verify(x => x.GetAttendanceByTraineeID(It.Is<Guid>(x => x.Equals(mockclass.Id))), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            ((OkObjectResult)result)!.Value.Should().Be(mockAttendance);
        }
    }
}