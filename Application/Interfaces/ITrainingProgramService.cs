using Application.Services;
using Application.ViewModels.TrainingProgramModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrainingProgramService
    {
        Task<TrainingProgramViewModel> GetTrainingProgramDetail(Guid id);
        Task<TrainingProgram> CreateTrainingProgram(CreateTrainingProgramDTO createTrainingProgramDTO);
        Task<bool> UpdateTrainingProgram(UpdateTrainingProgramDTO updateProgramDTO);
        Task<bool> DeleteTrainingProgram(Guid trainingProgramId);
        Task<List<TrainingProgramViewModel>> SearchTrainingProgram(TrainingProgramSearchFilterModels.SearchTrainingProgramModel searchTrainingProgramModel);
        Task<List<TrainingProgramViewModel>> FilterTrainingProgram(TrainingProgramSearchFilterModels.FilterTrainingProgramModel filterTrainingProgramModel);
        Task<IEnumerable<ViewAllTrainingProgramDTO>> viewAllTrainingProgramDTOs();
        Task<TrainingProgram> DuplicateTrainingProgram(Guid trainingProgramId);


    }
}
