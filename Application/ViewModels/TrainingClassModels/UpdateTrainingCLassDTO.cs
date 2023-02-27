using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingClassModels
{
    public class UpdateTrainingCLassDTO
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid LocationID { get; set; }
        public Guid TrainingProgramId { get; set; }

    }
}
