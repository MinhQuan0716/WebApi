using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using AutoFixture;
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
using Xunit.Sdk;

namespace WebAPI.Tests.Controllers
{
 
   public  class SyllabusControllerTest:SetupTest
    {
        private readonly SyllabusController syllabusController;
        public SyllabusControllerTest()
        {

            syllabusController = new SyllabusController(_syllabusServiceMock.Object);
        }
        [Fact]
        public async Task GetAllSyllabus_Should_ReturnData()
        {
            //Arrange

            //Act
            var result = await syllabusController.GetAllSyllabus();
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task FilterSyllabus_Should_ReturnData()
        {
            //Arrange
            double firstDuration = 9;
            double secondDuration = 12;
            //Act
            var filterResult = syllabusController.FilerSyllabus(firstDuration, secondDuration);
            //Assert
            Assert.NotNull(filterResult);
        }

        [Fact]
        public async Task UpdateSyllabus_ShouldReturnNoContent()
        {
            Guid id = Guid.NewGuid();
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            _syllabusServiceMock.Setup(sm => sm.UpdateSyllabus(id, updateSyllabusDTO)).ReturnsAsync(true);
            var result = await syllabusController.UpdateSyllabus(id , updateSyllabusDTO);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateSyllabus_ShouldReturnBadRequest()
        {
            Guid id = Guid.NewGuid();
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            _syllabusServiceMock.Setup(sm => sm.UpdateSyllabus(id, updateSyllabusDTO)).ReturnsAsync(false);
            var result = await syllabusController.UpdateSyllabus(id, updateSyllabusDTO);
            result.Should().BeOfType<BadRequestObjectResult>();
            var objResult = result as BadRequestObjectResult;
            if(objResult is not null)
            objResult.Value.Should().BeSameAs("Update Failed");
        }


    }

}


        
