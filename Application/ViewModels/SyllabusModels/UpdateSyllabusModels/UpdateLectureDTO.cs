using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.UpdateSyllabusModels
{
    public class UpdateLectureDTO
    {
        public Guid? LectureID { get; set; }
        public string LectureName { get; set; }

        public string OutputStandards { get; set; }

        public float Duration { get; set; }

        public string DeliveryType { get; set; }

        public string Status { get; set; }

        public Guid? UnitID {get;set;}

    }
}
