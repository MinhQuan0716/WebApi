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
    }
}
