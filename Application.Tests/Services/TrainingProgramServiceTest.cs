using Application.Services;
using Application.ViewModels.TrainingProgramModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Services
{
    public class TrainingProgramServiceTest : SetupTest
    {
        private readonly TrainingProgramService trainingProgramService;
        public TrainingProgramServiceTest()
        {
            trainingProgramService = new TrainingProgramService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact] 
        public async Task GetTrainingProgramDetail_ShoudlReturnCorrectData()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create<TrainingProgram>();
            trainingProgram.IsDeleted = false;
            var syllabuses = (ICollection<Syllabus>) _fixture.Build<Syllabus>().Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.Units).Without(x => x.User).CreateMany<Syllabus>(2).ToList();
            var trainingProgramView = _mapperConfig.Map<TrainingProgramViewModel>(trainingProgram);
            trainingProgramView.Syllabuses = syllabuses;

            
            _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetSyllabusByTrainingProgramId(trainingProgram.Id)).ReturnsAsync(syllabuses);
            _unitOfWorkMock.Setup(um => um.TrainingProgramRepository.GetByIdAsync(trainingProgram.Id)).ReturnsAsync(trainingProgram);
            var result = await trainingProgramService.GetTrainingProgramDetail(trainingProgram.Id);
            result.Should().BeEquivalentTo(trainingProgramView);
                
        }

        [Fact]
        public async Task GetTrainingProgramDetail_ShouldReturnNull()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create<TrainingProgram>();
            var trainingProgramId = trainingProgram.Id;
            _unitOfWorkMock.Setup(um => um.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(trainingProgram = null);
            var result = await trainingProgramService.GetTrainingProgramDetail(trainingProgramId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnCorrectDate()
        {
            var createTrainingProgramDTO = _fixture.Build<UpdateTrainingProgramDTO>().Without(ct => ct.SyllabusesId).Create();
            createTrainingProgramDTO.SyllabusesId = new List<Guid>();
            var syllabuses = _fixture.Build<Syllabus>().Without(x => x.Units).Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.User).CreateMany<Syllabus>(2);
            foreach (var item in syllabuses) createTrainingProgramDTO.SyllabusesId.Add(item.Id);
            foreach (var item in syllabuses) _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(item.Id)).ReturnsAsync(item);
            _unitOfWorkMock.Setup(um => um.DetailTrainingProgramSyllabusRepository.AddAsync(It.IsAny<DetailTrainingProgramSyllabus>())).Verifiable();
            var trainingProgram = _mapperConfig.Map<TrainingProgram>(createTrainingProgramDTO);
            _unitOfWorkMock.Setup(um => um.TrainingProgramRepository.AddAsync(trainingProgram)).Verifiable();
            _unitOfWorkMock.Setup(um => um.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult =  await trainingProgramService.CreateTrainingProgram(createTrainingProgramDTO);
            actualResult.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateTrainingProgram_ShouldReturnNull()
        {
            var createTrainingProgramDTO = _fixture.Build<UpdateTrainingProgramDTO>().Without(x => x.SyllabusesId).Create();
            var result = await trainingProgramService.CreateTrainingProgram(createTrainingProgramDTO);
            result.Should().BeNull();
        }


        [Fact]
        public async Task UpdateTrainingProgram_ShouldReturnTrue()
        {
            var updateDTO = _fixture.Build<UpdateTrainingProgramDTO>().Without(x => x.SyllabusesId).Create();
            updateDTO.SyllabusesId = new List<Guid>();
            var detailProgramSyllabuses = _fixture.Build<DetailTrainingProgramSyllabus>().Without(x => x.Syllabus).Without(x => x.TrainingProgram).CreateMany<DetailTrainingProgramSyllabus>(1);
            var updateProgram = _mapperConfig.Map<TrainingProgram>(updateDTO);
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.Update(updateProgram)).Verifiable();
            var syllabuses = _fixture.Build<Syllabus>().Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.Units).Without(x => x.User).CreateMany<Syllabus>(2);
            foreach (var item in syllabuses) updateDTO.SyllabusesId.Add(item.Id);
            foreach (var item in syllabuses) _unitOfWorkMock.Setup(um => um.SyllabusRepository.GetByIdAsync(item.Id)).ReturnsAsync(item);
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(updateProgram);
            _unitOfWorkMock.Setup(m => m.DetailTrainingProgramSyllabusRepository.FindAsync(s => s.TrainingProgramId == updateProgram.Id)).ReturnsAsync(detailProgramSyllabuses.ToList());
            _unitOfWorkMock.Setup(m => m.DetailTrainingProgramSyllabusRepository.SoftRemoveRange(It.IsAny<List<DetailTrainingProgramSyllabus>>())).Verifiable();
            _unitOfWorkMock.Setup(m => m.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await trainingProgramService.UpdateTrainingProgram(updateDTO);
            actualResult.Should().BeTrue();

        }

        [Fact]
        public async Task UpdateTrainingProgram_ProgramNull_ShouldReturnFalse()
        {
            var updateDTO = _fixture.Build<UpdateTrainingProgramDTO>().Without(x => x.SyllabusesId).Create();
            var trainingProg = _mapperConfig.Map<TrainingProgram>(updateDTO);
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.GetByIdAsync(updateDTO.Id.Value)).ReturnsAsync(trainingProg = null);

            var actualResult = await trainingProgramService.UpdateTrainingProgram(updateDTO);
            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteTrainingProgram_ShouldReturnTrue()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.DetailTrainingProgramSyllabus).Without(x => x.TrainingClasses).Create();
            var detailTrainingSyllabus = _fixture.Build<DetailTrainingProgramSyllabus>().Without(x => x.TrainingProgram).Without(x => x.Syllabus).CreateMany(3);
            foreach (var detail in detailTrainingSyllabus) detail.TrainingProgramId = trainingProgram.Id;
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.GetByIdAsync(trainingProgram.Id)).ReturnsAsync(trainingProgram);
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.SoftRemove(It.IsAny<TrainingProgram>())).Verifiable();
            _unitOfWorkMock.Setup(m => m.DetailTrainingProgramSyllabusRepository.FindAsync(x => x.TrainingProgramId == trainingProgram.Id)).ReturnsAsync(detailTrainingSyllabus.ToList());
            _unitOfWorkMock.Setup(m => m.DetailTrainingProgramSyllabusRepository.SoftRemoveRange(It.IsAny<List<DetailTrainingProgramSyllabus>>())).Verifiable();
            _unitOfWorkMock.Setup(m => m.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await trainingProgramService.DeleteTrainingProgram(trainingProgram.Id);
            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTrainingProgram_ShouldReturnFalse()
        {
            var trainingProgram = _fixture.Build<TrainingProgram>().Without(x => x.TrainingClasses).Without(x => x.DetailTrainingProgramSyllabus).Create();
            var id = trainingProgram.Id;
            _unitOfWorkMock.Setup(m => m.TrainingProgramRepository.GetByIdAsync(trainingProgram.Id)).ReturnsAsync(trainingProgram = null);
            var actualResult = await trainingProgramService.DeleteTrainingProgram(id);
            actualResult.Should().BeFalse();
        }

    }
}
