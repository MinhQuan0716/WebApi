﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TrainingClassModels
{
    public class CreateTrainingClassDTO
    {
        public string Name { get; set; } = default!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid LocationID { get; set; }
        public Guid TrainingProgramId { get; set; }
        public string StatusClassDetail { get; set; }
        public string Attendee { get; set; }
        public string Branch { get; set; }

        public ICollection<Guid>? UserId { get; set; }
    }
}
