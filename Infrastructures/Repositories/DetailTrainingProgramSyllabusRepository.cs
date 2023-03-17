using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class DetailTrainingProgramSyllabusRepository : GenericRepository<DetailTrainingProgramSyllabus>, IDetailTrainingProgramSyllabusRepository
    {
        private readonly AppDbContext _dbContext;
        public DetailTrainingProgramSyllabusRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public DetailTrainingProgramSyllabus GetDetailByClassID(Guid programID)
        {
            var detailProgram=_dbContext.DetailTrainingProgramSyllabuses.Where(x=>x.TrainingProgramId==programID).FirstOrDefault();
            return detailProgram;
        }

        //To take detailTrainingProgram
        public Guid TakeDetailTrainingID(Guid user_id, Guid training_class_id)
        {
            Guid guid = new Guid();
            var ketqua = from trainingclass in _dbContext.TrainingClasses
                         join detailtrainingclass in _dbContext.DetailTrainingClassParticipates on trainingclass.Id equals detailtrainingclass.TrainingClassID
                         where trainingclass.Id == training_class_id && detailtrainingclass.UserId == user_id
                         //&& detailtrainingclass.StatusClassDetail.Equals("Active")
                         select new { guid = detailtrainingclass.Id };

            return guid;
        }
    }
}
