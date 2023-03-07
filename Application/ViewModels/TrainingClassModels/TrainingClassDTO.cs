using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingClassModels
{
    public  class TrainingClassDTO
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
      public string CreatedBy { get; set; } = default!; 
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string LocationName { get; set; } = default!;
    }
}
