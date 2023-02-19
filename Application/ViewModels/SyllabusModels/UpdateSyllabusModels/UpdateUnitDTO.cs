using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.UpdateSyllabusModels
{
    public class UpdateUnitDTO
    {
        public Guid? UnitID { get; set; }
        public string UnitName { get; set; }
        public float TotalTime { get; set; }
        public Guid? syllabusId { get; set; }
        public ICollection<UpdateLectureDTO>? UpdateLectureDTOs { get; set; }    
    }
}
