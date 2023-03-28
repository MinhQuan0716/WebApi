using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.FixViewSyllabus
{
    public class FinalViewSyllabusDTO
    {
        public Guid  ID { get; set; }
        public string ProgramName { get; set; } = default!;

        public string Status { get; set; } = default!;


        public DateTime CreateOn { get; set; }

        public string CreateBy { get; set; } = default!;

        public DurationView Durations { get; set; } = default!;


        public ShowDetailSyllabusNewDTO General { get; set; } = default!;
        
        public OutlineSyllabusDTO outlineSyllabusDTO { get; set; } = default!;

        public OtherSyllabusDTO OtherSyllabusDTOOther { get; set; } = default!;
    }
}
