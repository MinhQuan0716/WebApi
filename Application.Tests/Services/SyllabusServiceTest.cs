using Application.Services;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    public class SyllabusServiceTest : SetupTest
    {
        private readonly SyllabusService _syllabusService;
        public SyllabusServiceTest()
        {
            _syllabusService = new SyllabusService(_unitOfWorkMock.Object, _claimsServiceMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task UpdateSyllabus_ShoudlReturnTrue()
        {
            // Assert
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            var syllabus = _mapperConfig.Map<Syllabus>(updateSyllabusDTO);
            syllabus.Id = Guid.NewGuid();
            var lectures = _fixture.Build<Lecture>()
                .Without(x => x.TrainingMaterials)
                .Without(x => x.AuditPlans)
                .Without(x => x.DetailUnitLectures)
                .Without(x => x.AuditPlans)
                .Without(x => x.Gradings)
                .Without(x => x.Quiz)
                .CreateMany<Lecture>();

            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            _unitOfWorkMock.Setup(um => um.LectureRepository.GetLectureBySyllabusId(syllabus.Id)).ReturnsAsync(lectures);
            _unitOfWorkMock.Setup(um => um.LectureRepository.Update(It.IsAny<Lecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.Update(It.IsAny<Syllabus>())).Verifiable();

            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, updateSyllabusDTO);
            result.Should().BeTrue();

        }
        [Fact]
        public async Task UpdateSyllabus_UpdateLec_ShoudlReturnTrue()
        {
            // Assert
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            var syllabus = _mapperConfig.Map<Syllabus>(updateSyllabusDTO);
            syllabus.Id = Guid.NewGuid();
            
            var lecturesDTO = updateSyllabusDTO.Units.First().UpdateLectureDTOs;
            var lectures = _mapperConfig.Map<List<Lecture>>(lecturesDTO);


            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            _unitOfWorkMock.Setup(um => um.LectureRepository.GetLectureBySyllabusId(syllabus.Id)).ReturnsAsync(lectures);
            _unitOfWorkMock.Setup(um => um.LectureRepository.Update(It.IsAny<Lecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.Update(It.IsAny<Syllabus>())).Verifiable();

            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, updateSyllabusDTO);
            result.Should().BeTrue();

        }
        [Fact]
        public async Task UpdateSyllabus_SaveChangesFail_ShouldReturnFalse() 
        {
            var updateSyllabusDTO = _fixture.Create<UpdateSyllabusDTO>();
            var syllabus = _mapperConfig.Map<Syllabus>(updateSyllabusDTO);
            syllabus.Id = Guid.NewGuid();
            var lectures = _fixture.Build<Lecture>()
                  .Without(x => x.TrainingMaterials)
                  .Without(x => x.AuditPlans)
                  .Without(x => x.DetailUnitLectures)
                  .Without(x => x.Gradings)
                  .Without(x => x.Quiz).CreateMany<Lecture>();

            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(0);
            _unitOfWorkMock.Setup(um => um.LectureRepository.GetLectureBySyllabusId(syllabus.Id)).ReturnsAsync(lectures);
            _unitOfWorkMock.Setup(um => um.LectureRepository.Update(It.IsAny<Lecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.Update(It.IsAny<Syllabus>())).Verifiable();

            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, updateSyllabusDTO);
          
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateSyllabus_CreateNewLecture_ShouldReturnTrue()
        {
            var updateSyllabusDTO = _fixture.Build<UpdateSyllabusDTO>().Create();
            updateSyllabusDTO.Units.ToList().First().UpdateLectureDTOs.First().LectureID = null;
            var syllabus = _mapperConfig.Map<Syllabus>(updateSyllabusDTO);
            var lectures = _fixture.Build<Lecture>()
                .Without(x => x.TrainingMaterials)
                .Without(x => x.AuditPlans)
                .Without(x => x.DetailUnitLectures)
                .Without(x => x.Gradings)
                .Without(x => x.Quiz).CreateMany<Lecture>();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(syllabus.Id)).ReturnsAsync(syllabus);
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            _unitOfWorkMock.Setup(um => um.DetailUnitLectureRepository.AddAsync(It.IsAny<DetailUnitLecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.LectureRepository.GetLectureBySyllabusId(syllabus.Id)).ReturnsAsync(lectures);
            _unitOfWorkMock.Setup(um => um.LectureRepository.Update(It.IsAny<Lecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.Update(It.IsAny<Syllabus>())).Verifiable();

            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, updateSyllabusDTO);

            result.Should().BeTrue();
        }


        [Fact]
        public async Task UpdateSyllabus_CreateNewUnit_ShouldReturnTrue()
        {
            var updateSyllabusDTO = _fixture.Build<UpdateSyllabusDTO>().Create();
            updateSyllabusDTO.Units.First().UnitID = null;
            var syllabus = _fixture.Build<Syllabus>().Without(x => x.Units).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var lectures = _fixture.Build<Lecture>()
               .Without(x => x.TrainingMaterials)
               .Without(x => x.AuditPlans)
               .Without(x => x.DetailUnitLectures)
               .Without(x => x.AuditPlans)
               .Without(x => x.Gradings)
               .Without(x => x.Quiz)
               .CreateMany<Lecture>();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(syllabus);
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);
            _unitOfWorkMock.Setup(um => um.DetailUnitLectureRepository.AddAsync(It.IsAny<DetailUnitLecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.LectureRepository.GetLectureBySyllabusId(syllabus.Id)).ReturnsAsync(lectures);
            _unitOfWorkMock.Setup(um => um.LectureRepository.Update(It.IsAny<Lecture>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.Update(It.IsAny<Syllabus>())).Verifiable();
            _unitOfWorkMock.Setup(um => um.UnitRepository.AddRangeAsync(It.IsAny<List<Unit>>())).Verifiable();    

            var result = await _syllabusService.UpdateSyllabus(syllabus.Id, updateSyllabusDTO);

            result.Should().BeTrue();
        }
    }
}
