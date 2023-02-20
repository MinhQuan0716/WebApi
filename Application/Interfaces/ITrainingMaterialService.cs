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
        public Task<TrainingMaterial> GetFile(string name);
        public Task<TrainingMaterial> Upload(IFormFile file, string lectureNames);
        public Dictionary<string, string> GetMimeTypes();
        public Task<bool> DeleteTrainingMaterial(Guid id);
        public Task<bool> UpdateTrainingMaterial(IFormFile file, Guid id);
    }
}
