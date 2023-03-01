using Application.ViewModels.SyllabusModels;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
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
using Xunit.Sdk;

namespace WebAPI.Tests.Controllers
{

    public class SyllabusControllerTest : SetupTest
    {
        private readonly SyllabusController _syllabusController;
        public SyllabusControllerTest()
        {

            _syllabusController = new SyllabusController(_syllabusServiceMock.Object, _unitServiceMock.Object, _lectureServiceMock.Object);
            
        }
        [Fact]
        public async Task SearchNameSyllabus_Get_ShuuldReturnCorrectValues()
        {
            //_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            //   .ForEach(b => _fixture.Behaviors.Remove(b));
            //_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            List<Syllabus> mockData_1 = _fixture.Build<Syllabus>().Without(x => x.DetailTrainingProgramSyllabus).Without(u => u.Units).Without(u => u.User).CreateMany(5).ToList();
            await _dbContext.AddRangeAsync(mockData_1);
            await _dbContext.SaveChangesAsync();

            string name1 = "anything";
            //string name2 = "this shout return nothing";
            _syllabusServiceMock.Setup(s => s.GetByName(name1)).ReturnsAsync(mockData_1);
            //_syllabusServiceMock.Setup(s => s.GetByName(name2)).ReturnsAsync(mockData_2);
            var result = await _syllabusController.Get(name1);

            result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task GetAllSyllabus_Should_ReturnData()
        {
            //Arrange

            //Act
            var result = await _syllabusController.GetAllSyllabus();
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
            var filterResult = _syllabusController.FilerSyllabus(firstDuration, secondDuration);
            //Assert
            Assert.NotNull(filterResult);
        }
        [Fact]
        public async Task UpdateSyllabus_ShouldReturnNoContent()
        {
            Guid id = Guid.NewGuid();
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            _syllabusServiceMock.Setup(sm => sm.UpdateSyllabus(id, updateSyllabusDTO)).ReturnsAsync(true);
            var result = await _syllabusController.UpdateSyllabus(id, updateSyllabusDTO);
            result.Should().BeOfType<NoContentResult>();
        }
        [Fact]
        public async Task UpdateSyllabus_ShouldReturnBadRequest()
        {
            Guid id = Guid.NewGuid();
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            _syllabusServiceMock.Setup(sm => sm.UpdateSyllabus(id, updateSyllabusDTO)).ReturnsAsync(false);
            var result = await _syllabusController.UpdateSyllabus(id, updateSyllabusDTO);
            result.Should().BeOfType<BadRequestObjectResult>();
            var objResult = result as BadRequestObjectResult;
            if (objResult is not null)
                objResult.Value.Should().BeSameAs("Update Failed");
        }
        [Fact]
        public async Task AddNewSyllabus_ShouldReturnCorrectData()
        {
            //Arrange
            var mockData = _fixture.Build<SyllabusViewDTO>().Without(u => u.Units).Create();

            var units = mockData.Units = _fixture.Build<UnitDTO>()
                                                .Without(l => l.Lectures)
                                                .CreateMany(1)
                                                .ToList();
            var lectures = mockData.Units.First().Lectures = _fixture.Build<LectureDTO>()
                                                                    .CreateMany(1)
                                                                    .ToList();
            var syllabus = _fixture.Build<Syllabus>().Without(s=>s.Units).Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.User).Create();
            var detailUnitLectures = _fixture.Build<DetailUnitLecture>()
                                             .Without(u => u.Unit).Without(u => u.Lecture)
                                             .Create();
            _syllabusServiceMock.Setup(s => s.AddSyllabusAsync(mockData.SyllabusBase))
                                .ReturnsAsync(syllabus);
            _unitServiceMock.Setup(s => s.AddNewUnit(mockData.Units.First(), syllabus))
                            .ReturnsAsync(detailUnitLectures.Unit);
            _lectureServiceMock.Setup(l => l.AddNewLecture(lectures.First()))
                               .ReturnsAsync(detailUnitLectures.Lecture);
            _lectureServiceMock.Setup(l => l.AddNewDetailLecture(detailUnitLectures.Lecture,
                                                                 detailUnitLectures.Unit)).ReturnsAsync(detailUnitLectures);
            //Act
            var result = await _syllabusController.AddNewSyllabus(mockData);
        }
        [Fact]
        public async Task AddNewSyllabus_ShouldReturnBadResult()
        { 
            //Arrange
            var mockData_1 = _fixture.Build<SyllabusViewDTO>().Without(u => u.Units).Create();
            var mockData_2 = _fixture.Build<SyllabusViewDTO>().With(u => u.Units).Create();
            //Act
            var result = await _syllabusController.AddNewSyllabus(mockData_1);
            //Assert
            result.Should().BeOfType<BadRequestResult>();
                  
            


        }
    }

}



