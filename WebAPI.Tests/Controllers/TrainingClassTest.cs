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
            // Arrange
            string[] locationName = { "New York" };
            DateTime date1 = DateTime.Now.AddDays(-7);
            DateTime date2 = DateTime.Now.AddDays(-1);
            var emptyResult = new List<TrainingClassDTO>();
            _trainingClassServiceMock.Setup(x => x.FilterLocation(locationName, date1, date2)).ReturnsAsync(emptyResult);

            // Act
            var result = await _trainingClassController.FilterResult(locationName, date1, date2);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task FilterResult_ReturnsOkObject_WhenFilterResultIsNotEmpty()
        {
            // Arrange
            string[] locationName = { "New York" };
            DateTime date1 = DateTime.Now.AddDays(-7);
            DateTime date2 = DateTime.Now.AddDays(-1);
            var nonEmptyResult = _fixture.Build<TrainingClassDTO>()
                .CreateMany(10).ToList() ;
            _trainingClassServiceMock.Setup(x => x.FilterLocation(locationName, date1, date2)).ReturnsAsync(nonEmptyResult);

            // Act
            var result = await _trainingClassController.FilterResult(locationName, date1, date2);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<TrainingClassDTO>>(okObjectResult.Value);
        }
    }
}


