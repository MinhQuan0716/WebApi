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
        public virtual User User { get; set; }
        public Guid TrainingClassID { get; set; }
        public virtual TrainingClass TrainingClass { get; set; }
    }
}
