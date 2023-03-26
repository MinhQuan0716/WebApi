﻿using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.SyllabusModels;
using Application.ViewModels.TrainingClassModels;
using Application.ViewModels.TrainingProgramModels;
using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
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


        public TrainingProgramViewForTrainingClassDetail GetTrainingProgramByClassID(Guid id)
        {
            var getAllTrainingProgram = _dbContext.TrainingClasses
                                       .Include(x => x.TrainingProgram)
                                       
                                       .Where(x => x.Id == id)
                                      .Select(x => new TrainingProgramViewForTrainingClassDetail()
                                       {
                                        programId = x.TrainingProgram.Id,
                                        programName = x.TrainingProgram.ProgramName,
                                        programDuration = new DurationView
                                        {
                                        TotalHours = x.TrainingProgram.Duration
                                        },
                                        lastEdit =new LastEditDTO
                                        {
                                            modificationBy=_dbContext.Users.Where(u=>u.Id==x.TrainingProgram.ModificationBy).Select(u=>u.UserName).FirstOrDefault(),
                                            modificationDate=x.TrainingProgram.ModificationDate
                                        }
                                   }).FirstOrDefault();
            return getAllTrainingProgram;

        }
        public List<TrainingClassFilterDTO> GetTrainingClassesForFilter()
        {
            var getAllTrainingClass = _dbContext.TrainingClasses
                                        .Where(x => x.IsDeleted == false)
                                     .Select(t => new TrainingClassFilterDTO
                                     {
                                         ClassID = t.Id,
                                         Name = t.Name,
                                         LocationName = t.Location.LocationName,
                                         CreationDate = t.CreationDate,
                                         Code = t.Code,
                                         Branch = t.Branch,
                                         StartDate = t.StartTime,
                                         EndDate = t.EndTime,
                                         Attendee = t.Attendee,
                                         ClassDuration = new DurationView
                                         {
                                             TotalHours = t.Duration
                                         },
                                         Status = t.StatusClassDetail,
                                         CreatedBy = string.Join(",", _dbContext.Users.Where(x => x.Id == t.CreatedBy).Select(u => u.UserName))
                                     }).ToList();
            return getAllTrainingClass;
        }

        public List<TrainingClassViewAllDTO> GetTrainingClasses()
        {
            var getAllTrainingClass = _dbContext.TrainingClasses
                                     .Select(t => new TrainingClassViewAllDTO
                                     {
                                         id = t.Id,
                                         className = t.Name,
                                         location = t.Location.LocationName,
                                         createdOn = t.CreationDate,
                                         classCode = t.Code,
                                         fsu = t.Branch,
                                         attendee = t.Attendee,
                                         classDuration = new DurationView
                                         {
                                             TotalHours = t.Duration
                                         },
                                         createdBy = string.Join(",", _dbContext.Users.Where(x => x.Id == t.CreatedBy).Select(u => u.UserName))
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

        public TrainingClassFilterDTO GetTrainingClassFilterById(Guid id)
        {
            var getAllTrainingClass = _dbContext.TrainingClasses
                             .Where(x => x.IsDeleted == false && x.Id == id)

                          .Select(t => new TrainingClassFilterDTO
                          {
                              ClassID = t.Id,
                              Name = t.Name,
                              LocationName = t.Location.LocationName,
                              CreationDate = t.CreationDate,
                              Code = t.Code,
                              Branch = t.Branch,
                              StartDate = t.StartTime,
                              EndDate = t.EndTime,
                              Attendee = t.Attendee,
                              ClassDuration = new DurationView
                              {
                                  TotalHours = t.Duration
                              },
                              LastEditDTO = new LastEditDTO
                              {
                                  modificationBy = _dbContext.Users.Where(u => u.Id == t.ModificationBy).Select(u => u.UserName).SingleOrDefault(),
                                  modificationDate = t.ModificationDate,
                              },
                              Status = t.StatusClassDetail,
                              CreatedBy = _dbContext.Users.Where(x => x.Id == t.CreatedBy).Select(u => u.UserName).SingleOrDefault()
                          });
            TrainingClassFilterDTO trainingClassFilterDTO = getAllTrainingClass.FirstOrDefault();
            return trainingClassFilterDTO;
        }
    }
}
