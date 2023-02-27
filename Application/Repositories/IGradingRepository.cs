using Application.ViewModels.GradingModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories;

public interface IGradingRepository : IGenericRepository<Grading>
{
    List<MarkReportDto> GetMarkReportOfClass(Guid classID);
    List<MarkReportDto> GetMarkReportOfTrainee(Guid traineeId);
}
