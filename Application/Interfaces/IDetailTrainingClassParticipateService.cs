﻿using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDetailTrainingClassParticipateService
    {
        public Task<bool> UpdateTrainingStatus(Guid classId);

        public Task<DetailTrainingClassParticipate> CreateTrainingClassParticipate(Guid userId, Guid classId);
    }
}
