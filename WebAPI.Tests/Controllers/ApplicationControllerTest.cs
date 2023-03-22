using Application.Commons;
using Application.ViewModels;
using Application.ViewModels.ApplicationViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.Enums.Application;
using Domains.Test;
using FluentAssertions;
using FluentAssertions.Common;
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
    public class ApplicationControllerTest : SetupTest
    {
        private readonly ApplicationController _applicationController;

        public ApplicationControllerTest()
        {

            _applicationController = new ApplicationController(_applicationServiceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnNoContentResult()
        {
            var model = _fixture.Build<ApplicationDTO>().Create();
            // Act
            var result = await _applicationController.CreateApplication(model);
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();

        }
        [Fact]
        public async Task ViewAllApplication_ShouldReturnCorrectValue()
        {
            //Setup
            ApplicationDateTimeFilterDTO condition = _fixture.Build<ApplicationDateTimeFilterDTO>().Create();
            string searchString = null;
            string by = null;
            Guid classId = default;
            int pageNumber = 0;
            int pageSize = 0;
            var mockData = _fixture.Build<Pagination<Applications>>().Without(x => x.Items).Create();

            _applicationServiceMock.Setup(x => x.GetAllApplication(classId,condition, searchString, by, pageNumber, pageSize)).ReturnsAsync(mockData);
            // Act
            var result = await _applicationController.ViewAllApplication(classId, condition, searchString, by, pageNumber, pageSize);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(mockData);
        }
    }
}