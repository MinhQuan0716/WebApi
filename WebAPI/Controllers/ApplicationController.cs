using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.ApplicationViewModels;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [Authorize]
    public class ApplicationController : BaseController
    {
       private readonly IApplicationService _service;
        public ApplicationController(IApplicationService services)
        {
            _service = services;

        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication( [FromBody] ApplicationDTO applicationDTO)
        {
         await _service.CreateApplication(applicationDTO);
            return NoContent();
        }

        [HttpGet]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.FullAccess))]
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var result = await _service.UpdateStatus(id, status);
            return result;
        }
    }
}
