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
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        private readonly AppDbContext _appDbContext;
        public QuizRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _appDbContext = context;    
        }

        public async Task<List<DetailQuizQuestion>> GetAllQuestionByQuizTestId(Guid id)
        {
            var result = _appDbContext.DetailQuizQuestions.Where(x => x.QuizID == id).ToList();
            return result;
        }
    }
}
