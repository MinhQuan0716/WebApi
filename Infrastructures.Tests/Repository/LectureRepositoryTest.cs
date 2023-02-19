using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Infrastructures.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Tests.Repository
{
    public class LectureRepositoryTest : SetupTest
    {
        private readonly LectureRepository _lectureRepository;

        public LectureRepositoryTest()
        {
            _lectureRepository = new LectureRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task GetLectureBySyllabusId_ShouldReturnCorrectData()
        {

            var syllabusMockData = _fixture.Build<Syllabus>().With(s => s.Units).Create();
            
            await _dbContext.AddAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();

            var result = await _lectureRepository.GetLectureBySyllabusId(syllabusMockData.Id);
            result.Should().BeOfType<List<Lecture>>();
            result.Count().Should().BeGreaterThan(0);
            
            
        }

    }
}
