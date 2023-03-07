using Application.ViewModels.TrainingClassModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ITrainingClassRepository:IGenericRepository<TrainingClass>
    {
        public List<TrainingClass> SearchClassByName(string name);
        public List<TrainingClassDTO> GetTrainingClasses();
    }
}
