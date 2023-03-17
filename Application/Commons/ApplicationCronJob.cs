using Application.Interfaces;
using Application.Services;
using Application.Utils;
using Application.ViewModels.AtttendanceModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
            //var message = $"Dear trainee {x.FullName}," +
            //    $"\nYou have absented today {_currentTime.GetCurrentTime().Date.ToString("dd/MM/yyyy")}, in class {x.ClassName}" +
            //    $"\nTotal absented day: {x.NumOfAbsented}";
            //Get project's directory and fetch AbsentTemplate content from EmailTemplates
            string exePath = Environment.CurrentDirectory.ToString();
            if (exePath.Contains(@"\bin\Debug\net7.0"))
                exePath = exePath.Remove(exePath.Length - (@"\bin\Debug\net7.0").Length);
            string FilePath = exePath + @"\EmailTemplates\AbsentTemplate.html";
            StreamReader streamreader = new StreamReader(FilePath);
            string MailText = streamreader.ReadToEnd();
            streamreader.Close();
            //Replace [TraineeName] = email/fullname
            if (!x.FullName.IsNullOrEmpty())
                MailText = MailText.Replace("[TraineeName]", x.Email);
            else
                MailText = MailText.Replace("[TraineeName]", x.FullName);
            //Replace [resetpasswordkey] = current date
            MailText = MailText.Replace("[TodayDate]", _currentTime.GetCurrentTime().Date.ToString("dd/MM/yyyy"));
            //Replace [ClassName] = class name 
            MailText = MailText.Replace("[ClassName]", x.ClassName);
            //Replace [NumOfAbsented] = numOfAbsented
            MailText = MailText.Replace("[NumOfAbsented]", x.NumOfAbsented.ToString());
            await _mailHelper.SendMailAsync(x.Email, subject, MailText);
        }
    }
}
