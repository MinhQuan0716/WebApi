using Application.Interfaces;
using Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
