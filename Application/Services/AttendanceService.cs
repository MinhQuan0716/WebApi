﻿using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.AtttendanceModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Google.Apis.Logging;
using Google.Apis.Util;
using Microsoft.AspNetCore.Mvc;
using static Domain.Enums.AttendanceStatusEnums;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        public AttendanceService(IUnitOfWork unitOfWork, IMapper mapper, AppConfiguration configuration, ICurrentTime? currentTime)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _currentTime = currentTime;
        }
        public async Task<List<Attendance>> GetAttendanceByTraineeID(Guid id)
        {
            var finResult = _unitOfWork.AttendanceRepository.GetAttendancesByTraineeID(id);
            return finResult;
        }
        public async Task<List<Attendance>> GetAttendancesByTraineeClassID(Guid id)
        {
            var findResult = _unitOfWork.AttendanceRepository.GetAttendancesByTraineeClassID(id);
            return findResult;
        }
        public async Task<Attendance> UpdateAttendanceAsync(AttendanceDTO attendanceDto,Guid classId)
        {
            await GetAndCheckClassExist(classId);

            Attendance attendance = await MapAttendance(classId, attendanceDto);
            _unitOfWork.AttendanceRepository.Update(attendance);
            return await _unitOfWork.SaveChangeAsync() > 0 ? attendance : null;
        }
        public async Task<List<Attendance>> UploadAttendanceFormAsync(List<AttendanceDTO> attendanceDtos, Guid classId, string httpMethod)
        {
            //Guid ClassId = Guid.Parse(Id);
            List<Attendance> allList = new();
            await GetAndCheckClassExist(classId);

            foreach (var attendanceDto in attendanceDtos)
            {
                Attendance attendance;
                if (attendanceDto.AttendanceId != Guid.Empty && httpMethod == "PATCH")
                {
                    attendance = await _unitOfWork.AttendanceRepository.GetByIdAsync(attendanceDto.AttendanceId.Value);
                    attendance.ThrowIfNull($"AttendanceId: {attendanceDto.AttendanceId} Is Null");
                }
                //find the attendance Application that's approved by the admin
                attendance = await MapAttendance(classId, attendanceDto);
                allList.Add(attendance);
            }
            if (httpMethod == "POST")
                await _unitOfWork.AttendanceRepository.AddRangeAsync(allList);
            else
                _unitOfWork.AttendanceRepository.UpdateRange(allList);
            return await _unitOfWork.SaveChangeAsync() > 0 ? allList : null;
        }
        private async Task<Attendance> MapAttendance(Guid classId, AttendanceDTO attendanceDto)
        {
            var application = await _unitOfWork.ApplicationRepository.GetApplicationByUserAndClassId(attendanceDto, classId);
            Attendance attendance = _mapper.Map<Attendance>(attendanceDto);
            if (application is not null)
            {
                if (attendance.Status == nameof(Absent))
                    attendance.Status = nameof(AbsentPermit);
                attendance.ApplicationId = application.Id;
            }
            attendance.TrainingClassId = classId;
            return attendance;
        }
        private async Task<TrainingClass> GetAndCheckClassExist(Guid classId)
        {
            TrainingClass trainingClass = await _unitOfWork.TrainingClassRepository.GetByIdAsync(classId);
            trainingClass.ThrowIfNull("Training Class Missing");
            return trainingClass;
        }
    }
}