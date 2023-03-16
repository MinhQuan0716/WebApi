using Application.Interfaces;
using Application.Utils;
using Application.ViewModels;
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
        [HttpPost("{id}")]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.ApplicationPermission), nameof(PermissionEnum.View))]
        public async Task<IActionResult> ViewAllApplication([FromRoute(Name = "id")] Guid? classId,
                                                            [FromBody] ApplicationDateTimeFilterDTO condition = null,
                                                            [FromQuery] string by = nameof(ApplicationFilterByEnum.CreationDate),
                                                            [FromQuery(Name = "s")] string searchString = "",
                                                            [FromQuery(Name = "p")] int pageNumber = 0,
                                                            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            // Run
            var applications = await _service.GetAllApplication(classId.Value, condition, searchString, by, pageNumber, pageSize);
            return Ok(applications);
        }
    }
}
