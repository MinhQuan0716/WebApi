using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingProgramModels
{
    public class ViewAllTrainingProgramDTO
    {
        public Guid Id { get; set; }
        public string ProgramName { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public double Duration { get; set; }

        public string Status { get; set; }
        public ICollection<Syllabus>? Syllabuses { get; set; }

    }
}
