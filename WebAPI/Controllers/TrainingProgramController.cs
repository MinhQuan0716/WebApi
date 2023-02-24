﻿using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.TrainingProgramModels;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace WebAPI.Controllers
{
    public class TrainingProgramController : BaseController
    {
        private readonly ITrainingProgramService _trainingProgramService;

        public TrainingProgramController(ITrainingProgramService trainingProgramService)
        {
            _trainingProgramService = trainingProgramService;
        }

        [HttpGet]
       /* [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingProgramPermission), nameof(PermissionEnum.View))]*/
        public async Task<IActionResult> GetDetail(Guid trainingProgramId)
        {
            var trainingProgram = await _trainingProgramService.GetTrainingProgramDetail(trainingProgramId);
            if(trainingProgram is not null ) return Ok(trainingProgram); 
            return BadRequest();
        }

        [HttpPost]
       /* [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingProgramPermission), nameof(PermissionEnum.Create))]*/
        public async Task<IActionResult> Create(CreateTrainingProgramDTO createTrainingProgramDTO)
        {
            var result = await _trainingProgramService.CreateTrainingProgram(createTrainingProgramDTO);
            if (result is not null) return CreatedAtAction(nameof(GetDetail), new { trainingProgramId = result.Id }, result);
            else return BadRequest();
        }

        [HttpPut]
       /* [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingProgramPermission), nameof(PermissionEnum.Modifed))]*/
        public async Task<IActionResult> Update(UpdateTrainingProgramDTO updateProgramDTO) 
        {
            var result = await _trainingProgramService.UpdateTrainingProgram(updateProgramDTO);
            if (result) return NoContent();
            else return BadRequest();
        }

        [HttpDelete]
      /*  [Authorize]
        [ClaimRequirement(nameof(PermissionItem.TrainingProgramPermission), nameof(PermissionEnum.Modifed))]*/
        public async Task<IActionResult> Delete(Guid trainingProgramId)
        {
            var result = await _trainingProgramService.DeleteTrainingProgram(trainingProgramId);
            if(result) return NoContent();
            return BadRequest("Delete Failed");
        }
    }
}
