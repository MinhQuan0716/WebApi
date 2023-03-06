using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITrainingMaterialService
    {
        public Task<TrainingMaterial> GetFile(Guid id);
        public Task<TrainingMaterial> Upload(IFormFile file, Guid lectureId);
        public Dictionary<string, string> GetMimeTypes();
        public Task<bool> DeleteTrainingMaterial(Guid id);
        public Task<bool> UpdateTrainingMaterial(IFormFile file, Guid id);
    }
}
