using Application.Interfaces;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{

    public class DetailTrainingClassParticipateRepository : GenericRepository<DetailTrainingClassParticipate>, IDetailTrainingClassParticipateRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public DetailTrainingClassParticipateRepository(AppDbContext context, IMapper mapper, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<DetailTrainingClassParticipate> GetDetailTrainingClassParticipateAsync(Guid userId, Guid classId)
        {
            var result = _dbContext.DetailTrainingClassParticipates.SingleOrDefault(x => x.UserId == userId && x.TrainingClassID == classId);
            if (result == null)
            {
                throw new Exception("Detail does not exist!");
            }
            return result;
        }
    }
}
