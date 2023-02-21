using Application;
using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.SyllabusModels;
using AutoMapper;
using Domain.Entities;
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
            await AddAsync(newSyllabus);
            return newSyllabus;
        }
        public async Task<List<Syllabus>> FilterSyllabusByDuration(double duration1, double duration2)
        {
            List<Syllabus> result = _dbContext.Syllabuses.Where(x => x.Duration > duration1 && x.Duration < duration2).ToList();
            return result;
        }
        public async Task<List<Syllabus>> SearchByName(string name)
        {
            var result = _dbContext.Syllabuses.Where(x => x.SyllabusName.ToUpper().Contains(name)).ToList();
            return result;


        }
    }
}
