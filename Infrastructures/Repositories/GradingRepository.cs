using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.GradingModels;
using Application.ViewModels.QuizModels;
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
    public List<ViewQuizAndMarkBelowDTO> GetAllMarkOfTrainee(Guid traineeId)
    {
        List<ViewQuizAndMarkBelowDTO> listMark = new List<ViewQuizAndMarkBelowDTO>();

        var result = from detailtraining in _context.DetailTrainingClassParticipates
                     join grading in _context.Gradings on detailtraining.Id equals grading.DetailTrainingClassParticipateId
                     join lecture in _context.Lectures on grading.LectureId equals lecture.Id
                     join quiz in _context.Quizzes on lecture.QuizID equals quiz.Id
                     where detailtraining.UserId == traineeId
                     select new ViewQuizAndMarkBelowDTO() {
                         QuizMark = (int)grading.NumericGrade,
                         QuizName = quiz.QuizName
                     
                     };
       foreach( var item in result ) {
            listMark.Add(item);
        }
    return listMark;
    }

}
