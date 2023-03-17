using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AssignmetRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDbContext _dbContext;
        public AssignmetRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task CheckOverdue()
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC [Assignments_Update_Overdue];");
       }

    }
}
