using Application.Interfaces;
using Application.ViewModels.QuizModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IQuestionService _quizService;
        public QuestionController(IQuestionService quizService) => _quizService = quizService;

        [HttpPost]
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
        public async Task<IActionResult> UpdateQuizTest(Guid quizTestId, UpdateQuizTestDTO quizDto)
        {
            var checkUpdateSuccesfully = await _quizService.UpdateQuizTest(quizTestId,quizDto);
            if (checkUpdateSuccesfully)
            {
                return Ok("Update quiz test successfully!");
            }
            return BadRequest("Fail to update quiz test!");
        }

        [HttpDelete]
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
        public async Task<IActionResult> FilterQuizBank(FilterQuizModel name)
        {
            var listName = await _quizService.Filter(name.bun, name.comsuon);
            if (listName is not null)
            {
                return Ok(listName);
            }
            return BadRequest("Not Found");
        }
    }
}
