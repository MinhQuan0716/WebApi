using Application.Commons;
using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.AtttendanceModels;
using Application.ViewModels.SyllabusModels.UpdateSyllabusModels;
using AutoFixture;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.AttendanceStatusEnums;

namespace Infrastructures.Repositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        private readonly AppDbContext _dbContext;

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

       
        public async Task<Pagination<Attendance>> GetAllAttendanceWithFilter(Expression<Func<Attendance, bool>> expression, int pageIndex, int pageSize)
        {
           
            var value = _dbSet.Include(x => x.User).Include(x=>x.Application);
            Pagination<Attendance> pagination = await ToPagination(value,expression, pageIndex, pageSize);
            return pagination.Items.IsNullOrEmpty() ? null : pagination;
        }
    }
}
