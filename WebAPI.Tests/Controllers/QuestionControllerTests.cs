using Application.ViewModels.QuizModels;
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
    public class QuestionControllerTests : SetupTest
    {
        public readonly QuestionController _questionController;
        public QuestionControllerTests()
        {
            _questionController = new QuestionController(_questionServiceMock.Object);
        }
        [Fact]
        public async Task AddQuestionIntoBank_ShouldReturnCorrectValue()
        {
            // Setup
            _questionServiceMock.Setup(x => x.AddQuestionToBank(It.IsNotNull<CreateQuizIntoBankDTO>())).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.AddQuestionToBank(It.Is<CreateQuizIntoBankDTO>(x => x == null))).ReturnsAsync(false);
            // Act
            var result = await _questionController.AddQuestionIntoBank(new CreateQuizIntoBankDTO());
            var result_badRequest = await _questionController.AddQuestionIntoBank(null);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task CreateEmptyQuizTest_ShouldReturnCorrectValue()
        {
            // Setup
            _questionServiceMock.Setup(x => x.CreateEmptyQuizTest(It.IsNotNull<CreateEmptyQuizDTO>())).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.CreateEmptyQuizTest(It.Is<CreateEmptyQuizDTO>(x => x == null))).ReturnsAsync(false);
            // Act
            var result = await _questionController.CreateEmptyQuizTest(new CreateEmptyQuizDTO());
            var result_badRequest = await _questionController.CreateEmptyQuizTest(null);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        public async Task AddQuestionToQuizTest_ShouldReturnCorrectValue()
        {
            // Setup
            _questionServiceMock.Setup(x => x.AddQuestionToQuizTest(It.IsNotNull<AddQuestionToQuizTestDTO>())).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.AddQuestionToQuizTest(It.Is<AddQuestionToQuizTestDTO>(x => x == null))).ReturnsAsync(false);
            // Act
            var result = await _questionController.AddQuestionToQuizTest(new AddQuestionToQuizTestDTO());
            var result_badRequest = await _questionController.AddQuestionToQuizTest(null);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task UpdateQuizTest_ShouldReturnCorrectValue()
        {
            // Setup
            _questionServiceMock.Setup(x => x.UpdateQuizTest(It.IsAny<Guid>(), It.IsNotNull<UpdateQuizTestDTO>())).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.UpdateQuizTest(It.IsAny<Guid>(), It.Is<UpdateQuizTestDTO>(x => x == null))).ReturnsAsync(false);
            // Act
            var result = await _questionController.UpdateQuizTest(Guid.NewGuid(), new UpdateQuizTestDTO());
            var result_badRequest = await _questionController.UpdateQuizTest(Guid.NewGuid(), null);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task DeleteQuizTest_ShouldReturnCorrectValue()
        {
            // Setup
            _questionServiceMock.Setup(x => x.DeleteQuizTest(It.IsAny<Guid>())).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.DeleteQuizTest(It.Is<Guid>(x => x == Guid.Empty))).ReturnsAsync(false);
            // Act
            var result = await _questionController.DeleteQuizTest(Guid.NewGuid());
            var result_badRequest = await _questionController.DeleteQuizTest(Guid.Empty);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SearchByName_ShouldReturnCorrectValue()
        {
            // Setup
            var listName = new List<Question>();
            _questionServiceMock.Setup(x => x.Search(It.IsAny<string>())).ReturnsAsync(null as List<Question>);
            _questionServiceMock.Setup(x => x.Search(It.Is<string>(x => x == ""))).ReturnsAsync(listName);

            // Act
            var result = await _questionController.SearchByName("");
            var result_badRequest = await _questionController.SearchByName("comsuon");

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(listName);
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task FilterQuizBank_ShouldReturnCorrectValue()
        {
            // Setup
            FilterQuizModel filter = new FilterQuizModel()
            {
                bun = new List<Guid>(),
                comsuon = new List<int>()
            };
            FilterQuizModel filter_badrequest = new FilterQuizModel()
            {
                bun = null,
                comsuon = null
            };
            var listName = new List<Question?>();
            _questionServiceMock.Setup(x => x.Filter(filter.bun, filter.comsuon)).ReturnsAsync(listName);
            _questionServiceMock.Setup(x => x.Filter(It.Is<List<Guid>>(x => x == null), It.Is<List<int>>(x => x == null))).ReturnsAsync(null as List<Question?>);

            // Act
            var result = await _questionController.FilterQuizBank(filter);
            var result_badRequest = await _questionController.FilterQuizBank(filter_badrequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(listName);
            result_badRequest.Should().BeOfType<BadRequestObjectResult>();
        }
        [Fact]
        public async Task ViewQuiz_ShouldReturnCorrectValue()
        {
            // Setup
            var QuizView = new DoingQuizDTO();
            _questionServiceMock.Setup(x => x.ViewDoingQuiz(It.IsAny<Guid>())).ReturnsAsync(QuizView);
            _questionServiceMock.Setup(x => x.ViewDoingQuiz(It.Is<Guid>(x => x == Guid.Empty))).ReturnsAsync(null as DoingQuizDTO);

            // Act
            var result = await _questionController.ViewQuiz(Guid.NewGuid());
            var result_badRequest = await _questionController.ViewQuiz(Guid.Empty);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(QuizView);
            result_badRequest.Should().BeOfType<BadRequestResult>();
        }
        [Fact]
        public async Task DoingQuiz_ShouldReturnCorrectValue()
        {
            // Setup
            var doingQuizDTOs = new List<AnswerQuizQuestionDTO>();
            var quizDtos = new AnswerQuizQuestionDTO()
            {
                QuizID = new Guid()
            };
            doingQuizDTOs.Add(quizDtos);
            var mark = 10;
            _questionServiceMock.Setup(x => x.DoingQuizService(doingQuizDTOs)).ReturnsAsync(true);
            _questionServiceMock.Setup(x => x.MarkQuiz(quizDtos.QuizID, It.IsAny<Guid>())).ReturnsAsync(mark);
            // Act
            var result = await _questionController.DoingQuiz(doingQuizDTOs, Guid.NewGuid());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(mark);
        }
        [Fact]
        public async Task ViewDetailResult_ShouldReturnCorrectValue()
        {
            // Setup
            var markDetails = new List<ViewDetailResultDTO>();
            _questionServiceMock.Setup(x => x.ViewMarkDetail(It.IsAny<Guid>())).ReturnsAsync(markDetails);
            // Act
            var result = await _questionController.ViewDetailResult(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(markDetails);
        }
    }
}
