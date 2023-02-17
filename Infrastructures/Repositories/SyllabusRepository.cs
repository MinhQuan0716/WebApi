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
    public class SyllabusRepository : GenericRepository<Syllabus>, ISyllabusRepository
    {
        private readonly AppDbContext _dbContext;
        public SyllabusRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Syllabus>> FilterSyllabusByDuration(double duration1, double duration2)
        {
            List<Syllabus>? result = _dbContext.Syllabuses.Where(x => x.Duration > duration1 && x.Duration < duration2).ToList();
            return result;
        }
    }
}
