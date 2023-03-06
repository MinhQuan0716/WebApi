using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IDetailTrainingClassParticipateRepository : IGenericRepository<DetailTrainingClassParticipate>
    {
        Task<DetailTrainingClassParticipate> GetDetailTrainingClassParticipateAsync(Guid userId, Guid classId);
    }
}
