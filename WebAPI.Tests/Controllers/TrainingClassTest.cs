using Application;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Application.ViewModels.TrainingClassModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using FluentAssertions.Common;
using Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Writers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class TrainingClassTest : SetupTest
    {
        private readonly TrainingClassController _trainingClassController;

        public TrainingClassTest()
        {
            _trainingClassController = new TrainingClassController(_trainingClassServiceMock.Object,_claimsServiceMock.Object,_mapperConfig);

        }

        [Fact]
        public async Task SearchClassByName_Get_ShuuldReturnCorrectValues()
        {
            List<TrainingClass> trainingClasses = _fixture.Build<TrainingClass>()
                                                          .OmitAutoProperties()
                                                          .With(x => x.Code)
                                                          .With(x => x.Branch)
                                                          .With(x => x.StatusClassDetail)
                                                          .With(x => x.Attendee)
                                                          .With(x => x.Name)
                                                          .CreateMany(3)
                                                          .ToList();
            await _dbContext.AddRangeAsync(trainingClasses);
            await _dbContext.SaveChangesAsync();

            string name1 = "anything";

            _trainingClassServiceMock.Setup(s => s.SearchClassByName(name1)).ReturnsAsync(trainingClasses);

            var result = await _trainingClassController.SearchClassByName(name1);
            result.Should().BeOfType<OkObjectResult>();


        }
        [Fact]
        public async Task DuplicateClass_ShouldReturnOkObjectResult_WhenValidIdIsProvided()
        {
            var existingClassId = Guid.NewGuid();
            var existingClass = _fixture.Build<TrainingClass>().
                OmitAutoProperties()
                .With(x=> x.Name)
                .With(x=> x.StartTime)
                .With(x=> x.EndTime)
                .With(x=> x.Code)
                .With(x=> x.Duration)
                .With(x=> x.Attendee)
                .With(x=> x.Branch) 
                .With(x => x.Id, existingClassId).Create();
            await _dbContext.SaveChangesAsync();

            _trainingClassServiceMock.Setup(s => s.DuplicateClass(existingClassId)).ReturnsAsync(true);

            // Act
            var result = await _trainingClassController.DuplicateClass(existingClassId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllTraningClass_ReturnsOk_WhenListResultIsNotNull()
        {
            // Arrange
            List<TrainingClassDTO> trainingClasses = _fixture.CreateMany<TrainingClassDTO>(3).ToList();
            _trainingClassServiceMock.Setup(s => s.GetAllTrainingClassesAsync()).ReturnsAsync(trainingClasses);

            // Act
            var result = await _trainingClassController.GetAllTraningClass();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task FilterResult_ReturnsNotFound_WhenFilterResultIsEmpty()
        {
            string[] locationName = { "New York" };
            DateTime date1 = DateTime.Now.AddDays(-7);
            DateTime date2 = DateTime.Now.AddDays(-1);
            string[] statusClass = { "New" };
            string[] attendee = { "None" };
            string branchName = "new";
            var nonEmptyResult = _fixture.Build<TrainingClassDTO>()
                .CreateMany(10).ToList();
            var mockData = _fixture.Build<TrainingClassFilterModel>().Create();

            _trainingClassServiceMock.Setup(x => x.FilterLocation(locationName, branchName, date1, date2, attendee, statusClass)).ReturnsAsync(nonEmptyResult);

            // Act
            var result = await _trainingClassController.FilterResult(mockData);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task FilterResult_ReturnsOkObject_WhenFilterResultIsNotEmpty()
        {
            // Arrange
            string[] locationName = { "Ftown2" };
            DateTime date1 = DateTime.Parse("2023/02/28");
            DateTime date2 = DateTime.Parse("2023/03/30");
            string[] statusClass = { "Active" };
            string[] attendee = { "50" };
            string branchName = "stringstring";
            var nonEmptyResult = _fixture.Build<TrainingClassDTO>()
                .CreateMany(10).ToList() ;
            var mockData = new TrainingClassFilterModel()
            {
               branchName= branchName,
               locationName= locationName,
               date1= date1,
               date2= date2,
               attendInClass=attendee,
               classStatus=statusClass
            };
           _trainingClassServiceMock.Setup(x => x.FilterLocation(locationName, branchName, date1, date2,statusClass,attendee)).ReturnsAsync(nonEmptyResult);

            // Act
            var result = await _trainingClassController.FilterResult(mockData);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
           // var okFilterResult = Assert.IsType<OkObjectResult>(filterResult);
            var model = Assert.IsAssignableFrom<IEnumerable<TrainingClassDTO>>(okObjectResult.Value);
        }
    }
}


