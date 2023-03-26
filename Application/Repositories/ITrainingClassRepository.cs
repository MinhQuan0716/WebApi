using Application.ViewModels.TrainingClassModels;
using Application.ViewModels.TrainingProgramModels.TrainingProgramView;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ITrainingClassRepository : IGenericRepository<TrainingClass>
    {
        public List<TrainingClass> SearchClassByName(string name);
        public List<TrainingClassFilterDTO> GetTrainingClassesForFilter();
        public List<TrainingClassViewAllDTO> GetTrainingClasses();
        TrainingProgramViewForTrainingClassDetail GetTrainingProgramByClassID(Guid id);
       TrainingClassFilterDTO GetTrainingClassFilterById(Guid id);
    }
}
