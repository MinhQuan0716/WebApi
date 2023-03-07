using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Controllers
{
   // [Authorize]
    public class TrainingClassController : BaseController
    {
        private readonly ITrainingClassService _trainingClassService;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private ITrainingClassService @object;

        public TrainingClassController(ITrainingClassService trainingClassService, IClaimsService claimsService, IMapper mapper)
        {
            _trainingClassService = trainingClassService;
            _claimsService = claimsService;
            _mapper = mapper;
        }

       

        [HttpGet]
        public async Task<IActionResult> SearchClassByName(string name)
        {
            var result = await _trainingClassService.SearchClassByName(name);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> DuplicateClass(Guid id)
        {
            var result = await _trainingClassService.DuplicateClass(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
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
        [HttpGet]
        public async Task<IActionResult> GetAllTraningClass()
        {
            var listResult = _trainingClassService.GetAllTrainingClassesAsync();
            if (listResult != null)
            {
                return Ok(listResult);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> FilterResult(string[]? locationName, DateTime? date1, DateTime? date2)
        {
            var fiterResult = await _trainingClassService.FilterLocation(locationName, date1, date2);
            if (fiterResult.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(fiterResult);
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
