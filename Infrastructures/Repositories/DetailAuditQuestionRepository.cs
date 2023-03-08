using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.AuditModels.ViewModels;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class DetailAuditQuestionRepository : GenericRepository<DetailAuditQuestion>, IDetailAuditQuestionRepository
    {
        private readonly AppDbContext _dbContext;
        public DetailAuditQuestionRepository(AppDbContext dbContext,
        ICurrentTime timeService,
        IClaimsService claimsService)
        : base(dbContext,
              timeService,
              claimsService)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AuditQuestionViewModel>> GetAuditQuestionsByAuditId(Guid auditId)
        {
            var auditQuestionsList = from s in _dbContext.AuditQuestions
                                     join d in _dbContext.DetailAuditQuestions
                                     on s.Id equals d.AuditQuestionId
                                     where d.AuditPlanId == auditId
                                     select new AuditQuestionViewModel
                                     {
                                         Id = d.Id,
                                         Description = s.Description
                                     };

            return await auditQuestionsList.ToListAsync();
        }
    }
}
