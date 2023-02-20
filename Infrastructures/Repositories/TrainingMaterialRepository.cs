using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class TrainingMaterialRepository : GenericRepository<TrainingMaterial>, ITrainingMaterialRepository
    {
        private readonly AppDbContext _dbContext;

        public TrainingMaterialRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }
        public async Task<bool> DeleteTrainingMaterial(Guid id)
        {
            TrainingMaterial resultFile = await GetByIdAsync(id);
            if (resultFile != null)
            {
                _dbContext.TrainingMaterials.Remove(resultFile);
                return true;
            }
            return false;
        }

        public async Task<TrainingMaterial> GetFileWithName(string name)
        {
            var file = _dbContext.TrainingMaterials.SingleOrDefault(x => x.TMatName.Equals(name));

            if (file == null)
            {
                throw new Exception("No file was found");
            }
            return file;
        }
    }
}
