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
        public Task<List<Syllabus>> FilterSyllabusByDuration(double duration1, double duration2);

        public Task<List<Syllabus>> SearchByName(string name);
        public interface ISyllabusRepository : IGenericRepository<Syllabus>
        {
        }

    }
}
