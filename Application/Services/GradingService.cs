using Application.Commons;
using Application.Interfaces;
using Application.ViewModels.GradingModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class GradingService : IGradingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentTime _currentTime;
    private readonly AppConfiguration _configuration;

    public GradingService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, AppConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
    }
    public async Task CreateGradingAsync(GradingModel model)
    {
        var grading = _mapper.Map<Grading>(model);
        await _unitOfWork.GradingRepository.AddAsync(grading);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<bool> DeleteGradingAsync(Guid gradingId)
    {
        var grading = await _unitOfWork.GradingRepository.GetByIdAsync(gradingId);
        if (grading == null)
        {
            return false;
        }
        _unitOfWork.GradingRepository.SoftRemove(grading);
        await _unitOfWork.SaveChangeAsync();
        return true;
    }

    public async Task<List<Grading>> GetAllGradingsAsync()
    {
        var gradings = await _unitOfWork.GradingRepository.GetAllAsync();
        return gradings;
    }

    public async Task<Grading> GetGradingsAsync(Guid gradingId)
    {
        var grading = await _unitOfWork.GradingRepository.GetByIdAsync(gradingId);
        return grading;
    }

    public List<MarkReportDto> GetMarkReportOfClass(Guid classID)
    {
        var result = _unitOfWork.GradingRepository.GetMarkReportOfClass(classID);
        return result;
    }

    public List<MarkReportDto> GetMarkReportOfTrainee(Guid traineeId)
    {
        var result = _unitOfWork.GradingRepository.GetMarkReportOfTrainee(traineeId);
        return result;
    }

    public async Task<bool> UpdateGradingAsync(Guid gradingId,GradingModel model)
    {
        var grading = await _unitOfWork.GradingRepository.GetByIdAsync(gradingId);
        if (grading == null)
        {
            return false;
        }
        grading.LectureId = model.LectureId;
        grading.DetailTrainingClassParticipateId = model.DetailTrainingClassParticipateId;
        grading.LetterGrade = model.LetterGrade;
        grading.NumericGrade = model.NumericGrade;
        _unitOfWork.GradingRepository.Update(grading);
        await _unitOfWork.SaveChangeAsync();
        return true;
    }
}
