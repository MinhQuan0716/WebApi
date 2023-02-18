using Application.ViewModels.UserViewModels;
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

namespace WebAPI.Tests.ControllerT
{
    public class SyllabusControllerTest : SetupTest
    {
        private readonly SyllabusController _syllabusController;
        public SyllabusControllerTest()
        {
            _syllabusController = new SyllabusController(_syllabusServiceMock.Object);
        }


        [Fact]
        public async Task SearchNameSyllabus_Get_ShuuldReturnCorrectValues()
        {
            
            //_fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            //   .ForEach(b => _fixture.Behaviors.Remove(b));
            //_fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            List<Syllabus> mockData_1 = _fixture.Build<Syllabus>().Without(u => u.Units).Without(u => u.User).CreateMany(10).ToList();
            await _dbContext.AddRangeAsync(mockData_1);
            await _dbContext.SaveChangesAsync();
            
            string name1 = "anything";
            //string name2 = "this shout return nothing";
            _syllabusServiceMock.Setup(s => s.GetByName(name1)).ReturnsAsync(mockData_1);
            //_syllabusServiceMock.Setup(s => s.GetByName(name2)).ReturnsAsync(mockData_2);
            var result = await _syllabusController.Get(name1);

            result.Should().BeOfType<OkObjectResult>();


        }
    }
}
