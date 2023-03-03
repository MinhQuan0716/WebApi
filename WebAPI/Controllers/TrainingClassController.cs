using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    public class TrainingClassController : BaseController
    {
        private readonly ITrainingClassService _trainingClassService;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public TrainingClassController(ITrainingClassService trainingClassService, IClaimsService claimsService, IMapper mapper)
        {
            _trainingClassService = trainingClassService;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Trainer")]
        [HttpPut]
        public async Task<IActionResult> UpdateTrainingClass(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO)
        {
            try
            {
                if (await _trainingClassService.UpdateTrainingClass(trainingClassId, updateTrainingCLassDTO))
                {
                    return Ok("Update class successfully");
                }
                else
                {
                    throw new Exception("Saving fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update class fail: " + ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> CreateTrainingClass(CreateTrainingClassDTO classDTO)
        {
            try
            {
                var result = await _trainingClassService.CreateTrainingClassAsync(classDTO);
                if (result is null)
                {
                    throw new Exception("Saving fail");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Create training class fail: " + ex.Message);
            }

        }
        [Authorize(Roles = "Trainer")]
        [HttpPut]
        public async Task<IActionResult> SoftRemoveTrainingClass(string trainingClassId)
        {
            try
            {
                if (await _trainingClassService.SoftRemoveTrainingClass(trainingClassId))
                {
                    return Ok("SoftRemove class successfully");
                }
                else
                {
                    throw new Exception("Saving fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("SoftRemove class fail: " + ex.Message);
            }
        }

    }
}
