using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.SyllabusModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.EntityState;
namespace Infrastructures.Repositories
{
    public class SyllabusRepository : GenericRepository<Syllabus>, ISyllabusRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public SyllabusRepository(AppDbContext context, IMapper mapper, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        public async Task<Syllabus> AddSyllabusAsync(SyllabusGeneralDTO syllabusDTO)
        {
            var newSyllabus = new Syllabus()
            {
                UserId = _claimsService.GetCurrentUserId,
                CreationDate = DateTime.Now,
                IsDeleted = false
            };
            _mapper.Map(syllabusDTO, newSyllabus);
            //Khó dị trời
            await AddAsync(newSyllabus);
            return newSyllabus;
        }
        public async Task<List<Syllabus>> FilterSyllabusByDuration(double duration1, double duration2)
        {
            List<Syllabus> result = _dbContext.Syllabuses.Where(x => x.Duration > duration1 && x.Duration < duration2).ToList();
            return result;
        }

        public async Task<IEnumerable<Syllabus>> GetSyllabusByTrainingProgramId(Guid trainingProgramId)
        {
            var syllabusList = from s in _dbContext.Syllabuses
                               join d in _dbContext.detailTrainingProgramSyllabuses
                               on s.Id equals d.SyllabusId
                               where d.TrainingProgramId == trainingProgramId && d.IsDeleted == false
                               select s;
            if (!syllabusList.IsNullOrEmpty()) return await syllabusList.ToListAsync();
            else return null;
        }

        public async Task<List<Syllabus>> SearchByName(string name)
        {
            var result = _dbContext.Syllabuses.Where(x => x.SyllabusName.ToUpper().Contains(name)).ToList();
            return result;


        }
    }
}
