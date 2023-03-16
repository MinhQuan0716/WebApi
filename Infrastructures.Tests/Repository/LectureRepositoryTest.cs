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



    }
}
