using Application.Interfaces;
using Application.Repositories;
using Application.ViewModels.SyllabusModels;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LectureService : ILectureService
    {

        private readonly IUnitOfWork _UnitOfWork;

        public LectureService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Lecture> AddNewLecture(LectureDTO lecture)
        {
            Lecture NewLecture = new Lecture()
            {
                LectureName = lecture.LectureName,
                OutputStandards = lecture.OutputStandards,
                Duration = lecture.Duration,
                DeliveryType = lecture.DeliveryType,
                Status = lecture.Status,
            };
            await _UnitOfWork.LectureRepository.AddAsync(NewLecture);
            //await _UnitOfWork.SaveChangeAsync();
            return NewLecture;
        }

        public async Task<DetailUnitLecture> AddNewDetailLecture(Lecture lecture, Unit unit)
        {
            DetailUnitLecture NewLecture = new DetailUnitLecture()
            {
                UnitId = unit.Id,
                LectureID = lecture.Id,
                CreatedBy = lecture.CreatedBy,
                IsDeleted = false
            };
            await _UnitOfWork.DetailUnitLectureRepository.AddAsync(NewLecture);
            await _UnitOfWork.SaveChangeAsync();
            return NewLecture;


        }
    }
}
