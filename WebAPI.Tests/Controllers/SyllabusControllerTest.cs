using Domains.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

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
    }

}


        
