using Application.ViewModels.SyllabusModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{

    public interface ISyllabusRepository : IGenericRepository<Syllabus>
    {
        public Task<List<SyllabusViewAllDTO>>GetAllAsync();
        public Task<List<Syllabus>> FilterSyllabusByDuration(double duration1, double duration2);

        public Task<List<Syllabus>> SearchByName(string name);

        Task<Syllabus> AddSyllabusAsync(SyllabusGeneralDTO syllabusDTO);
        Task<IEnumerable<Syllabus>> GetSyllabusByTrainingProgramId(Guid trainingProgramId);
    }
}
