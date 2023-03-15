using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels.FixViewSyllabus
{
    public class FinalViewSyllabusDTO
    {
        public ShowDetailSyllabusNewDTO General { get; set; }

        public OutlineSyllabusDTO outlineSyllabusDTO { get; set; }

        public OtherSyllabusDTO OtherSyllabusDTOOther { get; set; }
    }
}
