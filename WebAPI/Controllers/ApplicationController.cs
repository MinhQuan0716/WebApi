using Application.Interfaces;
using Application.Models.ApplicationModels;
using Application.Utils;
using Application.ViewModels.ApplicationViewModels;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers
{

    public class ApplicationController : BaseController
    {
        private readonly IApplicationService _service;
        public ApplicationController(IApplicationService services)
        {
            _service = services;

        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] ApplicationDTO applicationDTO)
        {
            bool isAbsent = await _service.CreateApplication(applicationDTO);
            if (isAbsent)
            {
                return Ok("Create application succesfully");
            }
            return BadRequest("Error");
        }

        [HttpGet]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.FullAccess))]
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var result = await _service.UpdateStatus(id, status);
            return result;
        }

        
        [HttpGet]
        public async Task<IActionResult> ViewAllApplication(Guid classId)
        {
            return await ViewAllApplicationFilter(classId);
        }
        /// <summary>
        /// this method will help you get all of the application with 
        /// the filter you are inputting
        /// </summary>
        /// <param name="classId">The trainingclassId</param>
        /// <param name="filter">filter body</param>
        /// <returns>a pagination of a filter</returns>
        [HttpPost]
        [HttpPost("{classId:maxlength(50):guid?}")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.ApplicationPermission), nameof(PermissionEnum.View))]
        public async Task<IActionResult> ViewAllApplicationFilter(Guid classId = default,
                                                            [FromBody] ApplicationFilterDTO filter = null)
        {
            // Run
            var applications = await _service.GetAllApplication(classId, filter);

            if (applications is null) return BadRequest("Application Not Found!");
            return Ok(applications);
        }
    }
}
