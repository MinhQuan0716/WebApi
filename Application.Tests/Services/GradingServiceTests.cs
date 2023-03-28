using Application.Interfaces;
using Application.Services;
using Application.ViewModels.GradingModels;
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
    public class GradingServiceTests : SetupTest
    {
        private readonly IGradingService _gradingService;
        public GradingServiceTests()
        {
            //_gradingService = new GradingService(_unitOfWorkMock.Object,_mapperConfig,_currentTimeMock.Object,_appConfigurationMock.Object);
            _gradingService = new GradingService(_unitOfWorkMock.Object, _mapperConfig, _currentTimeMock.Object, _appConfigurationMock.Object, _claimsServiceMock.Object);

        }

        [Fact]
        public async Task AddToGrading_ShouldBeTrue()
        {
            var checkLecture = _fixture.Build<Lecture>()
                .Without(x => x.Assignments)
                .Without(x => x.DetailUnitLectures)
                .Without(x => x.TrainingMaterials)
                .Without(x => x.AuditPlans)
                .Without(x => x.Gradings)
                .Without(x => x.Quiz)
                .Create();
            var grading = _fixture.Build<Grading>().Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.Lecture)
                .Create();
            var gradingMapper = _mapperConfig.Map<GradingModel>(grading);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(grading.LectureId)).ReturnsAsync(checkLecture);

            var detailTrainingClass = _fixture.Build<DetailTrainingClassParticipate>()
                .Without(x => x.TrainingClass)
                //.Without(x => x.LocationName)
                .Without(x => x.User)
                .Create();
            _unitOfWorkMock.Setup(x => x.DetailTrainingClassParticipate.GetByIdAsync(grading.DetailTrainingClassParticipateId))
                .ReturnsAsync(detailTrainingClass);

            _unitOfWorkMock.Setup(x => x.GradingRepository.AddAsync(grading)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await _gradingService.AddToGrading(gradingMapper);


            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task AddToGrading_ShouldBeFalse_checkDetailTrainingNull()
        {
            var checkLecture = new Lecture();
            var grading = _fixture.Build<Grading>().Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.Lecture)
                .Create();
            var gradingMapper = _mapperConfig.Map<GradingModel>(grading);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(grading.LectureId)).ReturnsAsync(checkLecture);

            var detailTrainingClass = _fixture.Build<DetailTrainingClassParticipate>()
                .Without(x => x.TrainingClass)
                // .Without(x => x.LocationName)
                .Without(x => x.User)
                .Create();
            _unitOfWorkMock.Setup(x => x.DetailTrainingClassParticipate.GetByIdAsync(grading.DetailTrainingClassParticipateId))
                .ReturnsAsync(detailTrainingClass = null);


            var actualResult = await _gradingService.AddToGrading(gradingMapper);


            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddToGrading_ShouldBeFalse_checkLectureNull()
        {
            var checkLecture = new Lecture();
            var grading = _fixture.Build<Grading>().Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.Lecture)
                .Create();
            var gradingMapper = _mapperConfig.Map<GradingModel>(grading);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(grading.LectureId)).ReturnsAsync(checkLecture = null);

            var detailTrainingClass = _fixture.Build<DetailTrainingClassParticipate>()
                .Without(x => x.TrainingClass)
                //.Without(x => x.LocationName)
                .Without(x => x.User)
                .Create();
            _unitOfWorkMock.Setup(x => x.DetailTrainingClassParticipate.GetByIdAsync(grading.DetailTrainingClassParticipateId))
                .ReturnsAsync(detailTrainingClass);

            var actualResult = await _gradingService.AddToGrading(gradingMapper);


            actualResult.Should().BeFalse();
        }

        [Fact]
        public async Task AddToGrading_ShouldBeFalse_SaveChange0()
        {
            var checkLecture = new Lecture();
            var grading = _fixture.Build<Grading>().Without(x => x.DetailTrainingClassParticipate)
                .Without(x => x.Lecture)
                .Create();
            var gradingMapper = _mapperConfig.Map<GradingModel>(grading);
            _unitOfWorkMock.Setup(x => x.LectureRepository.GetByIdAsync(grading.LectureId)).ReturnsAsync(checkLecture);

            var detailTrainingClass = _fixture.Build<DetailTrainingClassParticipate>()
                .Without(x => x.TrainingClass)
                //.Without(x => x.LocationName)
                .Without(x => x.User)
                .Create();
            _unitOfWorkMock.Setup(x => x.DetailTrainingClassParticipate.GetByIdAsync(grading.DetailTrainingClassParticipateId))
                .ReturnsAsync(detailTrainingClass);

            _unitOfWorkMock.Setup(x => x.GradingRepository.AddAsync(grading)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var actualResult = await _gradingService.AddToGrading(gradingMapper);

            actualResult.Should().BeFalse();
        }
    }
}
