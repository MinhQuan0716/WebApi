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
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        private readonly AppDbContext _dbContext;
        public AssignmentRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task AddProcedure()
        {
            await _dbContext.Database.ExecuteSqlRawAsync(@"CREATE PROCEDURE [dbo].[Assignments_Update_Overdue]
                AS
                BEGIN
                    SET NOCOUNT ON;
                    Update [Assignments]
                    Set [IsOverDue] = 'True'
                    Where [DeadLine] < GETDATE() AND [IsDeleted] ='False' AND [IsOverDue] = 'False'
                END");
        }

        public async Task<bool> CheckExistedProcedure()
        {
            string query = String.Format(@"SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Assignments_Update_Overdue]') AND type in (N'P', N'PC')");
            return await _dbContext.Database.SqlQueryRaw<string>(query).AnyAsync();
        }

        public async Task CheckOverdue()
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC [Assignments_Update_Overdue];");
       }

    }
}
