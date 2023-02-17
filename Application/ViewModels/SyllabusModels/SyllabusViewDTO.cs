using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels
{
    public class SyllabusViewDTO
    {
        public SyllabusGeneralDTO newSyllabus { get; set; }

        public string Code { get; set; }

        public string CourseObject { get; set; }

        public string TechRequirements { get; set; }

        public double Duration { get; set; }

        public User User { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<Unit> Units { get; set; }
    }
}
