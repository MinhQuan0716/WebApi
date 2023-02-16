using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class SyllabusController : BaseController
    {
        private readonly ISyllabusService _syllabusService;
        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        [HttpGet("{id:maxlength(50):Guid}")]
        public async Task<IActionResult> DeleteSyllabus(string id)
        {
            var checkSyllabus = await _syllabusService.DeleteSyllabussAsync(id);


            if (checkSyllabus)
            {
                return NoContent();
            }


            return BadRequest("Delete Syllabus Not Successfully");
        }
    }
}
