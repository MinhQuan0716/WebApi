using Application.Interfaces;
using Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.AtttendanceModels;
using Application.Interfaces;
using Domain.Entities;
using Application.Utils;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{

    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAttendanceByClassId(Guid id)
        {
            var findResult = await _attendanceService.GetAttendancesByTraineeClassID(id);
            if (findResult != null)
            {
                return Ok(findResult);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceByTraineeId(Guid id)
        {
            var findResult = await _attendanceService.GetAttendanceByTraineeID(id);
            if (findResult != null)
            {
                return Ok(findResult);
            }
            return BadRequest();
        }

        [HttpPost, HttpPatch]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Create))]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Modifed))]

        public async Task<IActionResult> CheckAttendance(Guid classId, [FromBody] List<AttendanceDTO> attendances)
        {

            List<Attendance> checkList = await _attendanceService.UploadAttendanceFormAsync(attendances, classId, Request.Method);

            return checkList is null ? BadRequest() : Ok(checkList);
        }

        [HttpPatch]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Create))]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Modifed))]

        public async Task<IActionResult> EditAttendance( Guid classId, [FromBody] AttendanceDTO attendanceDto)
        {
            var attendance = await _attendanceService.UpdateAttendanceAsync(attendanceDto, classId);
            return attendance is null ? BadRequest() : Ok(attendance);
        }
    }
}
