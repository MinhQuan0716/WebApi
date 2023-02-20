using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DetailTrainingClassParticipate : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
        public Guid TrainingClassID { get; set; }
        public TrainingClass TrainingClass { get; set; } = default!;
    }
}
