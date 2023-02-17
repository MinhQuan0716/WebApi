using Application.Services;
using Application.Interfaces;
using Application.Repositories;
using Application.Utils;
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

namespace Infrastructures.Tests.Repository
{
    public class UsersRepositoryTests : SetupTest
    {
        private readonly IUserService _userService;
        public UsersRepositoryTests()
        {
            _userService = new UserService(_unitOfWorkMock.Object,
                                           _mapperConfig,
                                           _currentTimeMock.Object,
                                           configuration);
        }

    }
}
