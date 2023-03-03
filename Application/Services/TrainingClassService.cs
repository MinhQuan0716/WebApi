using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TrainingClassService : ITrainingClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainingClassService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        /// <summary>
        /// This method set the isDeleted value of object to true
        /// </summary>
        /// <param name="trainingClassId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SoftRemoveTrainingClass(string trainingClassId)
        {
            var trainingClassObj = await GetTrainingClassByIdAsync(trainingClassId);

            _unitOfWork.TrainingClassRepository.SoftRemove(trainingClassObj);
            return (await _unitOfWork.SaveChangeAsync() > 0);
        }

        public async Task<TrainingClassViewModel?> CreateTrainingClassAsync(CreateTrainingClassDTO createTrainingClassDTO)
        {
            try
            {
                var trainingClassObj = _mapper.Map<TrainingClass>(createTrainingClassDTO);
                await _unitOfWork.TrainingClassRepository.AddAsync(trainingClassObj);
                
                //set location
                trainingClassObj.Location = await _unitOfWork.LocationRepository.GetByIdAsync(createTrainingClassDTO.LocationID) ?? throw new Exception("Invalid location Id");

                //set training program
                trainingClassObj.TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(createTrainingClassDTO.TrainingProgramId) ?? throw new Exception("Invalid training program Id");

                return (await _unitOfWork.SaveChangeAsync() > 0) ? _mapper.Map<TrainingClassViewModel>(trainingClassObj) : null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// This method uses _classId to find and update that class
        /// </summary>
        /// <param name="_classId"></param>
        /// <param name="updateTrainingCLassDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTrainingClass(string trainingClassId, UpdateTrainingCLassDTO updateTrainingCLassDTO)
        {
            var trainingClassObj= await GetTrainingClassByIdAsync(trainingClassId);
            _mapper.Map<UpdateTrainingCLassDTO, TrainingClass>(updateTrainingCLassDTO, trainingClassObj);
            //set location
            trainingClassObj.Location = await _unitOfWork.LocationRepository.GetByIdAsync(updateTrainingCLassDTO.LocationID) ?? throw new NullReferenceException("Invalid location Id");

            //set training program
            trainingClassObj.TrainingProgram = await _unitOfWork.TrainingProgramRepository.GetByIdAsync(updateTrainingCLassDTO.TrainingProgramId) ?? throw new NullReferenceException("Invalid training program Id");

            _unitOfWork.TrainingClassRepository.Update(trainingClassObj);
            return (await _unitOfWork.SaveChangeAsync() > 0);
        }
        /// <summary>
        /// This method find, return Training class and throw exception if can't find or get a mapping exception
        /// </summary>
        /// <param name="trainingClassId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TrainingClass> GetTrainingClassByIdAsync(string trainingClassId)
        {
            try
            {
                var _classId = _mapper.Map<Guid>(trainingClassId);
                var trainingClassObj = await _unitOfWork.TrainingClassRepository.GetByIdAsync(_classId);
                if (trainingClassObj == null)
                {
                    throw new NullReferenceException("Incorrect Id");
                }
                return trainingClassObj;
            }
            catch (AutoMapperMappingException)
            {
                throw new AutoMapperMappingException("Id is not a guid");
            }
        }
    }
}
