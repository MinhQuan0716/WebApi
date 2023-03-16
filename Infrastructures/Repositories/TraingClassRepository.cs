using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.TrainingClassModels;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    /// <summary>
    /// Training class repository
    /// </summary>
    public class TraingClassRepository : GenericRepository<TrainingClass>, ITrainingClassRepository
    {
        private readonly AppDbContext _dbContext;
        public TraingClassRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService)
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
        /// <summary>
        /// GetTrainingClasses return all training classes
        /// </summary>
        /// <returns>List of training classes</returns>
        public List<TrainingClassDTO> GetTrainingClasses()
        {
            var getAllTrainingClass = _dbContext.TrainingClasses
                                   .Select(t => new TrainingClassDTO
                                   {
                                       Name = t.Name,
                                       LocationName = t.Location.LocationName,
                                       CreationDate = t.CreationDate,
                                       Code = t.Code,
                                       Branch = t.Branch,
                                       StartDate = t.StartTime,
                                       EndDate = t.EndTime,
                                       Attendee = t.Attendee,
                                       TotalDay = (t.EndTime - t.StartTime).TotalDays,
                                       TotalHour = (t.EndTime - t.StartTime).TotalHours,
                                       Status = t.StatusClassDetail,
                                       CreatedBy = string.Join(",", _dbContext.Users.Where(x => x.Id == t.CreatedBy).Select(u => u.UserName))
                                   }).ToList();
            return getAllTrainingClass;
        }

        /// <summary>
        /// SearchClassByName find and return training classes
        /// which name are the same as the name parameter
        /// </summary>
        /// <param name="name">name of a training class</param>
        /// <returns>List of training classes</returns>
        public List<TrainingClass> SearchClassByName(string name)
        {
            var nameClass = _dbContext.TrainingClasses.Where(x => x.Name.ToLower().Equals(name.ToLower())).ToList();
            return nameClass;
        }
    }
}
