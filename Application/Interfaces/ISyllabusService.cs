using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{

    public interface ISyllabusService
    {
        public Task<List<Syllabus>> FilterSyllabus(double duration1, double duration2);
        public Task<List<Syllabus>> GetAllSyllabus();
        public Task<bool> DeleteSyllabussAsync(string syllabusID);
    }
}

