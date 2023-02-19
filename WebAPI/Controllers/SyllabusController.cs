﻿using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Microsoft.AspNetCore.Authorization;
using Application.Utils;
using Domain.Enums;

namespace WebAPI.Controllers
{
    public class SyllabusController : BaseController
    {
        private readonly ISyllabusService _syllabusService;
        public SyllabusController(ISyllabusService syllabusService) => _syllabusService = syllabusService;

        [HttpGet]
        public async Task<IActionResult> GetAllSyllabus()
        {
            var getSyllabusList = _syllabusService.GetAllSyllabus();
            if (getSyllabusList != null)
            {
                return Ok(new
                {
                    Success = true,
                    Data = getSyllabusList
                });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> FilerSyllabus(double firstDuration, double secondDuration)
        {
            List<Syllabus> filterSyllabusList = await _syllabusService.FilterSyllabus(firstDuration, secondDuration);
            if (filterSyllabusList.Count() > 0)
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

            [HttpGet("GetName/{name}")]
            public async Task<IActionResult> Get(string name)
            {
                var result = await _syllabusService.GetByName(name);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("Cannot find");
            }
       

        [HttpPut] 
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.SyllabusPermission), nameof(PermissionEnum.FullAccess))]
        public async Task<IActionResult> UpdateSyllabus(Guid syllabusId, UpdateSyllabusDTO updateObject)
        {
            var result = await _syllabusService.UpdateSyllabus(syllabusId, updateObject);
            if (result) return NoContent();
            else return BadRequest("Update Failed");
        }
       
    }
}
