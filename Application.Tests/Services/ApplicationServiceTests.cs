using Application.Commons;
using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Repositories;
using Application.Services;
using Application.ViewModels.AtttendanceModels;
using AutoFixture;
using Domain.Entities;
using Domain.Enums;
using Domains.Test;
using FluentAssertions;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using static Domain.Enums.Application.ApplicationFilterByEnum;
namespace Application.Tests.Services
{
    public class ApplicationServiceTests : SetupTest
    {
        private readonly IApplicationService _applicationService;
        public ApplicationServiceTests()
        {
            _applicationService = new ApplicationService(_unitOfWorkMock.Object, _mapperConfig, configuration, _currentTimeMock.Object, _claimsServiceMock.Object);
            _unitOfWorkMock.Setup(x => x.ApplicationRepository).Returns(_applicationRepositoryMock.Object);
            _unitOfWorkMock.Setup(x => x.TrainingClassRepository).Returns(_trainingClassRepositoryMock.Object);

            _applicationRepositoryMock.Setup(x => x.AddRangeAsync(It.IsAny<List<Applications>>())).Verifiable();
            _applicationRepositoryMock.Setup(x => x.UpdateRange(It.IsAny<List<Applications>>())).Verifiable();
            _applicationRepositoryMock.Setup(x => x.Update(It.IsAny<Applications>())).Verifiable();

            _unitOfWorkMock.Setup(s => s.SaveChangeAsync()).ReturnsAsync(1);
        }
        [Fact]
        public async Task GetAllApplication_ShouldReturnCorrectValue()
        {
            ApplicationFilterDTO filter = new();
            Guid classId = default;
            var mockData_ListOf10 = _fixture.Build<Applications>().OmitAutoProperties().CreateMany(10).ToList();
            Pagination<Applications> mockData = new()
            {
                Items = mockData_ListOf10,
                PageIndex = filter.PageNumber,
                PageSize = 10,
                TotalItemsCount = 100
            };
            _applicationRepositoryMock.Setup(x => x.ToPagination(It.IsAny<Expression<Func<Applications, bool>>>(), filter.PageNumber, filter.PageSize)).ReturnsAsync(mockData);
            // Act
            Pagination<Applications> result = await _applicationService.GetAllApplication(classId, filter);
            // Assert
            result.Should().Be(mockData);
            result.PageIndex.Should().Be(filter.PageNumber);
            result.PageSize.Should().Be(filter.PageSize);
        }
    }
}
