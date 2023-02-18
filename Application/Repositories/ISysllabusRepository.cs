using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public  interface ISysllabusRepository:IGenericRepository<Syllabus>
    {
        public  Task<List<Syllabus>> SearchByName(string name);
    }
}
