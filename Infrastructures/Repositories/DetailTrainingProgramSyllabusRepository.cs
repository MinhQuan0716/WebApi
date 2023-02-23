using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class DetailTrainingProgramSyllabusRepository : GenericRepository<DetailTrainingProgramSyllabus>, IDetailTrainingProgramSyllabusRepository
    {
        private readonly AppDbContext _dbContext;
        public DetailTrainingProgramSyllabusRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

    }
}
