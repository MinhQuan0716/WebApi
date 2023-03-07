using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.TrainingClassModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class TraingClassRepository : GenericRepository<TrainingClass>, ITrainingClassRepository
    {
        private readonly AppDbContext _dbContext;
        public TraingClassRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
        public List<TrainingClassDTO> GetTrainingClasses()
        {
            var listGetAll = (from a in _dbContext.TrainingClasses
                              join l in _dbContext.Locations on a.LocationID equals l.Id
                              join u in _dbContext.Users on a.CreatedBy equals u.Id


                              select new TrainingClassDTO
                              {
                                  Name = a.Name,
                                  LocationName = l.LocationName,
                                  CreationDate = a.CreationDate,
                                  Code = a.Code,
                                  CreatedBy = u.UserName,
                                  StartDate = a.StartTime,
                                  EndDate = a.EndTime,
                              }

                             ).ToList();
            return listGetAll;
        }

        public List<TrainingClass> SearchClassByName(string name)
        {
            var nameClass = _dbContext.TrainingClasses.Where(x => x.Name.ToLower().Equals(name.ToLower())).ToList();
            return nameClass;
        }
    }
}
