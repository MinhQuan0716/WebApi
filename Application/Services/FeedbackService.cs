using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.FeedbackModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentTime _currentTime;
    private readonly AppConfiguration _configuration;
    private readonly ISendMailHelper _mailHelper;

    public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentTime currentTime, 
        AppConfiguration configuration, ISendMailHelper mailHelper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentTime = currentTime;
        _configuration = configuration;
        _mailHelper = mailHelper;
    }
    public async Task CreateFeedbackAsync(FeedbackModel model)
    {
        var feedback = _mapper.Map<Feedback>(model);
        await _unitOfWork.FeedbackRepository.AddAsync(feedback);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<bool> DeleteFeedbackAsync(Guid id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
        if (feedback == null)
        {
            return false;
        }
        _unitOfWork.FeedbackRepository.SoftRemove(feedback);
        await _unitOfWork.SaveChangeAsync();
        return true;
    }

    public async Task<FeedbackVM> GetFeedbackByIdAsync(Guid id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
        return _mapper.Map<FeedbackVM>(feedback);
    }

    public async Task<List<FeedbackVM>> GetFeedbacksAsync()
    {
        var feedbacks = await _unitOfWork.FeedbackRepository.GetAllAsync();
        return _mapper.Map<List<FeedbackVM>>(feedbacks);
    }

    public async Task<bool> SendFeedbacklink(Guid feedbackId)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(feedbackId);
        if (feedback != null)
        {
            var model = _mapper.Map<FeedbackModel>(feedback);
            var emailsList = _unitOfWork.FeedbackRepository.GetTraineeEmailsOfClass(model.TrainingCLassId!.Value);
            if (emailsList!= null)
            {
                var message = $"Feedback link: {model.FeedbackLink}";
                await _mailHelper.SendMailAsync(emailsList, model.FeedbackTitle, message);
                return true;
            } else
            {
                throw new Exception("There is any trainee in this class.");
            }
        } else
        {
            throw new Exception("Feedback does not exist.");
        }
    }

    public async Task<bool> UpdateFeedbackAsync(Guid feedbackId,FeedbackModel model)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(feedbackId);
        if (feedback == null)
        {
            return false;
        }
        feedback.FeedbackTitle = model.FeedbackTitle;
        feedback.FeedbackLink = model.FeedbackLink;
        feedback.TrainingCLassId = model.TrainingCLassId;
        _unitOfWork.FeedbackRepository.Update(feedback);
        await _unitOfWork.SaveChangeAsync();
        return true;
    }
}
