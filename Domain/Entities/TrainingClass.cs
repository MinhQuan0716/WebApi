using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TrainingClass : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid LocationID { get; set; }
        public Location Location { get; set; } = default!;

        public ICollection<DetailTrainingClassParticipate> TrainingClassParticipates { get; set; } = default!;
    }
}
