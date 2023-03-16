using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.ViewModels.AtttendanceModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons;

public class ApplicationCronJob
{
    private readonly IConfiguration _configuration;
    private readonly IAttendanceService _attendanceService;
    private readonly ISendMailHelper _mailHelper;
    private readonly ICurrentTime _currentTime;

    public ApplicationCronJob(IConfiguration configuration, IAttendanceService attendanceService, ISendMailHelper mailHelper, ICurrentTime currentTime)
    {
        _configuration = configuration;
        _attendanceService = attendanceService;
        _mailHelper = mailHelper;
        _currentTime = currentTime;
    }

    public async Task CheckAttendancesEveryDay()
    {
        // get all attendances info of today
        var absentList = await _attendanceService.GetAllAbsentInfoAsync();
        // send email to these trainee
        foreach (var x in absentList)
        {
            var subject = "Confirm absence";
            var message = $"Dear trainee {x.FullName}," +
                $"\nYou have absented today {_currentTime.GetCurrentTime().Date.ToString("dd/MM/yyyy")}, in class {x.ClassName}" +
                $"\nTotal absented day: {x.NumOfAbsented}";

            await _mailHelper.SendMailAsync(x.Email, subject, message);
        }
    }
}
