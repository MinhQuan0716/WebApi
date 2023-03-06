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
using System.Collections.Generic;
using AutoFixture;
using Infrastructures;
using Application.Commons;

namespace WebAPI.Controllers
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpGet]
        [AllowAnonymous]
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

        [HttpGet("{id:maxlength(50):guid?}")]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.View))]

        public async Task<IActionResult> GetAllAttendanceReports([FromRoute(Name = "id")] Guid classId = default,
                                                                 [FromQuery(Name = "from")] DateTime? toDate = null,
                                                                 [FromQuery(Name = "to")] DateTime? fromDate = null,
                                                                 [FromQuery(Name = "s")] string search = "",
                                                                 [FromQuery(Name = "by")] string by = nameof(AttendanceStatusEnums.None),
                                                                 [FromQuery(Name = "apli")] bool? containApplication = null,
                                                                 [FromQuery(Name = "p")] int pageIndex = 0,
                                                                 [FromQuery(Name = "ps")] int pageSize = 40)
        {
            fromDate ??= DateTime.UtcNow.Date;
            toDate ??= fromDate+DateTime.MaxValue.TimeOfDay;
            var result = await _attendanceService.GetAllAttendanceWithFilter(
                         classId, search,
                         by, containApplication,fromDate,toDate,
                         pageIndex, pageSize);
            return result is null ? NoContent() : Ok(result);
        }
        [HttpPost, HttpPatch]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Create))]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Modifed))]

        public async Task<IActionResult> CheckAttendance(Guid classId, [FromBody] List<AttendanceDTO> attendances)
        {

            List<Attendance> checkList = await _attendanceService.UploadAttendanceFormAsync(attendances, classId, Request.Method);

            return checkList is null ? BadRequest() : Ok(checkList);
        }

        [HttpPatch]
        [Authorize]
        [ClaimRequirement(nameof(PermissionItem.AttendancePermission), nameof(PermissionEnum.Modifed))]

        public async Task<IActionResult> EditAttendance(Guid classId, [FromBody] AttendanceDTO attendanceDto)
        {
            var attendance = await _attendanceService.UpdateAttendanceAsync(attendanceDto, classId);
            return attendance is null ? BadRequest() : Ok(attendance);
        }

        //[HttpGet]
        //public async Task<IActionResult> TestCronJob()
        //{
        //    try
        //    {
        //        ApplicationCronJob cronjob = new ApplicationCronJob(_config,_attendanceService,_mailHelper, _currentTime);
        //        await cronjob.CheckAttendancesEveryDay();
        //        return Ok();
        //    } catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

    }
}
