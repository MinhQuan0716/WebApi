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
        public Task<List<TrainingClass>> SearchClassByName(string name);
        public Task<bool> DuplicateClass(Guid id);
        public Task<bool> SoftRemoveTrainingClass(string traingingClassId);
        public Task<bool> UpdateTrainingClass(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO);
        public Task<TrainingClass> GetTrainingClassByIdAsync(string trainingClassId);
        public Task<TrainingClassViewModel?> CreateTrainingClassAsync(CreateTrainingClassDTO createTrainingClassDTO);
        public Task<List<TrainingClassDTO>> GetAllTrainingClassesAsync();
        public Task<List<TrainingClassDTO>> FilterLocation(string[]? locationName, DateTime? date1, DateTime? date2);
    }
}
