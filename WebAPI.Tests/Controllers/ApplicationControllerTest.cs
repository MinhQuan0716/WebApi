using Application.ViewModels.ApplicationViewModels;
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
            result.Should().BeOfType<NoContentResult>();

        }
    }
}