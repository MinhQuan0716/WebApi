using Application.Interfaces;
using Application.ViewModels.QuizModels;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IQuestionService _quizService;
        public TestController(IQuestionService quizService) => _quizService = quizService;



        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> AddQuestionIntoBank(CreateQuizIntoBankDTO quizObject)
        {
            var checkAddSuccesfully = await _quizService.AddQuestionToBank(quizObject);
            if (checkAddSuccesfully)
            {
                return Ok("Add quiz into bank successfully!");
            }
            return BadRequest("Fail to add quiz!");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> CreateEmptyQuizTest(CreateEmptyQuizDTO quizObject)
        {
            var checkAddSuccesfully = await _quizService.CreateEmptyQuizTest(quizObject);
            if (checkAddSuccesfully)
            {
                return Ok("Create quiz test successfully!");
            }
            return BadRequest("Fail to create quiz test!");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> AddQuestionToQuizTest(AddQuestionToQuizTestDTO quizObject)
        {
            var checkAddSuccesfully = await _quizService.AddQuestionToQuizTest(quizObject);
            if (checkAddSuccesfully)
            {
                return Ok("Add question successfully!");
            }
            return BadRequest("Fail to add question!");
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> UpdateQuizTest(Guid quizTestId, UpdateQuizTestDTO quizDto)
        {
            var checkUpdateSuccesfully = await _quizService.UpdateQuizTest(quizTestId, quizDto);
            if (checkUpdateSuccesfully)
            {
                return Ok("Update quiz test successfully!");
            }
            return BadRequest("Fail to update quiz test!");
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> DeleteQuizTest(Guid quizTestId)
        {
            var checkDeleteSuccesfully = await _quizService.DeleteQuizTest(quizTestId);
            if (checkDeleteSuccesfully)
            {
                return Ok("Delete quiz test successfully!");
            }
            return BadRequest("Fail to delete quiz test!");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> SearchByName(string ContentName)
        {
            var search = await _quizService.Search(ContentName);
            if (search is not null)
            {
                return Ok(search);
            }
            return BadRequest("Not Found ");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FilterQuizBank(FilterQuizModel name)
        {
            var listName = await _quizService.Filter(name.QuizTopic, name.QuizType);
            if (listName is not null)
            {
                return Ok(listName);
            }
            return BadRequest("Not Found");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewQuiz(Guid QuizID)
        {
            var QuizView = await _quizService.ViewDoingQuiz(QuizID);
            if (QuizView is not null)
            {
                return Ok(QuizView);
            }
            return BadRequest();

        }

        [HttpPost]
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> DoingQuiz(ICollection<AnswerQuizQuestionDTO> doingQuizDTOs, Guid TrainingClassParticipateID)
        {
            bool success = await _quizService.DoingQuizService(doingQuizDTOs);
            if (success)
            {
                return Ok(await _quizService.MarkQuiz(doingQuizDTOs.First().QuizID, TrainingClassParticipateID));
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> ViewQuizDetail(Guid QuizID)
        {
            List<ViewDetailResultDTO> markDetails = await _quizService.ViewMarkDetail(QuizID);
            return Ok(markDetails);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewDetailResultByTrainee(Guid QuizID)
        {
            AnswerQuizDetailTraineeDTO answerQuizDetailTraineeDTO = await _quizService.ViewDetaildoneQuiz(QuizID);
            return Ok(answerQuizDetailTraineeDTO);
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> RemoveQuestionInBank(Guid QuestionID)
        {
            bool check = await _quizService.RemoveQuestionInBank(QuestionID);
            if (check)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin,Trainer,Mentor")]
        public async Task<IActionResult> UpgradeQuestion(Guid QuestionID,UpdateQuestionDTO createQuizIntoBankDTO)
        {
            bool check = await _quizService.UpdateQuestion(QuestionID,createQuizIntoBankDTO);
            if(check)
            {
                return NoContent();
            }
            return BadRequest();
        }


    }
}
