using Application.ViewModels.AtttendanceModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<Attendance> GetAttendanceByUserAndClass(AttendanceDTO attendanceDto, Guid? classID);
        List<Attendance> GetAttendancesByTraineeClassID(Guid id);
        List<Attendance> GetAttendancesByTraineeID(Guid id);
    }
    
}
