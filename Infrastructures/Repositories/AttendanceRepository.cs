using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.AtttendanceModels;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly AppDbContext context;

        public AttendanceRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public List<Attendance> GetAttendancesByTraineeClassID(Guid id)
        {
            var findAttendanceResult = _dbContext.Attendances.Where(x => x.TrainingClassId == id).ToList();
            return findAttendanceResult;
        }

        public List<Attendance> GetAttendancesByTraineeID(Guid id)
        {
            var finAttendaceResult = _dbContext.Attendances.Where(x => x.UserId == id).ToList();
            return finAttendaceResult;
        }

        public async Task<Attendance> GetAttendanceByUserAndClass(AttendanceDTO attendanceDto, Guid? classID)
        {
            return null;
                //await _dbContext.Attendances.FirstOrDefaultAsync(
                //                        x => x.TrainingClassId == classID
                //                        && x.UserId == attendanceDto.UserId
                //                        && DateOnly.FromDateTime( x.Date) == DateOnly.FromDateTime(date) );
        }
    }
}
