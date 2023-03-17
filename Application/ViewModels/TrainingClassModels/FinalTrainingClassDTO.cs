using Application.ViewModels.SyllabusModels;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingClassModels
{
    public  class FinalTrainingClassDTO
    {
       public TrainingClassViewDetail TrainingClass { get; set; }
        public string location { get; set; }
        public string FSU { get; set; }
        public GeneralTrainingClassDTO general { get; set; }
        public ICollection<SyllabusViewForTrainingClassDetail> syllabusDTO { get; set; }  
        public AttendeeDTO attendeeDTO { get; set; }
        public CreatedByDTO createdDTO { get; set; }    
        public TrainingProgramViewForTrainingClassDetail programModel { get; set; }
     }
}
