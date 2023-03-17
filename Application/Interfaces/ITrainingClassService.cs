using Application.ViewModels.TrainingClassModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrainingClassService
    {
        public Task<List<TrainingClass>> SearchClassByNameAsync(string name);
        public Task<bool> DuplicateClassAsync(Guid id);
        public Task<bool> SoftRemoveTrainingClassAsync(string traingingClassId);
        public Task<bool> UpdateTrainingClassAsync(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO);
        public Task<TrainingClass> GetTrainingClassByIdAsync(string trainingClassId);
        public Task<TrainingClassViewModel?> CreateTrainingClassAsync(CreateTrainingClassDTO createTrainingClassDTO);
        public Task<List<TrainingClassDTO>> GetAllTrainingClassesAsync();
        public Task<List<TrainingClassDTO>> FilterLocation(string[]? locationName, string branchName, DateTime? date1, DateTime? date2, string[]? classStatus, string[]? attendInClass);
        public Task<FinalTrainingClassDTO> GetFinalTrainingClassesAsync(Guid id);
    }
}
