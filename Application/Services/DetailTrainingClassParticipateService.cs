﻿using Application.Commons;
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

        public async Task<Guid?> CheckJoinClass(Guid userId, Guid classId)
        {
            Guid? result = null;
            var item = (await _unitOfWork.DetailTrainingClassParticipateRepository.FindAsync(x => x.UserId == userId && x.TrainingClassID == classId)).FirstOrDefault();
            if(item is not null) 
            {
                result = item.Id;
            }
            return result;
        }

        public async Task<DetailTrainingClassParticipate> CreateTrainingClassParticipate(Guid userId, Guid classId)
        {
            var trainingClass = await _unitOfWork.TrainingClassRepository.GetByIdAsync(classId);
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var newDetailTrainingClassParticipate = new DetailTrainingClassParticipate { UserId = user.Id, TrainingClassID = trainingClass.Id, TraineeParticipationStatus = nameof(TraineeParticipationStatusEnum.NotJoined) };
            await _unitOfWork.DetailTrainingClassParticipateRepository.AddAsync(newDetailTrainingClassParticipate);
            await _unitOfWork.SaveChangeAsync();
            return newDetailTrainingClassParticipate;
        }

        public async Task<bool> UpdateTrainingStatus(Guid classid)
        {
            bool isTraining = false;
            var userid = _claimsService.GetCurrentUserId;
            //classid = GetCurrentClassId;
            DetailTrainingClassParticipate detail = await _unitOfWork.DetailTrainingClassParticipateRepository.GetDetailTrainingClassParticipateAsync(userid, classid);
            if (detail != null)
            {
                detail.TraineeParticipationStatus = nameof(TraineeParticipationStatusEnum.Joined);
                _unitOfWork.DetailTrainingClassParticipateRepository.Update(detail);
                await _unitOfWork.SaveChangeAsync();
                isTraining = true;
            }
            return isTraining;
        }
    }
}
