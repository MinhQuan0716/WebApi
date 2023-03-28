using Application.ViewModels.TrainingMaterialModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ITrainingMaterialRepository : IGenericRepository<TrainingMaterial>
    {
        Task<bool> DeleteTrainingMaterial(Guid id);
        Task<List<TrainingMaterial>> GetAllFileWithLectureId(Guid lectureId);
        Task<TrainingMaterialDTO> GetTrainingMaterial(Guid lectureId);
    }
}
