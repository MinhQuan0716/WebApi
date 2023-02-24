using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDbContext _appDbContext;
        public AttendanceRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _appDbContext = context;
        }

        public List<Attendance> GetAttendancesByTraineeClassID(Guid id)
        {
            var findAttendanceResult = _appDbContext.Attendances.Where(x => x.TrainingClassId == id).ToList();
            return findAttendanceResult;
        }

        public List<Attendance> GetAttendancesByTraineeID(Guid id)
        {
            var finAttendaceResult = _appDbContext.Attendances.Where(x => x.UserId == id).ToList();
            return finAttendaceResult;
        }
    }
}
