using Application.ViewModels.AuditModels.AuditSubmissionModels.CreateModels;
using Application.ViewModels.AuditModels.AuditSubmissionModels.UpdateModels;
using Application.ViewModels.AuditModels.AuditSubmissionModels.ViewModels;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI.Tests.Controllers
{
    public class AuditSubmissionControllerTest : SetupTest
    {
        private readonly AuditSubmissionController auditSubmissionController;

        public AuditSubmissionControllerTest()
        {
            auditSubmissionController = new AuditSubmissionController(_auditSubmissionServiceMock.Object, _gradingServiceMock.Object, _auditPlanServiceMock.Object);
        }

/*        [Fact]
        public async Task Create_ShouldReturn200()
        {
            var audit = _fixture.Build<AuditSubmission>().Without(x => x.DetailAuditSubmissions).Without(x => x.AuditPlan).Create();
            _auditSubmissionServiceMock.Setup(x => x.CreateAuditSubmission(It.IsAny<CreateAuditSubmissionDTO>())).ReturnsAsync(audit);
            var result = await auditSubmissionController.Create(_fixture.Build<CreateAuditSubmissionDTO>().Create());
            result.Should().BeAssignableTo<OkResult>();
        }*/

/*        [Fact]
        public async Task Create_ShouldReturn400()
        {
            var audit = _fixture.Build<AuditSubmission>().Without(x => x.DetailAuditSubmissions).Without(x => x.AuditPlan).Create();
            _auditSubmissionServiceMock.Setup(x => x.CreateAuditSubmission(It.IsAny<CreateAuditSubmissionDTO>())).ReturnsAsync(audit = null);
            var result = await auditSubmissionController.Create(_fixture.Build<CreateAuditSubmissionDTO>().Create());
            *//*result.Should().BeAssignableTo<BadRequestResult>();
        }*/

        [Fact]
        public async Task GetDetail_ShouldReturn200()
        {
            var audit = _fixture.Build<AuditSubmissionViewModel>().Without(x => x.DetailAuditSubmisisonViewModel).Without(x => x.DetailAuditSubmisisonViewModel).Create();
            _auditSubmissionServiceMock.Setup(x => x.GetAuditSubmissionDetail(It.IsAny<Guid>())).ReturnsAsync(audit);
            var result = await auditSubmissionController.GetDetail(It.IsAny<Guid>());
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task GetDetail_ShouldReturn400()
        {
            var audit = _fixture.Build<AuditSubmissionViewModel>().Without(x => x.DetailAuditSubmisisonViewModel).Without(x => x.DetailAuditSubmisisonViewModel).Create();
            _auditSubmissionServiceMock.Setup(x => x.GetAuditSubmissionDetail(It.IsAny<Guid>())).ReturnsAsync(audit = null);
            var result = await auditSubmissionController.GetDetail(It.IsAny<Guid>());
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            _auditSubmissionServiceMock.Setup(x => x.DeleteSubmissionDetail(It.IsAny<Guid>())).ReturnsAsync(true);
            var result = await auditSubmissionController.Delete(It.IsAny<Guid>());
            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest()
        {
            _auditSubmissionServiceMock.Setup(x => x.DeleteSubmissionDetail(It.IsAny<Guid>())).ReturnsAsync(false);
            var result = await auditSubmissionController.Delete(It.IsAny<Guid>());
            result.Should().BeAssignableTo<BadRequestResult>();
        }
        [Fact]
        public async Task Update_ShouldReturnBadRequest()
        {
            _auditSubmissionServiceMock.Setup(x => x.UpdateSubmissionDetail(It.IsAny<UpdateSubmissionDTO>())).ReturnsAsync(false);
            var result = await auditSubmissionController.Update(It.IsAny<UpdateSubmissionDTO>());
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent()
        {
            _auditSubmissionServiceMock.Setup(x => x.UpdateSubmissionDetail(It.IsAny<UpdateSubmissionDTO>())).ReturnsAsync(true);
            var result = await auditSubmissionController.Update(It.IsAny<UpdateSubmissionDTO>());
            result.Should().BeAssignableTo<NoContentResult>();
        }
    }
}
