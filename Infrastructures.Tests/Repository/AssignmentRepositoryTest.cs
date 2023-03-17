using Application.Repositories;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Tests.Repository
{
    public class AssignmentRepositoryTest:SetupTest
    {
        private readonly IAssignmentRepository _assignmentRepository;
        public AssignmentRepositoryTest() 
        {
            _assignmentRepository= new AssignmetRepository(_dbContext,_currentTimeMock.Object,_claimsServiceMock.Object);
        }

        [Fact]
        public async Task CheckOverdue_ShouldSaveChange()
        {
           //Chịu Không biết viết
        }
    }
}
