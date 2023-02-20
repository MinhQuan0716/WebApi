using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public  interface ITrainingMaterialRepository:IGenericRepository<TrainingMaterial>
    {
        Task<TrainingMaterial> GetFileWithName(string name);
        Task<bool> DeleteTrainingMaterial(Guid id);
    }
}
