using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.GradingModels;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories;

public class GradingRepository : GenericRepository<Grading>, IGradingRepository
{
    private readonly AppDbContext _context;

    public GradingRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
        _context = context;
    }

    public List<MarkReportDto> GetMarkReportOfClass(Guid classID)
    {
        var result = from g in _context.Gradings
                     join d in _context.DetailTrainingClassParticipates
                            on g.DetailTrainingClassParticipateId equals d.Id
                     join u in _context.Users
                            on d.UserId equals u.Id
                     join c in _context.TrainingClasses
                             on d.TrainingClassID equals c.Id
                     join l in _context.Lectures
                            on g.LectureId equals l.Id
                    where c.Id == classID
                     select new MarkReportDto()
                     {
                         ClassName = c.Name,
                         Username = u.UserName,
                         TraineeName = u.FullName,
                         LectureName = l.LectureName,
                         DeliveryType = l.DeliveryType,
                         NumericGrade = g.NumericGrade.Value,
                         LetterGrade = g.LetterGrade
                     };
        return result.ToList();
    }
    public List<MarkReportDto> GetMarkReportOfTrainee(Guid traineeId)
    {
        var result = from g in _context.Gradings
                     join d in _context.DetailTrainingClassParticipates
                            on g.DetailTrainingClassParticipateId equals d.Id
                     join u in _context.Users
                            on d.UserId equals u.Id
                     join c in _context.TrainingClasses
                             on d.TrainingClassID equals c.Id
                     join l in _context.Lectures
                            on g.LectureId equals l.Id
                     where u.Id == traineeId
                     select new MarkReportDto()
                     {
                         ClassName = c.Name,
                         Username = u.UserName,
                         TraineeName = u.FullName,
                         LectureName = l.LectureName,
                         DeliveryType = l.DeliveryType,
                         NumericGrade = g.NumericGrade.Value,
                         LetterGrade = g.LetterGrade
                     };
        return result.ToList();
    }
}
