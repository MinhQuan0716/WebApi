using Application.ViewModels.TrainingProgramModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class TrainingProgramControllerTest : SetupTest
    {
        private readonly TrainingProgramController trainingProgramController;
        public TrainingProgramControllerTest()
        {
            trainingProgramController = new TrainingProgramController(_trainingProgramServiceMock.Object);

        }

        [Fact]
        public async Task GetDetailTrainingProgram_ShouldReturnOk()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.TrainingClasses).Create<TrainingProgram>();
            var trainingProgramView = _mapperConfig.Map<TrainingProgramViewModel>(trainingProgram);
            var trainingProgramId = trainingProgram.Id;
            _trainingProgramServiceMock.Setup(tp => tp.GetTrainingProgramDetail(trainingProgramId)).ReturnsAsync(trainingProgramView);

            var result = await trainingProgramController.GetDetail(trainingProgram.Id);
            result.Should().BeOfType(typeof(OkObjectResult));
            

        }

        [Fact]
        public async Task GetDetailTrainingProgram_ShouldReturnBadRequest()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.TrainingClasses).Create<TrainingProgram>();
            var trainingProgramId = trainingProgram.Id;
            var trainingProgramView = _mapperConfig.Map<TrainingProgramViewModel>(trainingProgram);
            _trainingProgramServiceMock.Setup(tp => tp.GetTrainingProgramDetail(trainingProgramId)).ReturnsAsync(trainingProgramView = null);
            var result = await trainingProgramController.GetDetail(trainingProgram.Id);
            result.Should().BeOfType(typeof(BadRequestResult));
            
        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturn201()
        {
            var createTrainingDTO = _fixture.Build<UpdateTrainingProgramDTO>().Create();
            var trainingProgram = _mapperConfig.Map<TrainingProgram>(createTrainingDTO);
            _trainingProgramServiceMock.Setup(tp => tp.CreateTrainingProgram(createTrainingDTO)).ReturnsAsync(trainingProgram);
            var result = await trainingProgramController.Create(createTrainingDTO);
            result.Should().BeAssignableTo<CreatedAtActionResult>();
        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturn400()
        {
            var createTrainingDTO = _fixture.Build<UpdateTrainingProgramDTO>().Create();
            var trainingProgram = _mapperConfig.Map<TrainingProgram>(createTrainingDTO = null);
            var result = await trainingProgramController.Create(createTrainingDTO);
            result.Should().BeAssignableTo<BadRequestResult>();
        }
        [Fact]
        public async Task UpdateTrainingProgram_ShouldReturn204()
        {
            var updateProgramDTO = _fixture.Build<UpdateTrainingProgramDTO>().Create();
            _trainingProgramServiceMock.Setup(m => m.UpdateTrainingProgram(It.IsAny<UpdateTrainingProgramDTO>())).ReturnsAsync(true);

            var actualResult = await trainingProgramController.Update(updateProgramDTO);
            actualResult.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task UpdateTrainingProgram_ShouldReturn400()
        {
            var updateProgramDTO = _fixture.Build<UpdateTrainingProgramDTO>().Create();
            _trainingProgramServiceMock.Setup(m => m.UpdateTrainingProgram(It.IsAny<UpdateTrainingProgramDTO>())).ReturnsAsync(false);

            var actualResult = await trainingProgramController.Update(updateProgramDTO);
            actualResult.Should().BeAssignableTo<BadRequestResult>();
        }


    }
}
