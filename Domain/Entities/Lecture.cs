﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lecture : BaseEntity
    {
        public string LectureName { get; set; }
        public string OutputStandards { get; set; }
        public double Duration { get; set; }
        public string DeliveryType { get; set; }
        public string Status { get; set; }

         public ICollection<DetailUnitLecture> DetailUnitLectures { get; set; }

    }
}