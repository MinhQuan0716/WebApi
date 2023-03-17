using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SyllabusModels
{
    public class SyllabusViewAllDTO
    {
        public Guid SyllabusID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string syllabusStatus { get; set; }  
        public DateTime CreatedOn { get; set; }
        public double Duration { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<string> OutputStandard { get; set; }
    }
}
