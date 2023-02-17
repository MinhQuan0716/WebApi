﻿using Application.Interfaces;
using Domain.Entities;
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
        [HttpGet]
        public async Task<IActionResult>GetAllSyllabus()
        {
            var getSyllabusList= _syllabusService.GetAllSyllabus();
            if (getSyllabusList != null)
            {
                return Ok(new
                { Success=true,
                Data= getSyllabusList
                });
            } else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async  Task<IActionResult> FilerSyllabus(double firstDuration,double secondDuration)
        {
            var filterSyllabusList=_syllabusService.FilterSyllabus(firstDuration,secondDuration);
            if (filterSyllabusList != null)
            {
                return Ok(new
                {
                    Success = true,
                    Data = filterSyllabusList
                });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
