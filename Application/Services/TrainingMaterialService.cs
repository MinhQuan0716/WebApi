using Application.Commons;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TrainingMaterialService : ITrainingMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;

        public TrainingMaterialService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, AppConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _configuration = configuration;
        }

        public async Task<TrainingMaterial> GetFile(string name)
        {
            var file = await _unitOfWork.TrainingMaterialRepository.GetFileWithName(name);
            return file;
        }

        public async Task<TrainingMaterial> Upload(IFormFile file, string lectureName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var material = new TrainingMaterial();
                if (memoryStream.Length > 0)
                {
                    material = new TrainingMaterial
                    {
                        TMatName = file.FileName,
                        TMatType = System.IO.Path.GetExtension(file.FileName),
                        TMatContent = memoryStream.ToArray(),
                        lectureID = _unitOfWork.LectureRepository.GetLectureIdByName(lectureName),
                    };

                    await _unitOfWork.TrainingMaterialRepository.AddAsync(material);
                    await _unitOfWork.SaveChangeAsync();
                }
                else
                {
                    throw new Exception("File not existed");
                }
                return material;
            }
        }
        // get type of the upload file
        public Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".xlsx", "application/vnd.ms-excel"},
                {".xls", "application/vnd.ms-excel"},
                {".rar", "application/vnd.rar"},
                {".zip", "application/zip"},
                {".mp3", "audio/mp3"},
                {".mp4", "video/mp4"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            };
        }

        public async Task<bool> DeleteTrainingMaterial(Guid id)
        {
            bool isDelete = await _unitOfWork.TrainingMaterialRepository.DeleteTrainingMaterial(id);
            await _unitOfWork.SaveChangeAsync();
            return isDelete;

        }

        public async Task<bool> UpdateTrainingMaterial(IFormFile file, Guid id)
        {
            TrainingMaterial findTrainingMaterial = await _unitOfWork.TrainingMaterialRepository.GetByIdAsync(id);
            bool isUpdated = false;
            if (findTrainingMaterial != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    if (memoryStream.Length > 0)
                    {

                        findTrainingMaterial.TMatName = file.FileName;
                        findTrainingMaterial.TMatType = System.IO.Path.GetExtension(file.FileName);
                        findTrainingMaterial.TMatContent = memoryStream.ToArray();
                        findTrainingMaterial.lectureID = findTrainingMaterial.lectureID;


                        _unitOfWork.TrainingMaterialRepository.Update(findTrainingMaterial);
                        await _unitOfWork.SaveChangeAsync();
                        isUpdated = true;
                    }
                    else
                    {
                        throw new Exception("File not existed");
                        isUpdated = false;
                    }

                }

            }
            return isUpdated;
        }
    }
}
