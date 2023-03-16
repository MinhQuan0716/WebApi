using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.UpdateSyllabusModels
{
    public class UpdateSyllabusDTO
    {
        public string? SyllabusName { get; set; }

        public string? Code { get; set; }

        public string? CourseObject { get; set; }

        public string? TechRequirement { get; set; }

        public double Duration { get; set; }

        public ICollection<UpdateUnitDTO>? Units { get; set; }
    }
}
