using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.GradingModels;

public class GradingModel
{
    public Guid LectureId { get; set; }
    public Guid DetailTrainingClassParticipateId { get; set; }    
    public string? LetterGrade { get; set; }
    public int? NumericGrade { get; set; }
    
}
