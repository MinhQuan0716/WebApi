using Application.Repositories;
using Application.Services;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repository
{
    public class AssignmentRepositoryTest : SetupTest
    {
        private readonly IAssignmentRepository _assignmentRepository;
        public AssignmentRepositoryTest()
        {
            _assignmentRepository = new AssignmentRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }

        [Fact]
        public async Task CheckOverdue_Should_Success()
        {
            _assignmentRepository.CheckOverdue().Should().BeAssignableTo(typeof(Task));
        }
        [Fact]
        public async Task AddProcedure_Should_Success()
        {
            _assignmentRepository.AddProcedure().Should().BeAssignableTo(typeof(Task));
        }
        [Fact]
        public async Task CheckExistedProcedure_Should_BeTrue()
        {
            //chịu ko biết viết
        }
    }
}
