using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.ViewModels.AssignmentModel;
using AutoFixture;
using Domain.Entities;
using Domains.Test;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Application.Tests.Services
{
    public class AssignmentServiceTests : SetupTest
    {
        private readonly IAssignmentService _assignmentService;
        private readonly Mock<IFormFile> file;
        private readonly Guid id = Guid.NewGuid();

        public AssignmentServiceTests()
        {
            _assignmentService = new AssignmentService(_unitOfWorkMock.Object, _mapperConfig,_claimsServiceMock.Object);
            //Mock File
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net7.0", string.Empty));
            var filePath = dirName + "\\Resources\\Assignments\\ctgbate.jpg";
            file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(filePath);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sourceImg);
            writer.Flush();
            stream.Position = 0;
            var fileName = "ctgbate.jpg";
            file.Setup(f => f.OpenReadStream()).Returns(stream);
            file.Setup(f => f.FileName).Returns(fileName);
            file.Setup(f => f.Length).Returns(stream.Length);
            var contentType = "image/jpeg";
            string val = "form-data; name=";

            val += "\"";
            val += "files";
            val += "\"";
            val += "; filename=";
            val += "\"";
            val += "ctgbate.jpg";
            val += "\"";
            file.Setup(f => f.ContentType).Returns(contentType);
            file.Setup(f => f.ContentDisposition).Returns(val);
        }

        [Fact]
        public async Task CreateAssignment_ReturnTrue()
        {
            var listAssignment = _fixture.Build<Assignment>().Without(x => x.Lecture).Without(x => x.AssignmentSubmissions).CreateMany(0).ToList();
            var assignmentView = _fixture.Build<AssignmentViewModel>().Without(x => x.File).Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.FindAsync(x => x.LectureID == assignmentView.LectureID && x.IsDeleted == false && x.IsOverDue == false)).ReturnsAsync(listAssignment);

            assignmentView.DeadLine = DateTime.Today.AddDays(1);
            assignmentView.File = file.Object;
            var mapAssignment = _mapperConfig.Map<Assignment>(assignmentView);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddAsync(mapAssignment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await _assignmentService.CreateAssignment(assignmentView);

            actualResult.Should().BeTrue();

        }

        [Fact]
        public async Task CreateAssignment_ReturnFalse_WhenSaveChangFalse()
        {
            var listAssignment = new List<Assignment>();
            var assignment = _fixture.Build<AssignmentViewModel>().Without(x => x.File).Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.FindAsync(x => x.LectureID == assignment.LectureID && x.IsDeleted == false && x.IsOverDue == false)).ReturnsAsync(listAssignment);
            assignment.DeadLine = DateTime.Today.AddDays(1);
            assignment.File = file.Object;
            var mapAssignment = _mapperConfig.Map<Assignment>(assignment);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.AddAsync(mapAssignment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var actualResult = await _assignmentService.CreateAssignment(assignment);

            actualResult.Should().BeFalse();

        }

        [Fact]
        public async Task CreateAssignment_ThrowException_WhenOutofDate()
        {
            var listAssignment = new List<Assignment>();
            var assignment = _fixture.Build<AssignmentViewModel>().Without(x => x.File).Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.FindAsync(x => x.LectureID == assignment.LectureID && x.IsDeleted == false && x.IsOverDue == false)).ReturnsAsync(listAssignment);
            assignment.DeadLine = DateTime.Today.AddDays(-1);

            Func<Task> act = async () => await _assignmentService.CreateAssignment(assignment);

            await act.Should().ThrowAsync<Exception>();

        }
        [Fact]
        public async Task CreateAssignment_ThrowException_WhenAssignmentExisted()
        {
            var listAssignment = _fixture.Build<Assignment>().Without(x => x.Lecture).Without(x => x.AssignmentSubmissions).CreateMany(2).ToList();
            var assignment = _fixture.Build<AssignmentViewModel>().Without(x => x.File).Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.FindAsync(x => x.LectureID == assignment.LectureID && x.IsDeleted == false && x.IsOverDue == false)).ReturnsAsync(listAssignment);

            Func<Task> act = async () => await _assignmentService.CreateAssignment(assignment);

            await act.Should().ThrowAsync<Exception>();

        }
        [Fact]
        public async Task DeleteAssignment_ReturnTrue()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(id)).ReturnsAsync(assignment);
            assignment.IsDeleted = false;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(assignment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await _assignmentService.DeleteAssignment(id);

            actualResult.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAssignment_ThrowException_WhenAssignmentIsDeleted()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            assignment.IsDeleted = true;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(id)).ReturnsAsync(assignment = null);

            Func<Task> act = async () => await _assignmentService.DeleteAssignment(id);

            await act.Should().ThrowAsync<Exception>();
        }
        [Fact]
        public async Task DeleteAssignment_ReturnFalse_WhenSaveChangeFalse()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            assignment.IsDeleted = false;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(id)).ReturnsAsync(assignment);
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(assignment)).Verifiable();
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var actualResult = await _assignmentService.DeleteAssignment(id);

            actualResult.Should().BeFalse();
        }
        [Fact]
        public async Task UpdateAssignment_ReturnTrue()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.DeadLine).Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            assignment.DeadLine = DateTime.Now.AddDays(1);
            assignment.Version= 0;
            assignment.CreatedBy = Guid.Empty;
            var assignmentUpdate = _fixture.Build<AssignmentUpdateModel>().Without(x => x.File).Create();
            assignmentUpdate.File = file.Object;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentUpdate.AssignmentID)).ReturnsAsync(assignment);

            _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(assignment)).Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(1);

            var actualResult = await _assignmentService.UpdateAssignment(assignmentUpdate);

            actualResult.Should().BeTrue();
               
        }

        [Fact]
        public async Task UpdateAssignment_ReturnFalse()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.DeadLine).Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            assignment.DeadLine = DateTime.Now.AddDays(1);
            assignment.Version = 0;
            assignment.CreatedBy = Guid.Empty;
            var assignmentUpdate = _fixture.Build<AssignmentUpdateModel>().Without(x => x.File).Create();
            assignmentUpdate.File = file.Object;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentUpdate.AssignmentID)).ReturnsAsync(assignment);

            _unitOfWorkMock.Setup(x => x.AssignmentRepository.Update(assignment)).Verifiable();

            _unitOfWorkMock.Setup(x => x.SaveChangeAsync()).ReturnsAsync(0);

            var actualResult = await _assignmentService.UpdateAssignment(assignmentUpdate);

            actualResult.Should().BeFalse();

        }

        [Fact]
        public async Task UpdateAssignment_ThrowException_WhenDeadlineSmallerthanToday()
        {
            var assignmentUpdate = _fixture.Build<AssignmentUpdateModel>().Without(x => x.File).Create();
            assignmentUpdate.File = file.Object;
            Assignment checkAssignment = null;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignmentUpdate.AssignmentID)).ReturnsAsync(checkAssignment);
            Func<Task> act = async () => await _assignmentService.UpdateAssignment(assignmentUpdate);

            await act.Should().ThrowAsync<Exception>();
        }


        [Fact]
        public async Task Download_ReturnFile()
        {
            var dirName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\net7.0", string.Empty));
            var assignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();
            var fileName =dirName+ "\\Resources\\Assignments\\1_00000000-0000-0000-0000-000000000000_ctgbate.jpg";
            assignment.FileName=fileName;
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment);
            var actualResult = await _assignmentService.DownLoad(assignment.Id);
            actualResult.Should().BeOfType<FileEntity>();
        }

        [Fact]
        public async Task Download_ThrowException_WhenAssignmentNull()
        {
            var assignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).Create();


            _unitOfWorkMock.Setup(x => x.AssignmentRepository.GetByIdAsync(assignment.Id)).ReturnsAsync(assignment = null);
            //_dbContext.Add(assignment);
            Func<Task> act = async () => await _assignmentService.DownLoad(id);
            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]

        public async Task GetAllAssignmentByLecture_ReturnAssignment()
        {
            Guid id = Guid.NewGuid();
            var listAssignment = _fixture.Build<Assignment>().Without(x => x.AssignmentSubmissions).Without(a => a.Lecture).CreateMany(2).ToList();

            _unitOfWorkMock.Setup(x => x.AssignmentRepository.FindAsync(a => a.LectureID == id && a.IsDeleted == false && a.IsOverDue == false, a => a.AssignmentSubmissions)).ReturnsAsync(listAssignment);

            var actualResult = await _assignmentService.GetAllAssignmentByLectureID(id);

            actualResult.Should().BeOfType<List<Assignment>>();
        }

        [Fact]
        public async Task CheckOverdue()
        {
            _unitOfWorkMock.Setup(x => x.AssignmentRepository.CheckOverdue()).Verifiable();
            await _assignmentService.CheckOverDue();
            _unitOfWorkMock.Verify(x => x.AssignmentRepository.CheckOverdue(), Times.Once);
        }

    }
}
