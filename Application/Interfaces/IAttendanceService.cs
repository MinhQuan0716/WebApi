using Application.ViewModels.AtttendanceModels;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
   public  interface IAttendanceService
    {
        public Task<List<Attendance>> GetAttendancesByTraineeClassID(Guid id); 
        public Task<List<Attendance>> GetAttendanceByTraineeID(Guid id);

        Task<List<Attendance>> UploadAttendanceFormAsync(List<AttendanceDTO> attendanceDtos,
                                                         Guid ClassId,
                                                         string httpMethod);
        Task<Attendance> UpdateAttendanceAsync(AttendanceDTO attendanceDto, Guid classId);
    }
}
