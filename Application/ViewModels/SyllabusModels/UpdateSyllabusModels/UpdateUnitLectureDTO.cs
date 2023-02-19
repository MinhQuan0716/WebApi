using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.UpdateSyllabusModels
{
    public class UpdateUnitLectureDTO
    {
        public Guid? Id { get; set; }
        public Guid UnitId { get; set; }
        public Guid? LectureId { get; set; }
    }
}
