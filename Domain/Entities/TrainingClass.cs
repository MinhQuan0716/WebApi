using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrainingClass : BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid? LocationID { get; set; }
        public Location? Location { get; set; }

        public ICollection<DetailTrainingClassParticipate> TrainingClassParticipates { get; set; } = default!;
        public ICollection<Attendance> Attendances { get; set; } = default!;

        public ICollection<Feedback> Feedbacks { get; set; } = default!;
        public StatusClassDetail StatusClassDetail { get; set; } = StatusClassDetail.Inactive;
        public Guid TrainingProgramId { get; set; }
        public TrainingProgram TrainingProgram { get; set; } = default!;
        public ICollection<Applications> Applications { get; set; }
    }
}
