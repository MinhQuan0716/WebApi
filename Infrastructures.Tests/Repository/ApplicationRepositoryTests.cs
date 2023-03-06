using Application;
using Application.Repositories;
using Application.ViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Application.ApplicationFilterByEnum;
using static System.Net.Mime.MediaTypeNames;
using FluentAssertions;

namespace Infrastructures.Tests.Repository
{
    public class ApplicationRepositoryTests : SetupTest
    {
        private readonly IApplicationRepository _applicationRepository;
        public ApplicationRepositoryTests()
        {
            _applicationRepository = new ApplicationRepository(_dbContext, _currentTimeMock.Object, _claimsServiceMock.Object);
        }
        /// <summary>
        /// This Test is 2 second long 
        /// if you can improve it i will allow it
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task ToPagination_ShouldReturnCorrectValue()
        {

            #region CreatingData
            var trainingClassMock = _fixture.Build<TrainingClass>()
                .OmitAutoProperties()
                .With(x => x.Name)
                .With(x=>x.Attendee)
                .With(x=>x.StatusClassDetail)
                .With(x=>x.Branch)
                .Create();
            _dbContext.Add(trainingClassMock);
            _dbContext.SaveChanges();
            var user_1_Mock = _fixture.Build<User>()
                .OmitAutoProperties()
                .With(x => x.UserName)
                .With(x => x.PasswordHash)
                .With(x => x.AvatarUrl)
                .With(x => x.FullName)
                .With(x => x.Email)
                .With(x => x.IsDeleted,false)
                .With(x => x.Gender)
                .With(x => x.RoleId, 1)
                .Create();
            var user_2_Mock = _fixture.Build<User>()
                .OmitAutoProperties()
                .With(x => x.UserName)
                .With(x => x.PasswordHash)
                .With(x => x.AvatarUrl)
                .With(x => x.FullName)
                .With(x => x.Email)
                .With(x => x.IsDeleted, false)
                .With(x => x.Gender)
                .With(x => x.RoleId, 1)
                .Create();
            _dbContext.Add(user_1_Mock);
            _dbContext.Add(user_2_Mock);
            _dbContext.SaveChanges();
            var applicationList = new List<Applications>();
            for (int i = 0; i < 10; i++)
            {
                var application = _fixture.Build<Applications>().OmitAutoProperties().With(x => x.Reason).Create();
                application.UserId = user_1_Mock.Id;
                application.TrainingClassId = trainingClassMock.Id;
                application.Reason = "a-bzy-c";
                application.CreationDate = DateTime.Parse("01/01/2023").AddDays(1);
                applicationList.Add(application);

            }
            for (int i = 0; i < 10; i++)
            {
                var application = _fixture.Build<Applications>().OmitAutoProperties().With(x => x.Reason).Create();
                application.UserId = user_2_Mock.Id;
                application.TrainingClassId = trainingClassMock.Id;
                application.Reason = "a-bzy";
                if (i > 4)
                    application.AbsentDateRequested = DateTime.Parse("01/01/2023").AddDays(1);
                applicationList.Add(application);
            }



            _dbContext.AddRange(applicationList);
            _dbContext.SaveChanges();
            #endregion
            // Setup
            string searchString = "";
            #region conditionSettings
            var condition_empty = new ApplicationDateTimeFilterDTO()
            {
                AbsentDateRequested = DateTime.UtcNow,
                Approved = null,
                UserID = Guid.Empty,
                Reason = searchString
            };
            var condition_user_1 = new ApplicationDateTimeFilterDTO()
            {
                AbsentDateRequested = DateTime.UtcNow,
                Approved = null,
                UserID = user_1_Mock.Id,
                Reason = searchString
            };
            var condition_contains = new ApplicationDateTimeFilterDTO()
            {
                AbsentDateRequested = DateTime.UtcNow,
                Approved = null,
                UserID = Guid.Empty,
                Reason = searchString
            };
            var condition_dateTime = new ApplicationDateTimeFilterDTO()
            {
                Approved = null,
                UserID = Guid.Empty,
                Reason = searchString,
            };
            condition_dateTime.FromDate = DateTime.Parse("01/01/2023").AddDays(1);
            condition_dateTime.AbsentDateRequested = condition_dateTime.FromDate.AddDays(1);
            condition_dateTime.ToDate = condition_dateTime.AbsentDateRequested.AddDays(1);
            ApplicationDateTimeFilterDTO condition = null;
            string by = nameof(CreationDate);
            #endregion
            Guid classId = Guid.Empty;
            Expression<Func<Applications, bool>> expression = x =>
                x.Reason.Contains(searchString)
                // 3 condition below are checking if it is null if not it will be used to check the value
                && (condition.Approved != null && condition.Approved == x.Approved || condition.Approved == null)
                && (condition.UserID != Guid.Empty && condition.UserID == x.UserId || condition.UserID == Guid.Empty)
                && (classId != Guid.Empty && classId == x.TrainingClassId || classId == Guid.Empty)
                &&
                (
                    (by == nameof(CreationDate) && x.CreationDate >= condition.FromDate && x.CreationDate <= condition.ToDate)
                    || (by == nameof(RequestDate) && x.AbsentDateRequested >= condition.FromDate && x.AbsentDateRequested <= condition.ToDate)
                );
            //Act & assert
            condition = condition_empty;
            var applications = await _applicationRepository.ToPagination(expression: expression, 0, 10);

            condition = condition_user_1;
            var applicationsOfUser_1 = await _applicationRepository.ToPagination(expression: expression, 0, 10);

            condition = condition_contains;
            searchString = "a-b";
            var applicationsOfSearch = await _applicationRepository.ToPagination(expression: expression, 0, 10);

            condition = condition_dateTime;
            var applicationsOfDatetime = await _applicationRepository.ToPagination(expression: expression, 0, 10);

            by = nameof(RequestDate);
            condition.UserID = user_2_Mock.Id;
            classId = trainingClassMock.Id;
            var applicationsCombine = await _applicationRepository.ToPagination(expression: expression, 0, 10);

            // Assert
            applications.Items.Should().NotBeEmpty();
            applications.TotalPagesCount.Should().Be(2);
            applications.TotalItemsCount.Should().Be(20);
            applications.TotalItemsCount.Should().Be(20);
            applicationsOfUser_1.TotalItemsCount.Should().Be(10);
            applicationsOfSearch.TotalItemsCount.Should().Be(20);
            applicationsOfDatetime.TotalItemsCount.Should().Be(10);
            applicationsCombine.Items.Should().NotBeEmpty();
            applicationsCombine.Items.Count.Should().Be(5);
        }
    }
}
