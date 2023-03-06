using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.TrainingClassModels;
using Application.ViewModels.TrainingProgramModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DetailTrainingClassParticipateService : IDetailTrainingClassParticipateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IClaimsService _claimsService;

        public DetailTrainingClassParticipateService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, AppConfiguration configuration, IClaimsService claims)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentTime = currentTime;
            _configuration = configuration;
            _claimsService = claims;
        }

        public async Task<DetailTrainingClassParticipate> CreateTrainingClassParticipate(Guid userId, Guid classId)
        {            
            var trainingClass = await _unitOfWork.TrainingClassRepository.GetByIdAsync(classId);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is not null)
            {
                if (user.RoleId == 4)
                {
                    var newDetailTrainingClassParticipate = new DetailTrainingClassParticipate { UserId = user.Id, TrainingClassID = trainingClass.Id, TraineeParticipationStatus = "NotJoined" };
                    await _unitOfWork.DetailTrainingClassParticipateRepository.AddAsync(newDetailTrainingClassParticipate);
                    await _unitOfWork.SaveChangeAsync();
                    return newDetailTrainingClassParticipate;
                }
            }
            return null;
        }

        public async Task<bool> IsTraining(Guid classid)
        {
            bool isTraining = false;
            var userid = _claimsService.GetCurrentUserId;
            //classid = GetCurrentClassId;
            DetailTrainingClassParticipate detail = await _unitOfWork.DetailTrainingClassParticipateRepository.GetDetailTrainingClassParticipateAsync(userid, classid);
            if (detail != null)
            {
                detail.TraineeParticipationStatus = nameof(TraineeParticipationStatus.Training);
                _unitOfWork.DetailTrainingClassParticipateRepository.Update(detail);
                await _unitOfWork.SaveChangeAsync();
                isTraining = true;
            }
            return isTraining;
        }
    }
}
