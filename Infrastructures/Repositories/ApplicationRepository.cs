using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.AtttendanceModels;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class ApplicationRepository : GenericRepository<Applications>, IApplicationRepository
    {
        private readonly AppDbContext _dbContext;
        public ApplicationRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }


        public async Task<IList<Applications>> GetAllApplicationByClassAndDateTime(Guid? classId, DateTime dateTime)
        {
            var application = await _dbContext.Applications.AsNoTracking().Where(x => x.Id == classId && DateOnly.FromDateTime(x.AbsentDateRequested) == DateOnly.FromDateTime(dateTime))
                                                           .ToListAsync();
            return application;
        }

        public async Task<Applications> GetApplicationByUserAndClassId(AttendanceDTO attendance, Guid id)
        {
            var application = await _dbContext.Applications.AsNoTracking().FirstOrDefaultAsync(x => x.TrainingClassId == id
                                                                                     && x.UserId == attendance.UserId
                                                                                     && x.AbsentDateRequested.Date == attendance.Date
                                                                                     && x.Approved
                                                                                     );
            return application;
        }

    }
}
