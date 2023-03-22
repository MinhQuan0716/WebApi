using Application.Commons;
using Application.Interfaces;
using Application.ViewModels;
using Application.ViewModels.ApplicationViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Application.ApplicationFilterByEnum;

namespace Application.Services
{



    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppConfiguration _configuration;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration configuration, ICurrentTime currentTime, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            var Tset = await _unitOfWork.ApplicationRepository.GetByIdAsync(id);
            if (Tset != null)
            {
                Tset.Approved = status;
                _unitOfWork.ApplicationRepository.Update(Tset);
                await _unitOfWork.SaveChangeAsync();
                return true;

            }
            return false;
        }

        public async Task<bool> CreateApplication(ApplicationDTO applicationDTO)
        {

            //   var Test = _mapper.Map<Applications>(applicationDTO);
            var detailTrainingClass = await _unitOfWork.DetailTrainingClassParticipateRepository.GetDetailTrainingClassParticipateAsync(_claimsService.GetCurrentUserId, applicationDTO.TrainingClassID);
            if (detailTrainingClass != null)
            {
                Applications applications = new Applications()
                {
                    TrainingClassId = applicationDTO.TrainingClassID,
                    UserId = _claimsService.GetCurrentUserId,
                    AbsentDateRequested = applicationDTO.AbsentDateRequested,
                    Reason = applicationDTO.Reason,
                };

                await _unitOfWork.ApplicationRepository.AddAsync(applications);
            }
            return await _unitOfWork.SaveChangeAsync() > 0;


        }

        public async Task<Pagination<Applications>> GetAllApplication(Guid classId,
                                                                      ApplicationDateTimeFilterDTO condition = null,
                                                                      string searchString = "",
                                                                      string by = nameof(CreationDate),
                                                                      int pageNumber = 0,
                                                                      int pageSize = 10)
        {


            if (condition == null)
                condition = new ApplicationDateTimeFilterDTO()
                {
                    AbsentDateRequested = _currentTime.GetCurrentTime(),
                    Approved = false,
                    UserID = Guid.Empty,
                    Reason = searchString
                };

            var applications = await _unitOfWork.ApplicationRepository.ToPagination
                 (expression: x =>
                   x.Reason.Contains(searchString)
                   // 3 condition below are checking if it is null if not it will be used to check the value
                   && (condition.Approved != null && condition.Approved == x.Approved || condition.Approved == null)
                   && (condition.UserID != Guid.Empty && condition.UserID == x.UserId || condition.UserID == Guid.Empty)
                   && (classId != Guid.Empty && classId == x.TrainingClassId || classId == Guid.Empty)
                   &&
                   (
                       (by == nameof(CreationDate) && x.CreationDate >= condition.FromDate && x.CreationDate <= condition.ToDate)
                       ||
                       (by == nameof(RequestDate) && x.AbsentDateRequested >= condition.FromDate && x.AbsentDateRequested <= condition.ToDate)
                   )
                 , pageNumber, pageSize);

            //if (condition.Approved != null && condition.UserID == Guid.Empty)
            //{
            //    applications = await _unitOfWork.ApplicationRepository.ToPagination(
            //        x => x.Reason.Contains(searchString)
            //          && x.Approved == condition.Approved
            //          && (by == nameof(CreationDate) && x.CreationDate >= dateTimeFilter.FromDate && x.CreationDate <= dateTimeFilter.ToDate)
            //          || (by == nameof(RequestDate) && x.AbsentDateRequested >= dateTimeFilter.FromDate && x.AbsentDateRequested <= dateTimeFilter.ToDate)
            //        , pageNumber, pageSize);
            //}

            //if (condition.Approved == null && condition.UserID != Guid.Empty)
            //{
            //    applications = await _unitOfWork.ApplicationRepository.ToPagination(
            //        x => x.Reason.Contains(searchString)
            //          && x.UserId == condition.UserID
            //          && (by == nameof(CreationDate) && x.CreationDate >= dateTimeFilter.FromDate && x.CreationDate <= dateTimeFilter.ToDate)
            //          || (by == nameof(RequestDate) && x.AbsentDateRequested >= dateTimeFilter.FromDate && x.AbsentDateRequested <= dateTimeFilter.ToDate)
            //        , pageNumber, pageSize);
            //}

            //if (condition.Approved != null && condition.UserID != Guid.Empty)
            //{
            //    applications = await _unitOfWork.ApplicationRepository.ToPagination(
            //        x => x.Reason.Contains(searchString)
            //          && x.Approved == condition.Approved
            //          && x.UserId == condition.UserID
            //          && (by == nameof(CreationDate) && x.CreationDate >= dateTimeFilter.FromDate && x.CreationDate <= dateTimeFilter.ToDate)
            //          || (by == nameof(RequestDate) && x.AbsentDateRequested >= dateTimeFilter.FromDate && x.AbsentDateRequested <= dateTimeFilter.ToDate)
            //        , pageNumber, pageSize);
            //}
            return applications;
        }

    }
}
