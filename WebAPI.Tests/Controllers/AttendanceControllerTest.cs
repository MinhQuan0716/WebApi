﻿using AutoFixture;
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
            var mockclass = _fixture.Build<TrainingClass>().Create();
            var mockAttendence = _fixture.Build<Attendance>().With(x => x.TrainingClass, mockclass).CreateMany().ToList();
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
        public async Task GetAttendanceByTraineeId_Sould_ReturnData()
        {
            var mockclass = _fixture.Build<TrainingClass>().Create();
            var mockAttendance = _fixture.Build<Attendance>().With(XmlAssertionExtensions => XmlAssertionExtensions.TrainingClass, mockclass).CreateMany().ToList();
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