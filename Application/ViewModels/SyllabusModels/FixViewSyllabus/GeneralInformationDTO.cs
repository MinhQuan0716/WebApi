using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.FixViewSyllabus
{
    public class GeneralInformationDTO
    {
        public Guid Id { get; set; }

        public string ProgramName { get; set; } = default!;

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; } = default!;

        public DurationView Duration { get; set; } = default!;


    }
}
